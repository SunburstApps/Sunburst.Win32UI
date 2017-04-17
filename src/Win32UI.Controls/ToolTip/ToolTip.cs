using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.UserInterface.Graphics;
using Microsoft.Win32.UserInterface.Interop;

namespace Microsoft.Win32.UserInterface.CommonControls
{
    public class ToolTip : Window
    {
        #region Styles
        public const int TTS_ALWAYSTIP = 0x01;
        public const int TTS_NOPREFIX = 0x02;
        public const int TTS_NOANIMATE = 0x10;
        public const int TTS_NOFADE = 0x20;
        public const int TTS_BALLOON = 0x40;
        public const int TTS_CLOSE = 0x80;
        public const int TTS_USEVISUALSTYLE = 0x100;
        #endregion

        #region Messages
        private const uint TTM_ACTIVATE = (WindowMessages.WM_USER + 1);
        private const uint TTM_SETDELAYTIME = (WindowMessages.WM_USER + 3);
        private const uint TTM_ADDTOOLA = (WindowMessages.WM_USER + 4);
        private const uint TTM_ADDTOOLW = (WindowMessages.WM_USER + 50);
        private const uint TTM_DELTOOLA = (WindowMessages.WM_USER + 5);
        private const uint TTM_DELTOOLW = (WindowMessages.WM_USER + 51);
        private const uint TTM_NEWTOOLRECTA = (WindowMessages.WM_USER + 6);
        private const uint TTM_NEWTOOLRECTW = (WindowMessages.WM_USER + 52);
        private const uint TTM_RELAYEVENT = (WindowMessages.WM_USER + 7);
        private const uint TTM_GETTOOLINFOA = (WindowMessages.WM_USER + 8);
        private const uint TTM_GETTOOLINFOW = (WindowMessages.WM_USER + 53);
        private const uint TTM_SETTOOLINFOA = (WindowMessages.WM_USER + 9);
        private const uint TTM_SETTOOLINFOW = (WindowMessages.WM_USER + 54);
        private const uint TTM_HITTESTA = (WindowMessages.WM_USER + 10);
        private const uint TTM_HITTESTW = (WindowMessages.WM_USER + 55);
        private const uint TTM_GETTEXTA = (WindowMessages.WM_USER + 11);
        private const uint TTM_GETTEXTW = (WindowMessages.WM_USER + 56);
        private const uint TTM_UPDATETIPTEXTA = (WindowMessages.WM_USER + 12);
        private const uint TTM_UPDATETIPTEXTW = (WindowMessages.WM_USER + 57);
        private const uint TTM_GETTOOLCOUNT = (WindowMessages.WM_USER + 13);
        private const uint TTM_ENUMTOOLSA = (WindowMessages.WM_USER + 14);
        private const uint TTM_ENUMTOOLSW = (WindowMessages.WM_USER + 58);
        private const uint TTM_GETCURRENTTOOLA = (WindowMessages.WM_USER + 15);
        private const uint TTM_GETCURRENTTOOLW = (WindowMessages.WM_USER + 59);
        private const uint TTM_WINDOWFROMPOINT = (WindowMessages.WM_USER + 16);
        private const uint TTM_TRACKACTIVATE = (WindowMessages.WM_USER + 17);
        private const uint TTM_TRACKPOSITION = (WindowMessages.WM_USER + 18);
        private const uint TTM_SETTIPBKCOLOR = (WindowMessages.WM_USER + 19);
        private const uint TTM_SETTIPTEXTCOLOR = (WindowMessages.WM_USER + 20);
        private const uint TTM_GETDELAYTIME = (WindowMessages.WM_USER + 21);
        private const uint TTM_GETTIPBKCOLOR = (WindowMessages.WM_USER + 22);
        private const uint TTM_GETTIPTEXTCOLOR = (WindowMessages.WM_USER + 23);
        private const uint TTM_SETMAXTIPWIDTH = (WindowMessages.WM_USER + 24);
        private const uint TTM_GETMAXTIPWIDTH = (WindowMessages.WM_USER + 25);
        private const uint TTM_SETMARGIN = (WindowMessages.WM_USER + 26);
        private const uint TTM_GETMARGIN = (WindowMessages.WM_USER + 27);
        private const uint TTM_POP = (WindowMessages.WM_USER + 28);
        private const uint TTM_UPDATE = (WindowMessages.WM_USER + 29);
        private const uint TTM_GETBUBBLESIZE = (WindowMessages.WM_USER + 30);
        private const uint TTM_ADJUSTRECT = (WindowMessages.WM_USER + 31);
        private const uint TTM_SETTITLEA = (WindowMessages.WM_USER + 32);
        private const uint TTM_SETTITLEW = (WindowMessages.WM_USER + 33);
        private const uint TTM_POPUP = (WindowMessages.WM_USER + 34);
        private const uint TTM_GETTITLE = (WindowMessages.WM_USER + 35);
        private const uint TTM_SETWINDOWTHEME = 0x200B;

