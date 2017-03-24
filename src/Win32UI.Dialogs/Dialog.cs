using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Win32.UserInterface.Graphics;
using Microsoft.Win32.UserInterface.Interop;

namespace Microsoft.Win32.UserInterface
{
    public class Dialog : EventedWindow
    {
        private static int mTopmostDialogTag = 1;
        private static readonly Dictionary<int, Dialog> mDialogs;

        static Dialog()
        {
            mDialogs = new Dictionary<int, Dialog>();
        }

        internal static IntPtr DialogProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            try
            {
                const string DialogTagPropertyName = "Microsoft.Win32.UserInterface.DialogTag";
                const uint WM_INITDIALOG = 0x110, WM_NCDESTROY = 0x82;

                if (msg == WM_INITDIALOG)
                {
                    NativeMethods.SetPropW(hWnd, DialogTagPropertyName, lParam);
                }

                int tag = (int)NativeMethods.GetPropW(hWnd, DialogTagPropertyName);
                Dialog dlg;
                bool success = mDialogs.TryGetValue(tag, out dlg);
                if (tag != 0 && success == false) throw new NullReferenceException("Could not find Dialog instance");
                if (dlg == null) return IntPtr.Zero;

                // Assign the hWnd to the Dialog's Handle property here as well,
                // so it is set if it is used before CreateDialogParam() returns.
                if (msg == WM_INITDIALOG) dlg.Handle = hWnd;

                bool handled = false;
                IntPtr result = dlg.ProcessMessage(msg, wParam, lParam, out handled);

                if (msg == WM_NCDESTROY)
                {
                    mDialogs.Remove(tag);
                    NativeMethods.RemovePropW(hWnd, DialogTagPropertyName);
                }

                if (!handled) return IntPtr.Zero;

                const uint WM_COMPAREITEM = 0x39, WM_VKEYTOITEM = 0x2E, WM_CHARTOITEM = 0x2F,
                    WM_QUERYDRAGICON = 0x37, WM_CTLCOLORMSGBOX = 0x132, WM_CTLCOLOREDIT = 0x133,
                    WM_CTLCOLORLISTBOX = 0x134, WM_CTLCOLORBTN = 0x135, WM_CTLCOLORDLG = 0x136,
                    WM_CTLCOLORSCROLLBAR = 0x137, WM_CTLCOLORSTATIC = 0x138, WM_DESTROY = 0x2;

                switch (msg)
                {
                    case WM_COMPAREITEM:
                    case WM_VKEYTOITEM:
                    case WM_CHARTOITEM:
                    case WM_INITDIALOG:
                    case WM_QUERYDRAGICON:
                    case WM_CTLCOLORMSGBOX:
                    case WM_CTLCOLOREDIT:
                    case WM_CTLCOLORLISTBOX:
                    case WM_CTLCOLORBTN:
                    case WM_CTLCOLORDLG:
                    case WM_CTLCOLORSCROLLBAR:
                    case WM_CTLCOLORSTATIC:
                        return result;

                    case WM_DESTROY:
                        dlg.mHandleDestroyed = true;
                        goto default;

                    default:
                        if (!dlg.mHandleDestroyed) dlg.SetWindowLongPtr(0, result);
                        return (IntPtr)1;
                }
            }
            catch (Exception ex)
            {
                UnhandledExceptionProxy.OnUnhandledException(ex);
                return IntPtr.Zero;
            }
        }

        internal IntPtr CreateTag()
        {
            // You must pass the return value of this function into the lpCreateParam of
            // your CreateDialog() call if you are using Dialog.DialogProc as the DLGPROC
            // in the call, or else Dialog.DialogProc will not work correctly.

            int tag = mTopmostDialogTag++;
            mDialogs[tag] = this;
            return (IntPtr)tag;
        }

        public void Load(ResourceLoader module, string dialogName, Window parent = null)
        {
            var tag = CreateTag();

            using (HGlobal buffer = HGlobal.WithStringUni(dialogName))
            {
                IntPtr wndProcPtr;

                if (RuntimeInformation.FrameworkDescription.Contains(".NET Native"))
                {
                    wndProcPtr = McgIntrinsics.AddrOf<DLGPROC>(DialogProc);
                }
                else
                {
                    DLGPROC callback = DialogProc;
                    wndProcPtr = Marshal.GetFunctionPointerForDelegate(callback);
                }

                IntPtr parentHandle = parent?.Handle ?? IntPtr.Zero;
                Handle = NativeMethods.CreateDialogParamW(module.ModuleHandle, buffer.Handle,
                    parentHandle, wndProcPtr, tag);
            }
        }

