namespace Sunburst.Win32UI.Interop
{
    internal struct GRADIENT_RECT
    {
        public GRADIENT_RECT(uint top, uint bot)
        {
            Top = top;
            Bottom = bot;
        }

        public uint Top;
        public uint Bottom;
    }
}
