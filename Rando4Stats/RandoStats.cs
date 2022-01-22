using Modding;

namespace RandoStats
{
    public class RandoStats : Mod
    {
        internal static RandoStats? Instance { get; private set; }

        public override string GetVersion() => GetType().Assembly.GetName().Version.ToString();

        public override void Initialize()
        {
            Log("Initializing");

            Instance = this;

            Log("Initialized");
        }
    }
}