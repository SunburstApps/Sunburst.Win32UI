using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sunburst.Win32UI.Graphics;
using Sunburst.Win32UI.Interop;
using System.Runtime.InteropServices;

namespace Sunburst.Win32UI
{
    public abstract class BufferedAnimationWindow : CustomWindow
    {
        public BufferedPaintFlags Flags { get; set; } = 0;
        public AnimationStyle AnimationStyle { get; set; } = AnimationStyle.Linear;
        public TimeSpan Duration { get; set; } = TimeSpan.FromMilliseconds(500);
        public object State { get; set; } = null;
        public object NewState { get; set; } = null;

        protected override IntPtr ProcessMessage(uint msg, IntPtr wParam, IntPtr lParam)
        {
            if (msg == WindowMessages.WM_CREATE)
            {
                OnCreate();
            }
            else if (msg == WindowMessages.WM_ERASEBKGND)
            {
                return (IntPtr)1; // no background needed
            }
            else if (msg == WindowMessages.WM_PAINT || msg == WindowMessages.WM_PRINTCLIENT)
            {
                return OnPaint(wParam, lParam);
            }

            return base.ProcessMessage(msg, wParam, lParam);
        }

        private IntPtr OnPaint(IntPtr wParam, IntPtr lParam)
        {
            if (wParam != IntPtr.Zero)
            {
                DoPaint(new NonOwnedGraphicsContext(wParam), ClientRect, NewState);
            }
            else
            {
                WindowPaintGraphicsContext graphicsContext = new WindowPaintGraphicsContext(this);
                DoAnimationPaint(graphicsContext, graphicsContext.RedrawRect);
            }

            return IntPtr.Zero;
        }

        private void OnCreate()
        {
            NewState = State;
        }

        public void DoAnimation(object newState)
        {
            NewState = newState;
            Invalidate(false);
            Update();
            State = NewState;
        }

        public void DoAnimation(object newState, Rect rect)
        {
            NewState = newState;
            Invalidate(rect, false);
            Update();
            State = NewState;
        }

        protected virtual bool AreStatesEqual => State.Equals(NewState);

        protected virtual void DoAnimationPaint(NonOwnedGraphicsContext graphicsContext, Rect frame)
        {
            if (BufferedAnimationContext.IsRendering(this, graphicsContext)) return;

            TimeSpan savedDuration = Duration;
            if (AreStatesEqual) Duration = TimeSpan.Zero;

            using (BufferedAnimationContext context = BufferedAnimationContext.Create(this, graphicsContext,
                frame, Convert.ToInt32(Duration.TotalMilliseconds), AnimationStyle,
                BufferingFormat.TopDownDeviceIndependentBitmap, Flags))
            {
                DoPaint(context.SourceGraphicsContext, frame, State);
                DoPaint(context.DestinationGraphicsContext, frame, NewState);
            }

            Duration = savedDuration;
        }

        // This method must only reference the state object passed as a parameter!
        protected abstract void DoPaint(NonOwnedGraphicsContext graphicsContext, Rect frame, object state);
    }
}
