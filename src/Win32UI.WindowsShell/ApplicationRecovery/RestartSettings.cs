using System;

namespace Microsoft.Win32.ApplicationRecovery
{
    public sealed class RestartSettings
    {
        public RestartSettings(string command, RestartRestrictions flags)
        {
            CommandLine = command;
            Restrictions = flags;
        }

        public string CommandLine { get; private set; }
        public RestartRestrictions Restrictions { get; private set; }

        public bool Register() => NativeMethods.RegisterApplicationRestart(CommandLine, Restrictions) == 0;
        public bool Unregister() => NativeMethods.UnregisterApplicationRestart() == 0;
    }

    [FlagsAttribute]
    public enum RestartRestrictions
    {
        None = 0,
        NotOnCrash = 1,
        NotOnHang = 2,
        NotOnPatch = 4,
        NotOnReboot = 8
    }
}
