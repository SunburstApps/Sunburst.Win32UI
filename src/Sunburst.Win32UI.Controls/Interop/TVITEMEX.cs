using System;
using System.Runtime.InteropServices;
using Sunburst.Win32UI.Graphics;

#pragma warning disable 0649
namespace Sunburst.Win32UI.Interop
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct TVITEMEX
    {
        public uint mask;
        public IntPtr hTreeItem;
        public uint state;
        public uint stateMask;
        public string pszText;
        public int cchTextMax;
        public int iImage;
        public int iSelectedImage;
        public int cChildren;
        public IntPtr lParam;
        public int iIntegral;
        public uint uStateEx;
        public IntPtr hWnd;
        public int iExpandedImage;

        #region Mask Values
        public const uint TVIF_TEXT = 0x0001;
        public const uint TVIF_IMAGE = 0x0002;
        public const uint TVIF_PARAM = 0x0004;
        public const uint TVIF_STATE = 0x0008;
        public const uint TVIF_HANDLE = 0x0010;
        public const uint TVIF_SELECTEDIMAGE = 0x0020;
        public const uint TVIF_CHILDREN = 0x0040;
        public const uint TVIF_INTEGRAL = 0x0080;
        public const uint TVIF_STATEEX = 0x0100;
        public const uint TVIF_EXPANDEDIMAGE = 0x0200;
        #endregion

        #region State Values
        public const uint TVIS_SELECTED = 0x0002;
        public const uint TVIS_CUT = 0x0004;
        public const uint TVIS_DROPHILITED = 0x0008;
        public const uint TVIS_BOLD = 0x0010;
        public const uint TVIS_EXPANDED = 0x0020;
        public const uint TVIS_EXPANDEDONCE = 0x0040;
        public const uint TVIS_EXPANDPARTIAL = 0x0080;
        public const uint TVIS_OVERLAYMASK = 0x0F00;
        public const uint TVIS_STATEIMAGEMASK = 0xF000;
        public const uint TVIS_USERMASK = 0xF000;
        #endregion

        #region Extended State Values
        public const uint TVIS_EX_FLAT = 0x0001;
        public const uint TVIS_EX_DISABLED = 0x0002;
        public const uint TVIS_EX_ALL = 0x0002;
        #endregion
    }

    internal struct TVINSERTSTRUCT
    {
        public IntPtr hParent;
        public IntPtr hInsertAfter;
        public TVITEMEX item;
    }

    internal struct TVHITTESTINFO
    {
        public Point pt;
        public uint flags;
        public IntPtr hItem;
    }
}
