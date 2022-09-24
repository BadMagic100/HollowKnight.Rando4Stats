using ConnectionMetadataInjector;
using ConnectionMetadataInjector.Util;
using FStats;
using FStats.StatControllers;
using ItemChanger;
using ItemChanger.Internal;
using RandomizerMod.Extensions;
using RandomizerMod.RC;
using RandoStats.Util;
using System.Collections.Generic;
using System.Linq;

namespace RandoStats.Stats
{
    public class LocationsCheckedCollector : StatController
    {
        const string TITLE = "Locations Checked";
        const int PRIORITY = -80_000;

        public override void Initialize() { }

        public override IEnumerable<DisplayInfo> GetDisplayInfos()
        {
            if (Ref.Settings == null || !RandomizerMod.RandomizerMod.IsRandoSave)
            {
                yield break;
            }

            Counter checkedByArea = new();
            Counter totalByArea = new();
            Counter checkedByGroup = new();
            Counter totalByGroup = new();

            List<AbstractPlacement> placements = Ref.Settings.Placements.Values.SelectValidPlacements().ToList();
            foreach (AbstractPlacement plt in placements)
            {
                string? area = null;
                RandoModLocation? loc = plt.RandoLocation();
                if (loc != null)
                {
                    string? randoScene = loc.LocationDef?.SceneName;
                    if (!string.IsNullOrEmpty(randoScene))
                    {
                        area = AreaName.CleanAreaName(randoScene);
                    }
                    else
                    {
                        string? randoArea = loc.LocationDef?.MapArea;
                        if (AreaName.Areas.Contains(randoArea))
                        {
                            area = randoArea;
                        }
                        else
                        {
                            area = AreaName.Other;
                        }
                    }
                }
                if (string.IsNullOrEmpty(area))
                {
                    continue;
                }

                string group = SupplementalMetadata.Of(plt).Get(InjectedProps.LocationPoolGroup);

                if (plt.CheckVisitedAny(VisitState.Previewed | VisitState.ObtainedAnyItem))
                {
                    checkedByArea[area!]++;
                    checkedByGroup[group]++;
                }
                totalByArea[area!]++;
                totalByGroup[group]++;
            }

            List<string> areas = FStatsMod.LS.Get<TimeByAreaStat>().AreaOrder()
                .Select(a => $"{a} - {checkedByArea[a]}/{totalByArea[a]}")
                .ToList()
                .TableColumns(10);

            IEnumerable<string> connectionProvidedGroups = InjectedProps.GetConnectionProvidedValues(
                    placements, SupplementalMetadata.Of<AbstractPlacement>, InjectedProps.LocationPoolGroup)
                .Except(PoolGroupUtil.BuiltInGroups);
            List<string> groups = PoolGroupUtil.BuiltInGroups
                .Concat(connectionProvidedGroups)
                .Append(SubcategoryFinder.OTHER)
                .Where(g => totalByGroup[g] > 0)
                .Select(g => $"{g} - {checkedByGroup[g]}/{totalByGroup[g]}")
                .ToList()
                .TableColumns(10);

            yield return new DisplayInfo()
            {
                Title = TITLE,
                MainStat = $"{checkedByArea.Total}/{totalByArea.Total}",
                StatColumns = areas,
                Priority = PRIORITY,
            };

            yield return new DisplayInfo()
            {
                Title = TITLE,
                MainStat = $"{checkedByGroup.Total}/{totalByGroup.Total}",
                StatColumns = groups,
                Priority = PRIORITY
            };
        }

        public override void Unload() { }
    }
}
