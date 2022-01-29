using System;

namespace RandoStats.Stats
{
    public abstract class PercentageStatistic : IRandomizerStatistic
    {
        public abstract bool IsEnabled { get; }

        public string Label { get; private set; }

        protected virtual string StatNamespace { get => GetType().Name; }

        public PercentageStatistic(string label)
        {
            Label = label;
        }

        protected abstract int GetObtained();
        protected abstract int GetTotal();

        public string GetContent()
        {
            // these computations may be expensive (in fact, they likely are). Just do them once.
            int obtained = GetObtained();
            int total = GetTotal();
            double rawPercent = (double)obtained / total * 100;
            int percent = (int)Math.Floor(rawPercent);
            string content = $"{percent}% ({obtained}/{total})";

            StatFormatRegistry.SetStat($"{StatNamespace}:{StatFormatRegistry.STAT_FULL}", content);
            StatFormatRegistry.SetStat($"{StatNamespace}:{StatFormatRegistry.STAT_PERCENT}", $"{percent}%");
            StatFormatRegistry.SetStat($"{StatNamespace}:{StatFormatRegistry.STAT_OBTAINED}", obtained.ToString());
            StatFormatRegistry.SetStat($"{StatNamespace}:{StatFormatRegistry.STAT_TOTAL}", total.ToString());
            StatFormatRegistry.SetStat($"{StatNamespace}:{StatFormatRegistry.STAT_FRACTION}", $"{obtained}/{total}");

            return content;
        }

        public string GetLabel()
        {
            return Label;
        }
    }
}
