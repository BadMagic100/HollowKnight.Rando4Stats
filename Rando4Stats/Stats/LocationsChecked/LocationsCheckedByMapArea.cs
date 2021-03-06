using ConnectionMetadataInjector.Util;
using ItemChanger;
using Modding;
using RandomizerMod.Extensions;
using RandomizerMod.RC;
using RandoStats.Util;

namespace RandoStats.Stats.LocationsChecked
{
    public class LocationsCheckedByMapArea : PercentageStatistic
    {
        private static readonly Loggable log = ScopedLoggers.GetLogger();

        private readonly string areaName;
        private readonly string areaShortName;

        public override string StatNamespace => base.StatNamespace + "." + areaShortName;

        public override bool IsComputable => true;

        public override bool IsEnabled => TotalSum > 0;

        public LocationsCheckedByMapArea(string area) : base(area == MapArea.FORGOTTEN_CROSSROADS ? "Crossroads" : area)
        {
            areaName = area;
            areaShortName = area.Replace(" ", "");
        }

        public override void HandlePlacement(AbstractPlacement placement)
        {
            RandoModLocation? loc = placement.RandoLocation();
            if (loc != null && loc.Name != LocationNames.Start && (loc.LocationDef?.MapArea ?? SubcategoryFinder.OTHER) == areaName)
            {
                if (placement.CheckVisitedAny(VisitState.Previewed | VisitState.ObtainedAnyItem))
                {
                    log.LogDebug($"Counting location {loc.Name} towards group {areaShortName} obtains");
                    ObtainedSum++;
                }
                log.LogDebug($"Counting location {loc.Name} towards group {areaShortName} total");
                TotalSum++;
            }
        }
    }
}
