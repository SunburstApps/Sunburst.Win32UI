using System;
using Sunburst.Win32UI.Interop;

namespace Sunburst.Win32UI.Graphics
{
    /// <summary>
    /// Wraps a GDI <c>HBRUSH</c>.
    /// </summary>
    public class Brush : IDisposable
    {
        public static Brush CreateSolid(Color color)
        {
            return new Brush(NativeMethods.CreateSolidBrush(Color.ToWin32Color(color)));
        }

        public static Brush CreateHatch(Color color, BrushHatchStyle hatchStyle)
        {
            return new Brush(NativeMethods.CreateHatchBrush((int)hatchStyle, Color.ToWin32Color(color)));
        }

        public static Brush CreatePattern(Bitmap bmp)
        {
            return new Brush(NativeMethods.CreatePatternBrush(bmp.Handle));
        }

        public Brush(IntPtr ptr)
        {
            Handle = ptr;
        }

        public void Dispose()
        {
            NativeMethods.DeleteObject(Handle);
        }

        public IntPtr Handle { get; private set; }
    }
}
