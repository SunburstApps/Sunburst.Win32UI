using System;

namespace Sunburst.Win32UI.Interop
{
    public sealed class MessageReflector
    {
        #region Reflected Messages
        private const uint WM_REFLECTED_MESSAGE_BASE = WindowMessages.WM_USER + 0x1C00;
        public const uint WM_COMMAND_REFLECTED = WM_REFLECTED_MESSAGE_BASE + WindowMessages.WM_COMMAND;
        public const uint WM_CTLCOLORBTN_REFLECTED = WM_REFLECTED_MESSAGE_BASE + WindowMessages.WM_CTLCOLORBTN;
        public const uint WM_CTLCOLOREDIT_REFLECTED = WM_REFLECTED_MESSAGE_BASE + WindowMessages.WM_CTLCOLOREDIT;
        public const uint WM_CTLCOLORDLG_REFLECTED = WM_REFLECTED_MESSAGE_BASE + WindowMessages.WM_CTLCOLORDLG;
        public const uint WM_CTLCOLORLISTBOX_REFLECTED = WM_REFLECTED_MESSAGE_BASE + WindowMessages.WM_CTLCOLORLISTBOX;
        public const uint WM_CTLCOLORMSGBOX_REFLECTED = WM_REFLECTED_MESSAGE_BASE + WindowMessages.WM_CTLCOLORMSGBOX;
        public const uint WM_CTLCOLORSCROLLBAR_REFLECTED = WM_REFLECTED_MESSAGE_BASE + WindowMessages.WM_CTLCOLORSCROLLBAR;
        public const uint WM_CTLCOLORSTATIC_REFLECTED = WM_REFLECTED_MESSAGE_BASE + WindowMessages.WM_CTLCOLORSTATIC;
        public const uint WM_DRAWITEM_REFLECTED = WM_REFLECTED_MESSAGE_BASE + WindowMessages.WM_DRAWITEM;
        public const uint WM_MEASUREITEM_REFLECTED = WM_REFLECTED_MESSAGE_BASE + WindowMessages.WM_MEASUREITEM;
        public const uint WM_DELETEITEM_REFLECTED = WM_REFLECTED_MESSAGE_BASE + WindowMessages.WM_DELETEITEM;
        public const uint WM_VKEYTOITEM_REFLECTED = WM_REFLECTED_MESSAGE_BASE + WindowMessages.WM_VKEYTOITEM;
        public const uint WM_CHARTOITEM_REFLECTED = WM_REFLECTED_MESSAGE_BASE + WindowMessages.WM_CHARTOITEM;
        public const uint WM_COMPAREITEM_REFLECTED = WM_REFLECTED_MESSAGE_BASE + WindowMessages.WM_COMPAREITEM;
        public const uint WM_HSCROLL_REFLECTED = WM_REFLECTED_MESSAGE_BASE + WindowMessages.WM_HSCROLL;
        public const uint WM_VSCROLL_REFLECTED = WM_REFLECTED_MESSAGE_BASE + WindowMessages.WM_VSCROLL;
        public const uint WM_PARENTNOTIFY_REFLECTED = WM_REFLECTED_MESSAGE_BASE + WindowMessages.WM_PARENTNOTIFY;
        public const uint WM_NOTIFY_REFLECTED = WM_REFLECTED_MESSAGE_BASE + WindowMessages.WM_NOTIFY;
        #endregion

        private const string HandledProperty = "Sunburst.Win32UI.HandledReflectedMessage";

