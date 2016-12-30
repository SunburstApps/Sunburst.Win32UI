using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.UserInterface.Graphics;
using Microsoft.Win32.UserInterface.Interop;

namespace Microsoft.Win32.UserInterface.Layout
{
    public class ScrollContainer : CustomWindow
    {
        public bool ScrollChildren { get; set; } = true;
        public bool DisableThumbTracking { get; set; } = false;
        public bool ShowDisabledHorizontalScrollBar { get; set; } = false;
        public bool ShowDisabledVerticalScrollBar { get; set; } = false;

        private Point mOffset;
        private Size mEntireSize, mPageSize, mLineSize;
        private int mWheelDelta, mWheelHorizontalDelta, mWheelLines;
        private uint mScrollFlags;

        public Point GetScrollOffset() => mOffset;
        public void SetScrollOffset(Point offset, bool redraw = true)
        {
            AdjustScrollOffset(offset);

            int dx = mOffset.x - offset.x;
            int dy = mOffset.y - offset.y;

            mOffset.x = offset.x;
            mOffset.y = offset.y;

            SCROLLINFO si = new SCROLLINFO();
            si.cbSize = Convert.ToUInt32(Marshal.SizeOf<SCROLLINFO>());
            si.fMask = SCROLLINFO.SIF_POS;
            if (ShowDisabledHorizontalScrollBar) si.fMask |= SCROLLINFO.SIF_DISABLENOSCROLL;
            si.nPos = offset.x;
            SetScrollInfo(SCROLLINFO.SB_HORZ, si, redraw);

            si = new SCROLLINFO();
            si.cbSize = Convert.ToUInt32(Marshal.SizeOf<SCROLLINFO>());
            si.fMask = SCROLLINFO.SIF_POS;
            if (ShowDisabledVerticalScrollBar) si.fMask |= SCROLLINFO.SIF_DISABLENOSCROLL;
            si.nPos = offset.x;
            SetScrollInfo(SCROLLINFO.SB_VERT, si, redraw);

            if (dx != 0 || dy != 0)
            {
                using (DeferWindowPos dwp = new DeferWindowPos(20))
                {
                    for (IntPtr child = NativeMethods.GetWindow(Handle, NativeMethods.GW_CHILD); child != IntPtr.Zero; child = NativeMethods.GetWindow(child, NativeMethods.GW_HWNDNEXT))
                    {
                        Window wnd = new Window(child);
                        Rect rect = wnd.WindowRect;

                        Point pt = new Point(rect.left, rect.top);
                        NativeMethods.MapWindowPoints(IntPtr.Zero, wnd.Handle, ref pt, 1);
                        rect.left = pt.x; rect.top = pt.y;
                        dwp.AddControl(wnd, rect, DeferWindowPosFlags.DoNotActivate | DeferWindowPosFlags.IgnoreSize | DeferWindowPosFlags.IgnoreZOrder);
                    }
                }
            }

            SetScrollLineSize(Size.Zero);
            SetScrollPageSize(Size.Zero);

            if (redraw) Invalidate();
        }

        public Size GetScrollSize() => mEntireSize;
        public void SetScrollSize(Size size, bool redraw = true, bool resetOffset = true)
        {
            mEntireSize.width = size.width;
            mEntireSize.height = size.height;

            Point newOffset = Point.Zero;
            if (!resetOffset) newOffset = AdjustScrollOffset(mOffset);

            int dx = mOffset.x - newOffset.x;
            int dy = mOffset.y - newOffset.y;
            mOffset = newOffset;

            Rect clientRect = ClientRect;
            SCROLLINFO si = new SCROLLINFO();
            si.cbSize = Convert.ToUInt32(Marshal.SizeOf<SCROLLINFO>());
            si.fMask = SCROLLINFO.SIF_RANGE | SCROLLINFO.SIF_PAGE | SCROLLINFO.SIF_POS;
            if (ShowDisabledHorizontalScrollBar) si.fMask |= SCROLLINFO.SIF_DISABLENOSCROLL;
            si.nMin = 0;
            si.nMax = mEntireSize.width - 1;
            si.nPage = Convert.ToUInt32(clientRect.Width);
            si.nPos = mOffset.x;
            SetScrollInfo(SCROLLINFO.SB_HORZ, si, redraw);

            si = new SCROLLINFO();
            si.cbSize = Convert.ToUInt32(Marshal.SizeOf<SCROLLINFO>());
            si.fMask = SCROLLINFO.SIF_RANGE | SCROLLINFO.SIF_PAGE | SCROLLINFO.SIF_POS;
            if (ShowDisabledHorizontalScrollBar) si.fMask |= SCROLLINFO.SIF_DISABLENOSCROLL;
            si.nMin = 0;
            si.nMax = mEntireSize.height - 1;
            si.nPage = Convert.ToUInt32(clientRect.Height);
            si.nPos = mOffset.y;
            SetScrollInfo(SCROLLINFO.SB_VERT, si, redraw);

            if (dx != 0 || dy != 0)
            {
                using (DeferWindowPos dwp = new DeferWindowPos(20))
                {
                    for (IntPtr child = NativeMethods.GetWindow(Handle, NativeMethods.GW_CHILD); child != IntPtr.Zero; child = NativeMethods.GetWindow(child, NativeMethods.GW_HWNDNEXT))
                    {
                        Window wnd = new Window(child);
                        Rect rect = wnd.WindowRect;

                        Point pt = new Point(rect.left, rect.top);
                        NativeMethods.MapWindowPoints(IntPtr.Zero, wnd.Handle, ref pt, 1);
                        rect.left = pt.x + dx; rect.top = pt.y + dy;
                        dwp.AddControl(wnd, rect, DeferWindowPosFlags.DoNotActivate | DeferWindowPosFlags.IgnoreSize | DeferWindowPosFlags.IgnoreZOrder);
                    }
                }
            }

            SetScrollLineSize(Size.Zero);
            SetScrollPageSize(Size.Zero);

            if (redraw) Invalidate();
        }

        public Size GetScrollLineSize() => mLineSize;
        public void SetScrollLineSize(Size sz)
        {
            if (sz.width < 0 || sz.height < 0) throw new ArgumentException("SetScrollLine size must be positive in both height and width", nameof(sz));
            if (mEntireSize.width == 0 && mEntireSize.height == 0) throw new InvalidOperationException("Cannot set the scroll line size at this time.");

            mLineSize.width = CalcLineOrPage(sz.width, mEntireSize.width, 100);
            mLineSize.height = CalcLineOrPage(sz.height, mEntireSize.height, 100);
        }

        public Size GetScrollPageSize() => mPageSize;
        public void SetScrollPageSize(Size sz)
        {
            if (sz.width < 0 || sz.height < 0) throw new ArgumentException("SetScrollLine size must be positive in both height and width", nameof(sz));
            if (mEntireSize.width == 0 && mEntireSize.height == 0) throw new InvalidOperationException("Cannot set the scroll line size at this time.");

            mPageSize.width = CalcLineOrPage(sz.width, mEntireSize.width, 100);
            mPageSize.height = CalcLineOrPage(sz.height, mEntireSize.height, 100);
        }

        private int CalcLineOrPage(int line, int entireSize, int unk1) => throw new NotImplementedException();
        private void SetScrollInfo(int barId, SCROLLINFO info, bool redraw) => throw new NotImplementedException();
        private Point AdjustScrollOffset(Point offset) => throw new NotImplementedException();
    }
}
