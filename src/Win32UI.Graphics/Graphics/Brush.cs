using System;
using Microsoft.Win32.UserInterface.Interop;

namespace Microsoft.Win32.UserInterface.Graphics
{
    /// <summary>
    /// Wraps a Win32 <c>HBRUSH</c>.
    /// </summary>
    public class Brush : NonOwnedBrush, IDisposable
    {
        public static Brush CreateSolid(Color color)
        {
            return new Brush(NativeMethods.CreateSolidBrush(Color.ToWin32Color(color)));
        }

        public static Brush CreateHatch(Color color, BrushHatchStyle hatchStyle)
        {
            return new Brush(NativeMethods.CreateHatchBrush((int)hatchStyle, Color.ToWin32Color(color)));
        }

        public static Brush CreatePattern(NonOwnedBitmap bmp)
        {
            return new Brush(NativeMethods.CreatePatternBrush(bmp.Handle));
        }

        public Brush(IntPtr ptr) : base(ptr) { }

        public void Dispose()
        {
            NativeMethods.DeleteObject(Handle);
        }
    }
}
