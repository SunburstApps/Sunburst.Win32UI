using System;

#pragma warning disable 0649
namespace Sunburst.WindowsForms.Interop
{
    internal struct NMHDR
    {
        public IntPtr hWndFrom;
        public ulong idFrom;
        public uint code;
    }
}
