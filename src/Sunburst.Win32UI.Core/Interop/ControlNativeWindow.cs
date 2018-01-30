using System;
using System.Runtime.InteropServices;

namespace Sunburst.Win32UI.Interop
{
    internal sealed class ControlNativeWindow : NativeWindow
    {
        public static ControlNativeWindow Create(Control owner, CreateParams createParams)
        {
            if (owner == null) throw new ArgumentNullException(nameof(owner));
            if (createParams == null) throw new ArgumentNullException(nameof(createParams));

            ControlNativeWindow nativeWindow = new ControlNativeWindow(owner);
            WindowClass windowClass = WindowClass.GetWindowClass(createParams.ClassName, createParams.ClassStyle);
            IntPtr wndProc = Marshal.GetFunctionPointerForDelegate((WNDPROC)WndProc);
            string fullClassName = windowClass.Register(wndProc, out nativeWindow.superclassWndProc);

            GCHandle gcHandle = GCHandle.Alloc(nativeWindow);

            var frame = createParams.Frame;
            nativeWindow.Handle = NativeMethods.CreateWindowEx(createParams.ExtendedStyle, fullClassName,
                createParams.Caption, createParams.Style, frame.left, frame.top, frame.Width, frame.Height,
                createParams.ParentHandle, IntPtr.Zero, IntPtr.Zero, GCHandle.ToIntPtr(gcHandle));

            return nativeWindow;
        }

        private static IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            const string propertyName = "Sunburst.WindowsForms.Control";

            if (msg == WindowMessages.WM_NCCREATE)
            {
                CREATESTRUCT createStruct = Marshal.PtrToStructure<CREATESTRUCT>(lParam);
                NativeMethods.SetProp(hWnd, propertyName, createStruct.lpCreateParams);
            }

            IntPtr instanceHandle = NativeMethods.GetProp(hWnd, propertyName);
            if (instanceHandle == IntPtr.Zero)
            {
                // If we haven't received WM_NCCREATE yet, there's not much we can do here.
                return NativeMethods.DefWindowProc(hWnd, msg, wParam, lParam);
            }

            ControlNativeWindow instance = (ControlNativeWindow)GCHandle.FromIntPtr(instanceHandle).Target;

            Message m = new Message(hWnd, msg, wParam, lParam);
            instance.Owner.CallWndProc(ref m);

            if (msg == WindowMessages.WM_NCDESTROY)
            {
                GCHandle.FromIntPtr(instanceHandle).Free();
                NativeMethods.RemoveProp(hWnd, propertyName);
            }

            return m.Result;
        }

        private ControlNativeWindow(Control owner)
        {
            if (owner == null) throw new ArgumentNullException(nameof(owner));
            Owner = owner;
        }

        public Control Owner { get; }
        private IntPtr superclassWndProc = IntPtr.Zero;

        public void DefWndProc(ref Message msg)
        {
            if (superclassWndProc != IntPtr.Zero)
            {
                msg.Result = NativeMethods.CallWindowProc(superclassWndProc, msg.TargetHandle, msg.MessageId, msg.WParam, msg.LParam);
            }
            else
            {
                msg.Result = NativeMethods.DefWindowProc(msg.TargetHandle, msg.MessageId, msg.WParam, msg.LParam);
            }
        }
    }
}
