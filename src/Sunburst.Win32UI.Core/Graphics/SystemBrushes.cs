using System;
using Sunburst.Win32UI.Interop;

namespace Sunburst.Win32UI.Graphics
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

        public static Brush WindowBackground
        {
            get
            {
                const int COLOR_WINDOW = 5;
                return new Brush(NativeMethods.GetSysColorBrush(COLOR_WINDOW));
            }
        }

        public static Brush ControlBackground
        {
            get
            {
                const int COLOR_BTNFACE = 15;
                return new Brush(NativeMethods.GetSysColorBrush(COLOR_BTNFACE));
            }
        }

        public static Brush DisabledText
        {
            get
            {
                const int COLOR_GRAYTEXT = 17;
                return new Brush(NativeMethods.GetSysColorBrush(COLOR_GRAYTEXT));
            }
        }

        public static Brush Text
        {
            get
            {
                const int COLOR_WINDOWTEXT = 8;
                return new Brush(NativeMethods.GetSysColorBrush(COLOR_WINDOWTEXT));
            }
        }
    }
}
