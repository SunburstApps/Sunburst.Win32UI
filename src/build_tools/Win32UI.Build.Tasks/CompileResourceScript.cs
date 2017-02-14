using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Win32UI.Build.Tasks
{
    public sealed class CompileResourceScript : ToolTask
    {
        [RequiredAttribute]
        public ITaskItem[] ResourceScripts { get; set; }
        [RequiredAttribute]
        public string OutputDirectory { get; set; }

        protected override string ToolName => "rc.exe";

        protected override string GenerateFullPathToTool()
        {
            return ToolPath ?? "rc.exe";
        }

        protected override string GenerateCommandLineCommands()
        {
            string firstInputFile = Path.GetFileNameWithoutExtension(ResourceScripts[0].GetMetadata("FullPath"));
            string outputPath = Path.Combine(OutputDirectory, Path.ChangeExtension(firstInputFile, "res"));

            List<string> argv = new List<string>();
            argv.Add("/nologo");
            argv.Add("/Fo");
            argv.Add(outputPath);
            argv.AddRange(ResourceScripts.Select(item => item.GetMetadata("FullPath")));

            return string.Join(" ", argv.Select(x => "\"" + x + "\""));
        }

        public override bool Execute()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Log.LogWarning("Skipping CompileResourceScript task on non-Windows platform");
                return true;
            }

            bool success = base.Execute();

            if (success)
            {
                if (ExitCode != 0)
                {
                    Log.LogError("{0} failed with code {1}", GenerateFullPathToTool(), ExitCode);
                    success = false;
                }
            }

            return success;
        }
    }
}
