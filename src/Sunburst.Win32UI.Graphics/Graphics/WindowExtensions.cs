using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sunburst.Win32UI.Graphics
{
    public static class WindowExtensions
    {
        public static NonOwnedIcon GetLargeIcon(this Control window)
        {
            const uint WM_GETICON = 0x007F;
            return new NonOwnedIcon(window.SendMessage(WM_GETICON, (IntPtr)1, IntPtr.Zero));
        }

        public static void SetLargeIcon(this Control window, NonOwnedIcon icon)
        {
            const uint WM_SETICON = 0x0080;
            window.SendMessage(WM_SETICON, (IntPtr)1, icon.Handle);
        }

        public static NonOwnedIcon GetSmallIcon(this Control window)
        {
            const uint WM_GETICON = 0x007F;
            return new NonOwnedIcon(window.SendMessage(WM_GETICON, (IntPtr)0, IntPtr.Zero));
        }

        public static void SetSmallIcon(this Control window, NonOwnedIcon icon)
        {
            const uint WM_SETICON = 0x0080;
            window.SendMessage(WM_SETICON, (IntPtr)0, icon.Handle);
        }

        public static Font GetFont(this Control window)
        {
            const uint WM_GETFONT = 0x0031;
            return new Font(window.SendMessage(WM_GETFONT, IntPtr.Zero, IntPtr.Zero));
        }

        public static void SetFont(this Control window, Font font)
        {
            const uint WM_SETFONT = 0x0030;
            window.SendMessage(WM_SETFONT, font.Handle, (IntPtr)1);
        }
    }
}
