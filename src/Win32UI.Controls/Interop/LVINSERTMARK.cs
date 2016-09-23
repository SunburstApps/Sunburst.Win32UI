using System;

namespace Microsoft.Win32.UserInterface.Interop
{
    internal struct LVINSERTMARK
    {
        public uint cbSize;
        public uint dwFlags;
        public int iItem;
        public uint dwReserved;

        public const uint LVIM_AFTER = 1;
    }
}
