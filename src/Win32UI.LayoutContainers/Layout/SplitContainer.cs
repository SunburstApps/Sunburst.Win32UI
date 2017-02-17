using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.UserInterface.Events;
using Microsoft.Win32.UserInterface.Graphics;
using Microsoft.Win32.UserInterface.Interop;

namespace Microsoft.Win32.UserInterface.Layout
{
    public class SplitContainer : CustomWindow
    {
        // Continue at: GetSystemSettings() method [atlsplit.h line 966]

        public bool ScaleSplitProportionally { get; set; } = true;
        public bool FavorRightPane { get; set; } = false; // ignored if ScaleSplitProportionally is true
        public bool AllowUserResize { get; set; } = true;
        public SplitContainerOrientation Orientation { get; set; } = SplitContainerOrientation.Vertical;

        private const int MaxPropertyValue = 10000;
        private const int SplitterStep = 10;

        private int? mSplitterPosition = null, mNewSplitterPosition = null, mProportionalPosition = null;
        private SplitContainerPane mDefaultActivePane = SplitContainerPane.None, mDefaultSinglePane = SplitContainerPane.None;
        private Window mSavedFocusWindow = null;
        private int mSplitterBarThickness = 4;
        private Cursor mCursor = null;
        private int mMinPaneSize = 0, mSplitterBarEdge = 0;
        private bool mFullDrag = true;
        private int mDragOffset = -1;
        private bool mUpdateProportionalPosition = true;
        private int? mSplitterDefaultPosition = null;
        private bool mProportionalDefaultPosition = false;
        private Rect mSplitterRect;

        private Dictionary<SplitContainerPane, Window> mSplitPanes = new Dictionary<SplitContainerPane, Window>(2);

        private void CommonConstructor()
        {
            // This method exists because SplitContainer constructor(hWnd) cannot inherit
            // from both base(hWnd) and this() at the same time.
            mSplitPanes[SplitContainerPane.LeftTop] = new Window(IntPtr.Zero);
            mSplitPanes[SplitContainerPane.RightBottom] = new Window(IntPtr.Zero);

            mSplitterRect = Rect.Zero;
        }

        public SplitContainer()
        {
            CommonConstructor();
        }

        public SplitContainer(IntPtr hWnd) : base(hWnd)
        {
            CommonConstructor();
        }

        public Rect GetSplitterRect() => mSplitterRect;
        public void SetSplitterRect(Rect r, bool update = true)
        {
            mSplitterRect = r;

            if (ScaleSplitProportionally) UpdateProportionalPosition();
            else if (FavorRightPane) UpdateRightAlignPosition();

            if (update) UpdateSplitterPosition();
        }

        public int? GetSplitterPosition() => mSplitterPosition;
        public void SetSplitterPosition(int? newPosition, bool update = true)
        {
            if (!newPosition.HasValue)
            {
                if (mProportionalDefaultPosition)
                {
                    if (mSplitterDefaultPosition < 0 || mSplitterDefaultPosition > MaxPropertyValue)
                        throw new IndexOutOfRangeException("Splitter default position out of range");

                    if (Orientation == SplitContainerOrientation.Vertical)
                    {
                        newPosition = mSplitterDefaultPosition * mSplitterRect.Width - mSplitterBarThickness - mSplitterBarEdge / MaxPropertyValue;
                    }
                    else
                    {
                        newPosition = mSplitterDefaultPosition * mSplitterRect.Height - mSplitterBarThickness - mSplitterBarEdge / MaxPropertyValue;
                    }
                }
                else if (mSplitterDefaultPosition.HasValue)
                {
                    newPosition = mSplitterDefaultPosition.Value;
                }
                else
                {
                    if (Orientation == SplitContainerOrientation.Vertical)
                    {
                        newPosition = (mSplitterRect.Width - mSplitterBarThickness - mSplitterBarEdge) / 2;
                    }
                    else
                    {
                        newPosition = (mSplitterRect.Height - mSplitterBarThickness - mSplitterBarEdge) / 2;
                    }
                }
            }

            int maxPosition = (Orientation == SplitContainerOrientation.Vertical) ? mSplitterRect.Width : mSplitterRect.Height;
            if (newPosition < (mMinPaneSize + mSplitterBarEdge))
                newPosition = mMinPaneSize;
            else if (newPosition > (maxPosition - mSplitterBarThickness - mSplitterBarEdge - mMinPaneSize))
                newPosition = maxPosition - mSplitterBarThickness - mSplitterBarEdge - mMinPaneSize;

            bool changed = (mSplitterPosition != newPosition);
            mSplitterPosition = newPosition;

            if (mUpdateProportionalPosition)
            {
                if (ScaleSplitProportionally) UpdateProportionalPosition();
                else if (FavorRightPane) UpdateRightAlignPosition();
            }
            else
            {
                mUpdateProportionalPosition = true;
            }

            if (update && changed) UpdateSplitterLayout();
        }

