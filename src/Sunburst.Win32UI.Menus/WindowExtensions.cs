using System;
using Sunburst.Win32UI.Interop;
using Sunburst.Win32UI.Menus;

namespace Sunburst.Win32UI
{
    public static class WindowExtensions
    {
        public static NonOwnedMenu GetMenu(this Control window)
        {
            NonOwnedMenu menu = new NonOwnedMenu();
            menu.Handle = NativeMethods.GetMenu(window.Handle);
            return menu;
        }

        public static void SetMenu(this Control window, NonOwnedMenu menu)
        {
            NativeMethods.SetMenu(window.Handle, menu.Handle);
        }

        public static NonOwnedMenu GetSystemMenu(this Control window)
        {
            NonOwnedMenu menu = new NonOwnedMenu();
            menu.Handle = NativeMethods.GetSystemMenu(window.Handle, false);
            return menu;
        }

        public static void ResetSystemMenu(this Control window)
        {
            IntPtr unused = NativeMethods.GetSystemMenu(window.Handle, true);
        }
    }
}
