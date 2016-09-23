using System;
using System.Runtime.InteropServices;

#pragma warning disable 0649
namespace Microsoft.Win32.UserInterface.Interop
{
    public struct LVCOLUMN
    {
        public uint mask;
        public int fmt;
        public int cxy;
        public IntPtr pszText;
        public int cchTextMax;
        public int iSubItem;
        public int iImage;
        public int iOrder;
        public int cxMin;
        public int cxDefault;
        public int cxIdeal;

        #region Mask Values
        public const uint LVCF_FMT = 0x0001;
        public const uint LVCF_WIDTH = 0x0002;
        public const uint LVCF_TEXT = 0x0004;
        public const uint LVCF_SUBITEM = 0x0008;
        public const uint LVCF_IMAGE = 0x0010;
        public const uint LVCF_ORDER = 0x0020;
        public const uint LVCF_MINWIDTH = 0x0040;
        public const uint LVCF_DEFAULTWIDTH = 0x0080;
        public const uint LVCF_IDEALWIDTH = 0x0100;
        #endregion

        #region Format Values
        public const int LVCFMT_LEFT = 0x0000;
        public const int LVCFMT_RIGHT = 0x0001;
        public const int LVCFMT_CENTER = 0x0002;
        public const int LVCFMT_JUSTIFYMASK = 0x0003;
        public const int LVCFMT_IMAGE = 0x0800;
        public const int LVCFMT_BITMAP_ON_RIGHT = 0x1000;
        public const int LVCFMT_COL_HAS_IMAGES = 0x8000;
        public const int LVCFMT_FIXED_WIDTH = 0x00100;  // Can't resize the column; same as HDF_FIXEDWIDTH
        public const int LVCFMT_NO_DPI_SCALE = 0x40000;  // If not set, CCM_DPISCALE will govern scaling up fixed width
        public const int LVCFMT_FIXED_RATIO = 0x80000;  // Width will augment with the row height
        public const int LVCFMT_LINE_BREAK = 0x100000; // Move to the top of the next list of columns
        public const int LVCFMT_FILL = 0x200000; // Fill the remainder of the tile area. Might have a title.
        public const int LVCFMT_WRAP = 0x400000; // This sub-item can be wrapped.
        public const int LVCFMT_NO_TITLE = 0x800000;  // This sub-item doesn't have an title.
        public const int LVCFMT_TILE_PLACEMENTMASK = (LVCFMT_LINE_BREAK | LVCFMT_FILL);
        public const int LVCFMT_SPLITBUTTON = 0x1000000; // Column is a split button; same as HDF_SPLITBUTTON
        #endregion
    }
}
