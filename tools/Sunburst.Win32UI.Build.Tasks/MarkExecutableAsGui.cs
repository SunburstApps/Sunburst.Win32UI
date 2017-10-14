using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Win32UI.Build.Tasks
{
    public sealed class MarkExecutableAsGui : ToolTask
    {
        [RequiredAttribute]
        public ITaskItem Executable { get; set; }

        protected override string ToolName => "editbin.exe";

        protected override string GenerateFullPathToTool()
        {
            if (!string.IsNullOrEmpty(ToolPath)) return ToolPath;
            else return "editbin.exe";
        }

        protected override string GenerateCommandLineCommands()
        {
            string path = Executable.GetMetadata("FullPath");
            return $"/nologo /subsystem:WINDOWS \"{path}\"";
        }

        public override bool Execute()
        {
#if IS_CORECLR
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Log.LogWarning("Skipping CompileResourceScript task on non-Windows platform");
                return true;
            }
#endif

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
