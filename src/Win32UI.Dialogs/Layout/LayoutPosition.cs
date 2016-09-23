using System;

namespace Microsoft.Win32.UserInterface.Layout
{
    [Flags]
    public enum LayoutPosition
    {
        AnchorTop = 1,
        AnchorLeft = 2,
        AnchorRight = 4,
        AnchorBottom = 8,

        DockTop = 16,
        DockLeft = 32,
        DockRight = 64,
        DockBottom = 128
    }
}
