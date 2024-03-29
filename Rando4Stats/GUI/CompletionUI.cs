﻿using MagicUI.Core;
using MagicUI.Elements;
using MagicUI.Graphics;
using Modding;
using RandomizerMod;
using RandoStats.Stats;
using RandoStats.Util;
using System.Linq;
using UnityEngine;
using Rando = RandomizerMod.RandomizerMod;

namespace RandoStats.GUI
{
    public static class CompletionUI
    {
        private static readonly Loggable log = ScopedLoggers.GetLogger();

        private static LayoutRoot? cutsceneLayout;

        private static float pressStartTime = 0;
        private static bool holdToSkipLock = false;

        private const float LENGTH_OF_PRESS_TO_SKIP = 1.5f;

        private static void CopyStats()
        {
            TextObject? clipboardPrompt = cutsceneLayout?.GetElement<TextObject>("Clipboard Prompt");
            StatFormatRegistry.GenerateBasicStats();
            GUIUtility.systemCopyBuffer = StatFormatRegistry.Format("$RACING_EXTENDED$");
            if (clipboardPrompt != null)
            {
                clipboardPrompt.Text = Localization.Localize("Copied!");
            }
        }

        private static bool AnyKeyExcept(params KeyCode[] keys)
        {
            return Input.anyKey && !keys.Any(Input.GetKey);
        }

        public static void BuildLayout()
        {
            if (Rando.RS.Context != null)
            {
                log.Log("Starting stat layout");

                holdToSkipLock = false;
                pressStartTime = 0;

                cutsceneLayout = new LayoutRoot(false, "Completion Layout");
                cutsceneLayout.ListenForHotkey(KeyCode.C, CopyStats, ModifierKeys.Ctrl);

                new TextObject(cutsceneLayout, "Clipboard Prompt")
                {
                    Text = Localization.Localize("Press Ctrl+C to copy completion"),
                    FontSize = 18,
                    Padding = new Padding(35, 0, 0, 125),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Bottom
                };

                new Image(cutsceneLayout, BuiltInSprites.CreateBox(), "Progress Bar")
                {
                    Padding = new Padding(0, 20),
                    Width = 0,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Bottom
                };

                log.Log("Finished stat layout");
            }
            else
            {
                log.Log("Not a randomizer save, skipping stats");
            }
        }

        /// <summary>
        /// Handles cutscene input
        /// </summary>
        /// <returns>Whether the input was successfully handled</returns>
        public static bool HandleInput()
        {
            if (holdToSkipLock)
            {
                return true;
            }

            Image? progressBar = cutsceneLayout?.GetElement<Image>("Progress Bar");

            // if this doesn't exist something has gone badly; fail out
            if (progressBar == null)
            {
                return false;
            }

            bool held = AnyKeyExcept(KeyCode.LeftControl, KeyCode.RightControl, KeyCode.LeftAlt, KeyCode.RightAlt)
                || InputHandler.Instance.gameController.AnyButton.IsPressed;

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
                    cutsceneLayout?.BeginFade(0, 0.5f);
                }
                float progressPercentage = (Time.time - pressStartTime) / LENGTH_OF_PRESS_TO_SKIP;
                progressBar.Width = UI.Screen.width * progressPercentage;
            }
            else
            {
                pressStartTime = 0;
                progressBar.Width = 0;
            }

            return true;
        }
    }
}
