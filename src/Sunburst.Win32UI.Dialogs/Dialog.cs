﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Sunburst.Win32UI.Graphics;
using Sunburst.Win32UI.Interop;

namespace Sunburst.Win32UI
{
    public class Dialog : NativeWindow
    {
        internal static IntPtr DialogProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            try
            {
                const string DialogTagPropertyName = "Sunburst.Win32UI.Dialog";
                const uint WM_INITDIALOG = 0x110, WM_NCDESTROY = 0x82;

                if (msg == WM_INITDIALOG)
                {
                    NativeMethods.SetPropW(hWnd, DialogTagPropertyName, lParam);
                }

                IntPtr handle = NativeMethods.GetPropW(hWnd, DialogTagPropertyName);
                Dialog dlg = (Dialog)GCHandle.FromIntPtr(handle).Target;
                if (dlg == null) return IntPtr.Zero;

                // Assign the hWnd to the Dialog's Handle property here as well,
                // so it is set if it is used before CreateDialogParam() returns.
                if (msg == WM_INITDIALOG) dlg.Handle = hWnd;

                Message m = new Message(hWnd, msg, wParam, lParam);
                bool handled = dlg.ProcessMessage(ref m);

                if (msg == WM_NCDESTROY)
                {
                    GCHandle.FromIntPtr(handle).Free();
                    NativeMethods.RemovePropW(hWnd, DialogTagPropertyName);
                }

                if (!handled) return IntPtr.Zero;

                const uint WM_COMPAREITEM = 0x39, WM_VKEYTOITEM = 0x2E, WM_CHARTOITEM = 0x2F,
                    WM_QUERYDRAGICON = 0x37, WM_CTLCOLORMSGBOX = 0x132, WM_CTLCOLOREDIT = 0x133,
                    WM_CTLCOLORLISTBOX = 0x134, WM_CTLCOLORBTN = 0x135, WM_CTLCOLORDLG = 0x136,
                    WM_CTLCOLORSCROLLBAR = 0x137, WM_CTLCOLORSTATIC = 0x138;

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
                        return m.Result;

                    default:
                        dlg.SetWindowLongPtr(0, m.Result);
                        return (IntPtr)1;
                }
            }
            catch (Exception ex)
            {
                Environment.FailFast("Unhandled exception in Dialog.DialogProc", ex);
                // Technically not reachable, but required by the compiler anyway.
                return IntPtr.Zero;
            }
        }

        public Dialog() : base() { }
        public Dialog(IntPtr hWnd, bool ownsHandle) : base(hWnd, ownsHandle) { }

        public void Create(DialogTemplate template, IWin32Window parent = null)
        {
            DLGPROC callback = DialogProc;
            IntPtr wndProcPtr = Marshal.GetFunctionPointerForDelegate(callback);

            IntPtr instanceHandle = GCHandle.ToIntPtr(GCHandle.Alloc(this));
            IntPtr parentHandle = parent?.Handle ?? IntPtr.Zero;

            using (HGlobal ptr = template.CreateTemplate())
            {
                Handle = NativeMethods.CreateDialogIndirectParamW(IntPtr.Zero, ptr.Handle, parentHandle, wndProcPtr, instanceHandle);
            }
        }

        public void RunModal(DialogTemplate template, IWin32Window parent = null)
        {
            DLGPROC callback = DialogProc;
            IntPtr wndProcPtr = Marshal.GetFunctionPointerForDelegate(callback);

            IntPtr instanceHandle = GCHandle.ToIntPtr(GCHandle.Alloc(this));
            IntPtr parentHandle = parent?.Handle ?? IntPtr.Zero;

            using (HGlobal ptr = template.CreateTemplate())
            {
                NativeMethods.DialogBoxIndirectParamW(IntPtr.Zero, ptr.Handle, parentHandle, wndProcPtr, instanceHandle);
            }
        }

        protected virtual bool ProcessMessage(ref Message m)
        {
            bool handled = false;
            MessageReflector.ReflectMessage(ref m, out handled);
            return handled;
        }

        public T GetControl<T>(int controlId) where T : Control
        {
            IntPtr hWnd = NativeMethods.GetDlgItem(Handle, controlId);
            if (hWnd == IntPtr.Zero) return default(T);

            return (T)Activator.CreateInstance(typeof(T), hWnd, false);
        }

        public void ReplaceControl(int controlId, Control replacementControl)
        {
            if (replacementControl == null) throw new ArgumentNullException(nameof(replacementControl));

            Control oldControl = GetControl<Control>(controlId);
            if (oldControl == null) throw new ArgumentException("Given control ID isn't present in this dialog", nameof(controlId));

            Rect frame = oldControl.WindowRect;
            oldControl.NativeWindow.DestroyWindow();
            oldControl = null;

            NativeMethods.SetParent(replacementControl.Handle, Handle);
            replacementControl.WindowRect = frame;

            const int GWLP_ID = -12;
            replacementControl.NativeWindow.SetWindowLongPtr(GWLP_ID, (IntPtr)controlId);
        }

        public void EndModal()
        {
            NativeMethods.EndDialog(Handle, IntPtr.Zero);
            Handle = IntPtr.Zero;
        }

        public void CenterInDesktop()
        {
            NativeWindow desktop = new NativeWindow(NativeMethods.GetDesktopWindow(), false);

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