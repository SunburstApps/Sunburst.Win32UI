using System;
using System.Collections.Generic;
using System.Linq;
using Sunburst.Win32UI.Graphics;
using Sunburst.Win32UI.Interop;

namespace Sunburst.Win32UI.CommonControls
{
    public class ToolbarItem
    {
        public ToolbarItem() { }

        internal ToolbarItem(TBBUTTON nativeStruct)
        {
            BitmapIndex = nativeStruct.iBitmap;
            Command = nativeStruct.idCommand;
            State = (ToolbarItemState)nativeStruct.fsState;
            Style = nativeStruct.fsStyle;
            UserData = nativeStruct.dwData;
            Text = nativeStruct.iString;
        }

        internal TBBUTTON ToNativeStruct()
        {
            TBBUTTON nativeStruct = new TBBUTTON();
            nativeStruct.iBitmap = BitmapIndex;
            nativeStruct.idCommand = Command;
            nativeStruct.fsState = (byte)State;
            nativeStruct.fsStyle = (byte)Style;
            nativeStruct.dwData = UserData;
            nativeStruct.iString = Text;
            return nativeStruct;
        }

        public int BitmapIndex { get; set; }
        public int Command { get; set; }
        public ToolbarItemState State { get; set; } = 0;
        public int Style { get; set; } = 0; // values from ToolbarStyles class
        public IntPtr UserData { get; set; } = IntPtr.Zero;
        public string Text { get; set; } = "";
    }

    public enum ToolbarItemState
    {
        // These must match the TBSTATE_* values in CommCtrl.h
        Checked = 0x01,
        Pressed = 0x02,
        Enabled = 0x04,
        Hidden = 0x08,
        Indeterminate = 0x10,
        IsWrapPoint = 0x20,
        HasEllipsis = 0x40,
        Market = 0x80
    }
}
