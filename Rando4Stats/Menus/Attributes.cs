using System;

namespace RandoStats.Menus
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class MenuNameAttribute : Attribute
    {
        public readonly string Name;
        public MenuNameAttribute(string name)
        {
            Name = name;
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class MenuSubpageAttribute : Attribute
    {
        public readonly string Name;
        public MenuSubpageAttribute(string name)
        {
            Name = name;
        }
    }
}
