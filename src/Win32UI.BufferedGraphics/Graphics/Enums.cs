using System;

namespace Microsoft.Win32.UserInterface.Graphics
{
    public enum BufferingFormat
    {
        // These must match BP_BUFFERFORMAT in UxTheme.h
        CompatibleBitmap = 0,
        DeviceIndependentBitmap = 1,
        TopDownDeviceIndependentBitmap = 2,
        TopDownMonochromeBitmap = 3,

        Composited = TopDownDeviceIndependentBitmap
    }

    public enum AnimationStyle
    {
        // These must match BP_ANIMATIONSTYLE in UxTheme.h
        None = 0,
        Linear = 1,
        Cubic = 2,
        Sinusoid = 3
    }

    [Flags]
    public enum BufferedPaintFlags
    {
        // These must match BPPF_* in UxTheme.h
        Erase = 0x1,
        DoNotApplyTargetClipRegion = 0x2,
        UseNonClientContext = 0x4
    }
}