        public void SetSplitterPositionPercentage(int percentange, bool update = true)
        {
            if (percentange < 0 || percentange > 100) throw new ArgumentOutOfRangeException("Percentage must be between 0% and 100%", nameof(percentange));

            mProportionalPosition = percentange * MaxPropertyValue / 100;
            UpdateProportionalPosition();

            if (update) UpdateSplitterLayout();
        }

        public int? GetSplitterPositionPercentage()
        {
            int total = (Orientation == SplitContainerOrientation.Vertical) ? (mSplitterRect.Width - mSplitterBarThickness - mSplitterBarEdge) : (mSplitterRect.Height - mSplitterBarThickness - mSplitterBarEdge);
            return (total > 0 && mSplitterPosition >= 0) ? ((mSplitterPosition * MaxPropertyValue / total) / 100) : null;
        }

        public SplitContainerPane GetSinglePaneMode() => mDefaultActivePane;
        public void SetSinglePaneMode(SplitContainerPane pane)
        {
            if (pane != SplitContainerPane.None)
            {
                if (!mSplitPanes[pane].IsVisible) mSplitPanes[pane].Show();
                SplitContainerPane otherPane = (pane == SplitContainerPane.LeftTop) ? SplitContainerPane.RightBottom : SplitContainerPane.LeftTop;
                mSplitPanes[otherPane].Hide();

                if (mDefaultActivePane != pane) mDefaultActivePane = pane;
            }
            else if (mDefaultActivePane != SplitContainerPane.None)
            {
                SplitContainerPane otherPane = (pane == SplitContainerPane.LeftTop) ? SplitContainerPane.RightBottom : SplitContainerPane.LeftTop;
                mSplitPanes[otherPane].Show();
            }

            mDefaultActivePane = pane;
            UpdateSplitterLayout();
        }

        public void SetSplitterDefaultPosition(int position)
        {
            mSplitterDefaultPosition = position;
            mProportionalDefaultPosition = false;
        }

        public void SetSplitterDefaultPositionPercentage(int percentage)
        {
            if (percentage < 0 || percentage > 100) throw new ArgumentOutOfRangeException("Percentage must be between 0% and 100%", nameof(percentage));

            mSplitterDefaultPosition = percentage * MaxPropertyValue / 100;
            mProportionalDefaultPosition = true;
        }

        public Window GetPane(SplitContainerPane pane)
        {
            if (pane == SplitContainerPane.None)
            {
                throw new ArgumentException($"You cannot pass {nameof(SplitContainerPane)}.{nameof(SplitContainerPane.None)} to this method", nameof(pane));
            }

            try
            {
                return mSplitPanes[pane];
            }
            catch (KeyNotFoundException)
            {
                throw new ArgumentException($"Invalid {nameof(SplitContainerPane)} value {pane}", nameof(pane));
            }
        }

