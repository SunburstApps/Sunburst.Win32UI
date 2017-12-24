using System;
using System.Runtime.InteropServices;
using Sunburst.Win32UI.Graphics;
using Sunburst.Win32UI.Handles;
using Sunburst.Win32UI.Interop;

namespace Sunburst.Win32UI.CommonControls
{
    public class ScrollBar : Control
    {
        private const int SB_CTL = 2;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassName = "SCROLLBAR";
                return cp;
            }
        }

        public bool RemoveWhenUnneeded { get; set; } = true;

        private SCROLLINFO NativeScrollInfo
        {
            get
            {
                SCROLLINFO info = new SCROLLINFO();
                info.cbSize = Convert.ToUInt32(Marshal.SizeOf<SCROLLINFO>());
                info.fMask = SCROLLINFO.SIF_ALL;
                NativeMethods.GetScrollInfo(Handle, SB_CTL, ref info);
                return info;
            }

            set
            {
                SCROLLINFO info = value;
                NativeMethods.SetScrollInfo(Handle, SB_CTL, ref info);
            }
        }

        public int ScrollPosition
        {
            get
            {
                SCROLLINFO info = NativeScrollInfo;
                return info.nPos;
            }

            set
            {
                SCROLLINFO info = NativeScrollInfo;
                info.nPos = value;
                if (!RemoveWhenUnneeded) info.fMask |= SCROLLINFO.SIF_DISABLENOSCROLL;
                NativeScrollInfo = info;
            }
        }

        public Range ScrollRange
        {
            get
            {
                SCROLLINFO info = NativeScrollInfo;
                return new Range(Convert.ToUInt32(info.nMin), Convert.ToUInt32(info.nMax - info.nMin));
            }

            set
            {
                SCROLLINFO info = NativeScrollInfo;
                info.nMin = Convert.ToInt32(value.Location);
                info.nMax = Convert.ToInt32(value.MaxValue);
                if (!RemoveWhenUnneeded) info.fMask |= SCROLLINFO.SIF_DISABLENOSCROLL;
                NativeScrollInfo = info;
            }
        }

        public int PageSize
        {
            get
            {
                SCROLLINFO info = NativeScrollInfo;
                return Convert.ToInt32(info.nPage);
            }

            set
            {
                SCROLLINFO info = NativeScrollInfo;
                info.nPage = Convert.ToUInt32(value);
                if (!RemoveWhenUnneeded) info.fMask |= SCROLLINFO.SIF_DISABLENOSCROLL;
                NativeScrollInfo = info;
            }
        }

        public void ShowScrollBar() => NativeMethods.ShowScrollBar(Handle, SB_CTL, true);
        public void HideScrollBar() => NativeMethods.ShowScrollBar(Handle, SB_CTL, false);
        public void EnableScrollBar(ScrollBarButtons flags) => NativeMethods.EnableScrollBar(Handle, SB_CTL, (int)flags);
    }

    [Flags]
    public enum ScrollBarButtons
    {
        // These must match the ESB_* values in WinUser.h
        Both = 0x0,
        LeftTop = 0x1,
        RightBottom = 0x2
    }
}
