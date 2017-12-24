using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Sunburst.Win32UI.Graphics;
using Sunburst.Win32UI.Interop;

namespace Sunburst.Win32UI.CommonControls
{
    public class TextBox : Control
    {
        #region Messages
        private const uint EM_GETSEL = 0x00B0;
        private const uint EM_SETSEL = 0x00B1;
        private const uint EM_GETRECT = 0x00B2;
        private const uint EM_SETRECT = 0x00B3;
        private const uint EM_SETRECTNP = 0x00B4;
        private const uint EM_SCROLL = 0x00B5;
        private const uint EM_LINESCROLL = 0x00B6;
        private const uint EM_SCROLLCARET = 0x00B7;
        private const uint EM_GETMODIFY = 0x00B8;
        private const uint EM_SETMODIFY = 0x00B9;
        private const uint EM_GETLINECOUNT = 0x00BA;
        private const uint EM_LINEINDEX = 0x00BB;
        private const uint EM_SETHANDLE = 0x00BC;
        private const uint EM_GETHANDLE = 0x00BD;
        private const uint EM_GETTHUMB = 0x00BE;
        private const uint EM_LINELENGTH = 0x00C1;
        private const uint EM_REPLACESEL = 0x00C2;
        private const uint EM_GETLINE = 0x00C4;
        private const uint EM_LIMITTEXT = 0x00C5;
        private const uint EM_CANUNDO = 0x00C6;
        private const uint EM_UNDO = 0x00C7;
        private const uint EM_FMTLINES = 0x00C8;
        private const uint EM_LINEFROMCHAR = 0x00C9;
        private const uint EM_SETTABSTOPS = 0x00CB;
        private const uint EM_SETPASSWORDCHAR = 0x00CC;
        private const uint EM_EMPTYUNDOBUFFER = 0x00CD;
        private const uint EM_GETFIRSTVISIBLELINE = 0x00CE;
        private const uint EM_SETREADONLY = 0x00CF;
        private const uint EM_SETWORDBREAKPROC = 0x00D0;
        private const uint EM_GETWORDBREAKPROC = 0x00D1;
        private const uint EM_GETPASSWORDCHAR = 0x00D2;
        private const uint EM_SETMARGINS = 0x00D3;
        private const uint EM_GETMARGINS = 0x00D4;
        private const uint EM_SETLIMITTEXT = EM_LIMITTEXT;
        private const uint EM_GETLIMITTEXT = 0x00D5;
        private const uint EM_POSFROMCHAR = 0x00D6;
        private const uint EM_CHARFROMPOS = 0x00D7;
        private const uint EM_SETIMESTATUS = 0x00D8;
        private const uint EM_GETIMESTATUS = 0x00D9;
        private const uint ECM_FIRST = 0x1500;
        private const uint EM_SETCUEBANNER = (ECM_FIRST + 1);
        private const uint EM_GETCUEBANNER = (ECM_FIRST + 2);
        private const uint EM_SHOWBALLOONTIP = (ECM_FIRST + 3);
        private const uint EM_HIDEBALLOONTIP = (ECM_FIRST + 4);
        private const uint EM_SETHILITE = (ECM_FIRST + 5);
        private const uint EM_GETHILITE = (ECM_FIRST + 6);
        private const uint EM_NOSETFOCUS = (ECM_FIRST + 7);
        private const uint EM_TAKEFOCUS = (ECM_FIRST + 8);
        #endregion

