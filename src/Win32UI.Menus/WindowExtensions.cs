using System;
using Microsoft.Win32.UserInterface.Interop;
using Microsoft.Win32.UserInterface.Menus;

namespace Microsoft.Win32.UserInterface
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
