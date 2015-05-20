using System.Collections.Generic;
using System.Linq;

namespace soothsayer.Tests
{
    public static class EnumerableExtensions
    {
        public static void Enumerate<T>(this IEnumerable<T> enumerable)
        {
            enumerable.Count();
        }
    }
}