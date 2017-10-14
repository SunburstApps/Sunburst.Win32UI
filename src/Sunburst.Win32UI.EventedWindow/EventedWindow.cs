using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Sunburst.Win32UI.CommonControls;
using Sunburst.Win32UI.Events;
using Sunburst.Win32UI.Graphics;
using Sunburst.Win32UI.Interop;

namespace Sunburst.Win32UI
{
    /// <summary>
    /// The MessageProcessingWindow can be subclassed to enable
    /// automatic dispatching (conversion into .NET events) of
    /// common <c>WM_*</c> messages.
    /// </summary>
    public class EventedWindow : Window
    {
        public void SetTimer(uint timerId, TimeSpan interval)
        {
            if (timerId == 0) throw new ArgumentException("Timer ID cannot be zero", nameof(timerId));
            uint retval = NativeMethods.SetTimer(Handle, timerId, Convert.ToUInt32(Math.Round(interval.TotalMilliseconds)), IntPtr.Zero);
            if (retval == 0) throw new Win32Exception();
        }

        public void KillTimer(uint timerId)
        {
            if (timerId == 0) throw new ArgumentException("Timer ID cannot be zero", nameof(timerId));
            bool retval = NativeMethods.KillTimer(Handle, timerId);
            if (!retval) throw new Win32Exception();
        }

        public event EventHandler<ResultHandledEventArgs> Created;
        protected virtual void OnCreated(ResultHandledEventArgs e) => Created?.Invoke(this, e);

        public event EventHandler<ResultHandledEventArgs> DialogCreated;
        protected virtual void OnDialogCreated(ResultHandledEventArgs e) => DialogCreated?.Invoke(this, e);

        public event EventHandler<CopyDataEventArgs> CopyData;
        protected virtual void OnCopyData(CopyDataEventArgs e) => CopyData?.Invoke(this, e);

        public event EventHandler<HandledEventArgs> Destroyed;
        protected virtual void OnDestroyed(HandledEventArgs e) => Destroyed?.Invoke(this, e);

        public event EventHandler<WindowMovedEventArgs> Moved;
        protected virtual void OnMoved(WindowMovedEventArgs e) => Moved?.Invoke(this, e);

        public event EventHandler<WindowResizedEventArgs> Resized;
        protected virtual void OnResized(WindowResizedEventArgs e) => Resized?.Invoke(this, e);

        public event EventHandler<HandledEventArgs> Activated;
        protected virtual void OnActivated(HandledEventArgs e) => Activated?.Invoke(this, e);

        public event EventHandler<HandledEventArgs> GainedFocus;
        protected virtual void OnGainedFocus(HandledEventArgs e) => GainedFocus?.Invoke(this, e);

        public event EventHandler<HandledEventArgs> LostFocus;
        protected virtual void OnLostFocus(HandledEventArgs e) => LostFocus?.Invoke(this, e);

        public event EventHandler<WindowEnabledChangedEventArgs> EnabledChanged;
        protected virtual void OnEnabledChanged(WindowEnabledChangedEventArgs e) => EnabledChanged?.Invoke(this, e);

        public event EventHandler<PaintEventArgs> Paint;
        protected virtual void OnPaint(PaintEventArgs e) => Paint?.Invoke(this, e);

        public event EventHandler<HandledEventArgs> Close;
        protected virtual void OnClose(HandledEventArgs e) => Close?.Invoke(this, e);

        public event EventHandler<CancelEventArgs> ShuttingDown;
        protected virtual void OnShuttingDown(CancelEventArgs e) => ShuttingDown?.Invoke(this, e);

        public event EventHandler<PaintEventArgs> EraseBackground;
        protected virtual void OnEraseBackground(PaintEventArgs e) => EraseBackground?.Invoke(this, e);

        public event EventHandler<HandledEventArgs> SystemColorsChanged;
        protected virtual void OnSystemColorsChanged(HandledEventArgs e) => SystemColorsChanged?.Invoke(this, e);

        public event EventHandler<ControlBackgroundColorEventArgs<TextBox>> ComputeTextBoxBackgroundColor;
        protected virtual void OnComputeTextBoxBackgroundColor(ControlBackgroundColorEventArgs<TextBox> e) => ComputeTextBoxBackgroundColor?.Invoke(this, e);

