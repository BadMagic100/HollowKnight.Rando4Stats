using ItemChanger;
using RandomizerCore;
using RandomizerMod.IC;

namespace RandoStats.Util
{
    public static class RandoExtensions
    {
        public static ItemPlacement GetRandoPlacement(this AbstractItem item)
        {
            if (item.GetTag(out RandoItemTag tag))
            {
                return RandomizerMod.RandomizerMod.RS.Context.itemPlacements[tag.id];
            }
            return default;
        }
    }
}
