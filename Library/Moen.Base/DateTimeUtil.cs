using System;

namespace Moen
{
    public static class DateTimeUtil
    {
        public static readonly DateTimeOffset UnixEpoch = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);

        public static DateTimeOffset FromUnixTime(ulong rpValue)
        {
            return UnixEpoch.AddSeconds((double)rpValue);
        }

        public static ulong ToUnixTime(this DateTimeOffset rpDateTime)
        {
            return (ulong)rpDateTime.Subtract(UnixEpoch).TotalSeconds;
        }
    }
}
