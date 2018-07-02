using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Sunburst.Win32UI.BuildTasks
{
    public sealed class CompileResourceScript : ToolTask
    {
        [Required]
        public ITaskItem[] ResourceScripts { get; set; }
        [Required]
        public ITaskItem OutputFile { get; set; }
        [Required]
        public string WindowsSDKBinDirectory { get; set; }
        public string[] IncludePaths { get; set; }

        protected override string ToolName => "rc.exe";

        protected override string GenerateFullPathToTool()
        {
            return Path.Combine(WindowsSDKBinDirectory, "x86", "rc.exe");
        }

        protected override string GenerateCommandLineCommands()
        {
            List<string> argv = new List<string>();
            argv.Add("/nologo");
            argv.Add("/Fo");
            argv.Add(OutputFile.GetMetadata("FullPath"));
            argv.AddRange(IncludePaths.Select(path => "/i" + path));
            argv.AddRange(ResourceScripts.Select(item => item.GetMetadata("FullPath")));

            return string.Join(" ", argv.Select(x => "\"" + x + "\""));
        }

        public override bool Execute()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Log.LogError("CompileResourceScript task can only be run on Windows.");
                return false;
            }

            return base.Execute();
        }
    }
}
