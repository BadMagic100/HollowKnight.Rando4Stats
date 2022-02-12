using Modding;
using System;
using System.Reflection;

namespace RandoStats.Interop
{
    public static class BenchwarpInterop
    {
        static object? bwGlobalSettings;
        static FieldInfo? bwGlobalSettings_ShowScene;
        static bool? CachedSceneNameVisSetting;

        static BenchwarpInterop()
        {
            if (ModHooks.GetMod("Benchwarp", true) is Mod bw)
            {
                bwGlobalSettings = bw.GetType().GetProperty("GS").GetValue(bw);
                Type? bwGlobalSettingsType = bwGlobalSettings?.GetType();
                bwGlobalSettings_ShowScene = bwGlobalSettingsType?.GetField("ShowScene");
                if (bwGlobalSettings == null || bwGlobalSettingsType == null || bwGlobalSettings_ShowScene == null)
                {
                    RandoStats.Instance?.LogError("Found benchwarp installed, but couldn't get necessary interop info");
                }
            }
        }

        public static void TempHideSceneName()
        {
            if (bwGlobalSettings != null && bwGlobalSettings_ShowScene != null)
            {
                CachedSceneNameVisSetting = (bool)bwGlobalSettings_ShowScene.GetValue(bwGlobalSettings);
                bwGlobalSettings_ShowScene.SetValue(bwGlobalSettings, false);
            }
        }

        public static void UnhideSceneName()
        {
            if (bwGlobalSettings != null && bwGlobalSettings_ShowScene != null && CachedSceneNameVisSetting == true)
            {
                CachedSceneNameVisSetting = null;
                bwGlobalSettings_ShowScene.SetValue(bwGlobalSettings, true);
            }
        }
    }
}
