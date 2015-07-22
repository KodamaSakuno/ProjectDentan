using System;
using System.Threading;

namespace Moen
{
    public static class RandomUtil
    {
        static ThreadLocal<Random> r_InstanceOfCurrentThread = new ThreadLocal<Random>(() => new Random(GetSeedOfCurrentThread()));
        public static Random InstanceOfCurrentThread { get { return r_InstanceOfCurrentThread.Value; } }

        public static int GetSeedOfCurrentThread()
        {
            return Thread.CurrentThread.GetHashCode() ^ (int)DateTime.UtcNow.Ticks;
        }
    }
}
