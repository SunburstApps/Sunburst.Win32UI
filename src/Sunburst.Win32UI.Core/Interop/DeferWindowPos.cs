using System;
using Sunburst.Win32UI.Graphics;

namespace Sunburst.Win32UI.Interop
{
    public sealed class DeferWindowPos : IDisposable
    {
        static readonly IntPtr HWND_TOP = IntPtr.Zero;
        static readonly IntPtr HWND_BOTTOM = (IntPtr)1;
        static readonly IntPtr HWND_TOPMOST = (IntPtr)(-1);
        static readonly IntPtr HWND_NOTOPMOST = (IntPtr)(-2);

        public DeferWindowPos(int expectedControlCount)
        {
            Handle = NativeMethods.BeginDeferWindowPos(expectedControlCount);
        }

        public IntPtr Handle { get; private set; }

        public void Dispose()
        {
            NativeMethods.EndDeferWindowPos(Handle);
        }

        public void AddControl(Control windowToMove, Rect frame, MoveWindowFlags flags = 0)
        {
            Handle = NativeMethods.DeferWindowPos(Handle, windowToMove.Handle, IntPtr.Zero,
                frame.left, frame.top, frame.Width, frame.Height, flags | MoveWindowFlags.IgnoreZOrder);
        }

        public void AddControl(Control windowToMove, Control windowToInsertZOrderAfter, Rect frame, MoveWindowFlags flags = 0)
        {
            Handle = NativeMethods.DeferWindowPos(Handle, windowToMove.Handle, windowToInsertZOrderAfter.Handle,
                frame.left, frame.top, frame.Width, frame.Height, flags);
        }

        public void AddControl(Control windowToMove, ZOrderPosition specialZOrderPosition, Rect frame, MoveWindowFlags flags = 0)
        {
            IntPtr hWndSpecial;
            switch (specialZOrderPosition)
            {
                case ZOrderPosition.Top: hWndSpecial = HWND_TOP; break;
                case ZOrderPosition.Bottom: hWndSpecial = HWND_BOTTOM; break;
                case ZOrderPosition.Topmost: hWndSpecial = HWND_TOPMOST; break;
                case ZOrderPosition.AboveNonTopmost: hWndSpecial = HWND_NOTOPMOST; break;
                default: throw new ArgumentException("Invalid ZOrderPosition value", nameof(specialZOrderPosition));
            }

            Handle = NativeMethods.DeferWindowPos(Handle, windowToMove.Handle, hWndSpecial,
                frame.left, frame.top, frame.Width, frame.Height, flags);
        }
    }

    [Flags]
    public enum MoveWindowFlags : int
    {
        IgnoreSize = 0x1,
        IgnoreMove = 0x2,
        IgnoreZOrder = 0x4,
        DoNotRedraw = 0x8,
        DoNotActivate = 0x10,
        FrameChanged = 0x20
    }

    public enum ZOrderPosition
    {
        Top,
        Bottom,
        Topmost,
        AboveNonTopmost
    }
}
