using System;
using Sunburst.Win32UI.Interop;
using Sunburst.Win32UI.Menus;

namespace Sunburst.Win32UI
{
    public static class WindowExtensions
    {
        public static NonOwnedMenu GetMenu(this Window window)
        {
            NonOwnedMenu menu = new NonOwnedMenu();
            menu.Handle = NativeMethods.GetMenu(window.Handle);
            return menu;
        }

        public static void SetMenu(this Window window, NonOwnedMenu menu)
        {
            NativeMethods.SetMenu(window.Handle, menu.Handle);
        }

        public static NonOwnedMenu GetSystemMenu(this Window window)
        {
            NonOwnedMenu menu = new NonOwnedMenu();
            menu.Handle = NativeMethods.GetSystemMenu(window.Handle, false);
            return menu;
        }

        public static void ResetSystemMenu(this Window window)
        {
            IntPtr unused = NativeMethods.GetSystemMenu(window.Handle, true);
        }
    }
}
