using ItemChanger;
using ItemChanger.Tags;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RandoStats.Util
{
    public static class LinqExtensions
    {
        public static IEnumerable<AbstractPlacement> SelectValidPlacements(this IEnumerable<AbstractPlacement> placements)
        {
            foreach (AbstractPlacement placement in placements)
            {
                if (placement.Name == LocationNames.Start)
                {
                    continue;
                }

                if (placement.GetTag<CompletionWeightTag>() is not CompletionWeightTag t || t.Weight != 0)
                {
                    yield return placement;
                }
            }
        }

        public static IEnumerable<AbstractItem> SelectValidItems(this IEnumerable<AbstractItem> items)
        {
            foreach (AbstractItem item in items)
            {
                if (item.GetTag<CompletionWeightTag>() is not CompletionWeightTag t || t.Weight != 0)
                {
                    yield return item;
                }
            }
        }

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
