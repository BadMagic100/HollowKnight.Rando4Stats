namespace RandoStats.Stats
{
    public abstract class CounterStatistic : RandomizerStatistic
    {
        public string Label { get; private set; }

        protected int Counter { get; set; } = 0;

        public CounterStatistic(string label)
        {
            Label = label;
        }

        public override string GetLabel()
        {
            return Label;
        }

        public override string GetContent()
        {
            return Counter.ToString();
        }

        public override void BeginCompute()
        {
            Counter = 0;
        }

        public override void RegisterFormatterStats()
        {
            StatFormatRegistry.SetStat(StatNamespace, StatFormatRegistry.STAT_TOTAL, GetContent());
        }
    }
}
