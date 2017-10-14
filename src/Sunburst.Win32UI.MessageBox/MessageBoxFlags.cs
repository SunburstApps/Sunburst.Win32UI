using System;

namespace Sunburst.Win32UI
{
    [Flags]
    public enum MessageBoxFlags : uint
    {
        // These values must match the MB_* flags as documented on MSDN.

        ButtonOK = 0x0,
        ButtonOKCancel = 0x1,
        ButtonYesNoCancel = 0x3,
        ButtonYesNo = 0x4,
        ButtonRetryCancel = 0x5,
        ButtonCancelTryContinue = 0x6,
        ButtonHelp = 0x4000,

        IconWarning = 0x30,
        IconInformation = 0x40,
        IconError = 0x10,

        DefaultButtonFirst = 0x0,
        DefaultButtonSecond = 0x100,
        DefaultButtonThird = 0x200,
        DefaultButtonFourth = 0x300
    }
}