        public void SetPane(SplitContainerPane pane, Window content, bool update = true)
        {
            if (pane == SplitContainerPane.None)
            {
                throw new ArgumentException($"You cannot pass {nameof(SplitContainerPane)}.{nameof(SplitContainerPane.None)} to this method", nameof(pane));
            }

            IntPtr contentHandle = content?.Handle ?? IntPtr.Zero;
            if (pane == SplitContainerPane.LeftTop)
            {
                IntPtr otherHandle = mSplitPanes[SplitContainerPane.RightBottom].Handle;
                if (otherHandle != IntPtr.Zero && otherHandle == contentHandle)
                {
                    throw new ArgumentException("You cannot set the same control as both the left/top and right/bottom pane");
                }

                mSplitPanes[pane] = content;
            }
            else if (pane == SplitContainerPane.RightBottom)
            {
                IntPtr otherHandle = mSplitPanes[SplitContainerPane.LeftTop].Handle;
                if (otherHandle != IntPtr.Zero && otherHandle == contentHandle)
                {
                    throw new ArgumentException("You cannot set the same control as both the left/top and right/bottom pane");
                }

                mSplitPanes[pane] = content;
            }

            if (update) UpdateSplitterLayout();
        }

        public void SetActivePane(SplitContainerPane pane)
        {
            if (pane == SplitContainerPane.None)
            {
                throw new ArgumentException($"You cannot pass {nameof(SplitContainerPane)}.{nameof(SplitContainerPane.None)} to this method", nameof(pane));
            }

            IntPtr hWnd = mSplitPanes[pane].Handle;
            if (hWnd != IntPtr.Zero) NativeMethods.SetFocus(hWnd);

            mDefaultSinglePane = pane;
        }

        public SplitContainerPane GetActivePane()
        {
            IntPtr hWnd = NativeMethods.GetFocus();
            if (hWnd != IntPtr.Zero)
            {
                if (mSplitPanes[SplitContainerPane.LeftTop].Handle == hWnd || NativeMethods.IsChild(mSplitPanes[SplitContainerPane.LeftTop].Handle, hWnd)) return SplitContainerPane.LeftTop;
                else if (mSplitPanes[SplitContainerPane.RightBottom].Handle == hWnd || NativeMethods.IsChild(mSplitPanes[SplitContainerPane.RightBottom].Handle, hWnd)) return SplitContainerPane.RightBottom;
            }

            return SplitContainerPane.None;
        }

        public void ActivateNextPane(bool navigateForward = true)
        {
            var pane = mDefaultSinglePane;
            if (pane == SplitContainerPane.None)
            {
                switch (GetActivePane())
                {
                    case SplitContainerPane.LeftTop:
                        pane = SplitContainerPane.RightBottom;
                        break;
                    case SplitContainerPane.RightBottom:
                        pane = SplitContainerPane.LeftTop;
                        break;
                    default:
                        throw new InvalidOperationException("Invalid active pane value");
                }
            }

            SetActivePane(pane);
        }

        public SplitContainerPane GetDefaultActivaPen() => mDefaultActivePane;
        public void SetDefaultActivePane(SplitContainerPane pane)
        {
            if (pane == SplitContainerPane.None)
            {
                throw new ArgumentException($"You cannot pass {nameof(SplitContainerPane)}.{nameof(SplitContainerPane.None)} to this method", nameof(pane));
            }

            mDefaultActivePane = pane;
        }

        private void DrawSplitter(NonOwnedGraphicsContext dc)
        {
            if (mDefaultSinglePane == SplitContainerPane.None && !mSplitterPosition.HasValue) return;

            if (mDefaultSinglePane == SplitContainerPane.None)
            {
                DrawSplitterBarContent(dc);
                if (mSplitPanes[SplitContainerPane.LeftTop].Handle != IntPtr.Zero) DrawEmptyPaneContent(dc, SplitContainerPane.LeftTop);
                if (mSplitPanes[SplitContainerPane.RightBottom].Handle != IntPtr.Zero) DrawEmptyPaneContent(dc, SplitContainerPane.RightBottom);
            }
            else
            {
                if (mSplitPanes[mDefaultSinglePane].Handle != IntPtr.Zero) DrawEmptyPaneContent(dc, mDefaultSinglePane);
            }
        }

