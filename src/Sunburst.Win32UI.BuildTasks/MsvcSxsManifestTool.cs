using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Sunburst.Win32UI.BuildTasks
{
    public sealed class MsvcSxsManifestTool : ToolTask
    {
        public string InputManifestFile { get; set; }
        public string InputAssembly { get; set; }
        public string OutputManifestFile { get; set; }
        public string OutputAssembly { get; set; }
        public ITaskItem[] ManifestFragments { get; set; }
        public bool CanonicalizeXml { get; set; }

        protected override string ToolName => "mt.exe";

        protected override string GenerateFullPathToTool()
        {
            return ToolPath ?? "mt.exe";
        }

        protected override string GenerateCommandLineCommands()
        {
            List<string> argv = new List<string>();
            argv.Add("-nologo");

            if (InputManifestFile != null)
            {
                argv.Add("-manifest");
                argv.Add(InputManifestFile);
            }
            else if (InputAssembly != null)
            {
                argv.Add("-inputresource:" + InputAssembly + ";#1");
            }

            if (OutputManifestFile != null)
            {
                argv.Add("-out:" + OutputManifestFile);
            }
            else if (OutputAssembly != null)
            {
                argv.Add("-outputresource:" + OutputAssembly + ";#1");
            }

            if (ManifestFragments != null && ManifestFragments.Length > 0)
            {
                argv.Add("-manifest");
                foreach (var item in ManifestFragments) argv.Add(item.GetMetadata("FullPath"));
            }

            if (CanonicalizeXml) argv.Add("-canonicalize");
            return string.Join(" ", argv.Select(x => "\"" + x + "\""));
        }

        public override bool Execute()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Log.LogWarning("Skipping MsvcSxsManifestTool task on non-Windows platform");
                return true;
            }

            if (InputManifestFile == null && InputAssembly == null)
            {
                Log.LogError("Either InputManifestFile or InputAssembly must be specified");
                return false;
            }
            else if (InputManifestFile != null && InputAssembly != null)
            {
                Log.LogError("You cannot specify both InputManifestFile and InputAssembly");
                return false;
            }

            if (OutputManifestFile == null && OutputAssembly == null)
            {
                Log.LogError("Either OutputManifestFile or OutputAssembly must be specified");
                return false;
            }
            else if (OutputManifestFile != null && OutputAssembly != null)
            {
                Log.LogError("You cannot specify both OutputManifestFile and OutputAssembly");
                return false;
            }

            return base.Execute();
        }
    }
}
