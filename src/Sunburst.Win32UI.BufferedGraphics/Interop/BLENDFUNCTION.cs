namespace Sunburst.Win32UI.Interop
{
    internal struct BLENDFUNCTION
    {
        public byte BlendOp;
        public byte BlendFlags;
        public byte SourceConstantAlpha;
        public byte AlphaFormat;

        public const byte AC_SRC_OVER = 0, AC_SRC_ALPHA = 1;
    }
}
