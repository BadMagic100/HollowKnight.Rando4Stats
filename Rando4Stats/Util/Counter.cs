using ItemChanger.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace RandoStats.Util
{
    public class Counter
    {
        private Dictionary<string, int> internalCounter = new();

        public int this[string key]
        {
            get => internalCounter.GetOrDefault(key, 0);
            set => internalCounter[key] = value;
        }

        public int Total
        {
            get => internalCounter.Values.Sum();
        }
    }
}
