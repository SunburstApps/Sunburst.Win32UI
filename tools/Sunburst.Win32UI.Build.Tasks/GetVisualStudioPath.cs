using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Win32UI.Build.Tasks
{
    public sealed class GetVisualStudioPath : Task
    {
        public string[] RequiredWorkloads { get; set; }
        [Output]
        public string VisualStudioPath { get; set; }

        public override bool Execute()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Log.LogError("You cannot get the path to Visual Studio on a non-Windows platform");
                return false;
            }

            string vswhereExe = @"C:\Program Files (x86)\Microsoft Visual Studio\Installer\vswhere.exe";
            if (!File.Exists(vswhereExe))
            {
                Log.LogError("{0} not found - cannot continue", vswhereExe);
                return false;
            }

            ProcessStartInfo startInfo = new ProcessStartInfo(vswhereExe);
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.UseShellExecute = false;
            startInfo.Arguments = "-latest";

            if (RequiredWorkloads != null && RequiredWorkloads.Length > 0)
            {
                startInfo.Arguments += " -requires " + string.Join(" ", RequiredWorkloads);
            }

            Process proc = Process.Start(startInfo);
            proc.WaitForExit();
            if (proc.ExitCode != 0)
            {
                Log.LogError("vswhere.exe exited with code {0}", proc.ExitCode);
                Log.LogMessage(MessageImportance.High, "vswhere.exe output:\r\n\r\n{0}", proc.StandardOutput.ReadToEnd());
                return false;
            }

            string allOutput = proc.StandardOutput.ReadToEnd();
            string[] lines = allOutput.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in lines)
            {
                if (line.StartsWith("installationPath:"))
                {
                    VisualStudioPath = line.Substring("installationPath:".Length).TrimStart();
                    return true;
                }
            }

            Log.LogError("Could not determine Visual Studio path from vswhere.exe output");
            return false;
        }
    }
}
