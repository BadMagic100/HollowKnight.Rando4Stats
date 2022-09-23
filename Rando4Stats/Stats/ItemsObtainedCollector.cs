using ConnectionMetadataInjector;
using ConnectionMetadataInjector.Util;
using FStats;
using FStats.StatControllers.ModConditional;
using FStats.Util;
using ItemChanger;
using ItemChanger.Internal;
using RandoStats.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RandoStats.Stats
{
    public class ItemsObtainedCollector : StatController
    {
        public override void Initialize() { }

        public override IEnumerable<DisplayInfo> GetDisplayInfos()
        {
            if (Ref.Settings == null)
            {
                yield break;
            }

            Dictionary<string, int> obtained = new();
            Dictionary<string, int> total = new();

            List<AbstractItem> items = Ref.Settings.Placements.Values.SelectValidPlacements()
                .SelectMany(p => p.Items.SelectValidItems())
                .ToList();
            foreach (AbstractItem item in items)
            {
                string group = SupplementalMetadata.Of(item).Get(InjectedProps.ItemPoolGroup);
                if (!obtained.ContainsKey(group))
                {
                    obtained[group] = 0;
                }
                if (!total.ContainsKey(group))
                {
                    total[group] = 0;
                }

                total[group]++;
                if (item.WasEverObtained())
                {
                    obtained[group]++;
                }
            }

            IEnumerable<string> connectionAddedGroups = InjectedProps.GetConnectionProvidedValues(
                    items, SupplementalMetadata.Of<AbstractItem>, InjectedProps.ItemPoolGroup)
                .Except(PoolGroupUtil.BuiltInGroups);
            List<string> lines = PoolGroupUtil.BuiltInGroups
                .Concat(connectionAddedGroups)
                .Append(SubcategoryFinder.OTHER)
                .Where(g => total.ContainsKey(g))
                .Select(g => $"{g} - {obtained[g]}/{total[g]}")
                .ToList();

            List<string> cols = new();
            int requiredCols = (int)Math.Ceiling(lines.Count / 10f);
            for (int i = 0; i < requiredCols; i++)
            {
                cols.Add(string.Join("\n", lines.Slice(i, requiredCols)));
            }

            string totalFraction = $"{obtained.Values.Sum()}/{total.Values.Sum()}";
            // weeeeee artifacts of legacy code :crying:
            StatFormatRegistry.SetStat("ItemsObtainedTotal", "Fraction", totalFraction);

            yield return new DisplayInfo()
            {
                Title = "Items Obtained",
                MainStat = totalFraction,
                StatColumns = cols,
                Priority = BuiltinScreenPriorityValues.ICChecksDisplay + 100,
            };
        }

        public override void Unload() { }
    }
}
