using ConnectionMetadataInjector;
using ConnectionMetadataInjector.Util;
using ItemChanger;
using ItemChanger.Items;
using RandomizerMod.Extensions;
using RandoStats.Settings;
using RandoStats.Stats;
using RandoStats.Stats.ItemsObtained;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RandoStats.GUI.StatLayouts
{
    internal class ItemsObtainedLayoutFactory : StatGroupLayoutFactory
    {
        public ItemsObtainedLayoutFactory(StatLayoutSettings settings) : base(settings) { }

        public override string GroupName => "Items Obtained";

        protected override IReadOnlyCollection<RandomizerStatistic> RootStatistics { get; init; } = new RandomizerStatistic[]
        {
            new ItemsObtainedTotal("Total")
        };

        protected override IReadOnlyCollection<string> AllowedSubcategories { get; init; } = new string[]
        {
            StandardSubcategories.ByPoolGroup
        };

        private static IEnumerable<string> ConnectionProvidedGroups()
        {
            IEnumerable<AbstractItem> validItems = StatsEngine.GetEligiblePlacements()
                .SelectMany(plt => plt.Items)
                .Where(i => !(i.RandoLocation()?.Name == LocationNames.Start && i is SpawnGeoItem));
            return InjectedProps.GetConnectionProvidedValues(validItems, SupplementalMetadata.Of, InjectedProps.ItemPoolGroup);
        }

        private static IEnumerable<string> BuiltInGroups() => Enum.GetValues(typeof(PoolGroup))
            .OfType<PoolGroup>()
            .Where(g => g != PoolGroup.Other)
            .Select(g => g.FriendlyName());

        protected override Dictionary<string, IReadOnlyCollection<RandomizerStatistic>> SubcategoryStatistics { get; init; } = new()
        {
            [StandardSubcategories.ByPoolGroup] = BuiltInGroups()
                .Concat(ConnectionProvidedGroups().Except(BuiltInGroups()))
                .Append(SubcategoryFinder.OTHER)
                .Select(g => new ItemsObtainedByPoolGroup(g)).ToList()
        };
    }
}
