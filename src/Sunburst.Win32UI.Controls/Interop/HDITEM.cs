using System;
using System.Runtime.InteropServices;

namespace Sunburst.Win32UI.Interop
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct HDITEM
    {
        public int mask;
        public int cxy;
        public string pszText;
        public IntPtr hbm;
        public int cchTextMax;
        public int fmt;
        public IntPtr lParam;
        public int iImage;
        public int iOrder;
        public int filterType;
        public IntPtr pvFilter;
        public int state;

        public const int HDI_WIDTH = 0x0001;
        public const int HDI_HEIGHT = HDI_WIDTH;
        public const int HDI_TEXT = 0x0002;
        public const int HDI_FORMAT = 0x0004;
        public const int HDI_LPARAM = 0x0008;
        public const int HDI_BITMAP = 0x0010;
        public const int HDI_IMAGE = 0x0020;
        public const int HDI_DI_SETITEM = 0x0040;
        public const int HDI_ORDER = 0x0080;
        public const int HDI_FILTER = 0x0100;
        public const int HDI_STATE = 0x0200;

        public const int HDF_LEFT = 0x0000;
        public const int HDF_RIGHT = 0x0001;
        public const int HDF_CENTER = 0x0002;
        public const int HDF_JUSTIFYMASK = 0x0003;
        public const int HDF_RTLREADING = 0x0004;
        public const int HDF_BITMAP = 0x2000;
        public const int HDF_STRING = 0x4000;
        public const int HDF_OWNERDRAW = 0x8000;
        public const int HDF_IMAGE = 0x0800;
        public const int HDF_BITMAP_ON_RIGHT = 0x1000;
        public const int HDF_SORTUP = 0x0400;
        public const int HDF_SORTDOWN = 0x0200;
        public const int HDF_CHECKBOX = 0x0040;
        public const int HDF_CHECKED = 0x0080;
        public const int HDF_FIXEDWIDTH = 0x0100;
        public const int HDF_SPLITBUTTON = 0x1000000;

        public const int HDFT_ISSTRING = 0x0000; // HD_ITEM.pvFilter points to a HD_TEXTFILTER
        public const int HDFT_ISNUMBER = 0x0001; // HD_ITEM.pvFilter points to a INT
        public const int HDFT_ISDATE = 0x0002; // HD_ITEM.pvFilter points to a SYSTEMTIME structure
        public const int HDFT_HASNOVALUE = 0x8000; // clear the filter by setting this bit

        public const int HDIS_FOCUSED = 0x1;

        public void ClearFilterData()
        {
            // Don't call this method if you don't own the HDITEM! It will free data that most likely will be referenced later.4
            if (filterType == HDFT_ISSTRING || filterType == HDFT_ISDATE)
            {
                Marshal.FreeHGlobal(pvFilter);
                pvFilter = IntPtr.Zero;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode)]
    internal struct HD_TEXTFILTERW
    {
        public string pszText;
        public int cchTextMax;
    }
}
