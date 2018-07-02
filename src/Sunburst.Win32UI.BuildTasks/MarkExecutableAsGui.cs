using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Sunburst.Win32UI.BuildTasks
{
    public sealed class MarkExecutableAsGui : ToolTask
    {
        [Required]
        public string RuntimeIdentifier { get; set; }
        [Required]
        public ITaskItem Executable { get; set; }

        private string mLinkerArchitecture;
        private string mVisualStudioPath;
        private string mMsvcToolsVersion;

        protected override string ToolName => "editbin.exe";

        protected override string GenerateFullPathToTool()
        {
            return Path.Combine(mVisualStudioPath, @"VC\Tools\MSVC", mMsvcToolsVersion, @"bin\HostX86", mLinkerArchitecture, "editbin.exe");
        }

        protected override string GenerateCommandLineCommands()
        {
            string path = Executable.GetMetadata("FullPath");
            return $"/nologo /subsystem:WINDOWS \"{path}\"";
        }

        public override bool Execute()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Log.LogError("MarkExecutableAsGui task can only be run on Windows.");
                return false;
            }

            if (RuntimeIdentifier.EndsWith("-x64", StringComparison.Ordinal))
            {
                mLinkerArchitecture = "x64";
            }
            else if (RuntimeIdentifier.EndsWith("-x86", StringComparison.Ordinal))
            {
                mLinkerArchitecture = "x86";
            }
            else
            {
                Log.LogError("Could not determine the linker architecture from RID '{0}'", RuntimeIdentifier);
                return false;
            }

            mVisualStudioPath = VSLocator.GetVisualStudioPath($"Microsoft.VisualCpp.Tools.HostX86.Target{mLinkerArchitecture.ToUpperInvariant()}");
            string msvcToolsVersionFilePath = Path.Combine(mVisualStudioPath, @"VC\Auxiliary\Build\Microsoft.VCToolsVersion.default.txt");
            mMsvcToolsVersion = File.ReadAllLines(msvcToolsVersionFilePath)[0];


            string[] pathAdditions =
            {
                Path.Combine(mVisualStudioPath, @"VC\Tools\MSVC", mMsvcToolsVersion, @"bin\HostX86", mLinkerArchitecture),
                Path.Combine(mVisualStudioPath, @"VC\Tools\MSVC", mMsvcToolsVersion, @"bin\HostX86\x86"),
            };

            EnvironmentVariables = new[]
            {
                "PATH=" + string.Join(Path.PathSeparator.ToString(), pathAdditions) + Path.PathSeparator + Environment.GetEnvironmentVariable("PATH")
            };

            return base.Execute();
        }
    }
}
