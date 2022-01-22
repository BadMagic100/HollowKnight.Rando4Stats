namespace RandoStats.Stats
{
    public interface IRandomizerStatistic
    {
        public string GetLabel();
        public string GetContent();
        public bool IsEnabled { get; }
    }
}
