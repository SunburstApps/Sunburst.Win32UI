using System;
using System.Runtime.InteropServices;

namespace Sunburst.Win32UI
{
    public static class MessageBox
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "MessageBoxW")]
        private static extern int NativeMessageBox(IntPtr hWnd, string lpText, string lpCaption, uint flags);

        public static MessageBoxButton Show(string text, string caption, MessageBoxFlags flags, Window parent = null)
        {
            IntPtr parentHandle = parent?.Handle ?? IntPtr.Zero;
            return (MessageBoxButton)NativeMessageBox(parentHandle, text, caption, (uint)flags);
        }
    }
}
