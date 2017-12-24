using System;
using System.Runtime.InteropServices;

namespace Sunburst.Win32UI.Interop
{
    public class NativeWindow : IWin32Window
    {
        public IntPtr Handle { get; private set; } = IntPtr.Zero;
        private IntPtr superclassWndProc = IntPtr.Zero;

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

            NativeWindow instance = (NativeWindow)GCHandle.FromIntPtr(instanceHandle).Target;

            Message m = new Message(hWnd, msg, wParam, lParam);
            instance.ProcessMessage(ref m);

            if (msg == WindowMessages.WM_NCDESTROY)
            {
                GCHandle.FromIntPtr(instanceHandle).Free();
                NativeMethods.RemoveProp(hWnd, propertyName);
            }

            return m.Result;
        }

        protected virtual void ProcessMessage(ref Message msg)
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

        public void CreateHandle(CreateParams createParams)
        {
            if (createParams == null) throw new ArgumentNullException(nameof(createParams));
            if (Handle != IntPtr.Zero) throw new InvalidOperationException("This instance already has a handle, please destroy if first");

            WindowClass windowClass = WindowClass.GetWindowClass(createParams.ClassName, createParams.ClassStyle);

            IntPtr wndProc = Marshal.GetFunctionPointerForDelegate((WNDPROC)WndProc);
            string fullClassName = windowClass.Register(wndProc, out superclassWndProc);

            GCHandle gcHandle = GCHandle.Alloc(this);

            var frame = createParams.Frame;
            Handle = NativeMethods.CreateWindowEx(createParams.ExtendedStyle, fullClassName,
            createParams.Caption, createParams.Style, frame.left, frame.top, frame.Width, frame.Height,
            createParams.ParentHandle, IntPtr.Zero, IntPtr.Zero, GCHandle.ToIntPtr(gcHandle));
        }

        public void DestroyHandle()
        {
            if (Handle == IntPtr.Zero) throw new InvalidOperationException("This instance does not have a handle to destroy");
            NativeMethods.DestroyWindow(Handle);
        }
    }
}
