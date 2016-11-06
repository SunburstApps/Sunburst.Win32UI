using System;

#pragma warning disable 0649
namespace Microsoft.Win32.UserInterface.Interop
{
    internal struct DRAWITEMSTRUCT
    {
        public int CtlType;
        public int CtlID;
        public int itemID;
        public int itemAction;
        public int itemState;
        public IntPtr hWndItem;
        public IntPtr hDC;

        // Beginning of inline RECT struct (cannot use Rect type as a field, as it would be marshalled as a pointer)
        public int rcItemLeft;
        public int rcItemTop;
        public int rcItemRight;
        public int rcItemBottom;
        // End of inline RECT struct

        public IntPtr itemData;
    }

    internal struct MEASUREITEMSTRUCT
    {
        public int CtlType;
        public int CtlID;
        public int itemID;
        public int itemWidth;
        public int itemHeight;
        public IntPtr itemData;
    }
}
