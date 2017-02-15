using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Vestris.ResourceLib;

namespace Win32UI.Build.Tasks
{
    public sealed class GenerateEntryPointResources : Task
    {
        [Required]
        public ITaskItem OutputFile { get; set; }

        [Required]
        public string UacPrivilegeLevel { get; set; }
        public ITaskItem[] ManifestDependencies { get; set; }
        public ITaskItem[] ManifestFragments { get; set; }
        public ITaskItem[] Icons { get; set; }
        public ITaskItem[] EmbeddedFiles { get; set; }

        [Required]
        public string FileVersion { get; set; }
        [Required]
        public string ProductVersion { get; set; }
        [Required]
        public string CompanyName { get; set; }
        [Required]
        public string FileDescription { get; set; }
        [Required]
        public string LegalCopyright { get; set; }
        [Required]
        public string ProductName { get; set; }

        public override bool Execute()
        {
            List<Resource> resources = new List<Resource>();

            VersionResource version = new VersionResource();
            version.FileVersion = FileVersion;
            version.ProductVersion = ProductVersion;
            version["VarFileInfo"] = GetVersionVarFileInfo();
            version["StringFileInfo"] = GetVersionStringFileInfo();
            resources.Add(version);

            throw new NotImplementedException();
        }

        private StringFileInfo GetVersionStringFileInfo()
        {
            string originalFilename = System.IO.Path.GetFileName(OutputFile.GetMetadata("FullPath"));

            StringFileInfo stringInfo = new StringFileInfo();
            StringTable table = new StringTable();
            table.LanguageID = 1033;

            AddVersionString(table, "CompanyName", CompanyName);
            AddVersionString(table, "FileDescription", FileDescription);
            AddVersionString(table, "FileVersion", FileVersion);
            AddVersionString(table, "LegalCopyright", LegalCopyright);
            AddVersionString(table, "OriginalFilename", originalFilename);
            AddVersionString(table, "InternalName", originalFilename);
            AddVersionString(table, "ProductVersion", ProductVersion);

            stringInfo.Strings.Add("040904b0", table);
            return stringInfo;
        }

        private void AddVersionString(StringTable table, string key, string value)
        {
            table.Strings.Add(key, new StringTableEntry(key) { Value = value });
        }

        private VarFileInfo GetVersionVarFileInfo()
        {
            VarFileInfo varInfo = new VarFileInfo();
            VarTable table = new VarTable();
            table[1033] = 1252;
            varInfo.Vars.Add("Translation", table);
            return varInfo;
        }
    }
}
