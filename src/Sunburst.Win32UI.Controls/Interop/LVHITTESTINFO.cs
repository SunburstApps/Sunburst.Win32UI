using System;
using Sunburst.Win32UI.Graphics;

#pragma warning disable 0649
namespace Sunburst.Win32UI.Interop
{
    public struct LVHITTESTINFO
    {
        public Point pt;
        public uint flags;
        public int iItem;
        public int iSubItem;
        public int iGroup;

        #region Flags
        public const uint LVHT_NOWHERE = 0x00000001;
        public const uint LVHT_ONITEMICON = 0x00000002;
        public const uint LVHT_ONITEMLABEL = 0x00000004;
        public const uint LVHT_ONITEMSTATEICON = 0x00000008;
        public const uint LVHT_ONITEM = (LVHT_ONITEMICON | LVHT_ONITEMLABEL | LVHT_ONITEMSTATEICON);
        public const uint LVHT_ABOVE = 0x00000008;
        public const uint LVHT_BELOW = 0x00000010;
        public const uint LVHT_TORIGHT = 0x00000020;
        public const uint LVHT_TOLEFT = 0x00000040;
        public const uint LVHT_EX_GROUP_HEADER = 0x10000000;
        public const uint LVHT_EX_GROUP_FOOTER = 0x20000000;
        public const uint LVHT_EX_GROUP_COLLAPSE = 0x40000000;
        public const uint LVHT_EX_GROUP_BACKGROUND = 0x80000000;
        public const uint LVHT_EX_GROUP_STATEICON = 0x01000000;
        public const uint LVHT_EX_GROUP_SUBSETLINK = 0x02000000;
        public const uint LVHT_EX_GROUP = (LVHT_EX_GROUP_BACKGROUND | LVHT_EX_GROUP_COLLAPSE | LVHT_EX_GROUP_FOOTER | LVHT_EX_GROUP_HEADER | LVHT_EX_GROUP_STATEICON | LVHT_EX_GROUP_SUBSETLINK);
        public const uint LVHT_EX_ONCONTENTS = 0x04000000; // On item AND not on the background
        public const uint LVHT_EX_FOOTER = 0x08000000;
        #endregion
    }
}
