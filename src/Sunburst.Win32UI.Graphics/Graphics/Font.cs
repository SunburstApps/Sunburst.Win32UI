using System;
using Sunburst.Win32UI.Interop;

namespace Sunburst.Win32UI.Graphics
{
    /// <summary>
    /// Wraps a Windows font (GDI <c>HFONT</c>).
    /// </summary>
    public class Font : IDisposable
    {
        private static LOGFONT CreatePointFontStruct(string fontName, int pointSize, bool bold, bool italic)
        {
            const byte DEFAULT_CHARSET = 1;
            const long FW_BOLD = 700;
            const int LOGPIXELSY = 90;

            LOGFONT font = new LOGFONT();
            font.lfCharSet = DEFAULT_CHARSET;
            font.lfFaceName = fontName;
            if (bold) font.lfWeight = FW_BOLD;
            if (italic) font.lfItalic = 1;

            using (GraphicsContext context = GraphicsContext.CreateOffscreenContext())
            {
                font.lfHeight = -NativeMethods.MulDiv(pointSize, context.GetCapability(LOGPIXELSY), 72);
            }

            return font;
        }

        public Font(string fontName, int pointSize, bool bold = false, bool italic = false)
            : this(CreatePointFontStruct(fontName, pointSize, bold, italic)) { }

        public Font(LOGFONT font_struct)
        {
            Handle = NativeMethods.CreateFontIndirect(ref font_struct);
        }

        /// <summary>
        /// Creates a new instance of Font.
        /// </summary>
        /// <param name="ptr">
        /// The native handle to the font data.
        /// </param>
        public Font(IntPtr ptr)
        {
            Handle = ptr;
        }

        public void Dispose()
        {
            NativeMethods.DeleteObject(Handle);
        }

        public LOGFONT GetFontDescriptor()
        {
            using (StructureBuffer<LOGFONT> ptr = new StructureBuffer<LOGFONT>())
            {
                int returnedSize = NativeMethods.GetObject(Handle, ptr.Size, ptr.Handle);
                if (ptr.Size != returnedSize)
                    throw new InvalidOperationException($"GetObject() returned incorrect size (got {returnedSize} bytes, expected {ptr.Size} bytes)");
                return ptr.Value;
            }
        }

        /// <summary>
        /// The native handle to the font data.
        /// </summary>
        public IntPtr Handle { get; protected set; }
    }
}
