using System;
using System.Runtime.InteropServices;

namespace Microsoft.Win32.UserInterface
{
    public static class MessageBox
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "MessageBoxW")]
        public static extern int NativeMessageBox(IntPtr hWnd, string lpText, string lpCaption, uint flags);

        public static MessageBoxButton Show(string text, string caption, MessageBoxFlags flags, Window parent = null)
        {
            IntPtr parentHandle = parent?.Handle ?? IntPtr.Zero;
            return (MessageBoxButton)NativeMessageBox(parentHandle, text, caption, (uint)flags);
        }
    }
}
