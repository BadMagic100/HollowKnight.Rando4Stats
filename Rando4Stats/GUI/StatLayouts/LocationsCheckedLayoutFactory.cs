using ConnectionMetadataInjector;
using ConnectionMetadataInjector.Util;
using ItemChanger;
using RandoStats.Settings;
using RandoStats.Stats;
using RandoStats.Stats.LocationsChecked;
using RandoStats.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using CMI = ConnectionMetadataInjector.ConnectionMetadataInjector;

namespace RandoStats.GUI.StatLayouts
{
    internal class LocationsCheckedLayoutFactory : StatGroupLayoutFactory
    {
        public LocationsCheckedLayoutFactory(StatLayoutSettings settings) : base(settings) { }

        public override string GroupName => "Locations Checked";

        protected override IReadOnlyCollection<RandomizerStatistic> RootStatistics { get; init; } = new RandomizerStatistic[]
        {
            new LocationsCheckedTotal("Total")
        };

        protected override IReadOnlyCollection<string> AllowedSubcategories { get; init; } = new string[]
        {
            StandardSubcategories.ByPoolGroup,
            StandardSubcategories.ByMapArea
        };

        private static IEnumerable<string> ConnectionProvidedGroups()
        {
            IEnumerable<AbstractPlacement> validItems = StatsEngine.GetEligiblePlacements()
                .Where(plt => plt.RandoLocation()?.Name != LocationNames.Start);
            return CMI.GetConnectionProvidedValues(validItems, SupplementalMetadata.OfPlacementAndLocations, CMI.LocationPoolGroup);
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
                .Select(g => new LocationsCheckedByPoolGroup(g)).ToList(),
            [StandardSubcategories.ByMapArea] = MapArea.AllMapAreas
                .Append(SubcategoryFinder.OTHER)
                .Select(a => new LocationsCheckedByMapArea(a)).ToList()
        };
    }
}
