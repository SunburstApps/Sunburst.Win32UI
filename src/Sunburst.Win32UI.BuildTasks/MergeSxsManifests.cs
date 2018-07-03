using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Sunburst.Win32UI.BuildTasks
{
    public sealed class MergeSxsManifests : ToolTask
    {
        [Required]
        public string WindowsSDKBinDirectory { get; set; }
        [Required]
        public ITaskItem[] ManifestFragments { get; set; }
        [Required]
        public string OutputAssembly { get; set; }
        public bool CanonicalizeXml { get; set; }

        protected override string ToolName => "mt.exe";

        protected override string GenerateFullPathToTool()
        {
            return Path.Combine(WindowsSDKBinDirectory, "x86", "mt.exe");
        }

        protected override string GenerateCommandLineCommands()
        {
            List<string> argv = new List<string>();
            argv.Add("-nologo");
            argv.Add("-manifest");
            argv.AddRange(ManifestFragments.Select(x => x.GetMetadata("FullPath")));
            argv.Add("-outputresource:" + OutputAssembly);
            if (CanonicalizeXml) argv.Add("-canonicalize");
            return string.Join(" ", argv.Select(x => "\"" + x + "\""));
        }

        public override bool Execute()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Log.LogError("MergeSxsManifests task can only be run on Windows.");
                return false;
            }

            return base.Execute();
        }
    }
}
