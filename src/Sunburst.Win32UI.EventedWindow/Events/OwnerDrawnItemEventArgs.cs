using System;
using System.ComponentModel;
using Sunburst.Win32UI.Graphics;
using Sunburst.Win32UI.Interop;

namespace Sunburst.Win32UI.Events
{
    public sealed class MeasureOwnerDrawnItemEventArgs : HandledEventArgs
    {
        internal MeasureOwnerDrawnItemEventArgs(MEASUREITEMSTRUCT nativeStruct)
        {
            ItemType = (OwnerDrawnItemType)nativeStruct.CtlType;
            ControlID = nativeStruct.CtlID;
            ItemID = nativeStruct.itemID;
            ItemSize = new Size(nativeStruct.itemWidth, nativeStruct.itemHeight);
            ItemData = nativeStruct.itemData;
        }

        public OwnerDrawnItemType ItemType { get; private set; }
        public int ControlID { get; private set; }
        public int ItemID { get; private set; }
        public Size ItemSize { get; set; }
        public IntPtr ItemData { get; private set; }
    }

    public sealed class PaintOwnerDrawnItemEventArgs : HandledEventArgs
    {
        internal PaintOwnerDrawnItemEventArgs(DRAWITEMSTRUCT nativeStruct)
        {
            ItemType = (OwnerDrawnItemType)nativeStruct.CtlType;
            ControlID = nativeStruct.CtlID;
            ItemID = nativeStruct.itemID;
            Action = (OwnerDrawnItemAction)nativeStruct.itemAction;
            Flags = (OwnerDrawnItemFlags)nativeStruct.itemState;
            ItemWindow = (nativeStruct.hWndItem != IntPtr.Zero) ? new Window(nativeStruct.hWndItem) : null;
            GraphicsContext = new NonOwnedGraphicsContext(nativeStruct.hDC);
            DrawingRect = new Rect() { left = nativeStruct.rcItemLeft, top = nativeStruct.rcItemTop, right = nativeStruct.rcItemRight, bottom = nativeStruct.rcItemBottom };
            ItemData = nativeStruct.itemData;
        }

        public OwnerDrawnItemType ItemType { get; private set; }
        public int ControlID { get; private set; }
        public int ItemID { get; private set; }
        public OwnerDrawnItemAction Action { get; private set; }
        public OwnerDrawnItemFlags Flags { get; private set; }

        public Window ItemWindow { get; private set; }
        public NonOwnedGraphicsContext GraphicsContext { get; private set; }
        public Rect DrawingRect { get; private set; }
        public IntPtr ItemData { get; private set; }
    }

    [Flags]
    public enum OwnerDrawnItemFlags
    {
        // These values must match the ODS_* values in winuser.h
        Selected = 0x1,
        Grayed = 0x2,
        Disabled = 0x4,
        Checked = 0x8,
        Focused = 0x10,
        Default = 0x20,
        ComboBoxEdit = 0x40,
        InactiveWindow = 0x80,
        HotLight = 0x80,
        IgnoreAccelerators = 0x100,
        NoFocusRect = 0x200
    }

    [Flags]
    public enum OwnerDrawnItemType
    {
        // These values must match the ODT_* values in winuser.h
        Menu = 1,
        ListBox = 2,
        ComboBox = 3,
        Button = 4,
        Static = 5
    }

    [Flags]
    public enum OwnerDrawnItemAction
    {
        // These values must match the ODA_* values in winuser.h
        DrawnEntireItem = 0x1,
        Selected = 0x2,
        Focused = 0x4
    }
}
