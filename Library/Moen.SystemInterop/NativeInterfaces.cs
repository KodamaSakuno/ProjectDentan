using System;
using System.Runtime.InteropServices;

namespace Moen.SystemInterop
{
    public static partial class NativeInterfaces
    {
        [Guid("00000000-0000-0000-c000-000000000046")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IUnknown
        {
            [PreserveSig]
            IntPtr QueryInterface(ref Guid riid, ref IntPtr pVoid);
            [PreserveSig]
            ulong AddRef();
            [PreserveSig]
            ulong Release();
        }
    }
}
