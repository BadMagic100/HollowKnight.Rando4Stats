using ConnectionMetadataInjector.Util;
using ItemChanger;
using Modding;
using RandomizerMod.RC;
using RandoStats.Util;

namespace RandoStats.Stats.LocationsChecked
{
    public class LocationsCheckedByMapArea : PercentageStatistic
    {
        private static readonly Loggable log = ScopedLoggers.GetLogger();

        private readonly string areaName;
        private readonly string areaShortName;

        protected override string StatNamespace => base.StatNamespace + ":" + areaShortName;

        public override bool IsEnabled => TotalSum > 0;

        public LocationsCheckedByMapArea(string area) : base(area)
        {
            areaName = area;
            areaShortName = area.Replace(" ", "");
        }

        public override void HandlePlacement(AbstractPlacement placement)
        {
            RandoModLocation? loc = placement.RandoLocation();
            if (loc != null && loc.Name != "Start" && (loc.LocationDef?.MapArea ?? SubcategoryFinder.OTHER) == areaName)
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
