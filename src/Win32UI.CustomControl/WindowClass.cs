using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Win32.UserInterface.Graphics;
using Microsoft.Win32.UserInterface.Interop;

namespace Microsoft.Win32.UserInterface
{
    public sealed class WindowClass
    {
        private static readonly ConcurrentDictionary<string, IntPtr> mAtomTable = new ConcurrentDictionary<string, IntPtr>();

        public WindowClass(string className)
        {
            ClassName = className;
            Traits = WindowTraits.ChildControl;
            BackgroundBrush = SystemBrushes.WindowBackground;
            Cursor = Cursor.Arrow;
        }

        public string ClassName { get; private set; }
        public WindowTraits Traits { get; set; }
        public uint Style { get; set; }
        public NonOwnedBrush BackgroundBrush { get; set; }
        public Cursor Cursor { get; set; }

        public IntPtr Register()
        {
            IntPtr classAtom = IntPtr.Zero;
            bool found = mAtomTable.TryGetValue(ClassName, out classAtom);
            if (found) return classAtom;

            using (HGlobal ptr = HGlobal.WithStringUni(ClassName))
            {
                IntPtr wndProcPtr;

                if (RuntimeInformation.FrameworkDescription.Contains(".NET Native"))
                {
                    wndProcPtr = McgIntrinsics.AddrOf<WndProc>(CustomWindow.WndProc);
                }
                else
                {
                    WndProc callback = CustomWindow.WndProc;
                    wndProcPtr = Marshal.GetFunctionPointerForDelegate(callback);
                }

                WNDCLASSEXW nativeClass = new WNDCLASSEXW();
                nativeClass.cbSize = Convert.ToUInt32(Marshal.SizeOf<WNDCLASSEXW>());
                nativeClass.style = Traits.CalculateStyle(Style);
                nativeClass.lpfnWndProc = wndProcPtr;
                nativeClass.cbClsExtra = nativeClass.cbWndExtra = 0;
                nativeClass.hInstance = IntPtr.Zero;
                nativeClass.hCursor = Cursor.Handle;
                nativeClass.hbrBackground = BackgroundBrush.Handle;
                nativeClass.lpszClassName = ptr.Handle;

                classAtom = NativeMethods.RegisterClassExW(ref nativeClass);
                mAtomTable[ClassName] = classAtom;
                return classAtom;
            }
        }
    }
}
