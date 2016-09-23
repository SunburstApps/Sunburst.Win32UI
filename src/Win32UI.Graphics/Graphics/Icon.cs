using System;
using Microsoft.Win32.UserInterface.Interop;

namespace Microsoft.Win32.UserInterface.Graphics
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
