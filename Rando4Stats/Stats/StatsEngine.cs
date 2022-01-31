using ItemChanger;
using ItemChanger.Internal;
using RandomizerMod.IC;
using System.Collections.Generic;
using System.Linq;

namespace RandoStats.Stats
{
    public static class StatsEngine
    {
        private static HashSet<RandomizerStatistic> transientStats = new();
        private static HashSet<RandomizerStatistic> longLivedStats = new();

        public static void Initialize()
        {
            transientStats.Clear();
            longLivedStats.Clear();
        }

        /// <summary>
        /// Hooks a stat into the stats engine. A transient stat begins calculation at the end of the game and saves no persistent state.
        /// This is the recommended pattern if at all possible, as it allows the stats mod to be installed mid-seed.
        /// </summary>
        /// <param name="stat">The stat to hook into the engine</param>
        /// <param name="transient">Whether the stat should be transient or long-lived. Default is true; this is recommended.</param>
        public static void Hook(RandomizerStatistic stat, bool transient)
        {
            if (transient)
            {
                transientStats.Add(stat);
            }
            else
            {
                longLivedStats.Add(stat);
            }
        }

        private static IEnumerable<AbstractPlacement> GetEligiblePlacements()
        {
            return Ref.Settings.GetPlacements()
                .Where(p => p.HasTag<RandoPlacementTag>());
        }

        /// <summary>
        /// Begins computing long-lived stats (i.e. once a save is loaded)
        /// </summary>
        public static void BeginComputeLongLived()
        {
            foreach (RandomizerStatistic stat in longLivedStats)
            {
                stat.BeginCompute();
                RandoPlacementTag.OnRandoPlacementVisitStateChanged += stat.OnLongLivedPlacementVisited;
            }
        }

        /// <summary>
        /// Finalizes computing long-lived stats (i.e. when a save is exited)
        /// </summary>
        public static void FinalizeComputeLongLived()
        {
            foreach (RandomizerStatistic stat in longLivedStats)
            {
                RandoPlacementTag.OnRandoPlacementVisitStateChanged -= stat.OnLongLivedPlacementVisited;
                stat.FinalizeCompute();
            }
        }

        /// <summary>
        /// Computes transient stats (i.e. on the completion screen)
        /// </summary>
        public static void ComputeTransient()
        {
            foreach (RandomizerStatistic stat in transientStats)
            {
                stat.BeginCompute();
                foreach (AbstractPlacement placement in GetEligiblePlacements())
                {
                    stat.HandlePlacement(placement);
                }
                stat.FinalizeCompute();
            }
        }

        public static void UpdateStatFormatRegistry()
        {
            foreach (RandomizerStatistic stat in transientStats.Concat(longLivedStats))
            {
                if (stat.IsEnabled) stat.RegisterFormatterStats();
            }
        }
    }
}
