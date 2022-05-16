using Modding;
using RandoStats.GUI;
using RandoStats.Interop;
using RandoStats.Menus;
using RandoStats.Settings;
using RandoStats.Stats;
using System;
using System.Collections;
using Rando = RandomizerMod.RandomizerMod;

namespace RandoStats
{
    public class RandoStats : Mod, IGlobalSettings<RandoStatsGlobalSettings>, ICustomMenuMod
    {
        internal static RandoStats? Instance { get; private set; }
        private const string END_GAME_COMPLETION = "End_Game_Completion";

        public override string GetVersion() => GetType().Assembly.GetName().Version.ToString();

        public RandoStatsGlobalSettings GlobalSettings { get; private set; } = new();
        private MenuHome? menuHome;

        // right now this is just checking rando enablement, but I think the naming makes sense as other conditions may apply at a future time
        private bool IsEnabled => Rando.RS.Context != null;

        public bool ToggleButtonInsideMenu => throw new NotImplementedException();

        public RandoStats() : base()
        {
            Instance = this;
        }

        public override void Initialize()
        {
            Log("Initializing");

            On.HeroController.Awake += OnSaveOpened;
            On.QuitToMenu.Start += OnSaveClosed;
            On.GameCompletionScreen.Start += OnCompletionStart;
            On.InputHandler.CutsceneInput += HandleCutsceneInput;

            Log("Initialized");
        }

        private void OnSaveOpened(On.HeroController.orig_Awake orig, HeroController self)
        {
            orig(self);
            if (IsEnabled)
            {
                try
                {
                    PauseUI.BuildLayout();
                    StatsEngine.Initialize();
                    StatLayoutHelper.ConstructLayoutFactories(GlobalSettings);
                    foreach (StatGroupLayoutFactory factory in StatLayoutHelper.LayoutFactories)
                    {
                        factory.HookStatsEngine();
                    }
                    StatsEngine.BeginComputeLongLived();
                }
                catch (Exception ex)
                {
                    LogError($"Unknown issue hooking RandoStats - {ex}");
                }
            }
        }

        private IEnumerator OnSaveClosed(On.QuitToMenu.orig_Start orig, QuitToMenu self)
        {
            if (IsEnabled)
            {
                try
                {
                    PauseUI.DestroyLayout();
                    BenchwarpInterop.UnhideSceneName();

                    StatsEngine.FinalizeComputeLongLived();
                }
                catch (Exception ex)
                {
                    LogError($"Unknown issue unhooking RandoStats - {ex}");
                }
            }
            return orig(self);
        }

        private void OnCompletionStart(On.GameCompletionScreen.orig_Start orig, GameCompletionScreen self)
        {
            orig(self);
            if (IsEnabled)
            {
                try
                {
                    PauseUI.DestroyLayout();
                    RecentItemsInterop.ToggleDisplay(false);
                    BenchwarpInterop.TempHideSceneName();
                    StatsEngine.ComputeTransient();
                    CompletionUI.BuildLayout();
                }
                catch (Exception ex)
                {
                    LogError($"Unknown error computing/displaying stats - {ex}");
                }
            }
        }

        private void HandleCutsceneInput(On.InputHandler.orig_CutsceneInput orig, InputHandler self)
        {
            string scene = GameManager.instance.GetSceneNameString();
            if (scene != END_GAME_COMPLETION || !IsEnabled || !CompletionUI.HandleInput())
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

        public MenuScreen GetMenuScreen(MenuScreen modListMenu, ModToggleDelegates? toggleDelegates)
        {
            menuHome ??= new();
            return menuHome.Menu.GetMenuScreen(modListMenu);
        }
    }
}