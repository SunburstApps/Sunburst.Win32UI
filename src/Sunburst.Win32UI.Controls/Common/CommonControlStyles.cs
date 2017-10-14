using System;

namespace Sunburst.Win32UI.CommonControls
{
    public static class CommonControlStyles
    {
        #region Button Styles
        public const int BS_PUSHBUTTON = 0x00000000;
        public const int BS_DEFPUSHBUTTON = 0x00000001;
        public const int BS_CHECKBOX = 0x00000002;
        public const int BS_AUTOCHECKBOX = 0x00000003;
        public const int BS_RADIOBUTTON = 0x00000004;
        public const int BS_3STATE = 0x00000005;
        public const int BS_AUTO3STATE = 0x00000006;
        public const int BS_GROUPBOX = 0x00000007;
        public const int BS_USERBUTTON = 0x00000008;
        public const int BS_AUTORADIOBUTTON = 0x00000009;
        public const int BS_PUSHBOX = 0x0000000A;
        public const int BS_OWNERDRAW = 0x0000000B;
        public const int BS_TYPEMASK = 0x0000000F;
        public const int BS_LEFTTEXT = 0x00000020;
        public const int BS_TEXT = 0x00000000;
        public const int BS_ICON = 0x00000040;
        public const int BS_BITMAP = 0x00000080;
        public const int BS_LEFT = 0x00000100;
        public const int BS_RIGHT = 0x00000200;
        public const int BS_CENTER = 0x00000300;
        public const int BS_TOP = 0x00000400;
        public const int BS_BOTTOM = 0x00000800;
        public const int BS_VCENTER = 0x00000C00;
        public const int BS_PUSHLIKE = 0x00001000;
        public const int BS_MULTILINE = 0x00002000;
        public const int BS_NOTIFY = 0x00004000;
        public const int BS_FLAT = 0x00008000;
        public const int BS_RIGHTBUTTON = BS_LEFTTEXT;
        public const int BS_SPLITBUTTON = 0x0000000C;
        public const int BS_DEFSPLITBUTTON = 0x0000000D;
        public const int BS_COMMANDLINK = 0x0000000E;
        public const int BS_DEFCOMMANDLINK = 0x0000000F;
        #endregion

        #region TextBox Styles
        public const int ES_LEFT = 0x0000;
        public const int ES_CENTER = 0x0001;
        public const int ES_RIGHT = 0x0002;
        public const int ES_MULTILINE = 0x0004;
        public const int ES_UPPERCASE = 0x0008;
        public const int ES_LOWERCASE = 0x0010;
        public const int ES_PASSWORD = 0x0020;
        public const int ES_AUTOVSCROLL = 0x0040;
        public const int ES_AUTOHSCROLL = 0x0080;
        public const int ES_NOHIDESEL = 0x0100;
        public const int ES_OEMCONVERT = 0x0400;
        public const int ES_READONLY = 0x0800;
        public const int ES_WANTRETURN = 0x1000;
        public const int ES_NUMBER = 0x2000;
        #endregion

        #region Label Styles
        public const int SS_LEFT = 0x00000000;
        public const int SS_CENTER = 0x00000001;
        public const int SS_RIGHT = 0x00000002;
        public const int SS_ICON = 0x00000003;
        public const int SS_BLACKRECT = 0x00000004;
        public const int SS_GRAYRECT = 0x00000005;
        public const int SS_WHITERECT = 0x00000006;
        public const int SS_BLACKFRAME = 0x00000007;
        public const int SS_GRAYFRAME = 0x00000008;
        public const int SS_WHITEFRAME = 0x00000009;
        public const int SS_USERITEM = 0x0000000A;
        public const int SS_SIMPLE = 0x0000000B;
        public const int SS_LEFTNOWORDWRAP = 0x0000000C;
        public const int SS_OWNERDRAW = 0x0000000D;
        public const int SS_BITMAP = 0x0000000E;
        public const int SS_ENHMETAFILE = 0x0000000F;
        public const int SS_ETCHEDHORZ = 0x00000010;
        public const int SS_ETCHEDVERT = 0x00000011;
        public const int SS_ETCHEDFRAME = 0x00000012;
        public const int SS_TYPEMASK = 0x0000001F;
        public const int SS_REALSIZECONTROL = 0x00000040;
        public const int SS_NOPREFIX = 0x00000080;
        public const int SS_NOTIFY = 0x00000100;
        public const int SS_CENTERIMAGE = 0x00000200;
        public const int SS_RIGHTJUST = 0x00000400;
        public const int SS_REALSIZEIMAGE = 0x00000800;
        public const int SS_SUNKEN = 0x00001000;
        public const int SS_EDITCONTROL = 0x00002000;
        public const int SS_ENDELLIPSIS = 0x00004000;
        public const int SS_PATHELLIPSIS = 0x00008000;
        public const int SS_WORDELLIPSIS = 0x0000C000;
        public const int SS_ELLIPSISMASK = 0x0000C000;
        #endregion

        #region ListBox Styles
        public const int LBS_NOTIFY = 0x0001;
        public const int LBS_SORT = 0x0002;
        public const int LBS_NOREDRAW = 0x0004;
        public const int LBS_MULTIPLESEL = 0x0008;
        public const int LBS_OWNERDRAWFIXED = 0x0010;
        public const int LBS_OWNERDRAWVARIABLE = 0x0020;
        public const int LBS_HASSTRINGS = 0x0040;
        public const int LBS_USETABSTOPS = 0x0080;
        public const int LBS_NOINTEGRALHEIGHT = 0x0100;
        public const int LBS_MULTICOLUMN = 0x0200;
        public const int LBS_WANTKEYBOARDINPUT = 0x0400;
        public const int LBS_EXTENDEDSEL = 0x0800;
        public const int LBS_DISABLENOSCROLL = 0x1000;
        public const int LBS_NODATA = 0x2000;
        public const int LBS_NOSEL = 0x4000;
        public const int LBS_COMBOBOX = 0x8000;

        public const int LBS_STANDARD = (LBS_NOTIFY | LBS_SORT | WindowStyles.WS_VSCROLL | WindowStyles.WS_BORDER);
        #endregion

        #region ComboBox Styles
        public const int CBS_SIMPLE = 0x0001;
        public const int CBS_DROPDOWN = 0x0002;
        public const int CBS_DROPDOWNLIST = 0x0003;
        public const int CBS_OWNERDRAWFIXED = 0x0010;
        public const int CBS_OWNERDRAWVARIABLE = 0x0020;
        public const int CBS_AUTOHSCROLL = 0x0040;
        public const int CBS_OEMCONVERT = 0x0080;
        public const int CBS_SORT = 0x0100;
        public const int CBS_HASSTRINGS = 0x0200;
        public const int CBS_NOINTEGRALHEIGHT = 0x0400;
        public const int CBS_DISABLENOSCROLL = 0x0800;
        public const int CBS_UPPERCASE = 0x2000;
        public const int CBS_LOWERCASE = 0x4000;
        #endregion
    }
}
