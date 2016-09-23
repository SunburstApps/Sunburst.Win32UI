using Microsoft.Win32.UserInterface.Graphics;

namespace Microsoft.Win32.UserInterface.Events
{
    public class MouseEventArgs : HandledEventArgs
    {
        public MouseEventArgs(Point loc, MouseEventFlags flags)
        {
            MouseLocation = loc;
            Flags = flags;
        }

        public MouseEventFlags Flags { get; private set; }
        public Point MouseLocation { get; private set; }
    }

    public class MouseWheelEventArgs : MouseEventArgs
    {
        public MouseWheelEventArgs(Point loc, MouseEventFlags flags, short zdiff) : base(loc, flags)
        {
            WheelMovement = zdiff;
        }

        public short WheelMovement { get; private set; }
    }

    public enum MouseEventFlags
    {
        // These values must match the MK_* values in WinUser.h
        LeftButtonDown = 0x1,
        RightButtonDown = 0x2,
        ShiftKeyDown = 0x4,
        ControlKeyDown = 0x8,
        MiddleMouseButtonDown = 0x10,
        FirstExtendedMouseButtonDown = 0x20,
        SecondExtendedMouseButtonDown = 0x40
    }
}
