using DBPF_Compiler.ArgScript;

namespace DBPF_Compiler.Commands
{
    internal class CreateProjectCommand : ConsoleCommand
    {
        public override void ParseLine(Line line)
        {
            if (line.ArgumentCount < 2)
            {
                PrintError("Required argument missing: <projectFolderPath>");
                return;
            }

            var project = new ModProject()
            {
                FolderPath = line[1],
                Name = line.GetOption("n", 1)?[0] ?? Path.GetFileName(line[1])
            };
            WriteLine(project.Name + ":\n" + ModProject.Serialize(project));
        }

        public override string? GetDescription(DescriptionMode mode = DescriptionMode.Basic)
        {
            if (mode == DescriptionMode.Basic)
                return "create project.";
            if (mode == DescriptionMode.Complete)
                return @"create project.
Usage:  create-project <projectFolderPath> [-n <projectName>]
    <projectFolderPath> path to project folder
    <projectName>       project name";

            return base.GetDescription(mode);
        }
    }
}
