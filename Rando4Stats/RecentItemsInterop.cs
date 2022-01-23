using MonoMod.ModInterop;
using System;

namespace RandoStats
{
    internal static class RecentItemsInterop
    {
        [ModImportName("RecentItems")]
        private static class RecentItemsImport
        {
            public static Action<bool>? ToggleDisplay = null;
        }
        static RecentItemsInterop()
        {
            typeof(RecentItemsImport).ModInterop();
        }

        public static void ToggleDisplay(bool show)
        {
            RecentItemsImport.ToggleDisplay?.Invoke(show);
        }
    }
}
