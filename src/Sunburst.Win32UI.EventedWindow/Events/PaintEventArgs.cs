using System;
using Sunburst.Win32UI.Graphics;

namespace Sunburst.Win32UI.Events
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
