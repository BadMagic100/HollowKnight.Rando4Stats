using MonoMod.Utils;
using RandoStats.Settings;
using Satchel.BetterMenus;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RandoStats.Menus
{
    internal class MenuHome : MenuPage
    {
        private static readonly IEnumerable<PropertyInfo> settingProperties = typeof(RandoStatsGlobalSettings)
            .GetProperties()
            .Where(x => x.PropertyType == typeof(StatLayoutSettings));

        protected override Menu ConstructMenu() => new("RandoStats", settingProperties
            .Select(prop => {
                string name = prop.Name.Replace("LayoutSettings", "").SpacedPascalCase();
                StatLayoutSettings settings = (StatLayoutSettings)prop.GetValue(RandoStats.Instance!.GlobalSettings);
                return Blueprints.NavigateToMenu(name, $"Settings for the {name} stat", 
                    () => new LayoutSettingsPage(name, settings).Menu.GetMenuScreen(Menu.menuScreen));
            }).ToArray());
    }
}
