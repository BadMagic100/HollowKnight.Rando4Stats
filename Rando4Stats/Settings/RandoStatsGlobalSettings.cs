using FStats;
using FStats.Attributes;
using ItemChanger.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace RandoStats.Settings
{
    public record struct StatSubpage(string StatName, string PageName);

    public class RandoStatsGlobalSettings
    {
        [JsonProperty]
        private Dictionary<string, bool> GlobalToggles { get; set; } = new();

        [JsonProperty]
        [JsonConverter(typeof(KVDictionaryConverter<StatSubpage, bool>))]
        private Dictionary<StatSubpage, bool> PageToggles { get; set; } = new();

        public bool ShouldDisplay(StatController c)
        {
            return ShouldDisplay(c.GetType());
        }

        public bool ShouldDisplay(StatController c, string subpage)
        {
            return ShouldDisplay(c.GetType(), subpage);
        }

        public bool ShouldDisplay(Type controllerType)
        {
            if (GlobalToggles.TryGetValue(controllerType.Name, out bool display))
            {
                return display;
            }

            display = !Attribute.IsDefined(controllerType, typeof(DefaultHiddenScreenAttribute));
            GlobalToggles[controllerType.Name] = display;
            return display;
        }

        public void DisplayStat(Type controllerType, bool value)
        {
            GlobalToggles[controllerType.Name] = value;
        }

        /// <summary>
        /// Whether to display a subpage, dependent on whether the stat is enabled
        /// </summary>
        public bool ShouldDisplay(Type controllerType, string subpage)
        {
            return ShouldDisplay(controllerType) && ShouldDisplaySubpageIndependent(controllerType, subpage);
        }

        /// <summary>
        /// Whether to display a subpage, independent of whether the stat itself is actually enabled
        /// </summary>
        public bool ShouldDisplaySubpageIndependent(Type controllerType, string subpage)
        {
            return PageToggles.GetOrDefault(new(controllerType.Name, subpage), true);
        }

        public void DisplayStatSubpage(Type controllerType, string subpage, bool value)
        {
            PageToggles[new(controllerType.Name, subpage)] = value;
        }
    }
}
