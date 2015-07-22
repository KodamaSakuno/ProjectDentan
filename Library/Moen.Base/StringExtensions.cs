using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Moen
{
    public static class StringExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNullOrEmpty(this string rpString)
        {
            return string.IsNullOrEmpty(rpString);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Join(this IEnumerable<string> rpStrings, string rpSeparator)
        {
            return string.Join(rpSeparator, rpStrings);
        }
    }
}
