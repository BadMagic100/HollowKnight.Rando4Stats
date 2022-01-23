using Modding;
using RandoStats.GUI;
using System.Collections;
using Rando = RandomizerMod.RandomizerMod;

namespace RandoStats
{
    public class RandoStats : Mod
    {
        internal static RandoStats? Instance { get; private set; }
        private const string END_GAME_COMPLETION = "End_Game_Completion";

        public override string GetVersion() => GetType().Assembly.GetName().Version.ToString();

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
            orig(self);
        }

        private IEnumerator OnSaveClosed(On.QuitToMenu.orig_Start orig, QuitToMenu self)
        {
            PauseUI.DestroyLayout();
            return orig(self);
        }

        private void OnCompletionStart(On.GameCompletionScreen.orig_Start orig, GameCompletionScreen self)
        {
            PauseUI.DestroyLayout();
            RecentItemsInterop.ToggleDisplay(false);
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
    }
}