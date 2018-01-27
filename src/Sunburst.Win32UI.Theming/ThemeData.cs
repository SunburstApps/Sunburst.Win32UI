using System;
using System.Runtime.InteropServices;
using Sunburst.Win32UI.Graphics;
using Sunburst.Win32UI.Interop;

namespace Sunburst.Win32UI.Theming
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

        public ThemeData(Control window, string themeClassList)
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

        public void DrawBackground(GraphicsContext dc, int partId, int stateId, Rect drawRect)
        {
            AssertPartDefined(partId, stateId);
            int hr = NativeMethods.DrawThemeBackground(Handle, dc.Handle, partId, stateId, ref drawRect, IntPtr.Zero);
            if (hr != 0) Marshal.ThrowExceptionForHR(hr);
        }

        public void DrawBackground(GraphicsContext dc, int partId, int stateId, Rect drawRect, Rect clipRect)
        {
            AssertPartDefined(partId, stateId);
            using (StructureBuffer<Rect> clipRectPtr = new StructureBuffer<Rect>())
            {
                clipRectPtr.Value = clipRect;
                int hr = NativeMethods.DrawThemeBackground(Handle, dc.Handle, partId, stateId, ref drawRect, clipRectPtr.Handle);
                if (hr != 0) Marshal.ThrowExceptionForHR(hr);
            }
        }

        public void DrawText(GraphicsContext dc, int partId, int stateId, string text, Rect drawRect,
            TextAlignment halign = TextAlignment.Left, VerticalTextAlignment valign = VerticalTextAlignment.Top, StringDrawingFlags flags = 0)
        {
            AssertPartDefined(partId, stateId);
            int hr = NativeMethods.DrawThemeText(Handle, dc.Handle, partId, stateId, text, -1, GetGDIFlags(halign, valign, flags), 0, ref drawRect);
            if (hr != 0) Marshal.ThrowExceptionForHR(hr);
        }

        public Rect CalculateContentRect(GraphicsContext dc, int partId, int stateId, Rect boundingRect)
        {
            AssertPartDefined(partId, stateId);
            int hr = NativeMethods.GetThemeBackgroundContentRect(Handle, dc.Handle, partId, stateId, ref boundingRect, out var contentRect);
            if (hr != 0) Marshal.ThrowExceptionForHR(hr);

            return contentRect;
        }

        public Rect CalculateBoundingRect(GraphicsContext dc, int partId, int stateId, Rect contentRect)
        {
            AssertPartDefined(partId, stateId);
            int hr = NativeMethods.GetThemeBackgroundExtent(Handle, dc.Handle, partId, stateId, ref contentRect, out var boundingRect);
            if (hr != 0) Marshal.ThrowExceptionForHR(hr); ;

            return boundingRect;
        }

        public Size GetPartSize(GraphicsContext dc, int partId, int stateId, PartSizingMode mode, Rect rect)
        {
            AssertPartDefined(partId, stateId);
            int hr = NativeMethods.GetThemePartSize(Handle, dc.Handle, partId, stateId, ref rect, mode, out var size);
            if (hr != 0) Marshal.ThrowExceptionForHR(hr);

            return size;
        }

        public Rect GetTextSize(GraphicsContext dc, int partId, int stateId, string text, Rect drawRect,
            TextAlignment halign = TextAlignment.Left, VerticalTextAlignment valign = VerticalTextAlignment.Top, StringDrawingFlags flags = 0)
        {
            AssertPartDefined(partId, stateId);
            int hr = NativeMethods.GetThemeTextExtent(Handle, dc.Handle, partId, stateId, text, -1, GetGDIFlags(halign, valign, flags), ref drawRect, out var extentRect);
            if (hr != 0) Marshal.ThrowExceptionForHR(hr);

            return extentRect;
        }

        public Region GetBackgroundRegion(GraphicsContext dc, int partId, int stateId, Rect boundingRect)
        {
            AssertPartDefined(partId, stateId);
            int hr = NativeMethods.GetThemeBackgroundRegion(Handle, dc.Handle, partId, stateId, ref boundingRect, out var hRegion);
            if (hr != 0) Marshal.ThrowExceptionForHR(hr);

            return new Region(hRegion);
        }

        public void DrawImage(GraphicsContext dc, int partId, int stateId, Rect destinationRect, ImageList imageList, int imageIndex)
        {
            AssertPartDefined(partId, stateId);
            int hr = NativeMethods.DrawThemeIcon(Handle, dc.Handle, partId, stateId, ref destinationRect, imageList.Handle, imageIndex);
            if (hr != 0) Marshal.ThrowExceptionForHR(hr);
        }

        public void DrawImage(GraphicsContext dc, int partId, int stateId, Rect destinationRect, Bitmap bitmap, Color maskColor)
        {
            // Don't AssertPartDefined() here, as the other overload of DrawImage() will do that for us.
            using (ImageList list = new ImageList(bitmap.Size, 1, 1))
            {
                int index = list.AddImage(bitmap, maskColor);
                DrawImage(dc, partId, stateId, destinationRect, list, index);
            }
        }

        public Color GetColor(int partId, int stateId, int propId)
        {
            AssertPartDefined(partId, stateId);

            int hr = NativeMethods.GetThemeColor(Handle, partId, stateId, propId, out var color_ref);
            if (hr != 0) Marshal.ThrowExceptionForHR(hr);
            return Color.FromWin32Color(color_ref);
        }

        public int GetMetric(GraphicsContext dc, int partId, int stateId, int propId)
        {
            AssertPartDefined(partId, stateId);

            int hr = NativeMethods.GetThemeMetric(Handle, dc.Handle, partId, stateId, propId, out var value);
            if (hr != 0) Marshal.ThrowExceptionForHR(hr);
            return value;
        }

        public Point GetPosition(int partId, int stateId, int propId)
        {
            AssertPartDefined(partId, stateId);

            int hr = NativeMethods.GetThemePosition(Handle, partId, stateId, propId, out var value);
            if (hr != 0) Marshal.ThrowExceptionForHR(hr);
            return value;
        }

        public Font GetFont(GraphicsContext dc, int partId, int stateId, int propId)
        {
            AssertPartDefined(partId, stateId);

            int hr = NativeMethods.GetThemeFont(Handle, dc.Handle, partId, stateId, propId, out var value);
            if (hr != 0) Marshal.ThrowExceptionForHR(hr);
            return new Font(value);
        }

        public Rect GetRect(GraphicsContext dc, int partId, int stateId, int propId)
        {
            AssertPartDefined(partId, stateId);

            int hr = NativeMethods.GetThemeRect(Handle, dc.Handle, partId, stateId, propId, out var value);
            if (hr != 0) Marshal.ThrowExceptionForHR(hr);
            return value;
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