        public void InitiateKeyboardSplitterMovement()
        {
            int x = 0, y = 0;

            int splitterPosition = mSplitterPosition ?? 0;
            if (Orientation == SplitContainerOrientation.Vertical)
            {
                x = splitterPosition + (mSplitterBarThickness / 2) + mSplitterBarEdge;
                y = (mSplitterRect.Height - mSplitterBarThickness - mSplitterBarEdge) / 2;
            }
            else
            {
                x = (mSplitterRect.Width - mSplitterBarThickness - mSplitterBarEdge) / 2;
                y = splitterPosition + (mSplitterBarThickness / 2) + mSplitterBarEdge;
            }

            Point pt = new Point(x, y);
            NativeMethods.ClientToScreen(Handle, ref pt);
            NativeMethods.SetCursorPos(pt.x, pt.y);

            mNewSplitterPosition = mSplitterPosition;
            NativeMethods.SetFocus(Handle);
            Cursor.Current = mCursor;
            if (!mFullDrag) DrawGhostBar();

            if (Orientation == SplitContainerOrientation.Vertical) mDragOffset = x - mSplitterRect.left - (mSplitterPosition ?? 0);
            else mDragOffset = y - mSplitterRect.top - (mSplitterPosition ?? 0);
        }

        protected virtual void DrawSplitterBarContent(NonOwnedGraphicsContext dc)
        {
            Rect? barRect = GetSplitterBarRect();
            if (barRect.HasValue) dc.FillRect(barRect.Value, SystemBrushes.WindowBackground);
        }

        protected virtual void DrawEmptyPaneContent(NonOwnedGraphicsContext dc, SplitContainerPane pane)
        {
            Rect paneRect = GetPaneRect(pane);
            dc.FillRect(paneRect, SystemBrushes.ControlBackground);
        }

        protected Rect? GetSplitterBarRect()
        {
            if (mDefaultSinglePane != SplitContainerPane.None || !mSplitterPosition.HasValue) return null;

            if (Orientation == SplitContainerOrientation.Vertical)
            {
                return new Rect
                {
                    top = mSplitterRect.top,
                    left = mSplitterRect.left + (mSplitterPosition ?? 0),
                    right = mSplitterRect.left + (mSplitterPosition ?? 0) + mSplitterBarThickness + mSplitterBarEdge,
                    bottom = mSplitterRect.bottom
                };
            }
            else
            {
                return new Rect
                {
                    top = mSplitterRect.top + (mSplitterPosition ?? 0),
                    left = mSplitterRect.left,
                    right = mSplitterRect.right,
                    bottom = mSplitterRect.bottom + (mSplitterPosition ?? 0) + mSplitterBarThickness + mSplitterBarEdge
                };
            }
        }

        protected Rect GetPaneRect(SplitContainerPane pane)
        {
            if (pane == SplitContainerPane.None)
            {
                throw new ArgumentException($"You cannot pass {nameof(SplitContainerPane)}.{nameof(SplitContainerPane.None)} to this method", nameof(pane));
            }

            throw new NotImplementedException();
        }

