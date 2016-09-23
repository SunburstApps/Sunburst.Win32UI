using System;
using Microsoft.Win32.UserInterface.Interop;

namespace Microsoft.Win32.UserInterface.Menus
{
    public class Menu : NonOwnedMenu, IDisposable
    {
        public void CreateMenuBar()
        {
            Handle = NativeMethods.CreateMenu();
        }

        public void CreatePopupMenu()
        {
            Handle = NativeMethods.CreatePopupMenu();
        }

        public void Dispose()
        {
            if (Handle != IntPtr.Zero)
            {
                NativeMethods.DestroyMenu(Handle);
                Handle = IntPtr.Zero;
            }
        }
    }
}
