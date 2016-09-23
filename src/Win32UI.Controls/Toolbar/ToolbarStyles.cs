namespace Microsoft.Win32.UserInterface.CommonControls
{
    public static class ToolbarStyles
    {
        public const int TBSTYLE_BUTTON = 0x0000; // obsolete; use BTNS_BUTTON instead
        public const int TBSTYLE_SEP = 0x0001; // obsolete; use BTNS_SEP instead
        public const int TBSTYLE_CHECK = 0x0002; // obsolete; use BTNS_CHECK instead
        public const int TBSTYLE_GROUP = 0x0004; // obsolete; use BTNS_GROUP instead
        public const int TBSTYLE_CHECKGROUP = (TBSTYLE_GROUP | TBSTYLE_CHECK);     // obsolete; use BTNS_CHECKGROUP instead
        public const int TBSTYLE_DROPDOWN = 0x0008;  // obsolete; use BTNS_DROPDOWN instead
        public const int TBSTYLE_AUTOSIZE = 0x0010;  // obsolete; use BTNS_AUTOSIZE instead
        public const int TBSTYLE_NOPREFIX = 0x0020;  // obsolete; use BTNS_NOPREFIX instead
        public const int TBSTYLE_TOOLTIPS = 0x0100;
        public const int TBSTYLE_WRAPABLE = 0x0200;
        public const int TBSTYLE_ALTDRAG = 0x0400;
        public const int TBSTYLE_FLAT = 0x0800;
        public const int TBSTYLE_LIST = 0x1000;
        public const int TBSTYLE_CUSTOMERASE = 0x2000;
        public const int TBSTYLE_REGISTERDROP = 0x4000;
        public const int TBSTYLE_TRANSPARENT = 0x8000;
        public const int TBSTYLE_EX_DRAWDDARROWS = 0x00000001;
        public const int BTNS_BUTTON = TBSTYLE_BUTTON;  // 0x0000
        public const int BTNS_SEP = TBSTYLE_SEP;  // 0x0001
        public const int BTNS_CHECK = TBSTYLE_CHECK;  // 0x0002
        public const int BTNS_GROUP = TBSTYLE_GROUP;  // 0x0004
        public const int BTNS_CHECKGROUP = TBSTYLE_CHECKGROUP;  // (TBSTYLE_GROUP | TBSTYLE_CHECK)
        public const int BTNS_DROPDOWN = TBSTYLE_DROPDOWN;  // 0x0008
        public const int BTNS_AUTOSIZE = TBSTYLE_AUTOSIZE;  // 0x0010; automatically calculate the cx of the button
        public const int BTNS_NOPREFIX = TBSTYLE_NOPREFIX;  // 0x0020; this button should not have accel prefix
        public const int BTNS_SHOWTEXT = 0x0040;  // ignored unless TBSTYLE_EX_MIXEDBUTTONS is set
        public const int BTNS_WHOLEDROPDOWN = 0x0080; // draw drop-down arrow, but without split arrow section
        public const int TBSTYLE_EX_MIXEDBUTTONS = 0x00000008;
        public const int TBSTYLE_EX_HIDECLIPPEDBUTTONS = 0x00000010;  // don't show partially obscured buttons
        public const int TBSTYLE_EX_DOUBLEBUFFER = 0x00000080; // Double Buffer the toolbar
    }
}
