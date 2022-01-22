using MagicUI.Core;
using MagicUI.Elements;
using MagicUI.Graphics;
using Modding;
using RandoStats.Stats;
using RandoStats.Util;
using System.Collections;
using System.Linq;
using UnityEngine;
using Rando = RandomizerMod.RandomizerMod;

namespace RandoStats
{
    public class RandoStats : Mod
    {
        internal static RandoStats? Instance { get; private set; }

        private static LayoutRoot? persistentLayout;
        private static LayoutRoot? cutsceneLayout;
        private static readonly TextureLoader textureLoader = new(typeof(RandoStats).Assembly, "RandoStats.Resources");

        private float pressStartTime = 0;
        private bool holdToSkipLock = false;

        private const string END_GAME_COMPLETION = "End_Game_Completion";
        private const float LENGTH_OF_PRESS_TO_SKIP = 1.5f;

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
            // only enable on rando saves, and if we haven't set up the layout yet as a safeguard
            if (Rando.RS.Context != null && persistentLayout == null)
            {
                persistentLayout = new LayoutRoot(true, true, "SkipToCompletionLayout");

                persistentLayout.ListenForHotkey(KeyCode.C, () =>
                {
                    SkipToCompletionScreen.Start();
                    persistentLayout?.Destroy();
                    persistentLayout = null;
                }, ModifierKeys.Ctrl | ModifierKeys.Shift,
                () => GameManager.instance.IsGamePaused());

                Button warpButton = new(persistentLayout, "Warp Button")
                {
                    Content = "View Stats",
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Font = UI.TrajanBold,
                    FontSize = 12,
                    Margin = 20,
                    Padding = new Padding(15, 250)
                };
                warpButton.Click += (sender) =>
                {
                    sender.Enabled = false;
                    SkipToCompletionScreen.Start();
                };
            }
            orig(self);
        }

        private IEnumerator OnSaveClosed(On.QuitToMenu.orig_Start orig, QuitToMenu self)
        {
            persistentLayout?.Destroy();
            persistentLayout = null;
            return orig(self);
        }

        private void CopyStats()
        {
            TextObject? clipboardPrompt = cutsceneLayout?.GetElement<TextObject>("Clipboard Prompt");
            StatFormatRegistry.GenerateBasicStats();
            GUIUtility.systemCopyBuffer = StatFormatRegistry.Format("$RACING_SIMPLE$");
            if (clipboardPrompt != null)
            {
                clipboardPrompt.Text = "Copied!";
            }
        }

        private void OnCompletionStart(On.GameCompletionScreen.orig_Start orig, GameCompletionScreen self)
        {
            persistentLayout?.Destroy();
            persistentLayout = null;
            if (Rando.RS.Context != null)
            {
                Log("Starting layout step");

                cutsceneLayout = new LayoutRoot(false, false, "Completion Layout");
                cutsceneLayout.ListenForHotkey(KeyCode.C, CopyStats, ModifierKeys.Ctrl);

                new TextObject(cutsceneLayout, "Clipboard Prompt")
                {
                    Text = "Press Ctrl+C to copy completion",
                    FontSize = 18,
                    Padding = new Padding(40, 0, 0, 125),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Bottom
                };

                Texture2D progressTexture = textureLoader.GetTexture("Progress.png");
                Sprite progressSprite = Sprite.Create(progressTexture,
                    new Rect(0, 0, progressTexture.width, progressTexture.height),
                    new Vector2(0.5f, 0.5f)
                );
                new Image(cutsceneLayout, progressSprite, "Progress Bar")
                {
                    Padding = new Padding(0, 20),
                    Width = 0,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Bottom
                };

                Log("Finished setting up layout");
            }
            else
            {
                Log("Not a randomizer save, skipping stats");
            }
            orig(self);
        }

        private bool AnyKeyExcept(params KeyCode[] keys)
        {
            return Input.anyKey && !keys.Any(Input.GetKey);
        }

        private void HandleCutsceneInput(On.InputHandler.orig_CutsceneInput orig, InputHandler self)
        {
            string scene = GameManager.instance.GetSceneNameString();
            if (scene != END_GAME_COMPLETION || Rando.RS.Context == null)
            {
                // if we're in any other cutscene or we're not playing randomizer, just do the default behavior.
                orig(self);
            }
            else
            {
                if (holdToSkipLock) return;

                Image? progressBar = cutsceneLayout?.GetElement<Image>("Progress Bar");

                // if these don't exist something has gone badly; just do the default input behaviour instead
                if (progressBar == null)
                {
                    orig(self);
                    return;
                }

                bool held = AnyKeyExcept(KeyCode.LeftControl, KeyCode.RightControl, KeyCode.LeftAlt, KeyCode.RightAlt)
                    || self.gameController.AnyButton.IsPressed;

                // if ctrl is held, trigger on the frame where c pressed
                //if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.C))
                //{
                //    StatFormatRegistry.GenerateBasicStats();
                //    GUIUtility.systemCopyBuffer = StatFormatRegistry.Format(Settings.CompletionFormatString);
                //    clipboardPrompt.Text = "Copied!";
                //}

                if (held)
                {
                    if (pressStartTime <= float.Epsilon)
                    {
                        pressStartTime = Time.time;
                    }
                    else if (Time.time > pressStartTime + LENGTH_OF_PRESS_TO_SKIP)
                    {
                        // we've elapsed the designated time while held; we can now skip the cutscene.
                        // we should now further block the hold-to-skip behavior and animation until the next
                        // time we load into this scene.
                        holdToSkipLock = true;
                        GameManager.instance.SkipCutscene();
                        // fade our stuff out too
                        CanvasGroupLinearFade? fade = cutsceneLayout?.Canvas.AddComponent<CanvasGroupLinearFade>();
                        if (fade != null) 
                        {
                            fade.duration = 0.5f;
                        }
                    }
                    float progressPercentage = (Time.time - pressStartTime) / LENGTH_OF_PRESS_TO_SKIP;
                    progressBar.Width = UI.Screen.width * progressPercentage;
                }
                else
                {
                    pressStartTime = 0;
                    progressBar.Width = 0;
                }
            }
        }
    }
}