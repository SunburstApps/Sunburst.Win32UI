using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Win32UI.Build.Tasks
{
    public sealed class MsvcSxsManifestTool : ToolTask
    {
        [Required]
        public string FilePath { get; set; }
        public ITaskItem[] ManifestFragments { get; set; }

        protected override string ToolName => "mt.exe";

        protected override string GenerateFullPathToTool()
        {
            return ToolPath ?? "mt.exe";
        }

        protected override string GenerateCommandLineCommands()
        {
            List<string> argv = new List<string>();
            argv.Add("-nologo");
            argv.Add("-canonicalize");
            argv.Add("-updateresource:" + FilePath);

            if (ManifestFragments != null && ManifestFragments.Length > 0)
            {
                argv.Add("-manifest");
                foreach (var item in ManifestFragments) argv.Add(item.GetMetadata("FullPath"));
            }

            return string.Join(" ", argv.Select(x => "\"" + x + "\""));
        }

        public override bool Execute()
        {
#if IS_CORECLR
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Log.LogWarning("Skipping MsvcSxsManifestTool task on non-Windows platform");
                return true;
            }
#endif

            return base.Execute();
        }
    }
}
