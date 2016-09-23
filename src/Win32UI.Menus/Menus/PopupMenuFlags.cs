using System;

namespace Microsoft.Win32.UserInterface.Menus
{
    [Flags]
    public enum PopupMenuFlags : uint
    {
        // These must match the TPM_* values as passed to TrackPopUpMenuEx().

        AlignLeft = 0x0,
        AlignHorizontalCenter = 0x4,
        AlignRight = 0x8,
        AlignTop = 0x0,
        AlignVerticalCenter = 0x10,
        AlignBottom = 0x20,

        RightMouseMode = 0x2,
        PreferHorizontalAlignment = 0x0,
        PreferVerticalAlignment = 0x40
    }
}
