using DBPF_Compiler.DBPF;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DBPF_Compiler
{
    public class ModProject
    {
        [JsonIgnore]
        public string? Name { get; set; }
        [JsonIgnore]
        public string? FolderPath { get; set; }

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
    }
}
