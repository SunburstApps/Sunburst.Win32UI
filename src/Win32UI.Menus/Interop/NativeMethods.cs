using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.UserInterface.Graphics;

namespace Microsoft.Win32.UserInterface.Interop
{
    internal static class NativeMethods
    {
        [DllImport("user32.dll")]
        public static extern IntPtr CreateAcceleratorTableW([In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] AcceleratorTableEntry[] entries, int count);

        [DllImport("user32.dll")]
        public static extern IntPtr CreateMenu();

        [DllImport("user32.dll")]
        public static extern IntPtr CreatePopupMenu();

        [DllImport("user32.dll")]
        public static extern bool DestroyMenu(IntPtr hMenu);

        [DllImport("user32.dll")]
        public static extern bool DeleteMenu(IntPtr hMenu, int position, int flags);

        [DllImport("user32.dll")]
        public static extern bool ClientToScreen(IntPtr hWnd, ref Point pt);

        [DllImport("user32.dll")]
        public static extern bool TrackPopupMenuEx(IntPtr hMenu, uint flags, int x, int y, IntPtr hWnd, IntPtr parameterPtr);

        [DllImport("user32.dll")]
        public static extern bool InsertMenuItemW(IntPtr hMenu, uint item, bool byPosition, ref MENUITEMINFO info);

        [DllImport("user32.dll")]
        public static extern bool GetMenuItemInfoW(IntPtr hMenu, uint item, bool byPosition, ref MENUITEMINFO info);

        [DllImport("user32.dll")]
        public static extern bool SetMenuItemInfoW(IntPtr hMenu, uint item, bool byPosition, ref MENUITEMINFO info);

        [DllImport("user32.dll")]
        public static extern IntPtr GetMenu(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr SetMenu(IntPtr hWnd, IntPtr hMenu);

        [DllImport("user32.dll")]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool revert);
    }
}
