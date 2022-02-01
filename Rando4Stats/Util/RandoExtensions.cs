using ItemChanger;
using RandomizerCore;
using RandomizerMod.IC;

namespace RandoStats.Util
{
    public static class RandoExtensions
    {
        public static ItemPlacement RandoPlacement(this AbstractItem item)
        {
            if (item.GetTag(out RandoItemTag tag))
            {
                return RandomizerMod.RandomizerMod.RS.Context.itemPlacements[tag.id];
            }
            return default;
        }

        public static string RandoItem(this AbstractItem item)
        {
            return item.RandoPlacement().item.Name ?? "";
        }

        public static string RandoLocation(this AbstractItem item)
        {
            return item.RandoPlacement().location.Name ?? "";
        }
    }
}
