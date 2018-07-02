using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Sunburst.Win32UI.BuildTasks
{
    public sealed class LinkResourceDll : ToolTask
    {
        private enum ParsedUacPrivilegeLevel
        {
            AsInvoker,
            HighestAvailable,
            RequireAdministrator
        }

        [Required]
        public string OutputFilePath { get; set; }
        [Required]
        public ITaskItem[] Objects { get; set; }
        [Required]
        public string WindowsSDKBinDirectory { get; set; }
        public string UacPrivilegeLevel { get; set; }
        public string[] LibraryPaths { get; set; }

        private string mVisualStudioPath;
        private string mMsvcToolsVersion;

        public override bool Execute()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Log.LogError("LinkResourceDll task can only be run on Windows.");
                return false;
            }

            mVisualStudioPath = VSLocator.GetVisualStudioPath($"Microsoft.VisualCpp.Tools.HostX86.TargetX86");
            string msvcToolsVersionFilePath = Path.Combine(mVisualStudioPath, "VC", "Auxiliary", "Build",
                                                           "Microsoft.VCToolsVersion.default.txt");
            mMsvcToolsVersion = File.ReadAllLines(msvcToolsVersionFilePath)[0];

            string[] pathAdditions =
            {
                Path.Combine(WindowsSDKBinDirectory, "x86"),
                Path.Combine(mVisualStudioPath, "VC", "Tools", "MSVC", mMsvcToolsVersion, "bin", "HostX86", "x86")
            };

            Log.LogMessage("Prepending PATH: {0}", string.Join("; ", pathAdditions));
            EnvironmentVariables = new[]
            {
                "PATH=" + string.Join(Path.PathSeparator.ToString(), pathAdditions) + Path.PathSeparator + Environment.GetEnvironmentVariable("PATH")
            };

            return base.Execute();
        }

        protected override string ToolName => "link.exe";

        protected override string GenerateFullPathToTool()
        {
            return Path.Combine(mVisualStudioPath, "VC", "Tools", "MSVC",
                                mMsvcToolsVersion, "bin", "HostX86", "x86", "link.exe");
        }

        protected override string GenerateCommandLineCommands()
        {
            List<string> argv = new List<string>();
            argv.Add("/nologo");
            argv.Add("/dll");
            argv.Add("/noentry");
            argv.Add($"/out:{OutputFilePath}");
            argv.Add("/manifest:embed,id=1");
            argv.Add($"/manifestuac:{GetManifestUacString()}");
            argv.Add($"/machine:X86");
            argv.Add($"/manifestDependency:type='win32' name='Microsoft.Windows.Common-Controls' version='6.0.0.0' processorArchitecture='*' publicKeyToken='6595b64144ccf1df' language='*'");
            argv.AddRange(Objects?.Select(item => item.GetMetadata("Identity")) ?? Enumerable.Empty<string>());
            argv.AddRange(LibraryPaths?.Select(path => "/libpath:" + path) ?? Enumerable.Empty<string>());

            return string.Join(" ", argv.Select(x => "\"" + x + "\""));
        }

        private string GetManifestUacString()
        {
            string privilegeLevel = null;

            var levelEnum = Enum.Parse(typeof(ParsedUacPrivilegeLevel), UacPrivilegeLevel, true);
            switch (levelEnum)
            {
                case ParsedUacPrivilegeLevel.AsInvoker: privilegeLevel = "asInvoker"; break;
                case ParsedUacPrivilegeLevel.HighestAvailable: privilegeLevel = "highestAvailable"; break;
                case ParsedUacPrivilegeLevel.RequireAdministrator: privilegeLevel = "requireAdministrator"; break;
                default: throw new ArgumentException("Unrecognized UacPrivilegeLevel");
            }

            return $"level='{privilegeLevel}' uiAccess='false'";
        }
    }
}
