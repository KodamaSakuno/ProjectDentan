using System;
using System.Runtime.InteropServices;

namespace Moen.SystemInterop
{
    public class ComPointer<T> : IDisposable
        where T : class
    {
        IntPtr r_Pointer;
        T r_Object;
        public T Object { get { return r_Object; } }

        bool r_IsDisposed;
        public bool IsDisposed { get { return r_IsDisposed; } }

        public ComPointer(IntPtr rpPointer)
        {
            r_Pointer = rpPointer;
            r_Object = (T)Marshal.GetTypedObjectForIUnknown(rpPointer, typeof(T));
        }

        public void Dispose()
        {
            if (!r_IsDisposed)
            {
                Marshal.Release(r_Pointer);
                r_Object = null;
                r_IsDisposed = true;
            }
        }
    }
}
