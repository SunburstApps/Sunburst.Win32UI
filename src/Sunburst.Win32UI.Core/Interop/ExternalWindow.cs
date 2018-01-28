using System;
using System.Runtime.InteropServices;
using Sunburst.Win32UI.Graphics;

namespace Sunburst.Win32UI.Interop
{
    public sealed class ExternalWindow : IWin32Window
    {
        public ExternalWindow(IntPtr hWnd)
        {
            Handle = hWnd;
        }

        public IntPtr Handle { get; }

    }
}
