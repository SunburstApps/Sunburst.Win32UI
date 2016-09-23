using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.UserInterface.Graphics;

namespace Microsoft.Win32.UserInterface.Interop
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct LVITEM
    {
        public uint mask;
        public int iItem;
        public int iSubItem;
        public uint state;
        public uint stateMask;
        public IntPtr lpszText;
        public int cchTextMax;
        public int iImage;
        public IntPtr lParam;
        public int iIndent;
        public int iGroupId;
        public uint cColumns;
        public IntPtr puColumns;
        public IntPtr piColFmt;
        public int iGroup;

        #region Mask Flags
        public const uint LVIF_TEXT = 0x00000001;
        public const uint LVIF_IMAGE = 0x00000002;
        public const uint LVIF_PARAM = 0x00000004;
        public const uint LVIF_STATE = 0x00000008;
        public const uint LVIF_INDENT = 0x00000010;
        public const uint LVIF_NORECOMPUTE = 0x00000800;
        public const uint LVIF_GROUPID = 0x00000100;
        public const uint LVIF_COLUMNS = 0x00000200;
        public const uint LVIF_COLFMT = 0x00010000;
        #endregion

        #region State Flags
        public const uint LVIS_FOCUSED = 0x0001;
        public const uint LVIS_SELECTED = 0x0002;
        public const uint LVIS_CUT = 0x0004;
        public const uint LVIS_DROPHILITED = 0x0008;
        public const uint LVIS_GLOW = 0x0010;
        public const uint LVIS_ACTIVATING = 0x0020;
        public const uint LVIS_OVERLAYMASK = 0x0F00;
        public const uint LVIS_STATEIMAGEMASK = 0xF000;
        #endregion
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct LVFINDINFO
    {
        public uint flags;
        public string psz;
        public IntPtr lParam;
        public Point pt;
        public uint vkDirection;

        public const uint LVFI_PARAM = 0x0001;
        public const uint LVFI_STRING = 0x0002;
        public const uint LVFI_SUBSTRING = 0x0004;  // Same as LVFI_PARTIAL
        public const uint LVFI_PARTIAL = 0x0008;
        public const uint LVFI_WRAP = 0x0020;
        public const uint LVFI_NEARESTXY = 0x0040;
    }
}
