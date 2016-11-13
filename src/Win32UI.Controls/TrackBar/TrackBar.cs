using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.UserInterface.Graphics;
using Microsoft.Win32.UserInterface.Interop;

namespace Microsoft.Win32.UserInterface.CommonControls
{
    public class TrackBar : Window
    {
        #region Messages
        private const uint TBM_GETPOS             = (WindowMessages.WM_USER);
        private const uint TBM_GETRANGEMIN        = (WindowMessages.WM_USER+1);
        private const uint TBM_GETRANGEMAX        = (WindowMessages.WM_USER+2);
        private const uint TBM_GETTIC             = (WindowMessages.WM_USER+3);
        private const uint TBM_SETTIC             = (WindowMessages.WM_USER+4);
        private const uint TBM_SETPOS             = (WindowMessages.WM_USER+5);
        private const uint TBM_SETRANGE           = (WindowMessages.WM_USER+6);
        private const uint TBM_SETRANGEMIN        = (WindowMessages.WM_USER+7);
        private const uint TBM_SETRANGEMAX        = (WindowMessages.WM_USER+8);
        private const uint TBM_CLEARTICS          = (WindowMessages.WM_USER+9);
        private const uint TBM_SETSEL             = (WindowMessages.WM_USER+10);
        private const uint TBM_SETSELSTART        = (WindowMessages.WM_USER+11);
        private const uint TBM_SETSELEND          = (WindowMessages.WM_USER+12);
        private const uint TBM_GETPTICS           = (WindowMessages.WM_USER+14);
        private const uint TBM_GETTICPOS          = (WindowMessages.WM_USER+15);
        private const uint TBM_GETNUMTICS         = (WindowMessages.WM_USER+16);
        private const uint TBM_GETSELSTART        = (WindowMessages.WM_USER+17);
        private const uint TBM_GETSELEND          = (WindowMessages.WM_USER+18);
        private const uint TBM_CLEARSEL           = (WindowMessages.WM_USER+19);
        private const uint TBM_SETTICFREQ         = (WindowMessages.WM_USER+20);
        private const uint TBM_SETPAGESIZE        = (WindowMessages.WM_USER+21);
        private const uint TBM_GETPAGESIZE        = (WindowMessages.WM_USER+22);
        private const uint TBM_SETLINESIZE        = (WindowMessages.WM_USER+23);
        private const uint TBM_GETLINESIZE        = (WindowMessages.WM_USER+24);
        private const uint TBM_GETTHUMBRECT       = (WindowMessages.WM_USER+25);
        private const uint TBM_GETCHANNELRECT     = (WindowMessages.WM_USER+26);
        private const uint TBM_SETTHUMBLENGTH     = (WindowMessages.WM_USER+27);
        private const uint TBM_GETTHUMBLENGTH     = (WindowMessages.WM_USER+28);
        private const uint TBM_SETTOOLTIPS        = (WindowMessages.WM_USER+29);
        private const uint TBM_GETTOOLTIPS        = (WindowMessages.WM_USER+30);
        private const uint TBM_SETTIPSIDE         = (WindowMessages.WM_USER+31);
        private const uint TBM_SETBUDDY           = (WindowMessages.WM_USER+32);
        private const uint TBM_GETBUDDY           = (WindowMessages.WM_USER+33);
        private const uint TBM_SETPOSNOTIFY       = (WindowMessages.WM_USER+34);
        #endregion

        public const string WindowClass = "msctls_trackbar32";
        public override string WindowClassName => WindowClass;

        public int LineSize
        {
            get
            {
                return (int)SendMessage(TBM_GETLINESIZE, IntPtr.Zero, IntPtr.Zero);
            }

            set
            {
                SendMessage(TBM_SETLINESIZE, IntPtr.Zero, (IntPtr)value);
            }
        }

        public int PageSize
        {
            get
            {
                return (int)SendMessage(TBM_GETPAGESIZE, IntPtr.Zero, IntPtr.Zero);
            }

            set
            {
                SendMessage(TBM_SETPAGESIZE, IntPtr.Zero, (IntPtr)value);
            }
        }

