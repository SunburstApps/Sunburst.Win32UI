using System;

namespace Sunburst.Win32UI.CommonControls
{
    public static class TrackBarStyles
    {
        public const int TBS_AUTOTICKS          = 0x0001;
        public const int TBS_VERT               = 0x0002;
        public const int TBS_HORZ               = 0x0000;
        public const int TBS_TOP                = 0x0004;
        public const int TBS_BOTTOM             = 0x0000;
        public const int TBS_LEFT               = 0x0004;
        public const int TBS_RIGHT              = 0x0000;
        public const int TBS_BOTH               = 0x0008;
        public const int TBS_NOTICKS            = 0x0010;
        public const int TBS_ENABLESELRANGE     = 0x0020;
        public const int TBS_FIXEDLENGTH        = 0x0040;
        public const int TBS_NOTHUMB            = 0x0080;
        public const int TBS_TOOLTIPS           = 0x0100;
        public const int TBS_REVERSED           = 0x0200;  // Accessibility hint: the smaller number (usually the min value) means "high" and the larger number (usually the max value) means "low"
        public const int TBS_DOWNISLEFT         = 0x0400;  // Down=Left and Up=Right (default is Down=Right and Up=Left)
        public const int TBS_NOTIFYBEFOREMOVE   = 0x0800;  // Trackbar should notify parent before repositioning the slider due to user action (enables snapping)
        public const int TBS_TRANSPARENTBKGND   = 0x1000;  // Background is painted by the parent via WM_PRINTCLIENT
    } 
}
