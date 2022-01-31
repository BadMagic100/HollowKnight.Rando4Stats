using System;

namespace RandoStats.Stats
{
    public abstract class PercentageStatistic : RandomizerStatistic
    {
        public string Label { get; private set; }

        protected int ObtainedSum { get; set; } = 0;
        protected int TotalSum { get; set; } = 0;

        protected virtual string StatNamespace { get => GetType().Name; }

        public PercentageStatistic(string label)
        {
            Label = label;
        }

        private (int obtained, int total, int percent, string content) ComposeStats()
        {
            int obtained = ObtainedSum;
            int total = TotalSum;
            double rawPercent = (double)obtained / total * 100;
            int percent = (int)Math.Floor(rawPercent);
            string content = $"{percent}% ({obtained}/{total})";

            return (obtained, total, percent, content);
        }

        public override string GetContent()
        {
            return ComposeStats().content;
        }

        public override string GetLabel()
        {
            return Label;
        }

        /// <summary>
        /// Called by the stat engine when computation is about to start. Override to add additional handling if needed
        /// (such as restoring persistent state). By default, resets obtained and total sums to 0.
        /// </summary>
        public override void BeginCompute()
        {
            ObtainedSum = 0;
            TotalSum = 0;
        }

        public override void RegisterFormatterStats()
        {
            (int obtained, int total, int percent, string content) = ComposeStats();

            StatFormatRegistry.SetStat($"{StatNamespace}:{StatFormatRegistry.STAT_FULL}", content);
            StatFormatRegistry.SetStat($"{StatNamespace}:{StatFormatRegistry.STAT_PERCENT}", $"{percent}%");
            StatFormatRegistry.SetStat($"{StatNamespace}:{StatFormatRegistry.STAT_OBTAINED}", obtained.ToString());
            StatFormatRegistry.SetStat($"{StatNamespace}:{StatFormatRegistry.STAT_TOTAL}", total.ToString());
            StatFormatRegistry.SetStat($"{StatNamespace}:{StatFormatRegistry.STAT_FRACTION}", $"{obtained}/{total}");
        }
    }
}
