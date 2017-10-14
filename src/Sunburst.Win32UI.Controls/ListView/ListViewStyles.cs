namespace Sunburst.Win32UI.CommonControls
{
    public static class ListViewStyles
    {
        public const int LVS_ICON = 0x0000;
        public const int LVS_REPORT = 0x0001;
        public const int LVS_SMALLICON = 0x0002;
        public const int LVS_LIST = 0x0003;
        public const int LVS_TYPEMASK = 0x0003;
        public const int LVS_SINGLESEL = 0x0004;
        public const int LVS_SHOWSELALWAYS = 0x0008;
        public const int LVS_SORTASCENDING = 0x0010;
        public const int LVS_SORTDESCENDING = 0x0020;
        public const int LVS_SHAREIMAGELISTS = 0x0040;
        public const int LVS_NOLABELWRAP = 0x0080;
        public const int LVS_AUTOARRANGE = 0x0100;
        public const int LVS_EDITLABELS = 0x0200;
        public const int LVS_OWNERDATA = 0x1000;
        public const int LVS_NOSCROLL = 0x2000;
        public const int LVS_ALIGNTOP = 0x0000;
        public const int LVS_ALIGNLEFT = 0x0800;
        public const int LVS_ALIGNMASK = 0x0c00;
        public const int LVS_OWNERDRAWFIXED = 0x0400;
        public const int LVS_NOCOLUMNHEADER = 0x4000;
        public const int LVS_NOSORTHEADER = 0x8000;

        public const int LVS_EX_GRIDLINES = 0x00000001;
        public const int LVS_EX_SUBITEMIMAGES = 0x00000002;
        public const int LVS_EX_CHECKBOXES = 0x00000004;
        public const int LVS_EX_TRACKSELECT = 0x00000008;
        public const int LVS_EX_HEADERDRAGDROP = 0x00000010;
        public const int LVS_EX_FULLROWSELECT = 0x00000020; // applies to report mode only
        public const int LVS_EX_ONECLICKACTIVATE = 0x00000040;
        public const int LVS_EX_TWOCLICKACTIVATE = 0x00000080;
        public const int LVS_EX_FLATSB = 0x00000100;
        public const int LVS_EX_REGIONAL = 0x00000200;
        public const int LVS_EX_INFOTIP = 0x00000400; // listview does InfoTips for you
        public const int LVS_EX_UNDERLINEHOT = 0x00000800;
        public const int LVS_EX_UNDERLINECOLD = 0x00001000;
        public const int LVS_EX_MULTIWORKAREAS = 0x00002000;
        public const int LVS_EX_LABELTIP = 0x00004000; // listview unfolds partly hidden labels if it does not have infotip text
        public const int LVS_EX_BORDERSELECT = 0x00008000; // border selection style instead of highlight
        public const int LVS_EX_DOUBLEBUFFER = 0x00010000;
        public const int LVS_EX_HIDELABELS = 0x00020000;
        public const int LVS_EX_SINGLEROW = 0x00040000;
        public const int LVS_EX_SNAPTOGRID = 0x00080000;  // Icons automatically snap to grid.
        public const int LVS_EX_SIMPLESELECT = 0x00100000;  // Also changes overlay rendering to top right for icon mode.
        public const int LVS_EX_JUSTIFYCOLUMNS = 0x00200000;  // Icons are lined up in columns that use up the whole view area.
        public const int LVS_EX_TRANSPARENTBKGND = 0x00400000;  // Background is painted by the parent via WM_PRINTCLIENT
        public const int LVS_EX_TRANSPARENTSHADOWTEXT = 0x00800000;  // Enable shadow text on transparent backgrounds only (useful with bitmaps)
        public const int LVS_EX_AUTOAUTOARRANGE = 0x01000000;  // Icons automatically arrange if no icon positions have been set
        public const int LVS_EX_HEADERINALLVIEWS = 0x02000000;  // Display column header in all view modes
        public const int LVS_EX_AUTOCHECKSELECT = 0x08000000;
        public const int LVS_EX_AUTOSIZECOLUMNS = 0x10000000;
        public const int LVS_EX_COLUMNSNAPPOINTS = 0x40000000;
        public const int LVS_EX_COLUMNOVERFLOW = unchecked((int)0x80000000);
    }
}
