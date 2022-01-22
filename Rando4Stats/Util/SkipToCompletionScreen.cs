using GlobalEnums;
using System.Collections;
using UnityEngine;

namespace RandoStats.Util
{
    public static class SkipToCompletionScreen
    {
        public static void Start()
        {
            GameManager.instance.StartCoroutine(ForceEndScreen());
        }

        private static IEnumerator ForceEndScreen()
        {
            GameManager.instance.TimePasses();
            UIManager.instance.UIClosePauseMenu();
            GameManager.instance.cameraCtrl.FadeOut(CameraFadeType.LEVEL_TRANSITION);
            yield return new WaitForSecondsRealtime(0.5f);
            GameManager.instance.ChangeToScene("End_Game_Completion", "door1", 0);
            yield return new WaitWhile(() => GameManager.instance.IsInSceneTransition);
            Time.timeScale = 1f;
            GameManager.instance.FadeSceneIn();
            GameManager.instance.isPaused = false;
            // apparently possibly needed to allow the pause menu to set the timescale again, maybe not relevant because we're quitting out?
            TimeController.GenericTimeScale = 1f;

            HeroController.instance?.UnPause();
            MenuButtonList.ClearAllLastSelected();

            // reset audio to normal levels
            GameManager.instance.actorSnapshotUnpaused.TransitionTo(0f);
            GameManager.instance.ui.AudioGoToGameplay(.2f);
        }
    }
}
