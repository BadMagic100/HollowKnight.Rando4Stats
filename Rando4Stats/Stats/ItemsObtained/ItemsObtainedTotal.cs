using ItemChanger;
using ItemChanger.Items;
using RandomizerMod.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace RandoStats.Stats.ItemsObtained
{
    public class ItemsObtainedTotal : PercentageStatistic
    {
        public ItemsObtainedTotal(string label) : base(label) { }

        public override bool IsComputable => true;

        public override bool IsEnabled => true;

        public override void HandlePlacement(AbstractPlacement placement)
        {
            // ignore start geo - in theory a connection could add other SpawnGeoItems, but in reality it's unlikely because like... you can just increase
            // min and max start geo as desired, so why would you
            IEnumerable<AbstractItem> nonStartGeoItems = placement.Items
                .Where(x => !(x.RandoLocation()?.Name == LocationNames.Start && x is SpawnGeoItem));
            ObtainedSum += nonStartGeoItems.Count(x => x.WasEverObtained());
            TotalSum += nonStartGeoItems.Count();
        }
    }
}
