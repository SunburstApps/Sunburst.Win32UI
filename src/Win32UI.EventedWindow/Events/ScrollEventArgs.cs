using System.ComponentModel;
using Microsoft.Win32.UserInterface.CommonControls;

namespace Microsoft.Win32.UserInterface.Events
{
    public sealed class ScrollEventArgs : HandledEventArgs
    {
        public ScrollEventArgs(ScrollEventType type, int pos, ScrollBar bar)
        {
            Type = type;
            ScrollerPosition = pos;
            ScrollBar = bar;
        }

        public ScrollEventType Type { get; private set; }
        public int ScrollerPosition { get; private set; }
        public ScrollBar ScrollBar { get; private set; }
    }

    public enum ScrollEventType
    {
        // These must match the SB_* values in WinUser.h
        LineUp = 0,
        LineLeft = 0,
        LineDown = 1,
        LineRight = 1,
        PageUp = 2,
        PageLeft = 2,
        PageDown = 3,
        PageRIight = 3,
        ThumbPosition = 4,
        ThumbTrack = 5,
        ScrollToBeginning = 6,
        ScrollToEnd = 7,
        EndScroll = 8
    }
}