        public int MinimumValue
        {
            get
            {
                return (int)SendMessage(TBM_GETRANGEMIN, IntPtr.Zero, IntPtr.Zero);
            }

            set
            {
                SendMessage(TBM_SETRANGEMIN, IntPtr.Zero, (IntPtr)value);
                Invalidate();
            }
        }

        public int MaximumValue
        {
            get
            {
                return (int)SendMessage(TBM_GETRANGEMAX, IntPtr.Zero, IntPtr.Zero);
            }

            set
            {
                SendMessage(TBM_SETRANGEMAX, IntPtr.Zero, (IntPtr)value);
                Invalidate();
            }
        }

        public int SelectionStart
        {
            get
            {
                return (int)SendMessage(TBM_GETSELSTART, IntPtr.Zero, IntPtr.Zero);
            }

            set
            {
                SendMessage(TBM_SETSELSTART, IntPtr.Zero, (IntPtr)value);
                Invalidate();
            }
        }

        public int SelectionEnd
        {
            get
            {
                return (int)SendMessage(TBM_GETSELEND, IntPtr.Zero, IntPtr.Zero);
            }

            set
            {
                SendMessage(TBM_SETSELEND, IntPtr.Zero, (IntPtr)value);
                Invalidate();
            }
        }

        public Rect ChannelRect
        {
            get
            {
                using (StructureBuffer<Rect> buffer = new StructureBuffer<Rect>())
                {
                    SendMessage(TBM_GETCHANNELRECT, IntPtr.Zero, buffer.Handle);
                    return buffer.Value;
                }
            }
        }

        public Rect ThumbRect
        {
            get
            {
                using (StructureBuffer<Rect> buffer = new StructureBuffer<Rect>())
                {
                    SendMessage(TBM_GETTHUMBRECT, IntPtr.Zero, buffer.Handle);
                    return buffer.Value;
                }
            }
        }

        public int TickMarkCount
        {
            get
            {
                return (int)SendMessage(TBM_GETNUMTICS, IntPtr.Zero, IntPtr.Zero);
            }
        }

        public int GetTickMarkPosition(int tickIndex)
        {
            return (int)SendMessage(TBM_GETTIC, (IntPtr)tickIndex, IntPtr.Zero);
        }

        public void SetTickMark(int tickLocation)
        {
            SendMessage(TBM_SETTIC, IntPtr.Zero, (IntPtr)tickLocation);
        }

        public void AutoCreateTickMarks(int frequency)
        {
            SendMessage(TBM_SETTICFREQ, (IntPtr)frequency, IntPtr.Zero);
        }

        public int ThumbLength
        {
            get
            {
                return (int)SendMessage(TBM_GETTHUMBLENGTH, IntPtr.Zero, IntPtr.Zero);
            }

            set
            {
                SendMessage(TBM_SETTHUMBLENGTH, (IntPtr)value, IntPtr.Zero);
            }
        }

        public Window LeftBuddyWindow
        {
            get
            {
                return new Window(SendMessage(TBM_GETBUDDY, (IntPtr)1, IntPtr.Zero));
            }

            set
            {
                SendMessage(TBM_SETBUDDY, (IntPtr)1, value.Handle);
            }
        }

        public Window RightBuddyWindow
        {
            get
            {
                return new Window(SendMessage(TBM_GETBUDDY, IntPtr.Zero, IntPtr.Zero));
            }

            set
            {
                SendMessage(TBM_SETBUDDY, IntPtr.Zero, value.Handle);
            }
        }

        public ToolTip ToolTip
        {
            get
            {
                ToolTip ctrl = new ToolTip();
                ctrl.Handle = SendMessage(TBM_GETTOOLTIPS, IntPtr.Zero, IntPtr.Zero);
                return ctrl;
            }

            set
            {
                SendMessage(TBM_SETTOOLTIPS, value.Handle, IntPtr.Zero);
            }
        }

        public void SetToolTipSide(int side)
        {
            SendMessage(TBM_SETTIPSIDE, (IntPtr)side, IntPtr.Zero);
        }

        public void ClearTickMarks()
        {
            SendMessage(TBM_CLEARTICS, IntPtr.Zero, IntPtr.Zero);
            Invalidate();
        }
    }
}
