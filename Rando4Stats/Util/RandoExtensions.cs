using ItemChanger;
using RandomizerMod.IC;
using RandomizerMod.RC;

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
            return item.RandoPlacement().Item.Name ?? "";
        }

        public static string RandoLocation(this AbstractItem item)
        {
            return item.RandoPlacement().Location.Name ?? "";
        }
    }
}
