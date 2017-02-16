using System;
using System.Runtime.InteropServices;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Win32UI.Build.NativeResources;

namespace Win32UI.Build.Tasks
{
    public sealed class CopyResourceSegment : Task
    {
        [RequiredAttribute]
        public ITaskItem InputFile { get; set; }
        [RequiredAttribute]
        public ITaskItem OutputFile { get; set; }

        public override bool Execute()
        {
#if IS_CORECLR
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Log.LogWarning("Skipping task on non-Windows platform");
                return true;
            }
#endif

            ResourceCollection inputResources = new ResourceCollection();
            inputResources.Load(InputFile.GetMetadata("FullPath"));
            inputResources.Save(OutputFile.GetMetadata("FullPath"));
            return true;
        }
    }
}
