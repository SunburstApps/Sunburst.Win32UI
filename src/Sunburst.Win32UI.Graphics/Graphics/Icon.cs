using System;
using Sunburst.Win32UI.Interop;

namespace Sunburst.Win32UI.Graphics
{
    public class Icon : NonOwnedIcon, IDisposable
    {
        public Icon(IntPtr ptr) : base(ptr) { }

        public void Dispose()
        {
            NativeMethods.DestroyIcon(Handle);
        }
    }
}
