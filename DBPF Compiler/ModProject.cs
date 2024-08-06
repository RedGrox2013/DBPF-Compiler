using DBPF_Compiler.DBPF;
using DBPF_Compiler.ModsTemplates;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DBPF_Compiler
{
    public class ModProject : IModTemplate
    {
        [JsonIgnore]
        public string? Name { get; set; }
        [JsonIgnore]
        public string? FolderPath { get; set; }

        public List<IModTemplate>? Templates { get; set; }

        public void AddModTemplate(IModTemplate template)
        {
            if (Templates == null)
            {
                Templates = [template];
                return;
            }

            Templates.Add(template);
        }

        public static ModProject Deserialize(string projectFolderPath)
        {
            string? filePath = null;
            foreach (var f in Directory.GetFiles(projectFolderPath))
                if (f.EndsWith(".dbpfcproj", StringComparison.OrdinalIgnoreCase))
                {
                    filePath = f;
                    break;
                }
            if (filePath == null)
                throw new Exception("Project file not found.");

            var proj = JsonSerializer.Deserialize<ModProject>(File.ReadAllText(filePath)) ??
                throw new NotSupportedException(Path.GetFileName(filePath) + " is not mod project.");
            proj.Name = Path.GetFileNameWithoutExtension(filePath);
            proj.FolderPath = projectFolderPath;

            return proj;
        }

        public static bool TryDeserialize(string projectFolderPath, out ModProject? project)
        {
            try
            {
                project = Deserialize(projectFolderPath);
                return true;
            }
            catch
            {
                project = null;
                return false;
            }
        }

        public void BuildMod(DatabasePackedFile dbpf)
        {
            if (Templates == null)
                return;

            foreach (var template in Templates)
                template.BuildMod(dbpf);
        }
    }
}
