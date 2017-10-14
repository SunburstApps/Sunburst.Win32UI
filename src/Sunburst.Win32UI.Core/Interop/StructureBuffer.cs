using System;
using System.Runtime.InteropServices;

namespace Sunburst.Win32UI.Interop
{
    public sealed class StructureBuffer<TStruct> : IDisposable where TStruct : struct
    {
        public StructureBuffer()
        {
            Handle = Marshal.AllocHGlobal(Size);
        }

        public bool DeleteOldStructure { get; set; } = false;

        public TStruct Value
        {
            get
            {
                if (Handle == IntPtr.Zero) throw new InvalidOperationException("Cannot get the value of a StructureBuffer that has been disposed");

                TStruct retval = new TStruct();
                Marshal.PtrToStructure(Handle, retval);
                return retval;
            }

            set
            {
                if (Handle == IntPtr.Zero) throw new InvalidOperationException("Cannot set the value of a StructureBuffer that has been disposed");
                Marshal.StructureToPtr(value, Handle, DeleteOldStructure);
            }
        }
        
        public IntPtr Handle { get; private set; }

        public int Size
        {
            get
            {
                return Marshal.SizeOf<TStruct>();
            }
        }

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
	}
}
