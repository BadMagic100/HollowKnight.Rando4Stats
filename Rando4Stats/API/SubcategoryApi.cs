using RandoStats.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RandoStats.API
{
    public static class SubcategoryApi
    {
        private static readonly List<Func<string, string?>> ItemPoolGroupHandlers;
        private static readonly List<Func<string, string?>> LocationPoolGroupHandlers;
        private static readonly List<Func<string, string?>> LocationMapAreaHandlers;
        private static readonly List<Func<string, string?>> TransitionMapAreaHandlers;

        private static readonly List<string> AdditionalPoolGroups = new();
        private static readonly List<string> AdditionalMapAreas = new();

        public const string DEFAULT_SUBCATEGORY = "Other";

        static SubcategoryApi()
        {
            ItemPoolGroupHandlers = new();
            ItemPoolGroupHandlers.Add(x => DefaultSubcategoryHandlers.GetItemPoolGroup(x)?.FriendlyName());

            LocationPoolGroupHandlers = new();
            LocationPoolGroupHandlers.Add(x => DefaultSubcategoryHandlers.GetLocationPoolGroup(x)?.FriendlyName());

            LocationMapAreaHandlers = new();
            LocationMapAreaHandlers.Add(x => DefaultSubcategoryHandlers.GetLocationMapArea(x));

            TransitionMapAreaHandlers = new();
            TransitionMapAreaHandlers.Add(x => DefaultSubcategoryHandlers.GetTransitionMapArea(x));
        }

        private static string ResolveHandlerChain(string input, IEnumerable<Func<string, string?>> handlerChain)
        {
            foreach (Func<string, string?> handler in handlerChain)
            {
                string? result = handler(input);
                if (result != null)
                {
                    return result;
                }
            }
            return DEFAULT_SUBCATEGORY;
        }

        public static void AddPoolGroup(string groupFriendlyName)
        {
            AdditionalPoolGroups.Add(groupFriendlyName);
        }

        public static void HookItemPoolGroups(Func<string, string?> handler)
        {
            ItemPoolGroupHandlers.Insert(0, handler);
        }

        public static IEnumerable<string> GetPoolGroups()
        {
            return Enum.GetValues(typeof(PoolGroup))
                .OfType<PoolGroup>()
                .Where(x => x != PoolGroup.Other) // ignore the "other" group, manually add it at the end
                .Select(x => x.FriendlyName())
                .Concat(AdditionalPoolGroups)
                .Append(DEFAULT_SUBCATEGORY);
        }

        public static void AddMapArea(string mapAreaFriendlyName)
        {
            AdditionalMapAreas.Add(mapAreaFriendlyName);
        }

        public static IEnumerable<string> GetMapAreas()
        {
            return new string[]
            {
                "Ancient Basin",
                "City of Tears",
                "Crystal Peak",
                "Deepnest",
                "Dirtmouth",
                "Fog Canyon",
                "Forgotten Crossroads",
                "Fungal Wastes",
                "Greenpath",
                "Howling Cliffs",
                "Kingdom's Edge",
                "Queen's Gardens",
                "Resting Grounds",
                "Royal Waterways",
                "White Palace"
            }.Concat(AdditionalMapAreas)
            .Append(DEFAULT_SUBCATEGORY);
        }

        public static string GetItemPoolGroup(string item) => ResolveHandlerChain(item, ItemPoolGroupHandlers);

        public static string GetLocationPoolGroup(string location) => ResolveHandlerChain(location, LocationPoolGroupHandlers);

        public static string GetLocationMapArea(string location) => ResolveHandlerChain(location, LocationMapAreaHandlers);

        public static string GetTransitionPoolGroup(string transition) => ResolveHandlerChain(transition, TransitionMapAreaHandlers);
    }
}
