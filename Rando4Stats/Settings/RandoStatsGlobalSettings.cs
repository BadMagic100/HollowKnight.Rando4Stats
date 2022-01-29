using RandoStats.Stats;
using System.Collections.Generic;

namespace RandoStats.Settings
{
    public class RandoStatsGlobalSettings
    {
        public string StatFormatString { get; set; } = "$RACING_EXTENDED$";

        private StatLayoutData itemsObtainedSettingsStore = new(new HashSet<string>() { StandardSubcategories.ByPoolGroup }, StatPosition.TopLeft, 0);
        public StatLayoutData ItemsObtainedLayoutSettings
        {
            get => itemsObtainedSettingsStore;
            set => MergeSettingsWithDefault(value, ref itemsObtainedSettingsStore);
        }

        private void MergeSettingsWithDefault(StatLayoutData input, ref StatLayoutData target)
        {
            target.Position = input.Position;
            target.Order = input.Order;
            target.EnabledSubcategories = input.EnabledSubcategories;
        }
    }
}
