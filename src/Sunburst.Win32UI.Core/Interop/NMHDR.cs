using System;

namespace Sunburst.Win32UI.Interop
{
    public struct NMHDR
    {
        public IntPtr hWndFrom;
        public ulong idFrom;
        public uint code;
    }
}
