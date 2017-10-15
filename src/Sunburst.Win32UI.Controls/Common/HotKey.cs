using System;
using System.Collections.Generic;
using System.Text;

namespace Sunburst.Win32UI.CommonControls
{
    public class HotKey : Window
    {
        public HotKey()
        {
        }

        public HotKey(IntPtr hWnd) : base(hWnd)
        {
        }

        public override string WindowClassName => "msctls_hotkey32";

        public void GetHotKey(out ushort keyCode, out HotKeyModifiers modifiers)
        {
            const uint HKM_GETHOTKEY = WindowMessages.WM_USER + 2;
            int dw = (int)SendMessage(HKM_GETHOTKEY, IntPtr.Zero, IntPtr.Zero);
            keyCode = (ushort)(dw & 0xFF);
            modifiers = (HotKeyModifiers)((dw >> 8) & 0xFF);
        }

        public void SetHotKey(ushort keyCode, HotKeyModifiers modifiers)
        {
            const uint HKM_SETHOTKEY = WindowMessages.WM_USER + 1;
            SendMessage(HKM_SETHOTKEY, (IntPtr)(((int)modifiers << 8) | (byte)keyCode), IntPtr.Zero);
        }

        public void SetRules(HotKeyModifiers invalidModifiers, HotKeyModifiers replacementModifiers)
        {
            const uint HKM_SETRULES = WindowMessages.WM_USER + 3;
            SendMessage(HKM_SETRULES, (IntPtr)((int)invalidModifiers), (IntPtr)((int)replacementModifiers));
        }
    }

    public enum HotKeyModifiers
    {
        None = 0x1,
        Shift = 0x2,
        Ctrl = 0x4,
        Alt = 0x8,

        ShiftCtrl = 0x10,
        ShiftAlt = 0x20,
        CtrlAlt = 0x40,
        ShiftCtrlalt = 0x80
    }
}
