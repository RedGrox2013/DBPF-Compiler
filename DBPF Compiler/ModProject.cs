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

        public const string PROJECT_FILE_EXTENSION = ".dbpfcproj";

        // TODO: переделать хранение шаблонов
        [JsonIgnore]
        public List<IModTemplate>? Templates { get; set; }

        public ModTemplateType TemplateType => ModTemplateType.Project;

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

        /// <summary>
        /// Загружает проект из папки или из <c>.dbpfcproj</c>-файла
        /// </summary>
        /// <param name="projectFolderPath">Путь к папке проекта</param>
        /// <returns>Загруженный <see cref="ModProject"/></returns>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        public static ModProject Load(string projectFolderPath)
        {
            string? filePath;
            if (projectFolderPath.EndsWith(PROJECT_FILE_EXTENSION))
            {
                filePath = projectFolderPath;
                projectFolderPath = Path.GetDirectoryName(projectFolderPath) ??
                    throw new FileNotFoundException("Project file not found.");
            }
            else
            {
                filePath = null;
                foreach (var f in Directory.GetFiles(projectFolderPath))
                    if (f.EndsWith(PROJECT_FILE_EXTENSION, StringComparison.OrdinalIgnoreCase))
                    {
                        filePath = f;
                        break;
                    }
                if (filePath == null)
                    throw new FileNotFoundException("Project file not found.");
            }

            var proj = JsonSerializer.Deserialize<ModProject>(File.ReadAllText(filePath), _jsonSerializerOptions) ??
                throw new NotSupportedException(Path.GetFileName(filePath) + " is not mod project.");
            proj.Name = Path.GetFileNameWithoutExtension(filePath);
            proj.FolderPath = projectFolderPath;

            return proj;
        }

        public static bool TryLoad(string projectFolderPath, out ModProject? project)
        {
            try
            {
                project = Load(projectFolderPath);
                return true;
            }
            catch
            {
                project = null;
                return false;
            }
        }

        public static string Save(ModProject project, string projectFolderPath)
        {
            if (projectFolderPath.EndsWith(PROJECT_FILE_EXTENSION))
                projectFolderPath = Path.GetDirectoryName(projectFolderPath) ??
                    throw new FileNotFoundException("Project file not found.");

            string json = JsonSerializer.Serialize(project, _jsonSerializerOptions);
            File.WriteAllText(Path.Combine(projectFolderPath, project.Name + PROJECT_FILE_EXTENSION), json);

            //if (project.Templates != null)
            //    foreach (var template in project.Templates)
            //    {
            //        var path = new DirectoryInfo(Path.Combine(projectFolderPath, "Templates.dbpfc_ignore", template.TemplateType.ToString()));
            //        if (!path.Exists)
            //            path.Create();

            //        switch (template.TemplateType)
            //        {
            //            case ModTemplateType.Music:
            //                if (template is not MusicModTemplate musicTemplate)
            //                    throw new NullReferenceException();
            //                // доделать сериализацию
            //                break;
            //        }
            //    }

            return json;
        }

        public static string Save(ModProject project)
        {
            if (string.IsNullOrEmpty(project.FolderPath))
                throw new NullReferenceException();

            return Save(project, project.FolderPath);
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
