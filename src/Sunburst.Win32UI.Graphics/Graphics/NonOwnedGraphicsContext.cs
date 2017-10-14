using System;
using Sunburst.Win32UI.Interop;

namespace Sunburst.Win32UI.Graphics
{
    public class NonOwnedGraphicsContext
    {
        protected NonOwnedGraphicsContext() : this(IntPtr.Zero) { }

        public NonOwnedGraphicsContext(IntPtr ptr)
        {
            Handle = ptr;
        }

        public IntPtr Handle { get; protected set; }

        public Bitmap CreateBitmap(int width, int height)
        {
            return new Bitmap(NativeMethods.CreateCompatibleBitmap(Handle, width, height));
        }

        public void Select(NonOwnedBitmap bitmap, Action action)
        {
            IntPtr oldObject = NativeMethods.SelectObject(Handle, bitmap.Handle);
            try
            {
                action();
            }
            finally
            {
                NativeMethods.SelectObject(Handle, oldObject);
            }
        }

        public void Select(NonOwnedBrush brush, Action action)
        {
            IntPtr oldObject = NativeMethods.SelectObject(Handle, brush.Handle);
            try
            {
                action();
            }
            finally
            {
                NativeMethods.SelectObject(Handle, oldObject);
            }
        }

        public void Select(NonOwnedFont font, Action action)
        {
            IntPtr oldObject = NativeMethods.SelectObject(Handle, font.Handle);
            try
            {
                action();
            }
            finally
            {
                NativeMethods.SelectObject(Handle, oldObject);
            }
        }

        public Color BackgroundColor
        {
            get
            {
                return Color.FromWin32Color(NativeMethods.GetBkColor(Handle));
            }

            set
            {
                NativeMethods.SetBkColor(Handle, Color.ToWin32Color(value));
            }
        }

        public Color TextColor
        {
            get
            {
                return Color.FromWin32Color(NativeMethods.GetTextColor(Handle));
            }

            set
            {
                NativeMethods.SetTextColor(Handle, Color.ToWin32Color(value));
            }
        }

        public NonOwnedPen CurrentPen
        {
            get
            {
                return new NonOwnedPen(NativeMethods.GetCurrentObject(Handle, GDIConstants.OBJ_PEN));
            }

            set
            {
                NativeMethods.SelectObject(Handle, value.Handle);
            }
        }

        public NonOwnedBrush CurrentBrush
        {
            get
            {
                return new NonOwnedBrush(NativeMethods.GetCurrentObject(Handle, GDIConstants.OBJ_BRUSH));
            }

            set
            {
                NativeMethods.SelectObject(Handle, value.Handle);
            }
        }

        public NonOwnedFont CurrentFont
        {
            get
            {
                return new NonOwnedFont(NativeMethods.GetCurrentObject(Handle, GDIConstants.OBJ_FONT));
            }

            set
            {
                NativeMethods.SelectObject(Handle, value.Handle);
            }
        }

        public NonOwnedBitmap CurrentBitmap
        {
            get
            {
                return new NonOwnedBitmap(NativeMethods.GetCurrentObject(Handle, GDIConstants.OBJ_BITMAP));
            }

            set
            {
                NativeMethods.SelectObject(Handle, value.Handle);
            }
        }

        public GraphicsContext CreateCompatibleContext()
        {
            return new GraphicsContext(NativeMethods.CreateCompatibleDC(Handle));
        }

        public int SaveState()
        {
            return NativeMethods.SaveDC(Handle);
        }

        public bool RestoreState(int savedStateId)
        {
            return NativeMethods.RestoreDC(Handle, savedStateId);
        }

        public int GetCapability(int capabilityId)
        {
            return NativeMethods.GetDeviceCaps(Handle, capabilityId);
        }

        public Rect BoundsRect
        {
            get
            {
                Rect retval = new Rect();
                NativeMethods.GetBoundsRect(Handle, ref retval, 0);
                return retval;
            }

            set
            {
                NativeMethods.SetBoundsRect(Handle, ref value, 0);
            }
        }

        public void FillRegion(NonOwnedRegion region, NonOwnedBrush brush)
        {
            NativeMethods.FillRgn(Handle, region.Handle, brush.Handle);
        }

        public void FillRect(Rect rect, NonOwnedBrush brush)
        {
            NativeMethods.FillRect(Handle, ref rect, brush.Handle);
        }