        private const uint TTM_ADDTOOL = TTM_ADDTOOLW;
        private const uint TTM_DELTOOL = TTM_DELTOOLW;
        private const uint TTM_NEWTOOLRECT = TTM_NEWTOOLRECTW;
        private const uint TTM_GETTOOLINFO = TTM_GETTOOLINFOW;
        private const uint TTM_SETTOOLINFO = TTM_SETTOOLINFOW;
        private const uint TTM_HITTEST = TTM_HITTESTW;
        private const uint TTM_GETTEXT = TTM_GETTEXTW;
        private const uint TTM_UPDATETIPTEXT = TTM_UPDATETIPTEXTW;
        private const uint TTM_ENUMTOOLS = TTM_ENUMTOOLSW;
        private const uint TTM_GETCURRENTTOOL = TTM_GETCURRENTTOOLW;
        private const uint TTM_SETTITLE = TTM_SETTITLEW;
        #endregion

        public const string WindowClass = "tooltips_class32";
        public override string WindowClassName => WindowClass;

        public ToolTipInfo GetTipInfo(Window owner)
        {
            ToolTipInfo info = new ToolTipInfo();
            info.ShowOnEntireOwner = true;
            info.Owner = owner;

            using (StructureBuffer<TOOLINFOW> ptr = new StructureBuffer<TOOLINFOW>())
            {
                ptr.Value = info.ToNativeStruct();
                bool success = (int)SendMessage(TTM_GETTOOLINFO, IntPtr.Zero, ptr.Handle) == 1;
                if (!success) throw new System.ComponentModel.Win32Exception();
                return new ToolTipInfo(ptr.Value);
            }
        }

        public ToolTipInfo GetTipInfo(Window owner, int tipId)
        {
            ToolTipInfo info = new ToolTipInfo();
            info.Owner = owner;
            info.ToolTipId = tipId;

            using (StructureBuffer<TOOLINFOW> ptr = new StructureBuffer<TOOLINFOW>())
            {
                ptr.Value = info.ToNativeStruct();
                bool success = (int)SendMessage(TTM_GETTOOLINFO, IntPtr.Zero, ptr.Handle) == 1;
                if (!success) throw new System.ComponentModel.Win32Exception();
                return new ToolTipInfo(ptr.Value);
            }
        }

        public void AddTip(ToolTipInfo info)
        {
            using (StructureBuffer<TOOLINFOW> ptr = new StructureBuffer<TOOLINFOW>())
            {
                ptr.Value = info.ToNativeStruct();
                SendMessage(TTM_ADDTOOL, IntPtr.Zero, ptr.Handle);
            }
        }

        public void SetTipInfo(ToolTipInfo info)
        {
            if (info.Text.Length > 79) throw new ArgumentException($"{nameof(ToolTipInfo)}.{nameof(ToolTipInfo.Text)} too long (maximum 79 characters)");

            using (StructureBuffer<TOOLINFOW> ptr = new StructureBuffer<TOOLINFOW>())
            {
                ptr.Value = info.ToNativeStruct();
                SendMessage(TTM_SETTOOLINFO, IntPtr.Zero, ptr.Handle);
            }
        }

        public ToolTipInfo GetToolTip(int index)
        {
            TOOLINFOW nativeInfo = new TOOLINFOW();
            nativeInfo.cbSize = Marshal.SizeOf<TOOLINFOW>();

            using (StructureBuffer<TOOLINFOW> ptr = new StructureBuffer<TOOLINFOW>())
            {
                ptr.Value = nativeInfo;
                SendMessage(TTM_ENUMTOOLS, (IntPtr)index, ptr.Handle);
                return new ToolTipInfo(ptr.Value);
            }
        }

        public int ToolTipCount => (int)SendMessage(TTM_GETTOOLCOUNT, IntPtr.Zero, IntPtr.Zero);

        public TimeSpan GetTiming(ToolTipTimingType type)
        {
            return TimeSpan.FromMilliseconds(Convert.ToDouble((int)SendMessage(TTM_GETDELAYTIME, (IntPtr)(int)type, IntPtr.Zero)));
        }

