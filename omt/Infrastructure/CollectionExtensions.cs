using System.Collections.Generic;
using System.Linq;

namespace omt.Infrastructure
{
    public static class CollectionExtensions
    {
        public static bool IsEmpty<T>(this IEnumerable<T> collection)
        {
            return collection == null || !collection.Any();
        }
    }
}