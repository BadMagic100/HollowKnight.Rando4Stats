using RandoStats.Stats;
using System.Collections.Generic;

namespace RandoStats.Settings
{
    public class RandoStatsGlobalSettings
    {
        public string StatFormatString { get; set; } = "$RACING_EXTENDED$";

        private StatLayoutSettings itemsObtainedSettingsStore = new(new HashSet<string>() { StandardSubcategories.ByPoolGroup }, StatPosition.TopLeft, 0);
        public StatLayoutSettings ItemsObtainedLayoutSettings
        {
            get => itemsObtainedSettingsStore;
            set => MergeSettings(value, ref itemsObtainedSettingsStore);
        }

        private StatLayoutSettings locationsCheckedSettingsStore = new(new HashSet<string>() { StandardSubcategories.ByPoolGroup,
            StandardSubcategories.ByMapArea }, StatPosition.TopRight, 0);
        public StatLayoutSettings LocationsCheckedLayoutSettings
        {
            get => locationsCheckedSettingsStore;
            set => MergeSettings(value, ref locationsCheckedSettingsStore);
        }

        private void MergeSettings(StatLayoutSettings input, ref StatLayoutSettings target)
        {
            target.Position = input.Position;
            target.Order = input.Order;
            target.EnabledSubcategories = input.EnabledSubcategories;
        }
    }
}
