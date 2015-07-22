using System;
using System.Runtime.InteropServices;
using FILETIME = System.Runtime.InteropServices.ComTypes.FILETIME;

namespace Moen.SystemInterop
{
    public static partial class NativeStructs
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct SIZE
        {
            public int X;
            public int Y;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public int Width { get { return Right - Left; } }
            public int Height { get { return Bottom - Top; } }

            public RECT(int rpLeft, int rpTop, int rpRight, int rpBottom)
            {
                Left = rpLeft;
                Top = rpTop;
                Right = rpRight;
                Bottom = rpBottom;
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct MARGINS
        {
            public int Left;
            public int Right;
            public int Top;
            public int Bottom;

            public MARGINS(int rpUniformValue)
                : this(rpUniformValue, rpUniformValue, rpUniformValue, rpUniformValue) { }
            public MARGINS(double rpLeft, double rpTop, double rpRight, double rpBottom)
                : this((int)rpLeft, (int)rpTop, (int)rpRight, (int)rpBottom) { }
            public MARGINS(int rpLeft, int rpTop, int rpRight, int rpBottom)
            {
                Left = rpLeft;
                Top = rpTop;
                Right = rpRight;
                Bottom = rpBottom;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct INTERNET_CACHE_ENTRY_INFO
        {
            public uint dwStructSize;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string lpszSourceUrlName;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string lpszLocalFileName;
            public NativeEnums.CACHEENTRYTYPE CacheEntryType;
            public uint dwUseCount;
            public uint dwHitRate;
            public uint dwSizeLow;
            public uint dwSizeHigh;
            public FILETIME LastModifiedTime;
            public FILETIME ExpireTime;
            public FILETIME LastAccessTime;
            public FILETIME LastSyncTime;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string lpHeaderInfo;
            public uint dwHeaderInfoSize;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string lpszFileExtension;
            public IntPtr dwReserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct INTERNET_PROXY_INFO
        {
            public int dwAccessType;
            public IntPtr proxy;
            public IntPtr proxyBypass;
        }
    }
}
