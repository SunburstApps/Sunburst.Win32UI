using System.Runtime.InteropServices;

namespace Microsoft.Win32.UserInterface
{
    public static class MessageBeep
    {
        [DllImport("user32.dll", EntryPoint = "MessageBeep")]
        private static extern bool NativeMessageBeep(uint type);

        public static void PlayCriticalStopSound()
        {
            const uint MB_ICONERROR = 0x10;
            NativeMessageBeep(MB_ICONERROR);
        }

        public static void PlayAsteriskSound()
        {
            const uint MB_ICONINFORMATION = 0x40;
            NativeMessageBeep(MB_ICONINFORMATION);
        }

        public static void PlayQuestionSound()
        {
            const uint MB_ICONQUESTION = 0x20;
            NativeMessageBeep(MB_ICONQUESTION);
        }

        public static void PlayExclamationSound()
        {
            const uint MB_ICONWARNING = 0x30;
            NativeMessageBeep(MB_ICONWARNING);
        }

        public static void PlayDefaultBeepSound()
        {
            const uint MB_OK = 0;
            NativeMessageBeep(MB_OK);
        }
    }
}
