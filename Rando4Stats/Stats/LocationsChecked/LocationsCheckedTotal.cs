using ConnectionMetadataInjector.Util;
using ItemChanger;
using RandomizerMod.RC;

namespace RandoStats.Stats.LocationsChecked
{
    public class LocationsCheckedTotal : PercentageStatistic
    {
        public LocationsCheckedTotal(string label) : base(label) { }

        public override bool IsEnabled => true;

        public override void HandlePlacement(AbstractPlacement placement)
        {
            RandoModLocation? loc = placement.RandoLocation();
            if (loc != null && loc.Name != "Start")
            {
                if (placement.CheckVisitedAny(VisitState.Previewed | VisitState.ObtainedAnyItem))
                {
                    ObtainedSum++;
                }
                TotalSum++;
            }
        }
    }
}
