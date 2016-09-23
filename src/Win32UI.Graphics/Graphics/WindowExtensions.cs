using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Win32.UserInterface.Graphics
{
    public static class WindowExtensions
    {
        public static NonOwnedIcon GetLargeIcon(this Window window)
        {
            const uint WM_GETICON = 0x007F;
            return new NonOwnedIcon(window.SendMessage(WM_GETICON, (IntPtr)1, IntPtr.Zero));
        }

        public static void SetLargeIcon(this Window window, NonOwnedIcon icon)
        {
            const uint WM_SETICON = 0x0080;
            window.SendMessage(WM_SETICON, (IntPtr)1, icon.Handle);
        }

        public static NonOwnedIcon GetSmallIcon(this Window window)
        {
            const uint WM_GETICON = 0x007F;
            return new NonOwnedIcon(window.SendMessage(WM_GETICON, (IntPtr)0, IntPtr.Zero));
        }

        public static void SetSmallIcon(this Window window, NonOwnedIcon icon)
        {
            const uint WM_SETICON = 0x0080;
            window.SendMessage(WM_SETICON, (IntPtr)0, icon.Handle);
        }

        public static NonOwnedFont GetFont(this Window window)
        {
            const uint WM_GETFONT = 0x0031;
            return new NonOwnedFont(window.SendMessage(WM_GETFONT, IntPtr.Zero, IntPtr.Zero));
        }

        public static void SetFont(this Window window, NonOwnedFont font)
        {
            const uint WM_SETFONT = 0x0030;
            window.SendMessage(WM_SETFONT, font.Handle, (IntPtr)1);
        }
    }
}
