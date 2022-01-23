using Modding;
using RandoStats.Util;
using System.Linq;
using Rando = RandomizerMod.RandomizerMod;

namespace RandoStats.Stats
{
    public class ItemsObtainedByPoolGroup : PercentageStatistic
    {
        private static readonly Loggable log = LogHelper.GetLogger();

        private readonly PoolGroup group;

        public override bool IsEnabled => GetTotal() > 0;

        public ItemsObtainedByPoolGroup(PoolGroup group) : base(group.FriendlyName(), true)
        {
            this.group = group;
        }

        protected override int GetObtained()
        {
            return Rando.RS.TrackerData.obtainedItems
                .Select(i => Rando.RS.Context.itemPlacements[i])
                .Where(p => PoolFinder.GetItemPoolGroup(p.item.Name) == group)
                .SideEffect(x => log.LogDebug($"Counting item {x.item.Name} towards group {group} obtains"))
                .Count();
        }

        protected override int GetTotal()
        {
            return Rando.RS.Context.itemPlacements
                .Where(p => PoolFinder.GetItemPoolGroup(p.item.Name) == group)
                .SideEffect(x => log.LogDebug($"Counting item {x.item.Name} towards group {group} total"))
                .Count();
        }
    }
}
