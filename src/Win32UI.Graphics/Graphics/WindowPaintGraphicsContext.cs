using System;
using Microsoft.Win32.UserInterface.Interop;

namespace Microsoft.Win32.UserInterface.Graphics
{
    public class WindowPaintGraphicsContext : NonOwnedGraphicsContext, IDisposable
    {
        private static PAINTSTRUCT BeginPaint(Window parent)
        {
            PAINTSTRUCT ps = new PAINTSTRUCT();
            NativeMethods.BeginPaint(parent.Handle, ref ps);
            return ps;
        }

        private IntPtr ExtractHandle(PAINTSTRUCT ps)
        {
            PaintStruct = ps;
            return ps.hDC;
        }

        public WindowPaintGraphicsContext(Window parent)
        {
            NativeMethods.BeginPaint(parent.Handle, ref PaintStruct);
            Parent = parent;
            Handle = PaintStruct.hDC;
        }

        public WindowPaintGraphicsContext(IntPtr ptr) : base(ptr) { }

        public Window Parent { get; private set; }
        private PAINTSTRUCT PaintStruct;

        public Rect RedrawRect => PaintStruct.rcPaint;
        public bool EraseBackground => PaintStruct.fErase;

        public void Dispose()
        {
            NativeMethods.EndPaint(Parent.Handle, ref PaintStruct);
        }
    }
}
