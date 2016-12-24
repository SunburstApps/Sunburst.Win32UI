using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.UserInterface.Graphics;
using Microsoft.Win32.UserInterface.Interop;

namespace Microsoft.Win32.UserInterface
{
    public class ThemeData : IDisposable
    {
        public IntPtr Handle { get; private set; }

        public ThemeData(Window window, string themeClassList)
        {
            Handle = NativeMethods.OpenThemeData(window.Handle, themeClassList);
            if (Handle == IntPtr.Zero) throw new System.ComponentModel.Win32Exception();
        }

        public void Dispose()
        {
            if (Handle != IntPtr.Zero)
            {
                int hr = NativeMethods.CloseThemeData(Handle);
                if (hr != 0) Marshal.ThrowExceptionForHR(hr);
            }
        }

        public void DrawBackground(NonOwnedGraphicsContext dc, int partId, int stateId, Rect drawRect)
        {
            int hr = NativeMethods.DrawThemeBackground(Handle, dc.Handle, partId, stateId, ref drawRect, IntPtr.Zero);
            if (hr != 0) Marshal.ThrowExceptionForHR(hr);
        }

        public void DrawBackground(NonOwnedGraphicsContext dc, int partId, int stateId, Rect drawRect, Rect clipRect)
        {
            using (StructureBuffer<Rect> clipRectPtr = new StructureBuffer<Rect>())
            {
                clipRectPtr.Value = clipRect;
                int hr = NativeMethods.DrawThemeBackground(Handle, dc.Handle, partId, stateId, ref drawRect, clipRectPtr.Handle);
                if (hr != 0) Marshal.ThrowExceptionForHR(hr);
            }
        }
    }
}
