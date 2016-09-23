using Microsoft.Win32.UserInterface.Graphics;

namespace Microsoft.Win32.UserInterface.Interop
{
    internal struct BP_ANIMATIONPARAMS
    {
        public int cbSize;
        public int dwFlags;
        public AnimationStyle style;
        public int dwDuration;
    }
}
