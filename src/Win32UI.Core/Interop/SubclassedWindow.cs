using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Win32.UserInterface.Interop
{
    public class SubclassedWindow
    {
        private static readonly ConcurrentDictionary<IntPtr, SubclassedWindow> mControls = new ConcurrentDictionary<IntPtr, SubclassedWindow>();

        private static IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            try
            {
                SubclassedWindow instance;
                bool found = mControls.TryGetValue(hWnd, out instance);
                if (!found) return NativeMethods.DefWindowProc(hWnd, msg, wParam, lParam);

                IntPtr retval = instance.ReplacementProcessMessage(msg, wParam, lParam);
                if (msg == WindowMessages.WM_DESTROY) mControls.TryRemove(hWnd, out instance);
                return retval;
            }
            catch (Exception ex)
            {
                Application.OnUnhandledException(ex);
                return IntPtr.Zero;
            }
        }

        public SubclassedWindow(Window owner)
        {
            Owner = owner;
            ReplacementProcessMessage = (msg, wParam, lParam) => OriginalProcessMessage(msg, wParam, lParam);
        }

        private Window Owner;
        private IntPtr OriginalWndProc;
        private bool AlreadySubclassed = false;
        private const int GWLP_WNDPROC = -4;

        public void Subclass()
        {
            if (!AlreadySubclassed)
            {
                bool alreadyExists = mControls.ContainsKey(Owner.Handle);
                if (alreadyExists) throw new InvalidOperationException("Cannot subclass the same window twice");
                mControls.TryAdd(Owner.Handle, this);

                OriginalWndProc = NativeMethods.GetWindowLongPtr(Owner.Handle, GWLP_WNDPROC);

                IntPtr wndProcPtr;

                if (RuntimeInformation.FrameworkDescription.Contains(".NET Native"))
                {
                    wndProcPtr = McgIntrinsics.AddrOf<WNDPROC>(WndProc);
                }
                else
                {
                    WNDPROC wndProc = WndProc;
                    wndProcPtr = Marshal.GetFunctionPointerForDelegate(wndProc);
                }

                NativeMethods.SetWindowLongPtr(Owner.Handle, GWLP_WNDPROC, wndProcPtr);
                AlreadySubclassed = true;
            }
        }

        public void UndoSubclass()
        {
            if (AlreadySubclassed)
            {
                NativeMethods.SetWindowLongPtr(Owner.Handle, GWLP_WNDPROC, OriginalWndProc);

                SubclassedWindow ignored;
                mControls.TryRemove(Owner.Handle, out ignored);
                AlreadySubclassed = false;
            }
        }

        public IntPtr OriginalProcessMessage(uint msg, IntPtr wParam, IntPtr lParam) => NativeMethods.CallWindowProc(OriginalWndProc, Owner.Handle, msg, wParam, lParam);
        public Func<uint, IntPtr, IntPtr, IntPtr> ReplacementProcessMessage { get; set; }
    }
}
