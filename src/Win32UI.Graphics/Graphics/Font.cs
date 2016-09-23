using System;
using Microsoft.Win32.UserInterface.Interop;

namespace Microsoft.Win32.UserInterface.Graphics
{
    /// <summary>
    /// Wraps a Windows font.
    /// </summary>
    public class Font : NonOwnedFont, IDisposable
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

        public Font(IntPtr ptr) : base(ptr) { }

        public void Dispose()
        {
            NativeMethods.DeleteObject(Handle);
        }
    }
}
