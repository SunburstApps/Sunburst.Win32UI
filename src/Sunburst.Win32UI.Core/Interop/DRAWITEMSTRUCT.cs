using System;
using Sunburst.Win32UI.Graphics;

#pragma warning disable 0649
namespace Sunburst.Win32UI.Interop
{
    internal struct DRAWITEMSTRUCT
    {
        public uint CtlType;
        public uint CtrlID;
        public uint itemID;
        public uint itemAction;
        public uint itemState;
        public IntPtr hwndItem;
        public IntPtr hDC;
        public Rect rcItem;
        public IntPtr itemData;
    }

    internal struct MEASUREITEMSTRUCT
    {
        public uint CtlType;
        public uint CtlID;
        public uint itemID;
        public uint itemWidth;
        public uint itemHeight;
        public IntPtr itemData;
    }

    internal struct COMPAREITEMSTRUCT
    {
        public uint CtlType;
        public uint CtlID;
        public IntPtr hwndItem;
        public uint itemID1;
        public IntPtr itemData1;
        public uint itemID2;
        public IntPtr itemData2;
    }

    internal struct DELETEITEMSTRUCT
    {
        public uint CtlType;
        public uint CtlID;
        public uint itemID;
        public IntPtr hwndItem;
        public IntPtr itemData;
    }
}
