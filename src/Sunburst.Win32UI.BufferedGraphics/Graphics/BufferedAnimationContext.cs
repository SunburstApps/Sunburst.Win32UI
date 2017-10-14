using System;
using System.Runtime.InteropServices;
using Sunburst.Win32UI.Interop;

namespace Sunburst.Win32UI.Graphics
{
    public sealed class BufferedAnimationContext : IDisposable
    {
        private BufferedAnimationContext(IntPtr hAnimationBuffer, IntPtr hDCFrom, IntPtr hDCTo)
        {
            Handle = hAnimationBuffer;
            SourceGraphicsContext = new NonOwnedGraphicsContext(hDCFrom);
            DestinationGraphicsContext = new NonOwnedGraphicsContext(hDCTo);
        }

        public static BufferedAnimationContext Create(Window targetWindow, NonOwnedGraphicsContext targetContext,
            Rect targetRect, int duration, AnimationStyle style, BufferingFormat format, BufferedPaintFlags flags,
            byte masterOpacity = 255)
        {
            BufferedPaintContext.InitializeBufferedPaintSession();

            IntPtr hDCFrom = IntPtr.Zero, hDCTo = IntPtr.Zero, hAnimBuffer = IntPtr.Zero;

            BLENDFUNCTION blend = new BLENDFUNCTION();
            blend.BlendOp = BLENDFUNCTION.AC_SRC_OVER;
            blend.BlendFlags = 0;
            blend.SourceConstantAlpha = masterOpacity;
            blend.AlphaFormat = BLENDFUNCTION.AC_SRC_ALPHA;

            BP_PAINTPARAMS paintParams = new BP_PAINTPARAMS();
            paintParams.cbSize = Marshal.SizeOf<BP_PAINTPARAMS>();
            paintParams.dwFlags = (int)flags;

            paintParams.AssignPointerValues(Rect.Zero, blend, (value) =>
            {
                BP_ANIMATIONPARAMS animParams = new BP_ANIMATIONPARAMS();
                animParams.cbSize = Marshal.SizeOf<BP_ANIMATIONPARAMS>();
                animParams.dwDuration = duration;
                animParams.style = style;

                Rect tempRect = targetRect;
                hAnimBuffer = NativeMethods.BeginBufferedAnimation(targetWindow.Handle, targetContext.Handle,
                    ref tempRect, format, ref value, ref animParams, ref hDCFrom, ref hDCTo);
            });

            return new BufferedAnimationContext(hAnimBuffer, hDCFrom, hDCTo);
        }

        public bool UpdateAutomatically { get; set; } = true;
        public NonOwnedGraphicsContext SourceGraphicsContext { get; private set; }
        public NonOwnedGraphicsContext DestinationGraphicsContext { get; private set; }
        public IntPtr Handle { get; private set; }

        public void Dispose()
        {
            NativeMethods.EndBufferedAnimation(Handle, UpdateAutomatically);
        }

        public static bool IsRendering(Window owningWindow, NonOwnedGraphicsContext owningContext)
        {
            return NativeMethods.BufferedPaintRenderAnimation(owningWindow.Handle, owningContext.Handle);
        }

        public static void StopAllAnimations(Window targetWindow)
        {
            NativeMethods.BufferedPaintStopAllAnimations(targetWindow.Handle);
        }
    }
}