        public void FillRect(Rect rect, Color color1, Color color2, bool horizontalGradient)
        {
            TRIVERTEX[] points = new TRIVERTEX[] { new TRIVERTEX(), new TRIVERTEX() };

            points[0].x = rect.left;
            points[0].y = rect.top;
            points[0].Red = Convert.ToInt16(color1.Red << 8);
            points[0].Green = Convert.ToInt16(color1.Green << 8);
            points[0].Blue = Convert.ToInt16(color1.Blue << 8);

            points[1].x = rect.right;
            points[1].y = rect.bottom;
            points[1].Red = Convert.ToInt16(color2.Red << 8);
            points[1].Green = Convert.ToInt16(color2.Green << 8);
            points[1].Blue = Convert.ToInt16(color2.Blue << 8);

            GRADIENT_RECT gradientRect = new GRADIENT_RECT(0, 1);
            NativeMethods.GradientFill(Handle, points, 2, ref gradientRect, 1, horizontalGradient ? 0 : 1);
        }

        public void FrameRegion(NonOwnedRegion region, NonOwnedBrush brush, Size size)
        {
            NativeMethods.FrameRgn(Handle, region.Handle, brush.Handle,
                Convert.ToInt32(size.width), Convert.ToInt32(size.height));
        }

        public void FrameRect(Rect rect, NonOwnedBrush brush)
        {
            NativeMethods.FrameRect(Handle, ref rect, brush.Handle);
        }

        public bool InvertRegion(NonOwnedRegion region)
        {
            return NativeMethods.InvertRgn(Handle, region.Handle);
        }

        public Region GetClippingRegion()
        {
            Region initialRegion = Region.CreateRectangle(Rect.Zero);
            IntPtr ptr = initialRegion.Handle;
            int code = NativeMethods.GetClipRgn(Handle, ref ptr);
            if (code != 1)
            {
                initialRegion.Dispose();
                return null;
            }

            initialRegion.Handle = ptr;
            return initialRegion;
        }

        public int SetClippingRegion(NonOwnedRegion region, RegionCombinationMode combinationMode)
        {
            return NativeMethods.ExtSelectClipRgn(Handle, region.Handle,
                NonOwnedRegion.TranslateCombinationMode(combinationMode, nameof(combinationMode))); 
        }

        public bool DrawIcon(NonOwnedIcon icon, Point position, Size size)
        {
            return NativeMethods.DrawIconEx(Handle, Convert.ToInt32(position.x), Convert.ToInt32(position.y),
                icon.Handle, Convert.ToInt32(size.width), Convert.ToInt32(size.height), 0, IntPtr.Zero);
        }

        public bool BitBlt(NonOwnedGraphicsContext source, Rect frame)
        {
            return BitBlt(source, frame, Point.Zero);
        }

        public bool BitBlt(NonOwnedGraphicsContext source, Rect destinationFrame, Point sourcePoint)
        {
            const int ROP_SRCCOPY = 0x00CC0020;
            return NativeMethods.BitBlt(Handle,
                Convert.ToInt32(destinationFrame.left), Convert.ToInt32(destinationFrame.top),
                Convert.ToInt32(destinationFrame.Width), Convert.ToInt32(destinationFrame.Height),
                source.Handle, Convert.ToInt32(sourcePoint.x), Convert.ToInt32(sourcePoint.y), ROP_SRCCOPY);
        }

        public bool StretchBlt(NonOwnedGraphicsContext source, Rect destinationFrame, Rect sourceFrame)
        {
            const int ROP_SRCCOPY = 0x00CC0020;
            return NativeMethods.StretchBlt(Handle,
                Convert.ToInt32(destinationFrame.left), Convert.ToInt32(destinationFrame.top),
                Convert.ToInt32(destinationFrame.Width), Convert.ToInt32(destinationFrame.Height),
                source.Handle, Convert.ToInt32(sourceFrame.left), Convert.ToInt32(sourceFrame.top),
                Convert.ToInt32(sourceFrame.Width), Convert.ToInt32(sourceFrame.Height), ROP_SRCCOPY);
        }

