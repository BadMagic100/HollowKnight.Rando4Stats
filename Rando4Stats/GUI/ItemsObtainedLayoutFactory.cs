using RandoStats.API;
using RandoStats.Settings;
using RandoStats.Stats;
using System.Collections.Generic;
using System.Linq;

namespace RandoStats.GUI
{
    internal class ItemsObtainedLayoutFactory : StatGroupLayoutFactory
    {
        public ItemsObtainedLayoutFactory(StatLayoutSettings settings) : base(settings) { }

        public override bool CanDisplay => true;

        public override string GroupName => "Items Obtained";

        protected override IReadOnlyCollection<RandomizerStatistic> RootStatistics { get; init; } = new RandomizerStatistic[]
        {
            new ItemsObtainedTotal("Total")
        };

        protected override IReadOnlyCollection<string> AllowedSubcategories { get; init; } = new string[]
        {
            StandardSubcategories.ByPoolGroup
        };

        protected override Dictionary<string, IReadOnlyCollection<RandomizerStatistic>> SubcategoryStatistics { get; init; } = new()
        {
            [StandardSubcategories.ByPoolGroup] = SubcategoryApi.GetPoolGroups().Select(g => new ItemsObtainedByPoolGroup(g)).ToList()
        };
    }
}
