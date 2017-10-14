using System;

#pragma warning disable 0649
namespace Sunburst.Win32UI.Interop
{
    public struct LVGROUP
    {
        public uint cbSize;
        public uint mask;
        public IntPtr pszHeader;
        public int cchHeader;
        public IntPtr pszFooter;
        public int cchFooter;
        public int iGroupId;
        public uint stateMask;
        public uint state;
        public uint uAlign;
        public IntPtr pszSubtitle;
        public uint cchSubtitle;
        public IntPtr pszTask;
        public uint cchTask;
        public IntPtr pszDescriptionTop;
        public uint cchDescriptionTop;
        public IntPtr pszDescriptionBottom;
        public uint cchDescriptionBottom;
        public int iTitleImage;
        public int iExtendedImage;
        public int iFirstItem; // readonly
        public IntPtr cItems; // readonly
        // These two are not supported according to MSDN.
        private IntPtr pszSubsetTitle; // NULL if group is not subset
        private uint cchSubsetTitle;

        #region Mask Flags
        public const uint LVGF_NONE = 0x00000000;
        public const uint LVGF_HEADER = 0x00000001;
        public const uint LVGF_FOOTER = 0x00000002;
        public const uint LVGF_STATE = 0x00000004;
        public const uint LVGF_ALIGN = 0x00000008;
        public const uint LVGF_GROUPID = 0x00000010;
        public const uint LVGF_SUBTITLE = 0x00000100;
        public const uint LVGF_TASK = 0x00000200;
        public const uint LVGF_DESCRIPTIONTOP = 0x00000400;
        public const uint LVGF_DESCRIPTIONBOTTOM = 0x00000800;
        public const uint LVGF_TITLEIMAGE = 0x00001000;
        public const uint LVGF_EXTENDEDIMAGE = 0x00002000;
        public const uint LVGF_ITEMS = 0x00004000;
        public const uint LVGF_SUBSET = 0x00008000;
        public const uint LVGF_SUBSETITEMS = 0x00010000;
        #endregion

        #region State Flags
        public const uint LVGS_COLLAPSED = 0x00000001;
        public const uint LVGS_NORMAL = 0x00000000;
        public const uint LVGS_HIDDEN = 0x00000002;
        public const uint LVGS_NOHEADER = 0x00000004;
        public const uint LVGS_COLLAPSIBLE = 0x00000008;
        public const uint LVGS_FOCUSED = 0x00000010;
        public const uint LVGS_SELECTED = 0x00000020;
        public const uint LVGS_SUBSETED = 0x00000040;
        public const uint LVGS_SUBSETLINKFOCUSED = 0x00000080;
        #endregion

        #region Alignment Flags
        public const uint LVGA_HEADER_LEFT = 0x00000001;
        public const uint LVGA_HEADER_CENTER = 0x00000002;
        public const uint LVGA_HEADER_RIGHT = 0x00000004;  // Don't forget to validate exclusivity
        public const uint LVGA_FOOTER_LEFT = 0x00000008;
        public const uint LVGA_FOOTER_CENTER = 0x00000010;
        public const uint LVGA_FOOTER_RIGHT = 0x00000020;  // Don't forget to validate exclusivity
        #endregion
    }

    public struct LVGROUPMETRICS
    {
        public uint cbSize;
        public uint mask;
        public uint Left;
        public uint Top;
        public uint Right;
        public uint Bottom;
        public uint crLeft;
        public uint crTop;
        public uint crRight;
        public uint crBottom;
        public uint crHeader;
        public uint crFooter;

        #region Mask Flags
        public const uint LVGMF_NONE = 0x00000000;
        public const uint LVGMF_BORDERSIZE = 0x00000001;
        public const uint LVGMF_BORDERCOLOR = 0x00000002;
        public const uint LVGMF_TEXTCOLOR = 0x00000004;
        #endregion
    }
}
