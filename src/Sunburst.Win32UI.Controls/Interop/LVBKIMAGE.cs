using System;
using System.Runtime.InteropServices;

namespace Sunburst.Win32UI.Interop
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct LVBKIMAGE
    {
        public ulong ulFlags;
        public IntPtr hbm;
        public IntPtr pszImage;
        public uint cchImageMax;
        public int xOffsetPercent;
        public int yOffsetPercent;

        public const uint LVBKIF_SOURCE_NONE = 0x00000000;
        public const uint LVBKIF_SOURCE_HBITMAP = 0x00000001;
        public const uint LVBKIF_SOURCE_URL = 0x00000002;
        public const uint LVBKIF_SOURCE_MASK = 0x00000003;
        public const uint LVBKIF_STYLE_NORMAL = 0x00000000;
        public const uint LVBKIF_STYLE_TILE = 0x00000010;
        public const uint LVBKIF_STYLE_MASK = 0x00000010;
        public const uint LVBKIF_FLAG_TILEOFFSET = 0x00000100;
        public const uint LVBKIF_TYPE_WATERMARK = 0x10000000;
        public const uint LVBKIF_FLAG_ALPHABLEND = 0x20000000;
    }
}
