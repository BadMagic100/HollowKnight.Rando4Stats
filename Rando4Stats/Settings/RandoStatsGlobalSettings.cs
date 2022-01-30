using Newtonsoft.Json;
using RandoStats.GUI;
using RandoStats.Stats;
using System;
using System.Collections.Generic;
using System.Reflection;

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

        private void MergeSettings(StatLayoutSettings input, ref StatLayoutSettings target)
        {
            target.Position = input.Position;
            target.Order = input.Order;
            target.EnabledSubcategories = input.EnabledSubcategories;
        }

        [JsonIgnore]
        internal IEnumerable<StatGroupLayoutFactory> LayoutFactories
        {
            get
            {
                foreach (PropertyInfo prop in GetType().GetProperties())
                {
                    if (prop.PropertyType == typeof(StatLayoutSettings))
                    {
                        StatLayoutSettings settings = (StatLayoutSettings)prop.GetValue(this);
                        string factoryTypeFullName = "RandoStats.GUI." + prop.Name.Replace("Settings", "Factory");
                        Type factoryType = GetType().Assembly.GetType(factoryTypeFullName);

                        ConstructorInfo ctor = factoryType.GetConstructor(new Type[] { typeof(StatLayoutSettings) });
                        StatGroupLayoutFactory factory = (StatGroupLayoutFactory)ctor.Invoke(new object[] { settings });
                        yield return factory;
                    }
                }
            }
        }
    }
}
