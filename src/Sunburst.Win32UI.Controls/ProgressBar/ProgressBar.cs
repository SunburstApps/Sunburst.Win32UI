using System;
using System.Collections.Generic;
using System.Text;
using Sunburst.Win32UI.Graphics;
using Sunburst.Win32UI.Interop;

namespace Sunburst.Win32UI.CommonControls
{
    public class ProgressBar : Control
    {
        public ProgressBar()
        {
        }

        public ProgressBar(IntPtr hWnd) : base(hWnd)
        {
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassName = "msctls_progress32";
                return cp;
            }
        }

        public void OffsetValue(int delta)
        {
            const uint PBM_DELTAPOS = WindowMessages.WM_USER + 3;
            SendMessage(PBM_DELTAPOS, (IntPtr)delta, IntPtr.Zero);
        }

        public int Position
        {
            get
            {
                const uint PBM_GETPOS = WindowMessages.WM_USER + 8;
                return (int)SendMessage(PBM_GETPOS, IntPtr.Zero, IntPtr.Zero);
            }

            set
            {
                const uint PBM_SETPOS = WindowMessages.WM_USER + 2;
                SendMessage(PBM_SETPOS, (IntPtr)value, IntPtr.Zero);
            }
        }

        public void GetRange(out int lowerBound, out int upperBound)
        {
            using (StructureBuffer<PBRANGE> range = new StructureBuffer<PBRANGE>())
            {
                const uint PBM_GETRANGE = WindowMessages.WM_USER + 7;
                SendMessage(PBM_GETRANGE, (IntPtr)1, range.Handle);

                var value = range.Value;
                lowerBound = value.iLow;
                upperBound = value.iHigh;
            }
        }

        public void SetRange(int lowerBound, int upperBound)
        {
            const uint PBM_SETRANGE32 = WindowMessages.WM_USER + 6;
            SendMessage(PBM_SETRANGE32, (IntPtr)lowerBound, (IntPtr)upperBound);
        }

        public Color BarColor
        {
            get
            {
                const uint PBM_GETBARCOLOR = WindowMessages.WM_USER + 15;
                return Color.FromWin32Color((int)SendMessage(PBM_GETBARCOLOR, IntPtr.Zero, IntPtr.Zero));
            }

            set
            {
                const uint PBM_SETBARCOLOR = WindowMessages.WM_USER + 9;
                SendMessage(PBM_SETBARCOLOR, IntPtr.Zero, (IntPtr)Color.ToWin32Color(value));
            }
        }

        public Color BackgroundColor
        {
            get
            {
                const uint PBM_GETBKCOLOR = WindowMessages.WM_USER + 14;
                return Color.FromWin32Color((int)SendMessage(PBM_GETBKCOLOR, IntPtr.Zero, IntPtr.Zero));
            }

            set
            {
                const uint PBM_SETBKCOLOR = 0x2001;
                SendMessage(PBM_SETBKCOLOR, IntPtr.Zero, (IntPtr)Color.ToWin32Color(value));
            }
        }

        public bool SetMarquee(bool enableMarquee, int updateTime = 0)
        {
            const uint PBM_SETMARQUEE = WindowMessages.WM_USER + 10;
            return (int)SendMessage(PBM_SETMARQUEE, (IntPtr)(enableMarquee ? 1 : 0), (IntPtr)updateTime) != 0;
        }

        public int StepInterval
        {
            get
            {
                const uint PBM_GETSTEP = WindowMessages.WM_USER + 13;
                return (int)SendMessage(PBM_GETSTEP, IntPtr.Zero, IntPtr.Zero);
            }

            set
            {
                const uint PBM_SETSTEP = WindowMessages.WM_USER + 4;
                SendMessage(PBM_SETSTEP, (IntPtr)value, IntPtr.Zero);
            }
        }

        public ProgressBarState State
        {
            get
            {
                const uint PBM_GETSTATE = WindowMessages.WM_USER + 17;
                return (ProgressBarState)((int)SendMessage(PBM_GETSTATE, IntPtr.Zero, IntPtr.Zero));
            }

            set
            {
                if (!Enum.IsDefined(typeof(ProgressBarState), value))
                    throw new ArgumentException("Invalid ProgressBarState", nameof(value));

                const uint PBM_SETSTATE = WindowMessages.WM_USER + 16;
                SendMessage(PBM_SETSTATE, (IntPtr)((int)value), IntPtr.Zero);
            }
        }
    }

    public enum ProgressBarState
    {
        Normal = 1,
        Error = 2,
        Paused = 3
    }
}
