using ItemChanger;
using Modding;
using RandoStats.Util;
using System.Collections.Generic;
using System.Linq;

namespace RandoStats.Stats
{
    public class ItemsObtainedByPoolGroup : PercentageStatistic
    {
        private static readonly Loggable log = ScopedLoggers.GetLogger();

        private readonly PoolGroup group;

        protected override string StatNamespace => base.StatNamespace + ":" + group.ToString();

        public override bool IsEnabled => TotalSum > 0;

        public ItemsObtainedByPoolGroup(PoolGroup group) : base(group.FriendlyName())
        {
            this.group = group;
        }

        public override void HandlePlacement(AbstractPlacement placement)
        {
            IEnumerable<AbstractItem> itemsInGroup = placement.Items
                .Where(x => PoolFinder.GetItemPoolGroup(x.GetRandoPlacement().item.Name) == group);
            ObtainedSum += itemsInGroup
                .Where(x => x.WasEverObtained())
                .SideEffect(x => log.LogDebug($"Counting item {x.GetRandoPlacement().item.Name} towards group {group} obtains"))
                .Count();
            TotalSum += itemsInGroup
                .SideEffect(x => log.LogDebug($"Counting item {x.GetRandoPlacement().item.Name} towards group {group} total"))
                .Count();
        }
    }
}
