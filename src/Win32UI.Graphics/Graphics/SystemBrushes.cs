using System;
using Microsoft.Win32.UserInterface.Interop;

namespace Microsoft.Win32.UserInterface.Graphics
{
    /// <summary>
    /// Contains brushes corresponding to standard system colors.
    /// </summary>
    /// <remarks>
    /// Not all the constants supported by <see cref="NativeMethods.GetSysColorBrush(int)"/> will be
    /// exposed here. Most of these date back to Windows 95 and are not exactly applicable to today's
    /// window themes. Only the constants that still make sense will be honored with a property in SystemBrushes.
    /// </remarks>
    public sealed class SystemBrushes
    {
        private SystemBrushes()
        {
            // This class is not meant to be instantiated.
            throw new NotSupportedException();
        }

        public static NonOwnedBrush WindowBackground
        {
            get
            {
                const int COLOR_WINDOW = 5;
                return new NonOwnedBrush(NativeMethods.GetSysColorBrush(COLOR_WINDOW));
            }
        }

        public static NonOwnedBrush ControlBackground
        {
            get
            {
                const int COLOR_BTNFACE = 15;
                return new NonOwnedBrush(NativeMethods.GetSysColorBrush(COLOR_BTNFACE));
            }
        }

        public static NonOwnedBrush DisabledText
        {
            get
            {
                const int COLOR_GRAYTEXT = 17;
                return new NonOwnedBrush(NativeMethods.GetSysColorBrush(COLOR_GRAYTEXT));
            }
        }

        public static NonOwnedBrush Text
        {
            get
            {
                const int COLOR_WINDOWTEXT = 8;
                return new NonOwnedBrush(NativeMethods.GetSysColorBrush(COLOR_WINDOWTEXT));
            }
        }
    }
}
