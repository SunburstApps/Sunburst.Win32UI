using System;
using Sunburst.Win32UI.Interop;

namespace Sunburst.Win32UI.Graphics
{
    public class WindowGraphicsContext : GraphicsContext, IDisposable
    {
        public WindowGraphicsContext(Control parent) : base(NativeMethods.GetDC(parent.Handle))
        {
            Parent = parent;
        }

        public WindowGraphicsContext(IntPtr ptr) : base(ptr) { }

        public Control Parent { get; private set; }

        protected override void DisposeCore()
        {
            NativeMethods.ReleaseDC(Parent.Handle, Handle);
        }
    }
}
