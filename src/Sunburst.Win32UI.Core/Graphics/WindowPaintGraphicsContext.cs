using System;
using Sunburst.Win32UI.Interop;

namespace Sunburst.Win32UI.Graphics
{
    public class WindowPaintGraphicsContext : GraphicsContext, IDisposable
    {
        public WindowPaintGraphicsContext(Control parent)
        {
            NativeMethods.BeginPaint(parent.Handle, ref PaintStruct);
            Parent = parent;
            Handle = PaintStruct.hDC;
        }

        public WindowPaintGraphicsContext(IntPtr ptr) : base(ptr) { }

        public Control Parent { get; private set; }
        private PAINTSTRUCT PaintStruct;

        public Rect RedrawRect => PaintStruct.rcPaint;
        public bool EraseBackground => PaintStruct.fErase;

        protected override void DisposeCore()
        {
            NativeMethods.EndPaint(Parent.Handle, ref PaintStruct);
        }
    }
}
