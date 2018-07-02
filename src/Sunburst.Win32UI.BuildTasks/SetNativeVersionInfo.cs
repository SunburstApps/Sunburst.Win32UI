using System.Text.RegularExpressions;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Vestris.ResourceLib;

namespace Sunburst.Win32UI.BuildTasks
{
    public class SetNativeVersionInfo : Task
    {
        [Required]
        public ITaskItem[] InputFiles { get; set; }

        [Required]
        public string FileVersion { get; set; }
        [Required]
        public string ProductVersion { get; set; }
        public string CompanyName { get; set; }
        public string FileDescription { get; set; }
        public string LegalCopyright { get; set; }
        public string ProductName { get; set; }

        public override bool Execute()
        {
            if (!Regex.IsMatch(FileVersion, @"\d+\.\d+.\d+\.\d+"))
            {
                Log.LogError("FileVersion value '{0}' is invalid - must be NNN.NNN.NNN.NNN (where N is any digit).", FileVersion);
                return false;
            }

            if (!Regex.IsMatch(ProductVersion, @"\d+\.\d+\.\d+\.\d+"))
            {
                Log.LogError("ProductVersion value '{0}' is invalid - must be NNN.NNN.NNN.NNN (where N is any digit).", ProductVersion);
                return false;
            }

            foreach (ITaskItem file in InputFiles)
            {
                string fileName = file.GetMetadata("FileName") + file.GetMetadata("Extension");

                VersionResource resource = new VersionResource();
                resource.FileVersion = FileVersion;
                resource.ProductVersion = ProductVersion;
                resource.FileFlags = 0;

                StringTable table = new StringTable();
                table.LanguageID = 0x0409;
                table.CodePage = 0x04B0;

                table["CompanyName"] = CompanyName;
                table["FileDescription"] = FileDescription;
                table["FileVersion"] = FileVersion;
                table["InternalName"] = fileName;
                table["LegalCopyright"] = LegalCopyright;
                table["OriginalFilename"] = fileName;
                table["ProductName"] = ProductName;
                table["ProductVersion"] = ProductVersion;

                StringFileInfo stringInfo = new StringFileInfo();
                stringInfo.Strings["040904B0"] = table;
                resource.Resources["StringFileInfo"] = stringInfo;

                VarTable varTable = new VarTable("Translation");
                varTable.Languages[0x0409] = 0x04B0;
                VarFileInfo varInfo = new VarFileInfo();
                varInfo.Vars["Translation"] = varTable;
                resource.Resources["VarFileInfo"] = varInfo;

                resource.SaveTo(file.GetMetadata("FullPath"));
            }

            return true;
        }
    }
}