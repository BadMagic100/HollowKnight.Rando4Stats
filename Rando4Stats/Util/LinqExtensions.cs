using System;
using System.Collections.Generic;
using System.Linq;

namespace RandoStats.Util
{
    public static class LinqExtensions
    {
        public static IEnumerable<T> SideEffect<T>(this IEnumerable<T> ls, Action<T> sideEffect)
        {
            return ls.Select(x =>
            {
                sideEffect(x);
                return x;
            });
        }
    }
}
