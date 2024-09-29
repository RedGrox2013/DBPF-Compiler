using DBPF_Compiler.DBPF;
using DBPF_Compiler.ModsTemplates;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace DBPF_Compiler
{
    public class ModProject : IModTemplate
    {
        [JsonIgnore]
        public string? Name { get; set; }
        [JsonIgnore]
        public string? FolderPath { get; set; }

        public List<IModTemplate>? Templates { get; set; }

        private readonly static JsonSerializerOptions _jsonSerializerOptions = new()
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        };

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

            var proj = JsonSerializer.Deserialize<ModProject>(File.ReadAllText(filePath), _jsonSerializerOptions) ??
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

        public static string Serialize(ModProject project, string projectFolderPath)
        {
            string json = JsonSerializer.Serialize(project, _jsonSerializerOptions);
            File.WriteAllText(Path.Combine(projectFolderPath, project.Name + ".dbpfcproj"), json);

            return json;
        }

        public static string Serialize(ModProject project)
        {
            if (string.IsNullOrEmpty(project.FolderPath))
                throw new NullReferenceException();

            return Serialize(project, project.FolderPath);
        }

        public void BuildMod(DatabasePackedFile dbpf, DBPFPackerHelper helper)
        {
            if (Templates == null)
                return;

            helper.ProjectName = Name;
            foreach (var template in Templates)
                template.BuildMod(dbpf, helper);
        }
    }
}
