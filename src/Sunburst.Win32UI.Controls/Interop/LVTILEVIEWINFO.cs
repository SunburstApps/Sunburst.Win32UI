using System;
using Sunburst.Win32UI.Graphics;

#pragma warning disable 0649
namespace Sunburst.Win32UI.Interop
{
    public struct LVTILEVIEWINFO
    {
        public uint cbSize;
        public uint dwMask;
        public uint dwFlags;
        public Size sizeTile;
        public int cLines;
        public Rect rcLabelMargin;

        #region Mask Values
        public const uint LVTVIM_TILESIZE = 0x00000001;
        public const uint LVTVIM_COLUMNS = 0x00000002;
        public const uint LVTVIM_LABELMARGIN = 0x00000004;
        #endregion

        #region Flag Values
        public const uint LVTVIF_AUTOSIZE = 0x00000000;
        public const uint LVTVIF_FIXEDWIDTH = 0x00000001;
        public const uint LVTVIF_FIXEDHEIGHT = 0x00000002;
        public const uint LVTVIF_FIXEDSIZE = 0x00000003;
        public const uint LVTVIF_EXTENDED = 0x00000004;
        #endregion
    }

    public struct LVTILEINFO
    {
        public uint cbSize;
        public int iItem;
        public uint cColumns;
        public IntPtr puColumns;
        public IntPtr piColFmt;
    }
}
