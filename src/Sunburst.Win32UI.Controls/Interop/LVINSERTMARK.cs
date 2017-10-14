using System;

namespace Sunburst.Win32UI.Interop
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