        public bool AlphaBlend(NonOwnedBitmap source, Rect destinationFrame, Rect sourceFrame, byte masterOpacity = 255)
        {
            BLENDFUNCTION bf = new BLENDFUNCTION();
            bf.BlendOp = bf.AlphaFormat = bf.BlendFlags = 0;
            bf.SourceConstantAlpha = masterOpacity;

            return NativeMethods.AlphaBlend(Handle,
                Convert.ToInt32(destinationFrame.left), Convert.ToInt32(destinationFrame.top),
                Convert.ToInt32(destinationFrame.Width), Convert.ToInt32(destinationFrame.Height),
                source.Handle, Convert.ToInt32(sourceFrame.left), Convert.ToInt32(sourceFrame.top),
                Convert.ToInt32(sourceFrame.Width), Convert.ToInt32(sourceFrame.Height), bf);
        }

        public bool DrawText(string text, Point position, bool drawBackground = false)
        {
            const uint ETO_OPAQUE = 0x2;
            Rect frame = Rect.Zero;

            return NativeMethods.ExtTextOut(Handle, Convert.ToInt32(position.x), Convert.ToInt32(position.y),
                drawBackground ? ETO_OPAQUE : 0, ref frame, text, text.Length, null);
        }

        public bool DrawText(string text, Point position, Rect clipFrame, bool drawBackground = false)
        {
            uint flags = 0x4; // ETO_CLIPPED
            if (drawBackground) flags |= 0x2; // ETO_OPAQUE

            return NativeMethods.ExtTextOut(Handle, Convert.ToInt32(position.x), Convert.ToInt32(position.y),
                flags, ref clipFrame, text, text.Length, null);
        }

        public int DrawText(string text, Rect frame, TextAlignment halign = TextAlignment.Left, VerticalTextAlignment valign = VerticalTextAlignment.Top, StringDrawingFlags flags = 0)
        {
            uint formatFlags = 0;

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

            return NativeMethods.DrawText(Handle, text, -1, ref frame, formatFlags);
        }

        public Size GetSizeOfString(string text)
        {
            Size retval = new Size();
            bool success = NativeMethods.GetTextExtentPoint32(Handle, text, -1, ref retval);
            if (!success) throw new InvalidOperationException("GetTextExtentPoint32() failed");
            return retval;
        }

        public Size GetSizeOfString(string text, int? maximumWidth, TextAlignment halign = TextAlignment.Left, VerticalTextAlignment valign = VerticalTextAlignment.Top, StringDrawingFlags flags = 0)
        {
            uint formatFlags = GDIConstants.DT_CALCRECT;

            switch (halign)
            {
                case TextAlignment.Left: formatFlags |= GDIConstants.DT_LEFT; break;
                case TextAlignment.Center: formatFlags |= GDIConstants.DT_CENTER; break;
                case TextAlignment.Right: formatFlags |= GDIConstants.DT_RIGHT; break;
            }

            switch (valign)
            {
                case VerticalTextAlignment.Top: break; // there is no DT_* constant for top alignment
                case VerticalTextAlignment.Center: formatFlags |= GDIConstants.DT_VCENTER; break;
                case VerticalTextAlignment.Bottom: formatFlags |= GDIConstants.DT_BOTTOM; break;
            }

            if (flags.HasFlag(StringDrawingFlags.AddPathEllipsis)) formatFlags |= GDIConstants.DT_PATH_ELLIPSIS;
            else if (flags.HasFlag(StringDrawingFlags.AddWordEllipsis)) formatFlags |= GDIConstants.DT_WORD_ELLIPSIS;

            if (flags.HasFlag(StringDrawingFlags.BreakOnWords)) formatFlags |= GDIConstants.DT_WORDBREAK;
            if (flags.HasFlag(StringDrawingFlags.ExpandTabCharacters)) formatFlags |= GDIConstants.DT_EXPANDTABS;
            if (flags.HasFlag(StringDrawingFlags.IgnoreAmpersands)) formatFlags |= GDIConstants.DT_NOPREFIX;

            Rect frame = new Rect();
            frame.right = maximumWidth ?? int.MaxValue;
            NativeMethods.DrawText(Handle, text, -1, ref frame, formatFlags);
            return new Size(frame.right, frame.bottom);
        }

        public Point SetViewportOrigin(Point origin)
        {
            NativeMethods.SetViewportOrgEx(Handle, origin.x, origin.y, out var oldOrigin);
            return oldOrigin;
        }
    }
}
