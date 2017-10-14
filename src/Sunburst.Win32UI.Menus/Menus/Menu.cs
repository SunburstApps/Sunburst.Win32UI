using System;
using Sunburst.Win32UI.Interop;

namespace Sunburst.Win32UI.Menus
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