        protected override void OnCreated(ResultHandledEventArgs e)
        {
            base.OnCreated(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (mDefaultSinglePane == SplitContainerPane.None && !mSplitterPosition.HasValue)
                SetSplitterPosition(null);

            using (WindowPaintGraphicsContext dc = new WindowPaintGraphicsContext(this))
            {
                DrawSplitter(dc);
            }

            e.Handled = true;
            e.ResultPointer = IntPtr.Zero;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (NativeMethods.GetCapture() == Handle)
            {
                int newSplitPosition = 0;
                if (Orientation == SplitContainerOrientation.Vertical) newSplitPosition = e.MouseLocation.x - mSplitterRect.left - mDragOffset;
                else newSplitPosition = e.MouseLocation.y - mSplitterRect.top - mDragOffset;

                if (mSplitterPosition != newSplitPosition)
                {
                    if (mFullDrag)
                    {
                        SetSplitterPosition(newSplitPosition);
                        Update();
                    }
                    else
                    {
                        DrawGhostBar();
                        SetSplitterPosition(newSplitPosition, false);
                        DrawGhostBar();
                    }
                }
            }
            else
            {
                if (IsOverSplitterBar(e.MouseLocation.x, e.MouseLocation.y))
                {
                    Cursor.Current = mCursor;
                }
            }
        }

        protected override void OnMouseLeftButtonDown(MouseEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            if (NativeMethods.GetCapture() != Handle && IsOverSplitterBar(e.MouseLocation.x, e.MouseLocation.y))
            {
                mNewSplitterPosition = mSplitterPosition;
                NativeMethods.SetCapture(Handle);
                mSavedFocusWindow = new Window(NativeMethods.GetFocus());
                NativeMethods.SetFocus(Handle);
                Cursor.Current = mCursor;

                if (!mFullDrag) DrawGhostBar();
                if (Orientation == SplitContainerOrientation.Vertical) mDragOffset = e.MouseLocation.x - mSplitterRect.left - (mSplitterPosition ?? 0);
                else mDragOffset = e.MouseLocation.y - mSplitterRect.top - (mSplitterPosition ?? 0);
            }
            else if (NativeMethods.GetCapture() == Handle && !IsOverSplitterBar(e.MouseLocation.x, e.MouseLocation.y))
            {
                NativeMethods.ReleaseCapture();
            }

            e.Handled = false;
        }

        protected override void OnMouseLeftButtonUp(MouseEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            if (NativeMethods.GetCapture() == Handle)
            {
                mNewSplitterPosition = mSplitterPosition;
                NativeMethods.ReleaseCapture();
            }

            e.Handled = false;
        }

        protected override void OnMouseLeftButtonDoubleClick(MouseEventArgs e)
        {
            base.OnMouseLeftButtonDoubleClick(e);
            SetSplitterPosition(null);
        }

        protected override IntPtr ProcessMessage(uint msg, IntPtr wParam, IntPtr lParam)
        {
            if (msg == WindowMessages.WM_SETCURSOR)
            {
                const int HTCLIENT = 1;
                if (wParam == Handle && (((int)lParam) & 0xFFFF) == HTCLIENT)
                {
                    int position = NativeMethods.GetMessagePos();
                    Point pt = new Point((position & 0xFFFF), (position >> 16) & 0xFFFF);
                    if (IsOverSplitterBar(pt.x, pt.y)) return (IntPtr)1;
                }
            }
            else if (msg == WindowMessages.WM_CAPTURECHANGED)
            {
                if (!mFullDrag) DrawGhostBar();

                if (mSplitterPosition.HasValue && (!mFullDrag || (mSplitterPosition != mNewSplitterPosition)))
                {
                    mSplitterPosition = mNewSplitterPosition;
                    mNewSplitterPosition = null;

                    UpdateSplitterLayout();
                    Invalidate();
                }

                if (mSavedFocusWindow != null) NativeMethods.SetFocus(mSavedFocusWindow.Handle);
            }
            else if (msg == WindowMessages.WM_KEYDOWN)
            {
                if (NativeMethods.GetCapture() == Handle)
                {
                    ushort keyCode = (ushort)(int)wParam;
                    VirtualKeys key = (VirtualKeys)keyCode;
                    if (key == VirtualKeys.VK_RETURN)
                    {
                        mNewSplitterPosition = mSplitterPosition;
                        NativeMethods.ReleaseCapture();
                    }
                    else if (key == VirtualKeys.VK_ESCAPE)
                    {
                        NativeMethods.ReleaseCapture();
                    }
                    else if (key == VirtualKeys.VK_LEFT || key == VirtualKeys.VK_RIGHT)
                    {
                        if (Orientation == SplitContainerOrientation.Vertical)
                        {
                            NativeMethods.GetCursorPos(out Point pt);

                            int xyPos = (mSplitterPosition ?? 0) + ((key == VirtualKeys.VK_LEFT) ? -SplitterStep : SplitterStep);
                            if (xyPos < mSplitterRect.Width + mSplitterBarEdge)
                            {
                                xyPos = mSplitterRect.Width;
                            }
                            else if (xyPos > (mSplitterRect.Width - mSplitterBarThickness - mSplitterBarEdge - mMinPaneSize))
                            {
                                xyPos = mSplitterRect.Width - mSplitterBarThickness - mSplitterBarEdge - mMinPaneSize;
                            }

                            pt.x += xyPos - (mSplitterPosition ?? 0);
                            NativeMethods.SetCursorPos(pt.x, pt.y);
                        }
                    }
                    else if (key == VirtualKeys.VK_UP || key == VirtualKeys.VK_DOWN)
                    {
                        if (Orientation != SplitContainerOrientation.Vertical)
                        {
                            NativeMethods.GetCursorPos(out Point pt);

                            int xyPos = (mSplitterPosition ?? 0) + ((key == VirtualKeys.VK_UP) ? -SplitterStep : SplitterStep);
                            if (xyPos < mSplitterRect.Height + mSplitterBarEdge)
                            {
                                xyPos = mSplitterRect.Height;
                            }
                            else if (xyPos > (mSplitterRect.Height - mSplitterBarThickness - mSplitterBarEdge - mMinPaneSize))
                            {
                                xyPos = mSplitterRect.Height - mSplitterBarThickness - mSplitterBarEdge - mMinPaneSize;
                            }

                            pt.y += xyPos - (mSplitterPosition ?? 0);
                            NativeMethods.SetCursorPos(pt.x, pt.y);
                        }
                    }
                }
            }
            else if (msg == WindowMessages.WM_SETFOCUS)
            {
                if (NativeMethods.GetCapture() != Handle)
                {
                    if (mDefaultSinglePane == SplitContainerPane.None)
                    {
                        if (mDefaultActivePane == SplitContainerPane.LeftTop || mDefaultActivePane == SplitContainerPane.RightBottom)
                        {
                            NativeMethods.SetFocus(mSplitPanes[mDefaultActivePane].Handle);
                        }
                    }
                    else
                    {
                        NativeMethods.SetFocus(mSplitPanes[mDefaultSinglePane].Handle);
                    }
                }
            }
            else if (msg == WindowMessages.WM_CREATE)
            {
                const uint SPI_GETDRAGFULLWINDOWS = 0x26;
                using (HGlobal ptr = new HGlobal(Marshal.SizeOf<int>()))
                {
                    NativeMethods.SystemParametersInfoW(SPI_GETDRAGFULLWINDOWS, 0, ptr.Handle, 0);
                    mFullDrag = Marshal.ReadInt32(ptr.Handle) != 0;
                }
            }

            return base.ProcessMessage(msg, wParam, lParam);
        }

        private bool IsOverSplitterRect(int? x, int? y)
        {
            return ((!x.HasValue || (x.Value >= mSplitterRect.left && x.Value <= mSplitterRect.right)) && (!y.HasValue || (y.Value >= mSplitterRect.top && y.Value <= mSplitterRect.bottom)));
        }

        private bool IsOverSplitterBar(int x, int y)
        {
            if (mDefaultSinglePane == SplitContainerPane.None) return false;
            if (!mSplitterPosition.HasValue || !IsOverSplitterRect(x, y)) return false;

            int xy = Orientation == SplitContainerOrientation.Vertical ? x : y;
            int xyOff = Orientation == SplitContainerOrientation.Vertical ? mSplitterRect.left : mSplitterRect.top;

            return ((xy >= (xyOff + mSplitterPosition)) && (xy < xyOff + mSplitterPosition + mSplitterBarThickness + mSplitterBarEdge));
        }

        private void UpdateSplitterLayout()
        {
            if (mDefaultSinglePane == SplitContainerPane.None && !mSplitterPosition.HasValue) return;

            if (mDefaultSinglePane == SplitContainerPane.None)
            {
                var barRect = GetSplitterBarRect();
                if (barRect.HasValue) Invalidate(barRect.Value);

                using (DeferWindowPos dwp = new DeferWindowPos(2))
                {
                    foreach (var pane in new[] { SplitContainerPane.LeftTop, SplitContainerPane.RightBottom })
                    {
                        if (GetSplitterPaneRect(pane, out Rect paneRect))
                        {
                            if (mSplitPanes[pane].Handle != IntPtr.Zero)
                            {
                                dwp.AddControl(mSplitPanes[pane], paneRect);
                            }
                            else
                            {
                                Invalidate(paneRect);
                            }
                        }
                    }
                }
            }
            else
            {
                if (GetSplitterPaneRect(mDefaultSinglePane, out Rect paneRect))
                {
                    if (mSplitPanes[mDefaultSinglePane].Handle != IntPtr.Zero) mSplitPanes[mDefaultSinglePane].Move(paneRect);
                    else Invalidate(paneRect);
                }
            }
        }

        private static NonOwnedBrush GetHalftoneBrush()
        {
            short[] grayPattern = new short[8];
            for (int i = 0; i < 8; i++) grayPattern[i] = (short)(0x5555 << (i & 1));

            using (HGlobal bits = new HGlobal(Marshal.SizeOf<short>() * grayPattern.Length))
            {
                for (int i = 0; i < 8; i++) Marshal.WriteInt16(bits.Handle, i * Marshal.SizeOf<short>(), (short)(0x5555 << (i & 1)));

                using (Bitmap bmp = new Bitmap(NativeMethods.CreateBitmap(8, 8, 1, 1, bits.Handle)))
                {
                    IntPtr hBrush = NativeMethods.CreatePatternBrush(bmp.Handle);
                    return new NonOwnedBrush(hBrush);
                }
            }
        }

        private void DrawGhostBar()
        {
            Rect? rectValue = GetSplitterBarRect();
            if (!rectValue.HasValue) return;
            Rect rect = rectValue.Value;

            Rect windowRect = this.WindowRect;
            Point[] pt = new Point[]
            {
                new Point(windowRect.left, windowRect.top),
                new Point(windowRect.right, windowRect.bottom)
            };
            NativeMethods.MapWindowPoints(IntPtr.Zero, Handle, pt, pt.Length);

            using (WindowGraphicsContext dc = new WindowGraphicsContext(this))
            {
                dc.Select(GetHalftoneBrush(), () =>
                {
                    const int PATINVERT = 0x005A0049;
                    NativeMethods.PatBlt(dc.Handle, rect.left, rect.top, rect.Width, rect.Height, PATINVERT);
                });
            }
        }

        private void UpdateProportionalPosition() => throw new NotImplementedException();
        private void UpdateRightAlignPosition() => throw new NotImplementedException();
        private void UpdateSplitterPosition() => throw new NotImplementedException();

        private bool GetSplitterPaneRect(SplitContainerPane pane, out Rect rect)
        {
            rect = Rect.Zero;

            if (mDefaultSinglePane != SplitContainerPane.None)
            {
                if (pane == mDefaultSinglePane)
                {
                    rect = mSplitterRect;
                    return true;
                }

                return false;
            }
            else if (pane == SplitContainerPane.LeftTop)
            {
                if (Orientation == SplitContainerOrientation.Vertical)
                {
                    rect = new Rect
                    {
                        top = mSplitterRect.top,
                        left = mSplitterRect.left,
                        right = mSplitterRect.left + (mSplitterPosition ?? 0),
                        bottom = mSplitterRect.bottom
                    };
                }
                else
                {
                    rect = new Rect
                    {
                        top = mSplitterRect.top,
                        left = mSplitterRect.left,
                        right = mSplitterRect.right,
                        bottom = mSplitterRect.top + (mSplitterPosition ?? 0)
                    };
                }
            }
            else if (pane == SplitContainerPane.RightBottom)
            {
                if (Orientation == SplitContainerOrientation.Vertical)
                {
                    rect = new Rect
                    {
                        top = mSplitterRect.top,
                        left = mSplitterRect.left + (mSplitterPosition ?? 0) + mSplitterBarThickness + mSplitterBarEdge,
                        right = mSplitterRect.right,
                        bottom = mSplitterRect.bottom
                    };
                }
                else
                {
                    rect = new Rect
                    {
                        top = mSplitterRect.top + (mSplitterPosition ?? 0) + mSplitterBarThickness + mSplitterBarEdge,
                        left = mSplitterRect.left,
                        right = mSplitterRect.right,
                        bottom = mSplitterRect.bottom
                    };
                }
            }
            else
            {
                return false;
            }

            return true;
        }
    }

    public enum SplitContainerOrientation
    {
        Vertical = 0,
        Horizontal
    }

    public enum SplitContainerPane
    {
        None = -1,
        LeftTop = 0,
        RightBottom = 1
    }
}
