using System;
using System.Runtime.InteropServices;
using Sunburst.Win32UI.Graphics;
using Sunburst.Win32UI.Interop;

namespace Sunburst.Win32UI.CommonControls
{
    public sealed class ToolTipInfo
    {
        public ToolTipInfo() { }

        internal ToolTipInfo(TOOLINFOW nativeStruct)
        {
            const int TTF_IDISHWND = 0x1, TTF_PARSELINKS = 0x1000;

            if ((nativeStruct.uFlags & TTF_IDISHWND) == TTF_IDISHWND)
            {
                ShowOnEntireOwner = true;
                Owner = new Window(nativeStruct.uId);
            }
            else
            {
                ShowOnEntireOwner = false;
                Owner = new Window(nativeStruct.hWnd);
                ToolTipId = (int)nativeStruct.uId;
            }

            Rect = nativeStruct.rect;
            Text = nativeStruct.lpszText;
            ParseHyperlinks = (nativeStruct.uFlags & TTF_PARSELINKS) == TTF_PARSELINKS;
        }

        internal TOOLINFOW ToNativeStruct()
        {
            const int TTF_IDISHWND = 0x1, TTF_TRANSPARENT = 0x100, TTF_SUBCLASS = 0x10, TTF_PARSELINKS = 0x1000;

            TOOLINFOW info = new TOOLINFOW();
            info.cbSize = Marshal.SizeOf<TOOLINFOW>();
            info.uFlags = TTF_TRANSPARENT | TTF_SUBCLASS | (ParseHyperlinks ? TTF_PARSELINKS : 0);

            if (ShowOnEntireOwner)
            {
                info.uFlags |= TTF_IDISHWND;
                info.uId = Owner.Handle;
                info.hWnd = Owner.Parent.Handle;
            }
            else
            {
                info.uId = (IntPtr)ToolTipId;
                info.hWnd = Owner.Handle;
                info.rect = Rect;
            }

            info.hinst = IntPtr.Zero;
            info.lpszText = Text;
            return info;
        }

        public Window Owner { get; set; }
        public Rect Rect { get; set; }
        public string Text { get; set; } = "";
        public bool ParseHyperlinks { get; set; } = false;
        public int ToolTipId { get; set; } = 0;

        /// <summary>
        /// If this is set, the entire rect of the owner Window will be used to show the tooltip.
        /// In that case, <see cref="Rect"/> and <see cref="ToolTipId"/> are ignored.
        /// </summary>
        public bool ShowOnEntireOwner { get; set; } = false;
    }
}