        public TextBox() : base() { }
        public TextBox(IntPtr hWnd) : base(hWnd) { }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassName = "EDIT";
                return cp;
            }
        }

        public bool CanUndo
        {
            get
            {
                return (int)SendMessage(EM_CANUNDO, IntPtr.Zero, IntPtr.Zero) == 1;
            }
        }

        public int LineCount
        {
            get
            {
                return (int)SendMessage(EM_GETLINECOUNT, IntPtr.Zero, IntPtr.Zero);
            }
        }

        public bool Modified
        {
            get
            {
                return (int)SendMessage(EM_GETMODIFY, IntPtr.Zero, IntPtr.Zero) == 1;
            }

            set
            {
                SendMessage(EM_SETMODIFY, (IntPtr)(value ? 1 : 0), IntPtr.Zero);
            }
        }

        public Rect ContentRect
        {
            get
            {
                using (StructureBuffer<Rect> rectPtr = new StructureBuffer<Rect>())
                {
                    SendMessage(EM_GETRECT, IntPtr.Zero, rectPtr.Handle);
                    return rectPtr.Value;
                }
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public Range SelectedRange
        {
            get
            {
                using (HGlobal startPtr = new HGlobal(Marshal.SizeOf<uint>())) using (HGlobal endPtr = new HGlobal(Marshal.SizeOf<uint>()))
                {
                    SendMessage(EM_GETSEL, startPtr.Handle, endPtr.Handle);
                    uint start = Convert.ToUInt32(Marshal.ReadInt32(startPtr.Handle)), end = Convert.ToUInt32(Marshal.ReadInt32(endPtr.Handle));
                    return new Range(start, end - start - 1);
                }
            }

            set
            {
                SetSelectedRangeNoScroll(value);
                SendMessage(EM_SCROLLCARET, IntPtr.Zero, IntPtr.Zero);
            }
        }

        public void SelectAll()
        {
            SelectAllNoScroll();
            SendMessage(EM_SCROLLCARET, IntPtr.Zero, IntPtr.Zero);
        }

        public void SelectAllNoScroll()
        {
            SendMessage(EM_SETSEL, IntPtr.Zero, (IntPtr)(-1));
        }

        public void SelectNone()
        {
            SelectNoneNoScroll();
            SendMessage(EM_SCROLLCARET, IntPtr.Zero, IntPtr.Zero);
        }

        public void SelectNoneNoScroll()
        {
            SendMessage(EM_SETSEL, (IntPtr)(-1), IntPtr.Zero);
        }

        public void SetSelectedRangeNoScroll(Range value)
        {
            SendMessage(EM_SETSEL, (IntPtr)Convert.ToInt32(value.Location), (IntPtr)Convert.ToInt32(value.MaxValue));
        }

        public void GetMargins(out uint leftMargin, out uint rightMargin)
        {
            uint ret = Convert.ToUInt32((int)SendMessage(EM_GETMARGINS, IntPtr.Zero, IntPtr.Zero));
            leftMargin = ret & 0xFFFF;
            rightMargin = (ret >> 8) & 0xFFFF;
        }

        public void SetMargins(uint leftMargin, uint rightMargin)
        {
            SendMessage(EM_SETMARGINS, (IntPtr)3, (IntPtr)(leftMargin | (rightMargin << 8)));
        }

        public uint LengthLimit
        {
            get
            {
                return Convert.ToUInt32((int)SendMessage(EM_GETLIMITTEXT, IntPtr.Zero, IntPtr.Zero));
            }

            set
            {
                SendMessage(EM_SETLIMITTEXT, (IntPtr)Convert.ToInt32(value), IntPtr.Zero);
            }
        }

        public Point GetCharacterLocation(uint characterPosition)
        {
            uint ret = Convert.ToUInt32((int)SendMessage(EM_POSFROMCHAR, (IntPtr)Convert.ToInt32(characterPosition), IntPtr.Zero));
            int x = Convert.ToInt32(ret & 0xFFFF);
            int y = Convert.ToInt32((ret >> 16) & 0xFFFF);
            return new Point(x, y);
        }

        public void GetCharacterAtPoint(Point pt, out int lineNumber, out int columnNumber)
        {
            int ret = (int)SendMessage(EM_CHARFROMPOS, IntPtr.Zero, (IntPtr)(pt.x | (pt.y << 8)));
            lineNumber = (ret >> 8) & 0xFFFF;
            columnNumber = ret & 0xFFFF;
        }

        public TextBoxWordBreakCallback WordBreakCallback
        {
            get
            {
                return Marshal.GetDelegateForFunctionPointer<TextBoxWordBreakCallback>(SendMessage(EM_GETWORDBREAKPROC, IntPtr.Zero, IntPtr.Zero));
            }

            set
            {
                SendMessage(EM_SETWORDBREAKPROC, IntPtr.Zero, Marshal.GetFunctionPointerForDelegate(value));
            }
        }

        public int FirstVisibleLine
        {
            get
            {
                return (int)SendMessage(EM_GETFIRSTVISIBLELINE, IntPtr.Zero, IntPtr.Zero);
            }
        }

        public int GetThumb()
        {
            return (int)SendMessage(EM_GETTHUMB, IntPtr.Zero, IntPtr.Zero);
        }

        public void SetReadOnly(bool isReadOnly = true)
        {
            SendMessage(EM_SETREADONLY, (IntPtr)(isReadOnly ? 1 : 0), IntPtr.Zero);
        }

        public void SetCueBannerText(string text, bool showOnFocus = false)
        {
            // Note: Even though the Edit control defines EM_GETCUEBANNER, I cannot properly implement it
            // in C# because it requires that a correctly-sized text buffer be passed in, but does not give
            // me any way to get the size that the buffer should be!
            using (HGlobal ptr = HGlobal.WithStringUni(text))
            {
                SendMessage(EM_SETCUEBANNER, (IntPtr)(showOnFocus ? 1 : 0), ptr.Handle);
            }
        }

        public void EmptyUndoBuffer()
        {
            SendMessage(EM_EMPTYUNDOBUFFER, IntPtr.Zero, IntPtr.Zero);
        }

        public bool FormatLines(bool addEOL)
        {
            return (int)SendMessage(EM_FMTLINES, (IntPtr)(addEOL ? 1 : 0), IntPtr.Zero) == 1;
        }

        public int GetLineForCharacter(int index)
        {
            return (int)SendMessage(EM_LINEFROMCHAR, (IntPtr)index, IntPtr.Zero);
        }

        public int GetStartingCharacterOfLine(int line)
        {
            return (int)SendMessage(EM_LINEINDEX, (IntPtr)line, IntPtr.Zero);
        }

        public int GetLengthOfLine(int line)
        {
            return (int)SendMessage(EM_LINELENGTH, (IntPtr)line, IntPtr.Zero);
        }

        public string GetLineText(int line)
        {
            int length = GetLengthOfLine(line);
            using (HGlobal ptr = new HGlobal((length + 1) * Marshal.SystemDefaultCharSize))
            {
                Marshal.WriteInt32(ptr.Handle, length + 1);
                SendMessage(EM_GETLINE, (IntPtr)line, ptr.Handle);
                return Marshal.PtrToStringUni(ptr.Handle);
            }
        }

        public void Scroll(int lineCount, int characterCount = 0)
        {
            SendMessage(EM_LINESCROLL, (IntPtr)LineCount, (IntPtr)characterCount);
        }

        public void ReplaceSelection(string newText, bool canUndo = false)
        {
            using (HGlobal ptr = HGlobal.WithStringUni(newText))
            {
                SendMessage(EM_REPLACESEL, (IntPtr)(canUndo ? 1 : 0), ptr.Handle);
            }
        }

        public void SetTabStops(int[] tabStops)
        {
            int count = tabStops.Length;
            using (HGlobal ptr = new HGlobal(count * Marshal.SizeOf<int>()))
            {
                for (int i = 0; i < count; i++) Marshal.WriteInt32(ptr.Handle, i * Marshal.SizeOf<int>(), tabStops[i]);
                SendMessage(EM_SETTABSTOPS, (IntPtr)count, ptr.Handle);
            }
        }

        // This method sets the positon of each stop to the same value.
        public void SetTabStops(int tabStop) => SetTabStops(new[] { tabStop });
        public void ClearTabStops() => SendMessage(EM_SETTABSTOPS, IntPtr.Zero, IntPtr.Zero);
        public void ScrollToInsertionPoint() => SendMessage(EM_SCROLLCARET, IntPtr.Zero, IntPtr.Zero);

        public void InsertText(int position, string text, bool scroll = false, bool allowUndo = false)
        {
            SelectedRange = new Range(Convert.ToUInt32(position), 0U);
            ReplaceSelection(text, allowUndo);
        }

        public void AppendText(string text, bool scroll = false, bool allowUndo = false)
        {
            // FIXME: Use GetWindowTextLength() directly here instead.
            InsertText(GetText().Length, text, scroll, allowUndo);
        }

        public void ShowBalloonTip(EDITBALLOONTIP tipInfo)
        {
            using (StructureBuffer<EDITBALLOONTIP> tipPtr = new StructureBuffer<EDITBALLOONTIP>())
            {
                tipPtr.Value = tipInfo;
                SendMessage(EM_SHOWBALLOONTIP, IntPtr.Zero, tipPtr.Handle);
            }
        }

        public void HideBalloonTip()
        {
            SendMessage(EM_HIDEBALLOONTIP, IntPtr.Zero, IntPtr.Zero);
        }

        public Range HighlightedRange
        {
            get
            {
                int range = (int)SendMessage(EM_GETHILITE, IntPtr.Zero, IntPtr.Zero);
                uint start = Convert.ToUInt32(range & 0xFFFF), end = Convert.ToUInt32((range >> 8) & 0xFFFF);
                return new Range(start, end - start);
            }

            set
            {
                SendMessage(EM_SETHILITE, (IntPtr)value.Location, (IntPtr)value.MaxValue);
            }
        }

        public bool Undo() => (int)SendMessage(WindowMessages.WM_UNDO, IntPtr.Zero, IntPtr.Zero) == 1;
        public void Clear() => SendMessage(WindowMessages.WM_CLEAR, IntPtr.Zero, IntPtr.Zero);
        public void Cut() => SendMessage(WindowMessages.WM_CUT, IntPtr.Zero, IntPtr.Zero);
        public void Copy() => SendMessage(WindowMessages.WM_COPY, IntPtr.Zero, IntPtr.Zero);
        public void Paste() => SendMessage(WindowMessages.WM_PASTE, IntPtr.Zero, IntPtr.Zero);
    }

    [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    public delegate int TextBoxWordBreakCallback(string lpch, int ichCurrent, int cch, int code);

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct EDITBALLOONTIP
    {
        public int cbStruct;
        public string pszTitle;
        public string pszText;
        public ToolTipIcon icon;
    }

    public enum ToolTipIcon
    {
        // Must match TTI_* values in CommCtrl.h
        None = 0,
        Info = 1,
        Warning = 2,
        Error = 3,
        InfoLarge = 4,
        WarningLarge = 5,
        ErrorLarge = 6
    }
}
