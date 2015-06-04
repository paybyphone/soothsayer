using System;
using System.Collections.Generic;
using System.Linq;

namespace soothsayer.Infrastructure
{
    public static class CollectionExtensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
        {
            return collection == null || !collection.Any();
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey> (this IEnumerable<TSource> collection, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> keys = new HashSet<TKey>();

            return collection.Where(item => keys.Add(keySelector(item)));
        }
    }
}
