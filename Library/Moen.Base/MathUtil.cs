using System.Runtime.CompilerServices;

namespace Moen
{
    public static class MathUtil
    {
        public const double RadOf1Deg = 0.017453292519943295;
        public const double DegOf1Rad = 57.295779513082323;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double DegreesToRadians(double rpAngle)
        {
            return rpAngle * RadOf1Deg;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double RadiansToDegrees(double rpRadians)
        {
            return rpRadians * DegOf1Rad;
        }
    }
}
