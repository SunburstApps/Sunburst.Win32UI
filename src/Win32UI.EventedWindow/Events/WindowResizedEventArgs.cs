using System;
using System.ComponentModel;
using Microsoft.Win32.UserInterface.Graphics;

namespace Microsoft.Win32.UserInterface.Events
{
    public sealed class WindowResizedEventArgs : HandledEventArgs
    {
        internal WindowResizedEventArgs(IntPtr wParam, IntPtr lParam)
        {
            Mode = (WindowResizeMode)(int)wParam;

            int sizeComposite = (int)lParam;
            NewSize = new Size(sizeComposite & 0xFFFF, (sizeComposite >> 16) & 0xFFFF);
        }

        public WindowResizedEventArgs(Size size, WindowResizeMode mode = WindowResizeMode.Restored)
        {
            NewSize = size;
            Mode = mode;
        }

        public WindowResizeMode Mode { get; private set; }
        public Size NewSize { get; private set; }
    }

    public sealed class WindowMovedEventArgs : HandledEventArgs
    {
        internal WindowMovedEventArgs(IntPtr lParam)
        {
            int posComposite = (int)lParam;
            NewPosition = new Point(posComposite & 0xFFFF, (posComposite >> 16) & 0xFFFF);
        }

        public WindowMovedEventArgs(Point pt)
        {
            NewPosition = pt;
        }

        public Point NewPosition { get; private set; }
    }

    public enum WindowResizeMode
    {
        Restored = 0,
        Minimized = 1,
        Maximized = 2,
        OtherWindowRestored = 4,
        OtherWindowMaximized = 5
    }
}
