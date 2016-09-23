using System;
using Microsoft.Win32.UserInterface.Interop;

namespace Microsoft.Win32.UserInterface.Graphics
{
    public class WindowGraphicsContext : NonOwnedGraphicsContext, IDisposable
    {
        public WindowGraphicsContext(Window parent) : base(NativeMethods.GetDC(parent.Handle))
        {
            Parent = parent;
        }

        public WindowGraphicsContext(IntPtr ptr) : base(ptr) { }

        public Window Parent { get; private set; }

        public void Dispose()
        {
            NativeMethods.ReleaseDC(Parent.Handle, Handle);
        }
    }
}
