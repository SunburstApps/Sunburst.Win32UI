using System;
using Sunburst.Win32UI.Interop;

namespace Sunburst.Win32UI.CommonControls
{
    public class SpinnerControl : Control
    {
        #region Messages
        private const uint UDM_GETPOS = (WindowMessages.WM_USER + 104);
        private const uint UDM_SETBUDDY = (WindowMessages.WM_USER + 105);
        private const uint UDM_GETBUDDY = (WindowMessages.WM_USER + 106);
        private const uint UDM_SETACCEL = (WindowMessages.WM_USER + 107);
        private const uint UDM_GETACCEL = (WindowMessages.WM_USER + 108);
        private const uint UDM_SETBASE = (WindowMessages.WM_USER + 109);
        private const uint UDM_GETBASE = (WindowMessages.WM_USER + 110);
        private const uint UDM_SETRANGE32 = (WindowMessages.WM_USER + 111);
        private const uint UDM_GETRANGE32 = (WindowMessages.WM_USER + 112);
        private const uint UDM_SETPOS32 = (WindowMessages.WM_USER + 113);
        private const uint UDM_GETPOS32 = (WindowMessages.WM_USER + 114);
        #endregion

        public SpinnerControl() : base() { }
        public SpinnerControl(IntPtr hWnd) : base(hWnd) { }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassName = "msctls_updown32";
                return cp;
            }
        }

        public void GetAcceleration(int index, out uint timing, out uint increment)
        {
            using (StructureBuffer<UDACCEL> buffer = new StructureBuffer<UDACCEL>())
            {
                SendMessage(UDM_GETACCEL, (IntPtr)index, buffer.Handle);
                UDACCEL accel = buffer.Value;
                timing = accel.nSec;
                increment = accel.nAccel;
            }
        }

        public void SetAcceleration(int index, uint timing, uint increment)
        {
            UDACCEL accel = new UDACCEL();
            accel.nSec = timing;
            accel.nAccel = increment;

            using (StructureBuffer<UDACCEL> buffer = new StructureBuffer<UDACCEL>())
            {
                buffer.Value = accel;
                SendMessage(UDM_SETACCEL, (IntPtr)index, buffer.Handle);
            }
        }

        public int Base
        {
            get
            {
                return (int)SendMessage(UDM_GETBASE, IntPtr.Zero, IntPtr.Zero) & 0xFFFF;
            }

            set
            {
                SendMessage(UDM_SETBASE, (IntPtr)value, IntPtr.Zero);
            }
        }

        public Control BuddyWindow
        {
            get
            {
                return new Control(SendMessage(UDM_GETBUDDY, IntPtr.Zero, IntPtr.Zero));
            }

            set
            {
                SendMessage(UDM_SETBUDDY, value.Handle, IntPtr.Zero);
            }
        }

        public unsafe void GetRange(out int lowerBound, out int upperBound)
        {
            int lowerValue, upperValue;
            SendMessage(UDM_GETRANGE32, new IntPtr(&lowerValue), new IntPtr(&upperValue));
            lowerBound = lowerValue;
            upperBound = upperValue;
        }

        public unsafe void SetRange(int lowerBound, int upperBound)
        {
            SendMessage(UDM_SETRANGE32, new IntPtr(&lowerBound), new IntPtr(&upperBound));
        }

        public int Position
        {
            get
            {
                return (int)SendMessage(UDM_GETPOS32, IntPtr.Zero, IntPtr.Zero);
            }

            set
            {
                SendMessage(UDM_SETPOS32, IntPtr.Zero, (IntPtr)value);
            }
        }
    }
}
