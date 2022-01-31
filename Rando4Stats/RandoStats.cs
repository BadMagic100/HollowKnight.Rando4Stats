using Modding;
using RandoStats.GUI;
using RandoStats.Settings;
using RandoStats.Stats;
using System.Collections;
using Rando = RandomizerMod.RandomizerMod;

namespace RandoStats
{
    public class RandoStats : Mod, IGlobalSettings<RandoStatsGlobalSettings>
    {
        internal static RandoStats? Instance { get; private set; }
        private const string END_GAME_COMPLETION = "End_Game_Completion";

        public override string GetVersion() => GetType().Assembly.GetName().Version.ToString();

        public RandoStatsGlobalSettings GlobalSettings { get; private set; } = new();

        public override void Initialize()
        {
            Log("Initializing");

            Instance = this;
            On.HeroController.Awake += OnSaveOpened;
            On.QuitToMenu.Start += OnSaveClosed;
            On.GameCompletionScreen.Start += OnCompletionStart;
            On.InputHandler.CutsceneInput += HandleCutsceneInput;

            Log("Initialized");
        }

        private void OnSaveOpened(On.HeroController.orig_Awake orig, HeroController self)
        {
            PauseUI.BuildLayout();
            StatsEngine.Initialize();
            foreach (StatGroupLayoutFactory factory in GlobalSettings.LayoutFactories)
            {
                factory.HookStatsEngine();
            }
            StatsEngine.BeginComputeLongLived();
            orig(self);
        }

        private IEnumerator OnSaveClosed(On.QuitToMenu.orig_Start orig, QuitToMenu self)
        {
            PauseUI.DestroyLayout();
            StatsEngine.FinalizeComputeLongLived();
            return orig(self);
        }

        private void OnCompletionStart(On.GameCompletionScreen.orig_Start orig, GameCompletionScreen self)
        {
            PauseUI.DestroyLayout();
            RecentItemsInterop.ToggleDisplay(false);
            StatsEngine.ComputeTransient();
            CompletionUI.BuildLayout();
            orig(self);
        }

        private void HandleCutsceneInput(On.InputHandler.orig_CutsceneInput orig, InputHandler self)
        {
            string scene = GameManager.instance.GetSceneNameString();
            if (scene != END_GAME_COMPLETION || Rando.RS.Context == null || !CompletionUI.HandleInput())
            {
                // do the default if one of the following is true:
                // * We're in a different cutscene than the completion screen
                // * We're not playing rando
                // * The completion UI failed to handle the input
                orig(self);
            }
        }

        public void OnLoadGlobal(RandoStatsGlobalSettings s) => GlobalSettings = s;

        public RandoStatsGlobalSettings OnSaveGlobal() => GlobalSettings;
    }
}