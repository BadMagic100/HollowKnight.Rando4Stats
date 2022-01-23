using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

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
        public HashSet<string> EnabledSubcategories { get; set; } = new();
        public StatPosition Position { get; set; } = StatPosition.None;
        public int Order { get; set; } = 0;

        [JsonConstructor]
        public StatLayoutData() { }

        public StatLayoutData(HashSet<string> enabledSubcategories, StatPosition position, int sortOrder)
        {
            Position = position;
            EnabledSubcategories = enabledSubcategories;
            Order = sortOrder;
        }
    }
}
