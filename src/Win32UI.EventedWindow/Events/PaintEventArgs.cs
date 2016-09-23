using System;
using Microsoft.Win32.UserInterface.Graphics;

namespace Microsoft.Win32.UserInterface.Events
{
    public sealed class PaintEventArgs : ResultHandledEventArgs
    {
        public PaintEventArgs(NonOwnedGraphicsContext context)
        {
            GraphicsContext = context;
        }

        public NonOwnedGraphicsContext GraphicsContext { get; private set; }
    }
}
