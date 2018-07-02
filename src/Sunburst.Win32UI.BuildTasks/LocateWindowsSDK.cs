using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Sunburst.Win32UI.BuildTasks
{
    public sealed class LocateWindowsSDK : Task
    {
        [Output]
        public string[] IncludeDirectories { get; set; }
        [Output]
        public string[] LibraryDirectories { get; set; }
        [Output]
        public string BinDirectory { get; set; }

        public override bool Execute()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Log.LogError("LocateWindowsSDK task can only be run on Windows.");
                return false;
            }

            string windowsSDKRoot = @"C:\Program Files (x86)\Windows Kits\10";

            bool SDKVersionValid(string version)
            {
                string binDir = Path.Combine(windowsSDKRoot, "bin", version);
                if (!Directory.Exists(binDir)) return false;

                string incDir = Path.Combine(windowsSDKRoot, "include", version);
                if (!Directory.Exists(incDir)) return false;

                string libDir = Path.Combine(windowsSDKRoot, "lib", version);
                if (!Directory.Exists(libDir)) return false;

                BinDirectory = binDir;
                IncludeDirectories = Directory.GetDirectories(incDir);
                LibraryDirectories = Directory.GetDirectories(libDir);
                return true;
            }

            if (SDKVersionValid("10.0.17134.0")) return true;
            else if (SDKVersionValid("10.0.16299.0")) return true;
            else if (SDKVersionValid("10.0.15063.0")) return true;
            else if (SDKVersionValid("10.0.14393.0")) return true;
            else if (SDKVersionValid("10.0.10586.0")) return true;
            else if (SDKVersionValid("10.0.10240.0")) return true;

            Log.LogError("Could not find an installed Windows 10 SDK");
            return false;
        }
    }
}
