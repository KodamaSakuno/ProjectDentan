using System;

namespace Moen
{
    public static class OS
    {
        public static bool IsWindows { get { return Environment.OSVersion.Platform == PlatformID.Win32NT; } }
        public static bool Is64Bit { get { return IntPtr.Size == 8; } }
    }
}
