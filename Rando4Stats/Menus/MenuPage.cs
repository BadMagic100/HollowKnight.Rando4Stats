using Satchel.BetterMenus;

namespace RandoStats.Menus
{
    public abstract class MenuPage
    {
        private Menu? menuRef;
        public Menu Menu => menuRef ??= ConstructMenu();

        protected abstract Menu ConstructMenu();
    }
}
