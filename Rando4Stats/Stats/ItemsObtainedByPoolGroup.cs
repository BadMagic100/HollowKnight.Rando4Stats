using ItemChanger;
using ItemChanger.Items;
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
            // ignore start geo - in theory a connection could add other SpawnGeoItems, but in reality it's unlikely because like... you can just increase
            // min and max start geo as desired, so why would you
            IEnumerable<AbstractItem> itemsInGroup = placement.Items
                .Where(x => !(x.RandoLocation() == "Start" && x is SpawnGeoItem))
                .Where(x => PoolFinder.GetItemPoolGroup(x.RandoItem()) == group);
            ObtainedSum += itemsInGroup
                .Where(x => x.WasEverObtained())
                .SideEffect(x => log.LogDebug($"Counting item {x.RandoItem()} towards group {group} obtains"))
                .Count();
            TotalSum += itemsInGroup
                .SideEffect(x => log.LogDebug($"Counting item {x.RandoItem()} towards group {group} total"))
                .Count();
        }
    }
}
