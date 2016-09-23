using System;

namespace Microsoft.Win32.UserInterface.Interop
{
    internal static class StandardMenuBitmaps
    {
        public static readonly IntPtr HBMMENU_CALLBACK = (IntPtr)(-1);
        public static readonly IntPtr HBMMENU_MBAR_CLOSE = (IntPtr)5;
        public static readonly IntPtr HBMMENU_MBAR_CLOSE_D = (IntPtr)6;
        public static readonly IntPtr HBMMENU_MBAR_MINIMIZE = (IntPtr)3;
        public static readonly IntPtr HBMMENU_MBAR_MINIMIZE_D = (IntPtr)7;
        public static readonly IntPtr HBMMENU_MBAR_RESTORE = (IntPtr)2;
        public static readonly IntPtr HBMMENU_POPUP_CLOSE = (IntPtr)8;
        public static readonly IntPtr HBMMENU_POPUP_MAXIMIZE = (IntPtr)10;
        public static readonly IntPtr HBMMENU_POPUP_MINIMIZE = (IntPtr)11;
        public static readonly IntPtr HBMMENU_POPUP_RESTORE = (IntPtr)9;
        public static readonly IntPtr HBMMENU_SYSTEM = (IntPtr)1;
    }
}