        public event EventHandler<ControlBackgroundColorEventArgs<ListBox>> ComputeListBoxBackgroundColor;
        protected virtual void OnComputeListBoxBackgroundColor(ControlBackgroundColorEventArgs<ListBox> e) => ComputeListBoxBackgroundColor?.Invoke(this, e);

        public event EventHandler<ControlBackgroundColorEventArgs<Window>> ComputeDialogBackgroundColor;
        protected virtual void OnComputeDialogBackgroundColor(ControlBackgroundColorEventArgs<Window> e) => ComputeDialogBackgroundColor?.Invoke(this, e);

        public event EventHandler<ControlBackgroundColorEventArgs<ScrollBar>> ComputeScrollBarBackgroundColor;
        protected virtual void OnComputeScrollBarBackgroundColor(ControlBackgroundColorEventArgs<ScrollBar> e) => ComputeScrollBarBackgroundColor?.Invoke(this, e);

        public event EventHandler<ControlBackgroundColorEventArgs<Label>> ComputeLabelBackgroundColor;
        protected virtual void OnComputeLabelBackgroundColor(ControlBackgroundColorEventArgs<Label> e) => ComputeLabelBackgroundColor?.Invoke(this, e);

        public event EventHandler<WindowBoundsEventArgs> CalculateBounds;
        protected virtual void OnCalculateBounds(WindowBoundsEventArgs e) => CalculateBounds?.Invoke(this, e);

        public event EventHandler<KeyboardEventArgs> KeyDown;
        protected virtual void OnKeyDown(KeyboardEventArgs e) => KeyDown?.Invoke(this, e);

        public event EventHandler<KeyboardEventArgs> KeyUp;
        protected virtual void OnKeyUp(KeyboardEventArgs e) => KeyUp?.Invoke(this, e);

        public event EventHandler<TimerEventArgs> TimerTick;
        protected virtual void OnTimerTick(TimerEventArgs e) => TimerTick?.Invoke(this, e);

        public event EventHandler<ScrollEventArgs> HorizontalScroll;
        protected virtual void OnHorizontalScroll(ScrollEventArgs e) => HorizontalScroll?.Invoke(this, e);

        public event EventHandler<ScrollEventArgs> VerticalScroll;
        protected virtual void OnVerticalScroll(ScrollEventArgs e) => VerticalScroll?.Invoke(this, e);

        public event EventHandler<MouseEventArgs> MouseMove;
        protected virtual void OnMouseMove(MouseEventArgs e) => MouseMove?.Invoke(this, e);

        public event EventHandler<MouseWheelEventArgs> MouseWheel;
        protected virtual void OnMouseWheel(MouseWheelEventArgs e) => MouseWheel?.Invoke(this, e);

        public event EventHandler<MouseEventArgs> MouseLeftButtonDown;
        protected virtual void OnMouseLeftButtonDown(MouseEventArgs e) => MouseLeftButtonDown?.Invoke(this, e);

        public event EventHandler<MouseEventArgs> MouseLeftButtonUp;
        protected virtual void OnMouseLeftButtonUp(MouseEventArgs e) => MouseLeftButtonUp?.Invoke(this, e);

        public event EventHandler<MouseEventArgs> MouseLeftButtonDoubleClick;
        protected virtual void OnMouseLeftButtonDoubleClick(MouseEventArgs e) => MouseLeftButtonDoubleClick?.Invoke(this, e);

        public event EventHandler<MouseEventArgs> MouseRightButtonDown;
        protected virtual void OnMouseRightButtonDown(MouseEventArgs e) => MouseRightButtonDown?.Invoke(this, e);

        public event EventHandler<MouseEventArgs> MouseRightButtonUp;
        protected virtual void OnMouseRightButtonUp(MouseEventArgs e) => MouseRightButtonUp?.Invoke(this, e);

        public event EventHandler<MouseEventArgs> MouseRightButtonDoubleClick;
        protected virtual void OnMouseRightButtonDoubleClick(MouseEventArgs e) => MouseRightButtonDoubleClick?.Invoke(this, e);

        public event EventHandler<MouseEventArgs> MouseMiddleButtonDown;
        protected virtual void OnMouseMiddleButtonDown(MouseEventArgs e) => MouseMiddleButtonDown?.Invoke(this, e);

        public event EventHandler<MouseEventArgs> MouseMiddleButtonUp;
        protected virtual void OnMouseMiddleButtonUp(MouseEventArgs e) => MouseMiddleButtonUp?.Invoke(this, e);

