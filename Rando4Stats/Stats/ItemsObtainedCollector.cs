using ConnectionMetadataInjector;
using ConnectionMetadataInjector.Util;
using FStats;
using FStats.StatControllers.ModConditional;
using ItemChanger;
using ItemChanger.Internal;
using RandoStats.Util;
using System.Collections.Generic;
using System.Linq;

namespace RandoStats.Stats
{
    public class ItemsObtainedCollector : StatController
    {
        const string TITLE = "Items Obtained";

        public override void Initialize() { }

        public override IEnumerable<DisplayInfo> GetDisplayInfos()
        {
            if (Ref.Settings == null)
            {
                yield break;
            }

            Counter obtained = new();
            Counter total = new();

            List<AbstractItem> items = Ref.Settings.Placements.Values.SelectValidPlacements()
                .SelectMany(p => p.Items.SelectValidItems())
                .ToList();
            foreach (AbstractItem item in items)
            {
                string group = SupplementalMetadata.Of(item).Get(InjectedProps.ItemPoolGroup);

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
                .Where(g => total[g] > 0)
                .Select(g => $"{g} - {obtained[g]}/{total[g]}")
                .ToList();

            string totalFraction = $"{obtained.Total}/{total.Total}";
            // weeeeee artifacts of legacy code :crying:
            StatFormatRegistry.SetStat("ItemsObtainedTotal", "Fraction", totalFraction);

            yield return new DisplayInfo()
            {
                Title = TITLE,
                MainStat = totalFraction,
                StatColumns = lines.TableColumns(10),
                Priority = BuiltinScreenPriorityValues.ICChecksDisplay + 100,
            };
        }

        public override void Unload() { }
    }
}
