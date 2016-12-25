using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.UserInterface.Graphics;
using Microsoft.Win32.UserInterface.Interop;

namespace Microsoft.Win32.UserInterface.Theming
{
    public class ThemeData : IDisposable
    {
        private static int GetGDIFlags(TextAlignment halign, VerticalTextAlignment valign, StringDrawingFlags flags)
        {
            int formatFlags = 0;

            switch (halign)
            {
                case TextAlignment.Left: formatFlags |= GDIConstants.DT_LEFT; break;
                case TextAlignment.Center: formatFlags |= GDIConstants.DT_CENTER; break;
                case TextAlignment.Right: formatFlags |= GDIConstants.DT_RIGHT; break;
            }

            switch (valign)
            {
                case VerticalTextAlignment.Top: break; // there is no DT_* value for top alignmrnt
                case VerticalTextAlignment.Center: formatFlags |= GDIConstants.DT_VCENTER; break;
                case VerticalTextAlignment.Bottom: formatFlags |= GDIConstants.DT_BOTTOM; break;
            }

            if (flags.HasFlag(StringDrawingFlags.AddPathEllipsis))
            {
                formatFlags |= GDIConstants.DT_PATH_ELLIPSIS;
            }
            else if (flags.HasFlag(StringDrawingFlags.AddWordEllipsis))
            {
                formatFlags |= GDIConstants.DT_WORD_ELLIPSIS;
            }

            if (flags.HasFlag(StringDrawingFlags.BreakOnWords)) formatFlags |= GDIConstants.DT_WORDBREAK;
            if (flags.HasFlag(StringDrawingFlags.ExpandTabCharacters)) formatFlags |= GDIConstants.DT_EXPANDTABS;
            if (flags.HasFlag(StringDrawingFlags.IgnoreAmpersands)) formatFlags |= GDIConstants.DT_NOPREFIX;

            return formatFlags;
        }

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
            AssertPartDefined(partId, stateId);
            int hr = NativeMethods.DrawThemeBackground(Handle, dc.Handle, partId, stateId, ref drawRect, IntPtr.Zero);
            if (hr != 0) Marshal.ThrowExceptionForHR(hr);
        }

        public void DrawBackground(NonOwnedGraphicsContext dc, int partId, int stateId, Rect drawRect, Rect clipRect)
        {
            AssertPartDefined(partId, stateId);
            using (StructureBuffer<Rect> clipRectPtr = new StructureBuffer<Rect>())
            {
                clipRectPtr.Value = clipRect;
                int hr = NativeMethods.DrawThemeBackground(Handle, dc.Handle, partId, stateId, ref drawRect, clipRectPtr.Handle);
                if (hr != 0) Marshal.ThrowExceptionForHR(hr);
            }
        }

        public void DrawText(NonOwnedGraphicsContext dc, int partId, int stateId, string text, Rect drawRect,
            TextAlignment halign = TextAlignment.Left, VerticalTextAlignment valign = VerticalTextAlignment.Top, StringDrawingFlags flags = 0)
        {
            AssertPartDefined(partId, stateId);
            int hr = NativeMethods.DrawThemeText(Handle, dc.Handle, partId, stateId, text, -1, GetGDIFlags(halign, valign, flags), 0, ref drawRect);
            if (hr != 0) Marshal.ThrowExceptionForHR(hr);
        }

        public Rect CalculateContentRect(NonOwnedGraphicsContext dc, int partId, int stateId, Rect boundingRect)
        {
            AssertPartDefined(partId, stateId);
            int hr = NativeMethods.GetThemeBackgroundContentRect(Handle, dc.Handle, partId, stateId, ref boundingRect, out var contentRect);
            if (hr != 0) Marshal.ThrowExceptionForHR(hr);

            return contentRect;
        }

        public Rect CalculateBoundingRect(NonOwnedGraphicsContext dc, int partId, int stateId, Rect contentRect)
        {
            AssertPartDefined(partId, stateId);
            int hr = NativeMethods.GetThemeBackgroundExtent(Handle, dc.Handle, partId, stateId, ref contentRect, out var boundingRect);
            if (hr != 0) Marshal.ThrowExceptionForHR(hr); ;

            return boundingRect;
        }

        public Size GetPartSize(NonOwnedGraphicsContext dc, int partId, int stateId, PartSizingMode mode, Rect rect)
        {
            AssertPartDefined(partId, stateId);
            int hr = NativeMethods.GetThemePartSize(Handle, dc.Handle, partId, stateId, ref rect, mode, out var size);
            if (hr != 0) Marshal.ThrowExceptionForHR(hr);

            return size;
        }

        public Rect GetTextSize(NonOwnedGraphicsContext dc, int partId, int stateId, string text, Rect drawRect,
            TextAlignment halign = TextAlignment.Left, VerticalTextAlignment valign = VerticalTextAlignment.Top, StringDrawingFlags flags = 0)
        {
            AssertPartDefined(partId, stateId);
            int hr = NativeMethods.GetThemeTextExtent(Handle, dc.Handle, partId, stateId, text, -1, GetGDIFlags(halign, valign, flags), ref drawRect, out var extentRect);
            if (hr != 0) Marshal.ThrowExceptionForHR(hr);

            return extentRect;
        }

        public Region GetBackgroundRegion(NonOwnedGraphicsContext dc, int partId, int stateId, Rect boundingRect)
        {
            AssertPartDefined(partId, stateId);
            int hr = NativeMethods.GetThemeBackgroundRegion(Handle, dc.Handle, partId, stateId, ref boundingRect, out var hRegion);
            if (hr != 0) Marshal.ThrowExceptionForHR(hr);

            return new Region(hRegion);
        }

        public void DrawImage(NonOwnedGraphicsContext dc, int partId, int stateId, Rect destinationRect, NonOwnedImageList imageList, int imageIndex)
        {
            AssertPartDefined(partId, stateId);
            int hr = NativeMethods.DrawThemeIcon(Handle, dc.Handle, partId, stateId, ref destinationRect, imageList.Handle, imageIndex);
            if (hr != 0) Marshal.ThrowExceptionForHR(hr);
        }

        public void DrawImage(NonOwnedGraphicsContext dc, int partId, int stateId, Rect destinationRect, NonOwnedBitmap bitmap, Color maskColor)
        {
            // Don't AssertPartDefined() here, as the other overload of DrawImage() will do that for us.
            using (ImageList list = new ImageList(bitmap.Size, 1, 1))
            {
                int index = list.AddImage(bitmap, maskColor);
                DrawImage(dc, partId, stateId, destinationRect, list, index);
            }
        }

        public bool IsPartDefined(int partId, int stateId) => NativeMethods.IsThemePartDefined(Handle, partId, stateId);
        private void AssertPartDefined(int partId, int stateId)
        {
            if (!IsPartDefined(partId, stateId))
            {
                throw new ArgumentException("Combination of partId and stateId passed is not defined for the current theme.");
            }
        }
    }
}
