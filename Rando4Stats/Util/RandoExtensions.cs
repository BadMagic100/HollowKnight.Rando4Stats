using ItemChanger;
using RandomizerMod.IC;
using RandomizerMod.RC;
using System.Collections.Generic;
using System.Linq;

namespace RandoStats.Util
{
    /// <summary>
    /// Extension methods to read rando information of an <see cref="AbstractItem"/>'s <see cref="RandoItemTag"/> tag.
    /// </summary>
    public static class RandoExtensions
    {
        /// <summary>
        /// Gets the rando placement of all items in a placement
        /// </summary>
        public static IEnumerable<ItemPlacement> RandoPlacements(this AbstractPlacement placement)
        {
            if (placement.GetTag(out RandoPlacementTag tag))
            {
                return tag.ids.Select(id => RandomizerMod.RandomizerMod.RS.Context.itemPlacements[id]);
            }
            return Enumerable.Empty<ItemPlacement>();
        }

        /// <summary>
        /// Gets the rando location of a placement
        /// </summary>
        public static RandoModLocation? RandoLocation(this AbstractPlacement placement)
        {
            if (placement.GetTag(out RandoPlacementTag tag))
            {
                return RandomizerMod.RandomizerMod.RS.Context.itemPlacements[tag.ids.First()].Location;
            }
            return null;
        }

        /// <summary>
        /// Gets the rando placement of the item
        /// </summary>
        public static ItemPlacement RandoPlacement(this AbstractItem item)
        {
            if (item.GetTag(out RandoItemTag tag))
            {
                return RandomizerMod.RandomizerMod.RS.Context.itemPlacements[tag.id];
            }
            return default;
        }

        /// <summary>
        /// Gets the rando item name of the item
        /// </summary>
        public static RandoModItem? RandoItem(this AbstractItem item)
        {
            return item.RandoPlacement().Item;
        }

        /// <summary>
        /// Gets the rando location name of the location the item is placed at
        /// </summary>
        public static RandoModLocation? RandoLocation(this AbstractItem item)
        {
            return item.RandoPlacement().Location;
        }

    }
}
