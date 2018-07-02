using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace Sunburst.Win32UI.BuildTasks
{
    public static class VSLocator
    {
        public static string GetVisualStudioPath(params string[] requiredWorkloads)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                throw new InvalidOperationException("You cannot get the path to Visual Studio on a non-Windows platform");
            }

            string vswhereExe = @"C:\Program Files (x86)\Microsoft Visual Studio\Installer\vswhere.exe";
            if (!File.Exists(vswhereExe))
            {
                throw new InvalidOperationException($"{vswhereExe} not found - cannot continue");
            }

            ProcessStartInfo startInfo = new ProcessStartInfo(vswhereExe);
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.UseShellExecute = false;
            startInfo.Arguments = "-latest";

            if (requiredWorkloads != null && requiredWorkloads.Length > 0)
            {
                startInfo.Arguments += " -requires " + string.Join(" ", requiredWorkloads);
            }

            Process proc = Process.Start(startInfo);
            proc.WaitForExit();
            if (proc.ExitCode != 0)
            {
                throw new InvalidOperationException($"vswhere.exe exited with code {proc.ExitCode}");
            }

            string allOutput = proc.StandardOutput.ReadToEnd();
            string[] lines = allOutput.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in lines)
            {
                if (line.StartsWith("installationPath:", StringComparison.Ordinal))
                {
                    return line.Substring("installationPath:".Length).TrimStart();
                }
            }

            return null;
        }
    }
}
