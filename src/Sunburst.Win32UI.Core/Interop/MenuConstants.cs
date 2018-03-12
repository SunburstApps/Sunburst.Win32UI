using System;

namespace Sunburst.Win32UI.Interop
{
    internal static class MenuConstants
    {
        public const int MF_BYCOMMAND = 0x0;
        public const int MF_BYPOSITION = 0x400;

        public const uint MIIM_BITMAP = 0x80;
        public const uint MIIM_CHECKMARKS = 0x8;
        public const uint MIIM_DATA = 0x20;
        public const uint MIIM_FTYPE = 0x100;
        public const uint MIIM_ID = 0x20;
        public const uint MIIM_STATE = 0x1;
        public const uint MIIM_STRING = 0x40;
        public const uint MIIM_SUBMENU = 0x4;

        public const uint MFT_MENUBARBREAK = 0x20;
        public const uint MFT_MENUBREAK = 0x40;
        public const uint MFT_OWNERDRAW = 0x100;
        public const uint MFT_RADIOCHECK = 0x200;
        public const uint MFT_RIGHTJUSTIFY = 0x4000;
        public const uint MFT_RIGHTORDER = 0x2000;
        public const uint MFT_SEPARATOR = 0x800;

        public const uint MFS_CHECKED = 0x8;
        public const uint MFS_DEFAULT = 0x1000;
        public const uint MFS_DISABLED = 0x3;
        public const uint MFS_ENABLED = 0x0;
        public const uint MFS_UNCHECKED = 0x0;
    }
}
