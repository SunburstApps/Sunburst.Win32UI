using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.UserInterface.Interop;

namespace Microsoft.Win32.UserInterface.Graphics
{
    public sealed class BufferedPaintContext : IDisposable
    {
        public static void InitializeBufferedPaintSession()
        {
            bool ok = NativeMethods.BufferedPaintInit();
            if (!ok) throw new System.ComponentModel.Win32Exception();
        }

        public static void FinalizeBufferedPaintSession()
        {
            bool ok = NativeMethods.BufferedPaintUnInit();
            if (!ok) throw new System.ComponentModel.Win32Exception();
        }

        public static BufferedPaintContext Create(NonOwnedGraphicsContext targetContext, Rect targetRect,
            BufferingFormat bufferFormat, BufferedPaintFlags flags, byte masterOpacity = 255)
        {
            InitializeBufferedPaintSession();

            BLENDFUNCTION blend = new BLENDFUNCTION();
            blend.BlendOp = BLENDFUNCTION.AC_SRC_OVER;
            blend.BlendFlags = 0;
            blend.SourceConstantAlpha = masterOpacity;
            blend.AlphaFormat = BLENDFUNCTION.AC_SRC_ALPHA;

            BP_PAINTPARAMS paintParams = new BP_PAINTPARAMS();
            paintParams.cbSize = Marshal.SizeOf<BP_PAINTPARAMS>();
            paintParams.dwFlags = (int)flags;

            BufferedPaintContext ctx = null;
            paintParams.AssignPointerValues(Rect.Zero, blend, (value) =>
            {
                Rect tempRect = targetRect;
                IntPtr hPaintDC = IntPtr.Zero;
                ctx = new BufferedPaintContext(NativeMethods.BeginBufferedPaint(targetContext.Handle,
                    ref tempRect, (int)bufferFormat, ref value, ref hPaintDC));
            });

            return ctx;
        }

        private BufferedPaintContext(IntPtr hPaintBuffer)
        {
            Handle = hPaintBuffer;
        }

        public IntPtr Handle { get; private set; }
        public bool AutomaticallyUpdate { get; set; } = true;

        public Rect TargetRect
        {
            get
            {
                Rect retval = new Rect();
                NativeMethods.GetBufferedPaintTargetRect(Handle, ref retval);
                return retval;
            }
        }

        public NonOwnedGraphicsContext TargetContext => new NonOwnedGraphicsContext(NativeMethods.GetBufferedPaintTargetDC(Handle));
        public NonOwnedGraphicsContext GraphicsContext => new NonOwnedGraphicsContext(NativeMethods.GetBufferedPaintDC(Handle));

        public void Clear(Rect rect)
        {
            Rect tempRect = rect;
            NativeMethods.BufferedPaintClear(Handle, ref tempRect);
        }

        public void SetAlpha(byte opacity, Rect effectiveRect)
        {
            using (StructureBuffer<Rect> rectBuffer = new StructureBuffer<Rect>())
            {
                rectBuffer.Value = effectiveRect;
                NativeMethods.BufferedPaintSetAlpha(Handle, rectBuffer.Handle, opacity);
            }
        }

        public void MakeOpaque() => SetAlpha(255);
        public void MakeOpaque(Rect effectiveRect) => SetAlpha(255, effectiveRect);
        public void SetAlpha(byte opacity) => NativeMethods.BufferedPaintSetAlpha(Handle, IntPtr.Zero, opacity);
        public void ClearAll() => NativeMethods.BufferedPaintClear(Handle, IntPtr.Zero);

        public void Dispose()
        {
            NativeMethods.EndBufferedPaint(Handle, AutomaticallyUpdate);
            FinalizeBufferedPaintSession();
        }
    }
}
