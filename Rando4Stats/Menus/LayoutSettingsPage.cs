using MonoMod.Utils;
using RandoStats.Settings;
using RandoStats.Util;
using Satchel.BetterMenus;
using System;
using System.Linq;

namespace RandoStats.Menus
{
    public class LayoutSettingsPage : MenuPage
    {
        private readonly string name;
        private readonly StatLayoutSettings settingsInstance;

        public LayoutSettingsPage(string name, StatLayoutSettings settingsInstance)
        {
            this.name = name;
            this.settingsInstance = settingsInstance;
        }

        protected override Menu ConstructMenu() => new($"{name} Settings", new Element[]
        {
            new TextPanel("Positioning Settings", fontSize: 50),
            new HorizontalOption("Position", "The corner of the screen to place the stat in",
                Enum.GetNames(typeof(StatPosition)).Select(x => x.SpacedPascalCase()).ToArray(),
                (val) => settingsInstance.Position = (StatPosition)val,
                () => (int)settingsInstance.Position),
            new CustomSlider("Sort Order",
                (val) => settingsInstance.Order = (int)val,
                () => settingsInstance.Order, 0, 50, true)
        }
        .AppendIf(settingsInstance.EnabledSubcategories.Any(), new TextPanel("Subcategory Settings", fontSize: 50))
        .ConcatIf(settingsInstance.EnabledSubcategories.Any(), settingsInstance.EnabledSubcategories.Keys.Select(key =>
        {
            string subcategoryName = key.SpacedPascalCase();
            return new HorizontalOption(subcategoryName, $"Whether to subcategorize this stat {subcategoryName.ToLower()}",
                new string[] { "On", "Off" },
                (val) => settingsInstance.EnabledSubcategories[key] = (val == 0),
                () => (settingsInstance.EnabledSubcategories.TryGetValue(key, out bool val) && val) ? 0 : 1);
        }))
        .ToArray());
    }
}
