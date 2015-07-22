using System.Collections.Generic;
using System.Linq;

namespace Moen
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> rpSource, int rpChunkSize)
        {
            return rpSource.Select((r, i) => new { Index = i, Value = r })
                           .GroupBy(r => r.Index / rpChunkSize)
                           .Select(r => r.Select(rpValue => rpValue.Value));
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> rpEnumerable)
        {
            return rpEnumerable == null || !rpEnumerable.Any();
        }
    }
}
