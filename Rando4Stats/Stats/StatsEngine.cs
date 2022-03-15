using ItemChanger;
using ItemChanger.Internal;
using RandomizerMod.IC;
using RandomizerMod.RC;
using System.Collections.Generic;
using System.Linq;

namespace RandoStats.Stats
{
    public static class StatsEngine
    {
        private static readonly HashSet<RandomizerStatistic> transientStats = new();
        private static readonly HashSet<RandomizerStatistic> longLivedStats = new();

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
        public static void Hook(RandomizerStatistic stat)
        {
            if (stat.IsLongLived)
            {
                longLivedStats.Add(stat);
            }
            else
            {
                transientStats.Add(stat);
            }
        }

        public static IEnumerable<AbstractPlacement> GetEligiblePlacements()
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
                if (!stat.IsComputable)
                    continue;
                stat.BeginCompute();
                RandoItemTag.AfterRandoItemGive += stat.OnLongLivedItemObtained;
                TrackerUpdate.OnTransitionVisited += stat.HandleTransition;
            }
        }

        /// <summary>
        /// Finalizes computing long-lived stats (i.e. when a save is exited)
        /// </summary>
        public static void FinalizeComputeLongLived()
        {
            foreach (RandomizerStatistic stat in longLivedStats)
            {
                if (!stat.IsComputable)
                    continue;
                RandoItemTag.AfterRandoItemGive -= stat.OnLongLivedItemObtained;
                TrackerUpdate.OnTransitionVisited -= stat.HandleTransition;
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
                if (!stat.IsComputable)
                    continue;

                stat.BeginCompute();
                foreach (AbstractPlacement placement in GetEligiblePlacements())
                {
                    stat.HandlePlacement(placement);
                }
                foreach (TransitionPlacement transition in RandomizerMod.RandomizerMod.RS.Context.transitionPlacements ?? Enumerable.Empty<TransitionPlacement>())
                {
                    stat.HandleTransition(transition.Source.Name, transition.Target.Name);
                }
                stat.FinalizeCompute();
            }
        }

        public static void UpdateStatFormatRegistry()
        {
            foreach (RandomizerStatistic stat in transientStats.Concat(longLivedStats))
            {
                if (stat.IsComputable && stat.IsEnabled)
                {
                    stat.RegisterFormatterStats();
                }
            }
        }
    }
}
