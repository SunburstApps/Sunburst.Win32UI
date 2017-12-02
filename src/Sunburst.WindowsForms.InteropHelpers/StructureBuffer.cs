using System;
using System.Runtime.InteropServices;

namespace Sunburst.WindowsForms.Interop
{
    public sealed class StructureBuffer<TStruct> : IDisposable
    {
        public StructureBuffer()
        {
            Handle = Marshal.AllocHGlobal(Size);
            if (Handle == IntPtr.Zero) throw new InvalidOperationException("Could not allocate HGlobal");
        }

        public bool DeleteOldStructure { get; set; } = false;

        public TStruct Value
        {
            get
            {
                if (Handle == IntPtr.Zero) throw new InvalidOperationException("Cannot get the value of a StructureBuffer that has been disposed");

                TStruct retval = Activator.CreateInstance<TStruct>();
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
