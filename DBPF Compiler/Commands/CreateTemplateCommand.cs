using DBPF_Compiler.ArgScript;
using DBPF_Compiler.ModsTemplates;

namespace DBPF_Compiler.Commands
{
    internal class CreateTemplateCommand : ConsoleCommand
    {
        public override void ParseLine(Line line)
        {
            string? projectPath = line.GetOption("p", 1)?[0];
            if (string.IsNullOrEmpty(projectPath))
            {
                PrintError("Required argument missing: <projectPath>");
                return;
            }

            string? type = line.GetOption("t", 1)?[0]?.ToLower();
            if (string.IsNullOrEmpty(type))
            {
                PrintError("Required argument missing: <type>");
                return;
            }

            var project = ModProject.Deserialize(projectPath);
            switch (type)
            {
                case "music":
                    string? folder = line.GetOption("-folder", 1)?[0];
                    if (string.IsNullOrEmpty(folder))
                    {
                        // доделать
                        throw new NotImplementedException("Specify the folder with music");
                    }

                    foreach (var file in Directory.GetFiles(folder))
                    {
                        var extension = Path.GetExtension(file);
                        if (!extension.Equals(".mp3", StringComparison.OrdinalIgnoreCase) &&
                            !extension.Equals(".snr", StringComparison.OrdinalIgnoreCase))
                            continue;

                        var template = new MusicModTemplate() { FileName = file };
                        string thumb = Path.GetFileNameWithoutExtension(file) + ".png";
                        if (File.Exists(thumb))
                            template.PlannerThumbnail = thumb;

                        project.AddModTemplate(template);
                    }
                    break;
                default:
                    PrintError("Unknown template: " + type);
                    return;
            }

            WriteLine(project.Name + ":\n" + ModProject.Serialize(project, projectPath));
        }
    }
}
