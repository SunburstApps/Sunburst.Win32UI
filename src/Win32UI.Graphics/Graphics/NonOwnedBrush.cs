using System;
using Microsoft.Win32.UserInterface.Interop;

namespace Microsoft.Win32.UserInterface.Graphics
{
    /// <summary>
    /// Wraps a Win32 <c>HPEN</c> that was obtained from a system method.
    /// This class cannot be used to create brushes; use the <see cref="Brush"/> class instead.
    /// </summary>
    public class NonOwnedBrush
    {
        public NonOwnedBrush(IntPtr ptr)
        {
            Handle = ptr;
        }

        public IntPtr Handle { get; protected set; }
    }
}
