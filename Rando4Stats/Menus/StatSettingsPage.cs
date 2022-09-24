using RandoStats.Util;
using Satchel.BetterMenus;
using System;
using System.Linq;
using System.Reflection;

namespace RandoStats.Menus
{
    public class StatSettingsPage : MenuPage
    {
        private static readonly string[] Options = new string[] { "Disabled", "Enabled" };

        private readonly string name;
        private readonly Type controllerType;
        private readonly string[] subpages;

        public StatSettingsPage(string name, Type controllerType)
        {
            this.name = name;
            this.controllerType = controllerType;
            subpages = controllerType.GetCustomAttributes(typeof(MenuSubpageAttribute))
                .OfType<MenuSubpageAttribute>().Select(s => s.Name).ToArray();
        }

        protected override Menu ConstructMenu() => new($"{name} Settings", new Element[]
        {
            new HorizontalOption("Enabled", "Whether or not to enable this stat",
                Options,
                (val) => RandoStats.Instance!.GlobalSettings.DisplayStat(controllerType, val != 0),
                () => RandoStats.Instance!.GlobalSettings.ShouldDisplay(controllerType) ? 1 : 0),
        }
        .AppendIf(subpages.Any(), new TextPanel("Subcategory Settings", fontSize: 50))
        .ConcatIf(subpages.Any(), subpages.Select(subcategoryName =>
        {
            return new HorizontalOption(subcategoryName, $"Whether to subcategorize this stat {subcategoryName.ToLower()}",
                Options,
                (val) => RandoStats.Instance!.GlobalSettings.DisplayStatSubpage(controllerType, subcategoryName, val != 0),
                () => RandoStats.Instance!.GlobalSettings.ShouldDisplaySubpageIndependent(controllerType, subcategoryName) ? 1 : 0);
        }))
        .ToArray());
    }
}
