using MagicUI.Core;
using MagicUI.Elements;
using RandomizerMod;
using RandoStats.Util;
using UnityEngine;
using Rando = RandomizerMod.RandomizerMod;

namespace RandoStats.GUI
{
    public static class PauseUI
    {
        private static LayoutRoot? persistentLayout;

        public static void BuildLayout()
        {
            // only enable on rando saves, and if we haven't set up the layout yet as a safeguard
            if (Rando.RS.Context != null && persistentLayout == null)
            {
                persistentLayout = new LayoutRoot(true, "SkipToCompletionLayout");
                persistentLayout.VisibilityCondition = GameManager.instance.IsGamePaused;

                persistentLayout.ListenForHotkey(KeyCode.C, () =>
                {
                    DestroyLayout();
                    SkipToCompletionScreen.Start();
                }, ModifierKeys.Ctrl | ModifierKeys.Shift,
                () => GameManager.instance.IsGamePaused());

                Button warpButton = new(persistentLayout, "Warp Button")
                {
                    Content = Localization.Localize("View Stats"),
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
        }

        public static void DestroyLayout()
        {
            persistentLayout?.Destroy();
            persistentLayout = null;
        }
    }
}
