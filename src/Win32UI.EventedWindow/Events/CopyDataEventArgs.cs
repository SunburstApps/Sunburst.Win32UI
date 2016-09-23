using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.Win32.UserInterface.Events
{
    public sealed class CopyDataEventArgs : ResultHandledEventArgs
    {
        public CopyDataEventArgs(IntPtr ptr, int size)
        {
            List<byte> buffer = new List<byte>();
            for (int i = 0; i < size; i++)
            {
                buffer.Add(Marshal.ReadByte(ptr, i));
            }

            Data = buffer;
        }

        public CopyDataEventArgs(IReadOnlyList<byte> buffer)
        {
            Data = buffer;
        }

        public IReadOnlyList<byte> Data { get; private set; }
        public bool Result
        {
            get { return (int)ResultPointer == 1; }
            set { ResultPointer = (IntPtr)(value ? 1 : 0); }
        }
    }
}
