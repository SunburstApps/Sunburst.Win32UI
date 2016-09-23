using System;
using Microsoft.Win32.UserInterface.Graphics;

namespace Microsoft.Win32.UserInterface.Interop
{
    internal struct BP_PAINTPARAMS
    {
        public int cbSize;
        public int dwFlags;
        public IntPtr prcExclude; // RECT*
        public IntPtr pBlendFunction; // BLENDFUNCTION*

        public void AssignPointerValues(Rect rect, BLENDFUNCTION blendFunction, Action<BP_PAINTPARAMS> callback)
        {
            using (StructureBuffer<Rect> rectPtr = new StructureBuffer<Rect>())
            {
                rectPtr.Value = rect;
                prcExclude = rectPtr.Handle;

                using (StructureBuffer<BLENDFUNCTION> blendPtr = new StructureBuffer<BLENDFUNCTION>())
                {
                    blendPtr.Value = blendFunction;
                    pBlendFunction = blendPtr.Handle;

                    callback(this);
                }
            }

            prcExclude = pBlendFunction = IntPtr.Zero;
        }
    }
}