        public void SetTiming(ToolTipTimingType type, TimeSpan duration)
        {
            SendMessage(TTM_SETDELAYTIME, (IntPtr)(int)type, (IntPtr)Convert.ToInt32(Math.Round(duration.TotalMilliseconds)));
        }

        public Rect TextMargin
        {
            get
            {
                using (StructureBuffer<Rect> ptr = new StructureBuffer<Rect>())
                {
                    SendMessage(TTM_GETMARGIN, IntPtr.Zero, ptr.Handle);
                    return ptr.Value;
                }
            }

            set
            {
                using (StructureBuffer<Rect> ptr = new StructureBuffer<Rect>())
                {
                    ptr.Value = value;
                    SendMessage(TTM_SETMARGIN, IntPtr.Zero, ptr.Handle);
                }
            }
        }

        public int MaximumWidth
        {
            get
            {
                return (int)SendMessage(TTM_GETMAXTIPWIDTH, IntPtr.Zero, IntPtr.Zero);
            }

            set
            {
                SendMessage(TTM_SETMAXTIPWIDTH, IntPtr.Zero, (IntPtr)value);
            }
        }

        public Color BackgroundColor
        {
            get
            {
                return Color.FromWin32Color((int)SendMessage(TTM_GETTIPBKCOLOR, IntPtr.Zero, IntPtr.Zero));
            }

            set
            {
                SendMessage(TTM_SETTIPBKCOLOR, (IntPtr)Color.ToWin32Color(value), IntPtr.Zero);
            }
        }

        public Color TextColor
        {
            get
            {
                return Color.FromWin32Color((int)SendMessage(TTM_GETTIPTEXTCOLOR, IntPtr.Zero, IntPtr.Zero));
            }

            set
            {
                SendMessage(TTM_SETTIPTEXTCOLOR, (IntPtr)Color.ToWin32Color(value), IntPtr.Zero);
            }
        }

        public ToolTipInfo CurrentTool
        {
            get
            {
                using (StructureBuffer<TOOLINFOW> ptr = new StructureBuffer<TOOLINFOW>())
                {
                    SendMessage(TTM_GETCURRENTTOOL, IntPtr.Zero, ptr.Handle);
                    return new ToolTipInfo(ptr.Value);
                }
            }
        }

        public Size GetBubbleSize(Window owner)
        {
            ToolTipInfo info = new ToolTipInfo();
            info.ShowOnEntireOwner = true;
            info.Owner = owner;

            using (StructureBuffer<TOOLINFOW> ptr = new StructureBuffer<TOOLINFOW>())
            {
                ptr.Value = info.ToNativeStruct();
                int combined = (int)SendMessage(TTM_GETBUBBLESIZE, IntPtr.Zero, ptr.Handle);
                return new Size((combined & 0xFFFF), (int)((combined >> 16) & 0xFFFF));
            }
        }

        public bool SetTitle(string title, NonOwnedIcon icon)
        {
            if (title.Length > 99) throw new ArgumentException("Title too long (maximum 99 characters)", nameof(title));
            using (HGlobal ptr = HGlobal.WithStringUni(title))
            {
                return (int)SendMessage(TTM_SETTITLE, icon.Handle, ptr.Handle) == 1;
            }
        }

        public bool SetTitle(string title, ToolTipIcon icon = ToolTipIcon.None)
        {
            if (title.Length > 99) throw new ArgumentException("Title too long (maximum 99 characters)", nameof(title));
            using (HGlobal ptr = HGlobal.WithStringUni(title))
            {
                return (int)SendMessage(TTM_SETTITLE, (IntPtr)(int)icon, ptr.Handle) == 1;
            }
        }

        public void SetToolTipEnabled(bool enabled)
        {
            SendMessage(TTM_ACTIVATE, (IntPtr)(enabled ? 1 : 0), IntPtr.Zero);
        }

        public void HideToolTip() => SendMessage(TTM_POP, IntPtr.Zero, IntPtr.Zero);
        public void ShowToolTip() => SendMessage(TTM_POPUP, IntPtr.Zero, IntPtr.Zero);
    }

    public enum ToolTipTimingType
    {
        // Note: These must match the TTDT_* values in CommCtrl.h.
        /// <summary>
        /// The time before the tooltip will change as the pointer moves between controls.
        /// </summary>
        AppearanceChangeLimit = 1,
        /// <summary>
        /// The maximum time the tooltip is visible, given a stationary pointer.
        /// </summary>
        AppearanceLimit = 2,
        /// <summary>
        /// The time delay before the tooltip appears, given a stationary pointer.
        /// </summary>
        AppearanceDelay = 3
    }
}
