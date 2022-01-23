using System.Text.RegularExpressions;

namespace RandoStats.Util
{
    // PoolGroup is courtesy of Phenomenol: https://github.com/syyePhenomenol/HollowKnight.MapModS/blob/08f64d3454d8cdbc62773d44d20e6fbec08a9055/MapModS/Data/PoolGroup.cs
    // These are the same as the "Groups" in RandomizerMod's data. See pools.json
    public enum PoolGroup
    {
        // groups defined by rando
        Dreamers,
        Skills,
        Charms,
        Keys,
        MaskShards,
        VesselFragments,
        CharmNotches,
        PaleOre,
        GeoChests,
        RancidEggs,
        Relics,
        WhisperingRoots,
        BossEssence,
        Grubs,
        Mimics,
        Maps,
        Stags,
        LifebloodCocoons,
        GrimmkinFlames,
        JournalEntries,
        GeoRocks,
        BossGeo,
        SoulTotems,
        LoreTablets,

        // stuff that we have custom handling for
        Shops,
        Other
    }

    public static class PoolExtensions
    {
        private static readonly Regex nameFormatter = new Regex(@"([^A-Z])(?=[A-Z])");

        public static string FriendlyName(this PoolGroup group)
        {
            return nameFormatter.Replace(group.ToString(), "$1 ");
        }
    }
}
