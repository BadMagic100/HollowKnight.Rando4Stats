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

        public static IEnumerable<T> AppendIf<T>(this IEnumerable<T> ls, bool condition, T element)
        {
            if (condition)
            {
                return ls.Append(element);
            }
            else
            {
                return ls;
            }
        }

        public static IEnumerable<T> ConcatIf<T>(this IEnumerable<T> ls, bool condition, IEnumerable<T> elements)
        {
            if (condition)
            {
                return ls.Concat(elements);
            }
            else
            {
                return ls;
            }
        }
    }
}