        public void Load(ResourceLoader module, int dialogId, Window parent = null)
        {
            IntPtr wndProcPtr;

            if (RuntimeInformation.FrameworkDescription.Contains(".NET Native"))
            {
                wndProcPtr = McgIntrinsics.AddrOf<DLGPROC>(DialogProc);
            }
            else
            {
                DLGPROC callback = DialogProc;
                wndProcPtr = Marshal.GetFunctionPointerForDelegate(callback);
            }

            var tag = CreateTag();

            IntPtr parentHandle = parent?.Handle ?? IntPtr.Zero;
            Handle = NativeMethods.CreateDialogParamW(module.ModuleHandle, (IntPtr)dialogId,
                parentHandle, wndProcPtr, tag);
        }

        public Dialog() : base() { }
        public Dialog(IntPtr hWnd) { Handle = hWnd; }
        private bool mHandleDestroyed = false;

        public void CreateFromTemplate(DialogTemplate template, Window parent = null)
        {
            IntPtr wndProcPtr;

            if (RuntimeInformation.FrameworkDescription.Contains(".NET Native"))
            {
                wndProcPtr = McgIntrinsics.AddrOf<DLGPROC>(DialogProc);
            }
            else
            {
                DLGPROC callback = DialogProc;
                wndProcPtr = Marshal.GetFunctionPointerForDelegate(callback);
            }

            IntPtr tag = CreateTag();
            IntPtr parentHandle = parent?.Handle ?? IntPtr.Zero;

            using (HGlobal ptr = template.GetTemplatePointer())
            {
                Handle = NativeMethods.CreateDialogIndirectParamW(IntPtr.Zero, ptr.Handle, parentHandle, wndProcPtr, tag);
            }
        }

        public void RunModal(DialogTemplate template, Window parent = null)
        {
            IntPtr wndProcPtr;

            if (RuntimeInformation.FrameworkDescription.Contains(".NET Native"))
            {
                wndProcPtr = McgIntrinsics.AddrOf<DLGPROC>(DialogProc);
            }
            else
            {
                DLGPROC callback = DialogProc;
                wndProcPtr = Marshal.GetFunctionPointerForDelegate(callback);
            }

            IntPtr tag = CreateTag();
            IntPtr parentHandle = parent?.Handle ?? IntPtr.Zero;

            using (HGlobal ptr = template.GetTemplatePointer())
            {
                NativeMethods.DialogBoxIndirectParamW(IntPtr.Zero, ptr.Handle, parentHandle, wndProcPtr, tag);
            }
        }

        public void RunModal(ResourceLoader module, uint dialogId, Window parent = null)
        {
            IntPtr wndProcPtr;

            if (RuntimeInformation.FrameworkDescription.Contains(".NET Native"))
            {
                wndProcPtr = McgIntrinsics.AddrOf<DLGPROC>(DialogProc);
            }
            else
            {
                DLGPROC callback = DialogProc;
                wndProcPtr = Marshal.GetFunctionPointerForDelegate(callback);
            }

            int tag = mTopmostDialogTag++;
            mDialogs[tag] = this;

            IntPtr parentHandle = parent?.Handle ?? IntPtr.Zero;
            NativeMethods.DialogBoxParamW(module.ModuleHandle, (IntPtr)dialogId, parentHandle, wndProcPtr, (IntPtr)tag);
        }

        protected virtual IntPtr ProcessMessage(uint messageId, IntPtr wParam, IntPtr lParam, out bool handled)
        {
            IntPtr result = ProcessCommonMessage(messageId, wParam, lParam, out handled);
            if (handled) return result;

            handled = false;
            return IntPtr.Zero;
        }

        public T GetItem<T>(int controlId) where T : Window, new()
        {
            T retval = new T();
            retval.Handle = NativeMethods.GetDlgItem(Handle, controlId);
            return retval;
        }

        public void EndModal()
        {
            NativeMethods.EndDialog(Handle, IntPtr.Zero);
            Handle = IntPtr.Zero;
        }

        public void CenterInDesktop()
        {
            Window desktop = new Window(NativeMethods.GetDesktopWindow());

            Rect myRect = WindowRect;
            Rect parentRect = desktop.WindowRect;

            int oldWidth = myRect.Width, oldHeight = myRect.Height;
            int xpos = (parentRect.Width - myRect.Width) / 2 + parentRect.left;
            int ypos = (parentRect.Height - myRect.Height) / 2 + parentRect.top;

            // Make sure that the window never moves outside of the screen.
            const int SM_CXSCREEN = 0, SM_CYSCREEN = 1;
            int screenWidth = NativeMethods.GetSystemMetrics(SM_CXSCREEN);
            int screenHeight = NativeMethods.GetSystemMetrics(SM_CYSCREEN);

            if (xpos < 0) xpos = 0;
            if (ypos < 0) ypos = 0;
            if ((xpos + myRect.Width) > screenWidth) xpos = screenWidth - myRect.Width;
            if ((ypos + myRect.Height) > screenHeight) ypos = screenHeight - myRect.Height;

            Rect newRect = new Rect();
            newRect.left = xpos;
            newRect.top = ypos;
            newRect.right = newRect.left + oldWidth;
            newRect.bottom = newRect.top + oldHeight;
            Move(newRect);
        }
    }
}
