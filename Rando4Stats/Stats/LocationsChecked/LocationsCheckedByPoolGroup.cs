using ConnectionMetadataInjector;
using ItemChanger;
using Modding;
using RandomizerMod.RC;
using RandoStats.Util;
using CMI = ConnectionMetadataInjector.ConnectionMetadataInjector;

namespace RandoStats.Stats.LocationsChecked
{
    public class LocationsCheckedByPoolGroup : PercentageStatistic
    {
        private static readonly Loggable log = ScopedLoggers.GetLogger();

        private readonly string groupFriendlyName;
        private readonly string groupShortName;

        public override string StatNamespace => base.StatNamespace + "." + groupShortName;

        public override bool IsComputable => true;

        public override bool IsEnabled => TotalSum > 0;

        public LocationsCheckedByPoolGroup(string groupFriendlyName) : base(groupFriendlyName)
        {
            this.groupFriendlyName = groupFriendlyName;
            groupShortName = groupFriendlyName.Replace(" ", "");
        }

        public override void HandlePlacement(AbstractPlacement placement)
        {
            RandoModLocation? loc = placement.RandoLocation();
            if (loc != null && loc.Name != LocationNames.Start && SupplementalMetadata.OfPlacementAndLocations(placement).Get(CMI.LocationPoolGroup) == groupFriendlyName)
            {
                if (placement.CheckVisitedAny(VisitState.Previewed | VisitState.ObtainedAnyItem))
                {
                    log.LogDebug($"Counting location {loc.Name} towards group {groupShortName} obtains");
                    ObtainedSum++;
                }
                log.LogDebug($"Counting location {loc.Name} towards group {groupShortName} total");
                TotalSum++;
            }
        }
    }
}
