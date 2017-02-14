using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Win32UI.Build.Tasks
{
    public sealed class GenerateResourceScript : Task
    {
        [RequiredAttribute]
        public string OutputDirectory { get; set; }
        [OutputAttribute]
        public ITaskItem ResourceScriptPath { get; set; }
        public ITaskItem[] Icons { get; set; }
        public ITaskItem[] ScriptFragments { get; set; }
        public ITaskItem[] EmbeddedFiles { get; set; }

        public override bool Execute()
        {
            StringBuilder outputFile = new StringBuilder();
            outputFile.AppendLine("#include <Windows.h>");
            outputFile.AppendLine();

            foreach (var icon in Icons ?? Enumerable.Empty<ITaskItem>())
            {
                string indexString = icon.GetMetadata("IconIndex");
                if (!string.IsNullOrEmpty(indexString))
                {
                    bool parseOK = int.TryParse(indexString, out var iconIndex);
                    string path = icon.GetMetadata("FullPath").Replace("\\", "\\\\");
                    outputFile.AppendLine($"{iconIndex} ICON \"{path}\"");
                }
                else
                {
                    Log.LogError("Icon resource '{0}' must have a value for the IconIndex metadata property", icon.ItemSpec);
                }
            }

            outputFile.AppendLine();

            foreach (var file in EmbeddedFiles ?? Enumerable.Empty<ITaskItem>())
            {
                string resourceType = file.GetMetadata("ResourceType");
                string resourceName = file.GetMetadata("ResourceName");

                if (string.IsNullOrEmpty(resourceType))
                {
                    Log.LogError("Embedded file resource '{0}' must have a value for the ResourceType metadata property", file.ItemSpec);
                }
                else if (string.IsNullOrEmpty(resourceName))
                {
                    Log.LogError("Embedded file resource '{0}' must have a value for the ResourceName metadata property", file.ItemSpec);
                }
                else
                {
                    string path = file.GetMetadata("FullPath").Replace("\\", "\\\\");
                    outputFile.AppendLine($"{resourceName} {resourceType} \"{path}\"");
                }
            }

            outputFile.AppendLine();

            foreach (var fragment in ScriptFragments ?? Enumerable.Empty<ITaskItem>())
            {
                foreach (string line in File.ReadAllLines(fragment.GetMetadata("FullPath")))
                {
                    outputFile.AppendLine(line);
                }

                outputFile.AppendLine();
            }

            if (!Log.HasLoggedErrors)
            {
                string outputPath = Path.Combine(OutputDirectory, "Generated.rc");

                File.WriteAllText(outputPath, outputFile.ToString());
                ResourceScriptPath = new TaskItem(outputPath);
            }

            return !Log.HasLoggedErrors;
        }
    }
}
