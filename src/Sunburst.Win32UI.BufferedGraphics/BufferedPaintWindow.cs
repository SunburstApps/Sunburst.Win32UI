using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sunburst.Win32UI.Graphics;
using Sunburst.Win32UI.Interop;

namespace Sunburst.Win32UI
{
    public abstract class BufferedPaintWindow : CustomWindow
    {
        protected override IntPtr ProcessMessage(uint msg, IntPtr wParam, IntPtr lParam)
        {
            if (msg == WindowMessages.WM_ERASEBKGND)
            {
                // No background is needed.
                return (IntPtr)1;
            }
            else if (msg == WindowMessages.WM_PAINT)
            {
                if (wParam != IntPtr.Zero)
                {
                    OnPaint(new NonOwnedGraphicsContext(wParam), ClientRect);
                }
                else
                {
                    using (WindowPaintGraphicsContext context = new WindowPaintGraphicsContext(this))
                    {
                        OnBufferedPaint(context, context.RedrawRect);
                    }
                }

                return IntPtr.Zero;
            }

            return base.ProcessMessage(msg, wParam, lParam);
        }

        protected virtual void OnBufferedPaint(NonOwnedGraphicsContext graphicsContext, Rect frame)
        {
            using (BufferedPaintContext bufferContext = BufferedPaintContext.Create(graphicsContext, frame, BufferingFormat.TopDownDeviceIndependentBitmap, 0))
            {
                OnPaint(bufferContext.GraphicsContext, frame);
            }
        }

        protected abstract void OnPaint(NonOwnedGraphicsContext graphicsContext, Rect frame);
    }
}
