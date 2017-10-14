using System;
using System.Runtime.InteropServices;

namespace Sunburst.Win32UI.Interop
{
    public sealed class HGlobal : IDisposable
    {
        public static HGlobal WithStringUni(string value)
        {
            IntPtr ptr = (value != null) ? Marshal.StringToHGlobalUni(value) : IntPtr.Zero;
            return new HGlobal(ptr, (value.Length + 1) * Marshal.SystemDefaultCharSize);
        }

        public static HGlobal WithStringAnsi(string value)
        {
            IntPtr ptr = (value != null) ? Marshal.StringToHGlobalAnsi(value) : IntPtr.Zero;
            return new HGlobal(ptr, value.Length + 1);
        }

        public HGlobal(int byteSize) : this(Marshal.AllocHGlobal(byteSize), byteSize) { }

        private HGlobal(IntPtr ptr, int size)
        {
            Handle = ptr;
            Size = size;
        }

        public IntPtr Handle { get; private set; }

        public void Dispose()
        {
            lock (this)
            {
                if (Handle != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(Handle);
                    Handle = IntPtr.Zero;
                }
            }
        }

        public int Size { get; private set; }

        public void Resize(int newSize)
        {
            Handle = Marshal.ReAllocHGlobal(Handle, (IntPtr)newSize);
            Size = newSize;
        }
    }
}
