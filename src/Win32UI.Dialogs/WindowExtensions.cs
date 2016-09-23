using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Win32.UserInterface
{
    public static class WindowExtensions
    {
        public static int GetDialogId(this Window window)
        {
            const int GWLP_ID = -12;
            return (int)window.GetWindowLongPtr(GWLP_ID);
        }

        public static void SetDialogId(this Window window, int id)
        {
            const int GWLP_ID = -12;
            window.SetWindowLongPtr(GWLP_ID, (IntPtr)id);
        }
    }
}
