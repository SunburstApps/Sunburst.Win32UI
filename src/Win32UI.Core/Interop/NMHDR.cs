using System;

namespace Microsoft.Win32.UserInterface.Interop
{
    public struct NMHDR
    {
        public IntPtr hWndFrom;
        public ulong idFrom;
        public uint code;
    }
}
