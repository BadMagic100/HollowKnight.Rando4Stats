using ItemChanger;

namespace RandoStats.Stats
{
    /// <summary>
    /// Abstract class defining a randomizer statistic
    /// </summary>
    public abstract class RandomizerStatistic
    {
        /// <summary>
        /// Gets the label to display as the statistic header
        /// </summary>
        public abstract string GetLabel();
        /// <summary>
        /// Gets the content to display as the stat body
        /// </summary>
        public abstract string GetContent();
        /// <summary>
        /// Whether the statistic is able to be computed based on the current randomizer settings
        /// </summary>
        public abstract bool IsComputable { get; }
        /// <summary>
        /// Whether the statistic is eligible to be enabled based on the current randomizer settings
        /// </summary>
        public abstract bool IsEnabled { get; }

        /// <summary>
        /// Whether the stat must be computed in a long-lived context. Default is false; transient stats are preferred.
        /// </summary>
        public virtual bool IsLongLived { get; } = false;

        public virtual string StatNamespace { get => GetType().Name; }

        /// <summary>
        /// Called by the stat engine when stat computation should begin. Use this to reset/restore any needed state.
        /// </summary>
        public virtual void BeginCompute() { }

        /// <summary>
        /// Called by the stat engine for each rando placement. Use this to adjust computation state for your stats.
        /// </summary>
        /// <param name="placement">The placement to process</param>
        public virtual void HandlePlacement(AbstractPlacement placement) { }

        /// <summary>
        /// Called by the stat engine for each transition placement. Use this to adjust computation state for your stats.
        /// </summary>
        /// <param name="from">The source transition name</param>
        /// <param name="to">The targe transition name</param>
        public virtual void HandleTransition(string from, string to) { }

        /// <summary>
        /// Called by the stat engine when stat computation should end. Use this to finalize/cleanup/save any needed state.
        /// </summary>
        public virtual void FinalizeCompute() { }

        /// <summary>
        /// Called by the stat engine when stat formatter is about to run. Use this to request additional stats for the formatter.
        /// </summary>
        public abstract void RegisterFormatterStats();

        internal void OnLongLivedItemObtained(int _, ReadOnlyGiveEventArgs args)
        {
            HandlePlacement(args.Placement);
        }
    }
}
