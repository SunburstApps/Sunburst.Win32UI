using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Sunburst.Win32UI.BuildTasks
{
    public sealed class GenerateResourceScript : Task
    {
        [Required]
        public ITaskItem OutputFile { get; set; }
        public ITaskItem[] Icons { get; set; }
        public ITaskItem[] ScriptFragments { get; set; }

        public override bool Execute()
        {
            StringBuilder outputCode = new StringBuilder();
            outputCode.AppendLine("#include <Windows.h>");
            outputCode.AppendLine();

            foreach (var icon in Icons ?? Enumerable.Empty<ITaskItem>())
            {
                string indexString = icon.GetMetadata("IconIndex");
                if (!string.IsNullOrEmpty(indexString))
                {
                    bool parseOK = int.TryParse(indexString, out var iconIndex);
                    string path = icon.GetMetadata("FullPath").Replace("\\", "\\\\");
                    outputCode.AppendLine($"{iconIndex} ICON \"{path}\"");
                }
                else
                {
                    Log.LogError("Icon resource '{0}' must have a value for the IconIndex metadata property", icon.ItemSpec);
                }
            }

            outputCode.AppendLine();

            foreach (var fragment in ScriptFragments ?? Enumerable.Empty<ITaskItem>())
            {
                foreach (string line in File.ReadAllLines(fragment.GetMetadata("FullPath")))
                {
                    outputCode.AppendLine(line);
                }

                outputCode.AppendLine();
            }

            if (!Log.HasLoggedErrors)
            {
                Log.LogMessage(MessageImportance.High, "Writing to {0}", OutputFile.GetMetadata("FullPath"));
                File.WriteAllText(OutputFile.GetMetadata("FullPath"), outputCode.ToString(), Encoding.Unicode);
            }

            return !Log.HasLoggedErrors;
        }
    }
}
