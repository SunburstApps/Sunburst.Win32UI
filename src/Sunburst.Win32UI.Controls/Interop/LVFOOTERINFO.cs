using System;
using System.Runtime.InteropServices;

#pragma warning disable 0619
namespace Sunburst.Win32UI.Interop
{
    internal struct LVFOOTERINFO
    {
        public uint mask;
        // These two are not supported for public use (MSDN).
        private IntPtr pszText;
        private int cchTextMax;
        public int cItems;

        public const uint LVFF_ITEMCOUNT = 1;
    }

    public struct LVFOOTERITEM
    {
        public uint mask;
        public int iItem;
        public IntPtr pszText;
        public int cchTextMax;
        public uint state;
        public uint stateMask;

        public const uint LVFIF_TEXT = 0x00000001;
        public const uint LVFIF_STATE = 0x00000002;
        public const uint LVFIS_FOCUSED = 0x0001;
    }
}
