using Rando = RandomizerMod.RandomizerMod;

namespace RandoStats.Stats
{
    public class ItemsObtainedTotal : PercentageStatistic
    {
        public ItemsObtainedTotal(string label) : base(label) { }

        public override bool IsEnabled => true;

        protected override int GetObtained()
        {
            return Rando.RS.TrackerData.obtainedItems.Count;
        }

        protected override int GetTotal()
        {
            return Rando.RS.Context.itemPlacements.Count;
        }
    }
}