        public event EventHandler<MouseEventArgs> MouseMiddleButtonDoubleClick;
        protected virtual void OnMouseMiddleButtonDoubleClick(MouseEventArgs e) => MouseMiddleButtonDoubleClick?.Invoke(this, e);

        public event EventHandler<CommandEventArgs> Command;
        protected virtual void OnCommand(CommandEventArgs e) => Command?.Invoke(this, e);

        protected virtual void OnMeasureOwnerDrawnItem(MeasureOwnerDrawnItemEventArgs e) { }
        protected virtual void OnPaintOwnerDrawnItem(PaintOwnerDrawnItemEventArgs e) { }

        protected IntPtr ProcessCommonMessage(uint msg, IntPtr wParam, IntPtr lParam, out bool handled)
        {
            if (msg == WindowMessages.WM_CREATE)
            {
                ResultHandledEventArgs args = new ResultHandledEventArgs();
                OnCreated(args);
                handled = args.Handled;
                if (handled) return args.ResultPointer;
            }
            else if (msg == WindowMessages.WM_INITDIALOG)
            {
                ResultHandledEventArgs args = new ResultHandledEventArgs();
                args.ResultPointer = (IntPtr)1;
                OnDialogCreated(args);
                handled = args.Handled;
                if (handled) return args.ResultPointer;
            }
            else if (msg == WindowMessages.WM_COPYDATA)
            {
                COPYDATA dataStruct = Marshal.PtrToStructure<COPYDATA>(lParam);
                var args = new CopyDataEventArgs(dataStruct.dwData, dataStruct.cbData);
                OnCopyData(args);
                handled = args.Handled;
                if (handled) return args.ResultPointer;
            }
            else if (msg == WindowMessages.WM_DESTROY)
            {
                HandledEventArgs args = new HandledEventArgs();
                OnDestroyed(args);
                handled = args.Handled;
                if (handled) return IntPtr.Zero;
            }
            else if (msg == WindowMessages.WM_MOVE)
            {
                WindowMovedEventArgs args = new WindowMovedEventArgs(lParam);
                OnMoved(args);
                handled = args.Handled;
                if (handled) return IntPtr.Zero;
            }
            else if (msg == WindowMessages.WM_SIZE)
            {
                WindowResizedEventArgs args = new WindowResizedEventArgs(wParam, lParam);
                OnResized(args);
                handled = args.Handled;
                if (handled) return IntPtr.Zero;
            }
            else if (msg == WindowMessages.WM_ACTIVATE)
            {
                HandledEventArgs args = new HandledEventArgs();
                OnActivated(args);
                handled = args.Handled;
                if (handled) return IntPtr.Zero;
            }
            else if (msg == WindowMessages.WM_SETFOCUS)
            {
                HandledEventArgs args = new HandledEventArgs();
                OnGainedFocus(args);
                handled = args.Handled;
                if (handled) return IntPtr.Zero;
            }
            else if (msg == WindowMessages.WM_KILLFOCUS)
            {
                HandledEventArgs args = new HandledEventArgs();
                OnLostFocus(args);
                handled = args.Handled;
                if (handled) return IntPtr.Zero;
            }
            else if (msg == WindowMessages.WM_ENABLE)
            {
                WindowEnabledChangedEventArgs args = new WindowEnabledChangedEventArgs((int)wParam == 1);
                OnEnabledChanged(args);
                handled = args.Handled;
                if (handled) return IntPtr.Zero;
            }
            else if (msg == WindowMessages.WM_PAINT)
            {
                PaintEventArgs args;
                WindowGraphicsContext ownedContext = null;
                if (wParam == IntPtr.Zero)
                {
                    ownedContext = new WindowGraphicsContext(this);
                    args = new PaintEventArgs(ownedContext);
                }
                else
                {
                    args = new PaintEventArgs(new NonOwnedGraphicsContext(wParam));
                }

                OnPaint(args);
                handled = args.Handled;
                if (ownedContext != null) ownedContext.Dispose();
                if (handled) return args.ResultPointer;
            }
            else if (msg == WindowMessages.WM_CLOSE)
            {
                HandledEventArgs args = new HandledEventArgs();
                OnClose(args);
                handled = args.Handled;
                if (handled) return IntPtr.Zero;
            }
            else if (msg == WindowMessages.WM_QUERYENDSESSION)
            {
                CancelEventArgs args = new CancelEventArgs();
                OnShuttingDown(args);
                handled = true;

                // This seemingly backwards check is correct. WM_QUERYENDSESSION must return TRUE if the shutdown should proceed.
                return (IntPtr)(args.Cancel ? 0 : 1);
            }
            else if (msg == WindowMessages.WM_ERASEBKGND)
            {
                PaintEventArgs args;
                WindowGraphicsContext ownedContext = null;
                if (wParam == IntPtr.Zero)
                {
                    ownedContext = new WindowGraphicsContext(this);
                    args = new PaintEventArgs(ownedContext);
                }
                else
                {
                    args = new PaintEventArgs(new NonOwnedGraphicsContext(wParam));
                }

                OnEraseBackground(args);
                handled = args.Handled;
                if (ownedContext != null) ownedContext.Dispose();
                if (handled) return args.ResultPointer;
            }
            else if (msg == WindowMessages.WM_SYSCOLORCHANGE)
            {
                HandledEventArgs args = new HandledEventArgs();
                OnSystemColorsChanged(args);
                handled = args.Handled;
                if (handled) return IntPtr.Zero;
            }
            else if (msg == WindowMessages.WM_CTLCOLOREDIT)
            {
                var control = new TextBox();
                control.Handle = lParam;
                var args = new ControlBackgroundColorEventArgs<TextBox>(new NonOwnedGraphicsContext(wParam), control);
                OnComputeTextBoxBackgroundColor(args);
                handled = args.Handled;
                if (handled) return args.BackgroundBrush?.Handle ?? IntPtr.Zero;
            }
            else if (msg == WindowMessages.WM_CTLCOLORLISTBOX)
            {
                var control = new ListBox();
                control.Handle = lParam;
                var args = new ControlBackgroundColorEventArgs<ListBox>(new NonOwnedGraphicsContext(wParam), control);
                OnComputeListBoxBackgroundColor(args);
                handled = args.Handled;
                if (handled) return args.BackgroundBrush?.Handle ?? IntPtr.Zero;
            }
            else if (msg == WindowMessages.WM_CTLCOLORDLG)
            {
                var args = new ControlBackgroundColorEventArgs<Window>(new NonOwnedGraphicsContext(wParam), this);
                OnComputeDialogBackgroundColor(args);
                handled = args.Handled;
                if (handled) return args.BackgroundBrush?.Handle ?? IntPtr.Zero;
            }
            else if (msg == WindowMessages.WM_CTLCOLORSCROLLBAR)
            {
                ScrollBar control = new ScrollBar();
                control.Handle = lParam;
                var args = new ControlBackgroundColorEventArgs<ScrollBar>(new NonOwnedGraphicsContext(wParam), control);
                OnComputeScrollBarBackgroundColor(args);
                handled = args.Handled;
                if (handled) return args.BackgroundBrush?.Handle ?? IntPtr.Zero;
            }
            else if (msg == WindowMessages.WM_CTLCOLORSTATIC)
            {
                Label control = new Label();
                control.Handle = lParam;
                var args = new ControlBackgroundColorEventArgs<Label>(new NonOwnedGraphicsContext(wParam), control);
                OnComputeLabelBackgroundColor(args);
                handled = args.Handled;
                if (handled) return args.BackgroundBrush?.Handle ?? IntPtr.Zero;
            }
            else if (msg == WindowMessages.WM_GETMINMAXINFO)
            {
                WindowBoundsEventArgs args = new WindowBoundsEventArgs();
                unsafe
                {
                    MINMAXINFO* sizeInfo = (MINMAXINFO*)lParam;
                    args.MinimumSize = new Size(sizeInfo->ptMinTrackSize.x, sizeInfo->ptMinTrackSize.y);
                    args.MaximumSize = new Size(sizeInfo->ptMaxTrackSize.x, sizeInfo->ptMaxTrackSize.y);
                    OnCalculateBounds(args);
                    sizeInfo->ptMinTrackSize = new Point(args.MinimumSize.width, args.MinimumSize.height);
                    sizeInfo->ptMaxTrackSize = new Point(args.MaximumSize.width, args.MaximumSize.height);
                }
                handled = args.Handled;
                if (args.Handled) return IntPtr.Zero;
            }
            else if (msg == WindowMessages.WM_CHAR)
            {
                char code = (char)(int)wParam;
                int repeatCode = (int)lParam & 0xFFFF;
                bool keyDown = (((int)lParam) & 0x80000000) != 0;
                KeyboardEventArgs args = new KeyboardEventArgs(code, repeatCode);

                if (keyDown) OnKeyDown(args);
                else OnKeyUp(args);

                handled = args.Handled;
                if (handled) return IntPtr.Zero;
            }
            else if (msg == WindowMessages.WM_TIMER)
            {
                TimerEventArgs args = new TimerEventArgs((uint)(int)wParam);
                OnTimerTick(args);
                handled = args.Handled;
                if (handled) return IntPtr.Zero;
            }
            else if (msg == WindowMessages.WM_HSCROLL)
            {
                ScrollBar bar = null;
                if (lParam != null)
                {
                    bar = new ScrollBar();
                    bar.Handle = lParam;
                }

                ScrollEventArgs args = new ScrollEventArgs((ScrollEventType)((int)wParam & 0xFFFF), ((int)wParam >> 16) & 0xFFFF, bar);
                OnHorizontalScroll(args);
                handled = args.Handled;
                if (handled) return IntPtr.Zero;
            }
            else if (msg == WindowMessages.WM_VSCROLL)
            {
                ScrollBar bar = null;
                if (lParam != null)
                {
                    bar = new ScrollBar();
                    bar.Handle = lParam;
                }

                ScrollEventArgs args = new ScrollEventArgs((ScrollEventType)((int)wParam & 0xFFFF), ((int)wParam >> 16) & 0xFFFF, bar);
                OnVerticalScroll(args);
                handled = args.Handled;
                if (handled) return IntPtr.Zero;
            }
            else if (msg == WindowMessages.WM_MOUSEMOVE)
            {
                Point location = new Point(((int)lParam & 0xFFFF), ((int)wParam >> 16) & 0xFFFF);
                MouseEventFlags flags = (MouseEventFlags)((int)wParam & 0xFFFF);
                MouseEventArgs args = new MouseEventArgs(location, flags);
                OnMouseMove(args);
                handled = args.Handled;
                if (handled) return IntPtr.Zero;
            }
            else if (msg == WindowMessages.WM_MOUSEWHEEL)
            {
                Point location = new Point(((int)lParam & 0xFFFF), ((int)wParam >> 16) & 0xFFFF);
                MouseEventFlags flags = (MouseEventFlags)((int)wParam & 0xFFFF);
                MouseWheelEventArgs args = new MouseWheelEventArgs(location, flags, (short)(((int)wParam >> 16) & 0xFFFF));
                OnMouseWheel(args);
                handled = args.Handled;
                if (handled) return IntPtr.Zero;
            }
            else if (msg == WindowMessages.WM_LBUTTONDOWN)
            {
                Point location = new Point(((int)lParam & 0xFFFF), ((int)wParam >> 16) & 0xFFFF);
                MouseEventFlags flags = (MouseEventFlags)((int)wParam & 0xFFFF);
                MouseEventArgs args = new MouseEventArgs(location, flags);
                OnMouseLeftButtonDown(args);
                handled = args.Handled;
                if (handled) return IntPtr.Zero;
            }
            else if (msg == WindowMessages.WM_LBUTTONUP)
            {
                Point location = new Point(((int)lParam & 0xFFFF), ((int)wParam >> 16) & 0xFFFF);
                MouseEventFlags flags = (MouseEventFlags)((int)wParam & 0xFFFF);
                MouseEventArgs args = new MouseEventArgs(location, flags);
                OnMouseLeftButtonUp(args);
                handled = args.Handled;
                if (handled) return IntPtr.Zero;
            }
            else if (msg == WindowMessages.WM_LBUTTONDBLCLK)
            {
                Point location = new Point(((int)lParam & 0xFFFF), ((int)wParam >> 16) & 0xFFFF);
                MouseEventFlags flags = (MouseEventFlags)((int)wParam & 0xFFFF);
                MouseEventArgs args = new MouseEventArgs(location, flags);
                OnMouseLeftButtonDoubleClick(args);
                handled = args.Handled;
                if (handled) return IntPtr.Zero;
            }
            else if (msg == WindowMessages.WM_RBUTTONDOWN)
            {
                Point location = new Point(((int)lParam & 0xFFFF), ((int)wParam >> 16) & 0xFFFF);
                MouseEventFlags flags = (MouseEventFlags)((int)wParam & 0xFFFF);
                MouseEventArgs args = new MouseEventArgs(location, flags);
                OnMouseRightButtonDown(args);
                handled = args.Handled;
                if (handled) return IntPtr.Zero;
            }
            else if (msg == WindowMessages.WM_RBUTTONUP)
            {
                Point location = new Point(((int)lParam & 0xFFFF), ((int)wParam >> 16) & 0xFFFF);
                MouseEventFlags flags = (MouseEventFlags)((int)wParam & 0xFFFF);
                MouseEventArgs args = new MouseEventArgs(location, flags);
                OnMouseRightButtonUp(args);
                handled = args.Handled;
                if (handled) return IntPtr.Zero;
            }
            else if (msg == WindowMessages.WM_RBUTTONDBLCLK)
            {
                Point location = new Point(((int)lParam & 0xFFFF), ((int)wParam >> 16) & 0xFFFF);
                MouseEventFlags flags = (MouseEventFlags)((int)wParam & 0xFFFF);
                MouseEventArgs args = new MouseEventArgs(location, flags);
                OnMouseRightButtonDoubleClick(args);
                handled = args.Handled;
                if (handled) return IntPtr.Zero;
            }
            else if (msg == WindowMessages.WM_MBUTTONDOWN)
            {
                Point location = new Point(((int)lParam & 0xFFFF), ((int)wParam >> 16) & 0xFFFF);
                MouseEventFlags flags = (MouseEventFlags)((int)wParam & 0xFFFF);
                MouseEventArgs args = new MouseEventArgs(location, flags);
                OnMouseMiddleButtonDown(args);
                handled = args.Handled;
                if (handled) return IntPtr.Zero;
            }
            else if (msg == WindowMessages.WM_MBUTTONUP)
            {
                Point location = new Point(((int)lParam & 0xFFFF), ((int)wParam >> 16) & 0xFFFF);
                MouseEventFlags flags = (MouseEventFlags)((int)wParam & 0xFFFF);
                MouseEventArgs args = new MouseEventArgs(location, flags);
                OnMouseMiddleButtonUp(args);
                handled = args.Handled;
                if (handled) return IntPtr.Zero;
            }
            else if (msg == WindowMessages.WM_MBUTTONDBLCLK)
            {
                Point location = new Point(((int)lParam & 0xFFFF), ((int)wParam >> 16) & 0xFFFF);
                MouseEventFlags flags = (MouseEventFlags)((int)wParam & 0xFFFF);
                MouseEventArgs args = new MouseEventArgs(location, flags);
                OnMouseMiddleButtonDoubleClick(args);
                handled = args.Handled;
                if (handled) return IntPtr.Zero;
            }
            else if (msg == WindowMessages.WM_COMMAND)
            {
                int commandID = (int)wParam & 0xFFFF;
                CommandEventArgs args = new CommandEventArgs(commandID, new Window(lParam));
                OnCommand(args);
                handled = args.Handled;
                if (handled) return IntPtr.Zero;
            }
            else if (msg == WindowMessages.WM_MEASUREITEM)
            {
                unsafe
                {
                    MEASUREITEMSTRUCT* nativeStructPtr = (MEASUREITEMSTRUCT*)lParam;
                    MeasureOwnerDrawnItemEventArgs args = new MeasureOwnerDrawnItemEventArgs(*nativeStructPtr);
                    OnMeasureOwnerDrawnItem(args);
                    if (args.Handled)
                    {
                        nativeStructPtr->itemWidth = args.ItemSize.width;
                        nativeStructPtr->itemHeight = args.ItemSize.height;
                    }

                    handled = args.Handled;
                    if (handled) return (IntPtr)1;
                }
            }
            else if (msg == WindowMessages.WM_DRAWITEM)
            {
                unsafe
                {
                    DRAWITEMSTRUCT* nativeStructPtr = (DRAWITEMSTRUCT*)lParam;
                    PaintOwnerDrawnItemEventArgs args = new PaintOwnerDrawnItemEventArgs(*nativeStructPtr);
                    OnPaintOwnerDrawnItem(args);
                    handled = args.Handled;
                    if (handled) return (IntPtr)1;
                }
            }

            handled = false;
            return IntPtr.Zero;
        }
    }
}
