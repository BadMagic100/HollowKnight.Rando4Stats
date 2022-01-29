using RandoStats.Stats;
using RandoStats.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RandoStats.GUI
{
    internal class ItemsObtainedLayoutFactory : StatGroupLayoutFactory
    {
        public ItemsObtainedLayoutFactory(HashSet<string> enabledSubcategories) : base(enabledSubcategories) { }

        public override bool ShouldDisplayForRandoSettings() => true;

        protected override IEnumerable<string> GetAllowedSubcategories() => new string[]
        {
            StandardSubcategories.ByPoolGroup
        };

        protected override IEnumerable<IRandomizerStatistic> GetRootStatistics() => new IRandomizerStatistic[]
        {
            new ItemsObtainedTotal("Total")
        };

        protected override string GetSectionHeader() => "Items Obtained";

        protected override IEnumerable<IRandomizerStatistic> GetStatisticsForSubcategory(string subcategory) => subcategory switch
        {
            StandardSubcategories.ByPoolGroup => Enum.GetValues(typeof(PoolGroup)).OfType<PoolGroup>().Select(g => new ItemsObtainedByPoolGroup(g)),
            _ => throw new NotImplementedException($"Subcategory {subcategory} not implemented. This probably means it was added to GetAllowedSubcategories but not here")
        };
    }
}
