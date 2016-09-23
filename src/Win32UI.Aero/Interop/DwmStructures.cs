using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.UserInterface.Graphics;

namespace Microsoft.Win32.UserInterface.Interop
{
    internal enum DwmBlurBehindFlags : int
    {
        Enable = 0x1,
        BlurRegion = 0x2,
        TransitionOnMaximize = 0x4
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct DwmBlurBehind
    {
        DwmBlurBehindFlags dwFlags;
        int fEnable;
        IntPtr hRgnBlur;
        int fTransitionOnMaximized;

        public DwmBlurBehind(bool enable)
        {
            fEnable = enable ? 1 : 0;
            hRgnBlur = IntPtr.Zero;
            fTransitionOnMaximized = 0;
            dwFlags = DwmBlurBehindFlags.Enable;
        }

        public NonOwnedRegion Region
        {
            get
            {
                return new NonOwnedRegion(hRgnBlur);
            }

            set
            {
                hRgnBlur = value.Handle;
                dwFlags |= DwmBlurBehindFlags.BlurRegion;
            }
        }

        public bool TransitionOnMaximized
        {
            get
            {
                return fTransitionOnMaximized > 0;
            }

            set
            {
                fTransitionOnMaximized = value ? 1 : 0;
                dwFlags |= DwmBlurBehindFlags.TransitionOnMaximize;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct DwmColorizationParameters
    {
        public uint Color1, Color2, Intensity;
        private uint _Unknown1, _Unknown2, _Unknown3;
        public uint Opaque;
    }
}
