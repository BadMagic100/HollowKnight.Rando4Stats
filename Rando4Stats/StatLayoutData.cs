using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Linq;

namespace RandoStats
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum StatPosition
    {
        TopLeft,
        TopCenter,
        TopRight,
        BottomLeft,
        BottomRight,
        None
    }

    public class StatLayoutData
    {
        private Dictionary<string, bool> enabledSubcategories = new();
        public Dictionary<string, bool> EnabledSubcategories
        {
            get => enabledSubcategories;
            set
            {
                foreach (string key in value.Keys)
                {
                    enabledSubcategories[key] = value[key];
                }
            }
        }

        [JsonIgnore]
        public HashSet<string> EnabledSubcategoryNames => new(enabledSubcategories.Where(x => x.Value).Select(x => x.Key));

        public StatPosition Position { get; set; } = StatPosition.None;

        public int Order { get; set; } = 0;

        [JsonConstructor]
        public StatLayoutData() { }

        public StatLayoutData(HashSet<string> enabledSubcategories, StatPosition position, int sortOrder)
        {
            Position = position;
            EnabledSubcategories = enabledSubcategories.ToDictionary(x => x, _ => true);
            Order = sortOrder;
        }

        public StatLayoutData(Dictionary<string, bool> enabledSubcategories, StatPosition position, int sortOrder)
        {
            Position = position;
            EnabledSubcategories = enabledSubcategories;
            Order = sortOrder;
        }
    }
}
