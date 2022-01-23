using RandoStats.Stats;
using System.Collections.Generic;

namespace RandoStats.GUI
{
    internal class ItemsObtainedStatGroupLayoutFactory : StatGroupLayoutFactory
    {
        public ItemsObtainedStatGroupLayoutFactory(HashSet<string> enabledSubcategories) : base(enabledSubcategories) { }

        public override bool ShouldDisplayForRandoSettings() => true;

        protected override IEnumerable<string> GetAllowedSubcategories() => new string[0];

        protected override IEnumerable<IRandomizerStatistic> GetRootStatistics() => new IRandomizerStatistic[]
        {
            new ItemsObtainedTotal("Total")
        };

        protected override string GetSectionHeader() => "Items Obtained";

        protected override IEnumerable<IRandomizerStatistic> GetStatisticsForSubcategory(string subcategory) => new IRandomizerStatistic[0];
    }
}
