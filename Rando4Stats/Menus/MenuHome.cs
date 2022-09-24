using FStats;
using Modding.Utils;
using RandoStats.Settings;
using Satchel.BetterMenus;
using System;
using System.Linq;
using System.Reflection;

namespace RandoStats.Menus
{
    internal class MenuHome : MenuPage
    {
        protected override Menu ConstructMenu() => new("RandoStats", typeof(RandoStatsGlobalSettings).Assembly.GetTypesSafely()
            .Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(StatController)))
            .Where(t => Attribute.IsDefined(t, typeof(MenuNameAttribute)))
            .Select(t =>
            {
                MenuNameAttribute menuName = t.GetCustomAttribute<MenuNameAttribute>();
                return Blueprints.NavigateToMenu(menuName.Name, $"Settings for the {menuName.Name} stat",
                    () => new StatSettingsPage(menuName.Name, t).Menu.GetMenuScreen(Menu.menuScreen));
            }).ToArray());
    }
}
