using ItemChanger;
using System.Linq;

namespace RandoStats.Stats
{
    public class ItemsObtainedTotal : PercentageStatistic
    {
        public ItemsObtainedTotal(string label) : base(label) { }

        public override bool IsEnabled => true;

        public override void HandlePlacement(AbstractPlacement placement)
        {
            ObtainedSum += placement.Items.Count(x => x.WasEverObtained());
            TotalSum += placement.Items.Count;
        }
    }
}
