using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.UserInterface.Events;
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
        public bool EraseBackgroundOnScroll { get; set; } = false;

        private Point mOffset;
        private Size mEntireSize, mPageSize, mLineSize, mClientSize;
        private int mWheelDelta, mWheelHorizontalDelta;
        private uint mWheelLines = 3;

        public Point GetScrollOffset() => mOffset;
        public void SetScrollOffset(Point offset, bool redraw = true)
        {
            AdjustScrollOffset(offset, out offset);

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
                        NativeMethods.MapWindowPoints(IntPtr.Zero, wnd.Handle, new[] { pt }, 1);
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
            if (!resetOffset) AdjustScrollOffset(mOffset, out mOffset);

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
                        NativeMethods.MapWindowPoints(IntPtr.Zero, wnd.Handle, new[] { pt }, 1);
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

        private void DoScroll(int type, int scrollCode, ref int offset, int entireSize, int pageSize, int lineSize)
        {
            if (!NativeMethods.IsWindow(Handle)) throw new InvalidOperationException("Cannot scroll when the ScrollContainer has an invalid HWND");

            Rect rect = this.ClientRect;
            int cxyClient = (type == SCROLLINFO.SB_VERT) ? rect.bottom : rect.right;
            int cxyMax = entireSize - cxyClient;
            if (cxyMax < 0) return; // can't scroll, client area is bigger

            bool update = true;
            int scrollDistance = 0;

            switch (scrollCode)
            {
                case SCROLLINFO.SB_TOP:
                    scrollDistance = offset;
                    offset = 0;
                    break;
                case SCROLLINFO.SB_BOTTOM:
                    scrollDistance = offset - cxyMax;
                    offset = cxyMax;
                    break;
                case SCROLLINFO.SB_LINEUP:
                    if (offset >= lineSize)
                    {
                        scrollDistance = lineSize;
                        offset -= lineSize;
                    }
                    else
                    {
                        scrollDistance = offset;
                        offset = 0;
                    }

                    break;
                case SCROLLINFO.SB_LINEDOWN:
                    if (offset < cxyMax - lineSize)
                    {
                        scrollDistance = -lineSize;
                        offset += lineSize;
                    }
                    else
                    {
                        scrollDistance = offset - cxyMax;
                        offset = cxyMax;
                    }

                    break;
                case SCROLLINFO.SB_PAGEUP:
                    if (offset >= pageSize)
                    {
                        scrollDistance = pageSize;
                        offset -= pageSize;
                    }
                    else
                    {
                        scrollDistance = offset;
                        offset = 0;
                    }

                    break;
                case SCROLLINFO.SB_PAGEDOWN:
                    if (offset < cxyMax - pageSize)
                    {
                        scrollDistance = -pageSize;
                        offset += pageSize;
                    }
                    else
                    {
                        scrollDistance = offset - cxyMax;
                        offset = cxyMax;
                    }

                    break;
                case SCROLLINFO.SB_THUMBTRACK:
                case SCROLLINFO.SB_THUMBPOSITION:
                    if (scrollCode == SCROLLINFO.SB_THUMBTRACK && DisableThumbTracking) break;

                    SCROLLINFO si = new SCROLLINFO();
                    si.cbSize = Convert.ToUInt32(Marshal.SizeOf<SCROLLINFO>());
                    si.fMask = SCROLLINFO.SIF_TRACKPOS;

                    if (NativeMethods.GetScrollInfo(Handle, type, ref si))
                    {
                        scrollDistance = offset - si.nTrackPos;
                        offset = si.nTrackPos;
                    }

                    break;
                case SCROLLINFO.SB_ENDSCROLL:
                default:
                    update = false;
                    break;
            }

            if (update && scrollDistance != 0)
            {
                NativeMethods.SetScrollPos(Handle, type, scrollDistance, true);

                uint scrollFlags = 0;
                if (ScrollChildren) scrollFlags |= 0x1; // SW_SCROLLCHILDREN;
                if (EraseBackgroundOnScroll) scrollFlags |= 0x4; // SW_ERASE

                if (type == SCROLLINFO.SB_VERT) NativeMethods.ScrollWindowEx(Handle, 0, scrollDistance, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, scrollFlags);
                else NativeMethods.ScrollWindowEx(Handle, scrollDistance, 0, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, scrollFlags);
            }
        }

        public void ScrollLineDown() => DoScroll(SCROLLINFO.SB_VERT, SCROLLINFO.SB_LINEDOWN, ref mOffset.y, mEntireSize.height, mPageSize.height, mLineSize.height);
        public void ScrollLineUp() => DoScroll(SCROLLINFO.SB_VERT, SCROLLINFO.SB_LINEUP, ref mOffset.y, mEntireSize.height, mPageSize.height, mLineSize.height);
        public void ScrollPageDown() => DoScroll(SCROLLINFO.SB_VERT, SCROLLINFO.SB_PAGEDOWN, ref mOffset.y, mEntireSize.height, mPageSize.height, mLineSize.height);
        public void ScrollPageUp() => DoScroll(SCROLLINFO.SB_VERT, SCROLLINFO.SB_PAGEUP, ref mOffset.y, mEntireSize.height, mPageSize.height, mLineSize.height);
        public void ScrollToTop() => DoScroll(SCROLLINFO.SB_VERT, SCROLLINFO.SB_TOP, ref mOffset.y, mEntireSize.height, mPageSize.height, mLineSize.height);
        public void ScrollToBottom() => DoScroll(SCROLLINFO.SB_VERT, SCROLLINFO.SB_BOTTOM, ref mOffset.y, mEntireSize.height, mPageSize.height, mLineSize.height);

        public void ScrollLineLeft() => DoScroll(SCROLLINFO.SB_HORZ, SCROLLINFO.SB_LINELEFT, ref mOffset.x, mEntireSize.width, mPageSize.width, mLineSize.width);
        public void ScrollLineRight() => DoScroll(SCROLLINFO.SB_HORZ, SCROLLINFO.SB_LINERIGHT, ref mOffset.x, mEntireSize.width, mPageSize.width, mLineSize.width);
        public void ScrollPageLeft() => DoScroll(SCROLLINFO.SB_HORZ, SCROLLINFO.SB_PAGELEFT, ref mOffset.x, mEntireSize.width, mPageSize.width, mLineSize.width);
        public void ScrollPageRight() => DoScroll(SCROLLINFO.SB_HORZ, SCROLLINFO.SB_PAGERIGHT, ref mOffset.x, mEntireSize.width, mPageSize.width, mLineSize.width);
        public void ScrollToLeftEdge() => DoScroll(SCROLLINFO.SB_HORZ, SCROLLINFO.SB_TOP, ref mOffset.x, mEntireSize.width, mPageSize.width, mLineSize.width);
        public void ScrollToRightEdge() => DoScroll(SCROLLINFO.SB_HORZ, SCROLLINFO.SB_BOTTOM, ref mOffset.x, mEntireSize.width, mPageSize.width, mLineSize.width);

        public void ScrollIntoView(Point pt)
        {
            Rect rect = new Rect();
            rect.left = rect.right = pt.x;
            rect.top = rect.bottom = pt.y;
            ScrollIntoView(rect);
        }

        public void ScrollIntoView(Rect rect)
        {
            Rect clientRect = ClientRect;

            int x = mOffset.x;
            if (rect.left < mOffset.x) x = rect.left;
            else if (rect.right > (mOffset.x + clientRect.right)) x = rect.right - clientRect.right;

            int y = mOffset.y;
            if (rect.top < mOffset.y) y = rect.top;
            else if (rect.bottom > (mOffset.y + clientRect.bottom)) y = rect.bottom - clientRect.bottom;

            SetScrollOffset(new Point(x, y));
        }

        public void ScrollIntoView(Window window)
        {
            Rect windowRect = window.WindowRect;
            Point[] points = new Point[2];
            points[0].x = windowRect.left + mOffset.x;
            points[0].y = windowRect.top + mOffset.y;
            points[1].x = windowRect.right + mOffset.x;
            points[1].y = windowRect.bottom + mOffset.y;

            NativeMethods.MapWindowPoints(IntPtr.Zero, Handle, points, points.Length);

            Rect targetRect = new Rect();
            targetRect.left = points[0].x;
            targetRect.top = points[0].y;
            targetRect.right = points[1].x;
            targetRect.right = points[1].y;
            ScrollIntoView(targetRect);
        }

        protected override void OnCreated(ResultHandledEventArgs e)
        {
            base.OnCreated(e);
            GetSystemSettings();
        }

        protected override void OnVerticalScroll(ScrollEventArgs e)
        {
            DoScroll(SCROLLINFO.SB_VERT, (int)e.Type, ref mOffset.y, mEntireSize.height, mPageSize.height, mLineSize.height);
            base.OnVerticalScroll(e);
        }

        protected override void OnHorizontalScroll(ScrollEventArgs e)
        {
            DoScroll(SCROLLINFO.SB_HORZ, (int)e.Type, ref mOffset.x, mEntireSize.width, mPageSize.width, mLineSize.width);
            base.OnHorizontalScroll(e);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            const int ONE_WHEEL_DETENT = 120;

            int scrollCode = (mWheelLines == uint.MaxValue) ? ((e.WheelMovement > 0) ? SCROLLINFO.SB_PAGEUP : SCROLLINFO.SB_PAGEDOWN) : ((e.WheelMovement > 0) ? SCROLLINFO.SB_LINEUP : SCROLLINFO.SB_LINEDOWN);

            mWheelDelta += e.WheelMovement;
            int zTotal = Convert.ToInt32((mWheelLines == uint.MaxValue) ? Math.Abs(e.WheelMovement) : Math.Abs(e.WheelMovement) * mWheelLines);
            if (mEntireSize.height > mClientSize.height)
            {
                for (int i = 0; i < zTotal; i += ONE_WHEEL_DETENT)
                {
                    DoScroll(SCROLLINFO.SB_VERT, scrollCode, ref mOffset.y, mEntireSize.height, mPageSize.height, mLineSize.height);
                    Update();
                }
            }
            else
            {
                // Can't scroll vertically, so scroll horizontally.
                for (int i = 0; i < zTotal; i += ONE_WHEEL_DETENT)
                {
                    DoScroll(SCROLLINFO.SB_HORZ, scrollCode, ref mOffset.x, mEntireSize.width, mPageSize.width, mLineSize.width);
                    Update();
                }
            }

            mWheelDelta %= ONE_WHEEL_DETENT;
            base.OnMouseWheel(e);
        }

        protected override void OnResized(WindowResizedEventArgs e)
        {
            DoSize(e.NewSize);
            base.OnResized(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            using (WindowPaintGraphicsContext context = new WindowPaintGraphicsContext(this))
            {
                context.SetViewportOrigin(new Point(-mOffset.x, -mOffset.y));
                DoPaint(context);
            }

            e.Handled = true;
        }

        protected override IntPtr ProcessMessage(uint msg, IntPtr wParam, IntPtr lParam)
        {
            if (msg == WindowMessages.WM_SETTINGCHANGE)
            {
                GetSystemSettings();
            }
            else if (msg == WindowMessages.WM_MOUSEHWHEEL)
            {
                const int ONE_WHEEL_DETENT = 120;
                short zDelta = (short)wParam;

                int scrollCode = (mWheelLines == uint.MaxValue) ? ((zDelta > 0) ? SCROLLINFO.SB_PAGELEFT : SCROLLINFO.SB_PAGERIGHT) : ((zDelta > 0) ? SCROLLINFO.SB_LINELEFT : SCROLLINFO.SB_LINERIGHT);

                mWheelDelta += (short)wParam;
                int zTotal = Convert.ToInt32((mWheelLines == uint.MaxValue) ? Math.Abs(zDelta) : Math.Abs(zDelta) * mWheelHorizontalDelta);
                for (int i = 0; i < zTotal; i += ONE_WHEEL_DETENT)
                {
                    DoScroll(SCROLLINFO.SB_HORZ, scrollCode, ref mOffset.y, mEntireSize.height, mPageSize.height, mLineSize.height);
                    Update();
                }

                mWheelDelta %= ONE_WHEEL_DETENT;
                return IntPtr.Zero;
            }

            return base.ProcessMessage(msg, wParam, lParam);
        }

        private void DoSize(Size sz)
        {
            mClientSize = sz;

            // Set the horizontal scroll bar properties.
            {
                SCROLLINFO si = new SCROLLINFO();
                si.cbSize = Convert.ToUInt32(Marshal.SizeOf<SCROLLINFO>());
                si.fMask = SCROLLINFO.SIF_PAGE | SCROLLINFO.SIF_POS | SCROLLINFO.SIF_RANGE;
                si.nMin = 0;
                si.nMax = sz.width - 1;
                if (ShowDisabledHorizontalScrollBar) si.fMask |= SCROLLINFO.SIF_DISABLENOSCROLL;
                si.nPage = Convert.ToUInt32(mClientSize.width);
                si.nPos = mOffset.x;

                SetScrollInfo(SCROLLINFO.SB_HORZ, si, true);
            }

            // Set the vertical scroll bar properties.
            {
                SCROLLINFO si = new SCROLLINFO();
                si.cbSize = Convert.ToUInt32(Marshal.SizeOf<SCROLLINFO>());
                si.fMask = SCROLLINFO.SIF_PAGE | SCROLLINFO.SIF_POS | SCROLLINFO.SIF_RANGE;
                si.nMin = 0;
                si.nMax = sz.height - 1;
                if (ShowDisabledHorizontalScrollBar) si.fMask |= SCROLLINFO.SIF_DISABLENOSCROLL;
                si.nPage = Convert.ToUInt32(mClientSize.width);
                si.nPos = mOffset.y;

                SetScrollInfo(SCROLLINFO.SB_VERT, si, true);
            }

            if (AdjustScrollOffset(mOffset, out var adjusted))
            {
                uint scrollFlags = 0;
                if (EraseBackgroundOnScroll) scrollFlags |= 0x4; // SW_ERASE
                NativeMethods.ScrollWindowEx(Handle, mOffset.x - adjusted.x, mOffset.y - adjusted.y, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, scrollFlags);

                // Child windows will be moved here, if needed.
                SetScrollOffset(adjusted, false);
            }
        }

        private static int CalcLineOrPage(int value, int maximum, int dividend)
        {
            if (value != 0)
            {
                value = maximum / dividend;
                value = Math.Max(value, 1);
            }
            else if (value > maximum)
            {
                value = maximum;
            }

            return value;
        }

        private bool AdjustScrollOffset(Point offset, out Point adjusted)
        {
            adjusted = offset;

            int widthMax = mEntireSize.width - mClientSize.width;
            int heightMax = mEntireSize.height - mClientSize.height;

            if (offset.x > widthMax) adjusted.x = Math.Max(widthMax, 0);
            else adjusted.x = Math.Max(adjusted.x, 0);

            if (offset.y > heightMax) adjusted.y = Math.Max(heightMax, 0);
            else adjusted.y = Math.Max(adjusted.y, 0);

            return !offset.Equals(adjusted);
        }

        private void GetSystemSettings()
        {
            using (HGlobal ptr = new HGlobal(Marshal.SizeOf<int>()))
            {
                const uint SPI_GETWHEELSCROLLLINES = 0x68;
                NativeMethods.SystemParametersInfoW(SPI_GETWHEELSCROLLLINES, 0, ptr.Handle, 0);
                mWheelLines = Convert.ToUInt32(Marshal.ReadInt32(ptr.Handle));
            }

            using (HGlobal ptr = new HGlobal(Marshal.SizeOf<int>()))
            {
                const uint SPI_GETWHEELSCROLLCHARS = 0x6C;
                NativeMethods.SystemParametersInfoW(SPI_GETWHEELSCROLLCHARS, 0, ptr.Handle, 0);
                mWheelHorizontalDelta = Marshal.ReadInt32(ptr.Handle);
            }
        }

        private void SetScrollInfo(int barId, SCROLLINFO info, bool redraw)
        {
            NativeMethods.SetScrollInfo(Handle, barId, ref info, redraw);
        }

        protected virtual void DoPaint(NonOwnedGraphicsContext graphicsContext) { }
    }
}
