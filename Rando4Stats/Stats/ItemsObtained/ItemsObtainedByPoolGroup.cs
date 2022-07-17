using ConnectionMetadataInjector;
using ItemChanger;
using ItemChanger.Items;
using Modding;
using RandomizerMod.Extensions;
using RandoStats.Util;
using System.Collections.Generic;
using System.Linq;

namespace RandoStats.Stats.ItemsObtained
{
    public class ItemsObtainedByPoolGroup : PercentageStatistic
    {
        private static readonly Loggable log = ScopedLoggers.GetLogger();

        private readonly string groupFriendlyName;
        private readonly string groupShortName;

        public override string StatNamespace => base.StatNamespace + "." + groupShortName;

        public override bool IsComputable => true;

        public override bool IsEnabled => TotalSum > 0;

        public ItemsObtainedByPoolGroup(string groupFriendlyName) : base(groupFriendlyName)
        {
            this.groupFriendlyName = groupFriendlyName;
            groupShortName = groupFriendlyName.Replace(" ", "");
        }

        public override void HandlePlacement(AbstractPlacement placement)
        {
            // ignore start geo - in theory a connection could add other SpawnGeoItems, but in reality it's unlikely because like... you can just increase
            // min and max start geo as desired, so why would you
            IEnumerable<AbstractItem> itemsInGroup = placement.Items
                .Where(x => !(x.RandoLocation()?.Name == LocationNames.Start && x is SpawnGeoItem))
                .Where(x => SupplementalMetadata.Of(x).Get(InjectedProps.ItemPoolGroup) == groupFriendlyName);
            ObtainedSum += itemsInGroup
                .Where(x => x.WasEverObtained())
                .SideEffect(x => log.LogDebug($"Counting item {x.RandoItem()?.Name} towards group {groupShortName} obtains"))
                .Count();
            TotalSum += itemsInGroup
                .SideEffect(x => log.LogDebug($"Counting item {x.RandoItem()?.Name} towards group {groupShortName} total"))
                .Count();
        }
    }
}
