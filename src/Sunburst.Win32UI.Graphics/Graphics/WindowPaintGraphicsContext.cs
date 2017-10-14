using System;
using Sunburst.Win32UI.Interop;

namespace Sunburst.Win32UI.Graphics
{
    public class WindowPaintGraphicsContext : NonOwnedGraphicsContext, IDisposable
    {
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
