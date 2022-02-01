using ItemChanger;
using ItemChanger.Items;
using RandoStats.Util;
using System.Collections.Generic;
using System.Linq;

namespace RandoStats.Stats
{
    public class ItemsObtainedTotal : PercentageStatistic
    {
        public ItemsObtainedTotal(string label) : base(label) { }

        public override bool IsEnabled => true;

        public override void HandlePlacement(AbstractPlacement placement)
        {
            IEnumerable<AbstractItem> nonStartGeoItems = placement.Items
                // ignore start geo - in theory a connection could add other SpawnGeoItems, but in reality it's unlikely because like... you can just increase
                // min and max start geo as desired, so why would you
                .Where(x => !(x.GetRandoPlacement().location.Name == "Start" && x is SpawnGeoItem));
            ObtainedSum += nonStartGeoItems.Count(x => x.WasEverObtained());
            TotalSum += nonStartGeoItems.Count();
        }
    }
}
