using ConnectionMetadataInjector.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RandoStats.Util
{
    internal static class PoolGroupUtil
    {
        public static readonly IReadOnlyList<string> BuiltInGroups = Enum.GetValues(typeof(PoolGroup))
                .OfType<PoolGroup>()
                .Where(g => g != PoolGroup.Other)
                .Select(g => g.FriendlyName())
                .ToList();
    }
}
