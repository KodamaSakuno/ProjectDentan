using System.Runtime.CompilerServices;

namespace Moen
{
    public static class BooleanUtil
    {
        public static readonly object True = true;
        public static readonly object False = false;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object GetBoxed(bool rpBool) { return rpBool ? True : False; }
    }
}