        public static unsafe void ReflectMessage(ref Message msg, out bool handled)
        {
            IntPtr hWndChild = IntPtr.Zero;

            if (msg.MessageId == WindowMessages.WM_COMMAND)
            {
                if (msg.LParam != IntPtr.Zero) hWndChild = msg.LParam;
            }
            else if (msg.MessageId == WindowMessages.WM_NOTIFY)
            {
                NMHDR* header = (NMHDR*)msg.LParam;
                hWndChild = header->hWndFrom;
            }
            else if (msg.MessageId == WindowMessages.WM_PARENTNOTIFY)
            {
                uint submessage = (uint)((int)msg.WParam & 0xFFFF);
                if (submessage == WindowMessages.WM_CREATE || submessage == WindowMessages.WM_DESTROY)
                {
                    hWndChild = msg.LParam;
                }
                else
                {
                    int controlId = ((int)msg.WParam >> 16) & 0xFFFF;
                    hWndChild = NativeMethods.GetDlgItem(msg.TargetHandle, controlId);
                }
            }
            else if (msg.MessageId == WindowMessages.WM_DRAWITEM)
            {
                if (msg.WParam != IntPtr.Zero)
                {
                    DRAWITEMSTRUCT* drawItemStruct = (DRAWITEMSTRUCT*)msg.WParam;
                    hWndChild = drawItemStruct->hwndItem;
                }
            }
            else if (msg.MessageId == WindowMessages.WM_MEASUREITEM)
            {
                if (msg.WParam != IntPtr.Zero)
                {
                    MEASUREITEMSTRUCT* measureItemStruct = (MEASUREITEMSTRUCT*)msg.WParam;
                    hWndChild = NativeMethods.GetDlgItem(msg.TargetHandle, (int)measureItemStruct->CtlID);
                }
            }
            else if (msg.MessageId == WindowMessages.WM_COMPAREITEM)
            {
                if (msg.WParam != IntPtr.Zero)
                {
                    COMPAREITEMSTRUCT* compareStruct = (COMPAREITEMSTRUCT*)msg.WParam;
                    hWndChild = compareStruct->hwndItem;
                }
            }
            else if (msg.MessageId == WindowMessages.WM_DELETEITEM)
            {
                if (msg.WParam != IntPtr.Zero)
                {
                    DELETEITEMSTRUCT* deleteStruct = (DELETEITEMSTRUCT*)msg.WParam;
                    hWndChild = deleteStruct->hwndItem;
                }
            }
            else if (msg.MessageId == WindowMessages.WM_VKEYTOITEM || msg.MessageId == WindowMessages.WM_CHARTOITEM ||
                     msg.MessageId == WindowMessages.WM_HSCROLL || msg.MessageId == WindowMessages.WM_VSCROLL)
            {
                hWndChild = msg.LParam;
            }
            else if (msg.MessageId == WindowMessages.WM_CTLCOLORBTN || msg.MessageId == WindowMessages.WM_CTLCOLORDLG ||
                     msg.MessageId == WindowMessages.WM_CTLCOLOREDIT || msg.MessageId == WindowMessages.WM_CTLCOLORMSGBOX ||
                     msg.MessageId == WindowMessages.WM_CTLCOLORMSGBOX || msg.MessageId == WindowMessages.WM_CTLCOLORSCROLLBAR ||
                     msg.MessageId == WindowMessages.WM_CTLCOLORSTATIC)
            {
                hWndChild = msg.LParam;
            }

            if (hWndChild == IntPtr.Zero)
            {
                handled = false;
                return;
            }

            if (!NativeMethods.IsWindow(hWndChild)) throw new InvalidOperationException("hwndChild not a valid HWND");

            // This code relies on the fact that NativeMethods.GetProp() returns IntPtr.Zero
            // if the property has not been set. This code only cares if the handled flag
            // was set to true. If the flag is false, this code behaves the same as if
            // SetReflectedMessageHandled() was never called at all.

            NativeMethods.RemoveProp(hWndChild, HandledProperty); // clear it out, just in case
            IntPtr result = NativeMethods.SendMessage(hWndChild, msg.MessageId + WM_REFLECTED_MESSAGE_BASE, msg.WParam, msg.LParam);
            handled = NativeMethods.GetProp(hWndChild, HandledProperty) != IntPtr.Zero;
            if (handled) msg.Result = result;
        }

        public static void SetReflectedMessageHandled(IWin32Window recipient, bool handled)
        {
            NativeMethods.SetProp(recipient.Handle, HandledProperty, (IntPtr)(handled ? 1 : 0));
        }
    }
}
