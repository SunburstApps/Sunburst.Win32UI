using System;
using System.Runtime.InteropServices;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Vestris.ResourceLib;

namespace Sunburst.Win32UI.BuildTasks
{
    public sealed class CopyResourceSegment : Task
    {
        [RequiredAttribute]
        public ITaskItem InputFile { get; set; }
        [RequiredAttribute]
        public ITaskItem OutputFile { get; set; }

        public override bool Execute()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Log.LogWarning("Skipping task on non-Windows platform");
                return true;
            }

            string outputPath = OutputFile.GetMetadata("FullPath");
            ResourceInfo inputResources = new ResourceInfo();
            inputResources.Load(InputFile.GetMetadata("FullPath"));

            // ResourceInfo.Save() is not implemented, so I must save each resource one at a time.
            foreach (var rsrc in inputResources)
            {
                rsrc.SaveTo(outputPath);
            }

            inputResources.Dispose();
            return true;
        }
    }
}
