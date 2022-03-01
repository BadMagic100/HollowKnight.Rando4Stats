using ConnectionMetadataInjector.Util;
using RandoStats.Settings;
using RandoStats.Stats;
using RandoStats.Stats.TransitionsVisited;
using System.Collections.Generic;
using System.Linq;

namespace RandoStats.GUI.StatLayouts
{
    internal class TransitionsVisitedLayoutFactory : StatGroupLayoutFactory
    {
        public TransitionsVisitedLayoutFactory(StatLayoutSettings settings) : base(settings) { }

        public override string GroupName => "Transitions Visited";

        protected override IReadOnlyCollection<RandomizerStatistic> RootStatistics { get; init; } = new RandomizerStatistic[] 
        { 
            new TransitionsVisitedTotal("Total")
        };

        protected override IReadOnlyCollection<string> AllowedSubcategories { get; init; } = new string[]
        {
            StandardSubcategories.ByMapArea
        };

        protected override Dictionary<string, IReadOnlyCollection<RandomizerStatistic>> SubcategoryStatistics { get; init; } = new()
        {
            [StandardSubcategories.ByMapArea] = MapArea.AllMapAreas
                .Append(SubcategoryFinder.OTHER)
                .Select(a => new TransitionsVisitedByMapArea(a)).ToList()
        };
    }
}
