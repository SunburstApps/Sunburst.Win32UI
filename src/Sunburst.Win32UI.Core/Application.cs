using System;
using System.Collections.Generic;
using Sunburst.Win32UI.Interop;

namespace Sunburst.Win32UI
{
    public sealed class Application
    {
        private static Stack<Tuple<Control, AcceleratorTable>> mAcceleratorTables = new Stack<Tuple<Control, AcceleratorTable>>();

        public static void PushAcceleratorTable(Control hWnd, AcceleratorTable hAccel)
        {
            mAcceleratorTables.Push(new Tuple<Control, AcceleratorTable>(hWnd, hAccel));
        }

        public static void PopAcceleratorTable()
        {
            try
            {
                mAcceleratorTables.Pop();
            }
            catch (InvalidOperationException)
            {
                // Ignore the exception - trying to pop an accelerator-table handle when there is none is silently ignored.
            }
        }

        public static int Run(Form form)
        {
            form.FormClosed += (s, e) => Exit();
            form.Show();
            return Run();
        }

        public static int Run()
        {
            MSG msg = new MSG();

            while (NativeMethods.GetMessageW(out msg, IntPtr.Zero, 0, 0) != 0)
            {
                if (mAcceleratorTables.Count != 0)
                {
                    var table = mAcceleratorTables.Peek();
                    if (NativeMethods.TranslateAcceleratorW(table.Item1.Handle, table.Item2.Handle, ref msg) != 0) continue;
                }

                NativeMethods.TranslateMessage(ref msg);
                NativeMethods.DispatchMessageW(ref msg);
            }

            return (int)msg.wParam;
        }

        public static void Exit()
        {
            NativeMethods.PostQuitMessage(0);
        }

        public static bool CreateShutdownBlock(Control window, string reason)
        {
            return NativeMethods.ShutdownBlockReasonCreate(window.Handle, reason);
        }

        public static bool DestroyShutdownBlock(Control window)
        {
            return NativeMethods.ShutdownBlockReasonDestroy(window.Handle);
        }
    }
}
