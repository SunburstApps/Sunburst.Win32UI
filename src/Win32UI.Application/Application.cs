using System;
using System.Collections.Generic;
using Microsoft.Win32.UserInterface.Interop;

namespace Microsoft.Win32.UserInterface
{
    public sealed class Application
    {
        private static Stack<Window> mDialogBoxes = new Stack<Window>();
        private static Stack<Tuple<Window, AcceleratorTable>> mAcceleratorTables = new Stack<Tuple<Window, AcceleratorTable>>();

        public static void PushAcceleratorTable(Window hWnd, AcceleratorTable hAccel)
        {
            mAcceleratorTables.Push(new Tuple<Window, AcceleratorTable>(hWnd, hAccel));
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

        public static void PushDialog(Window hWnd)
        {
            mDialogBoxes.Push(hWnd);
        }

        public static void PopDialog()
        {
            try
            {
                mDialogBoxes.Pop();
            }
            catch (InvalidOperationException)
            {
                // Ignore the exception - trying to pop a dialog handle when there is none is silently ignored.
            }
        }

        public static int Run()
        {
            MSG msg = new MSG();

            while (NativeMethods.GetMessageW(out msg, IntPtr.Zero, 0, 0) != 0)
            {
                if (mDialogBoxes.Count != 0)
                {
                    if (NativeMethods.IsDialogMessage(mDialogBoxes.Peek().Handle, ref msg)) continue;
                }

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

        public static bool CreateShutdownBlock(Window window, string reason)
        {
            return NativeMethods.ShutdownBlockReasonCreate(window.Handle, reason);
        }

        public static bool DestroyShutdownBlock(Window window)
        {
            return NativeMethods.ShutdownBlockReasonDestroy(window.Handle);
        }
    }
}
