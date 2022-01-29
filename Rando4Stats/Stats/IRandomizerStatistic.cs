namespace RandoStats.Stats
{
    /// <summary>
    /// Interface defining a randomizer statistic
    /// </summary>
    public interface IRandomizerStatistic
    {
        /// <summary>
        /// Gets the label to display as the statistic header
        /// </summary>
        public string GetLabel();
        /// <summary>
        /// Gets the content to display as the stat body. This is when the stats get computed, and if applicable, should be registered to <see cref="StatFormatRegistry"/>
        /// </summary>
        public string GetContent();
        /// <summary>
        /// Whether the statistic is eligible to be enabled based on the current randomizer settings
        /// </summary>
        public bool IsEnabled { get; }
    }
}
