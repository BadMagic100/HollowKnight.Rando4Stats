using Satchel.BetterMenus;

namespace RandoStats.Menus
{
    internal abstract class MenuPage
    {
        private Menu? menuRef;
        public Menu Menu => menuRef ??= ConstructMenu();

        protected abstract Menu ConstructMenu();
    }
}
