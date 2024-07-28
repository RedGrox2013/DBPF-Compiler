using DBPF_Compiler.ArgScript;

namespace DBPF_Compiler.Commands
{
    internal class ClearCommand : ConsoleCommand
    {
        public override void ParseLine(Line line)
        {
            if (!CommandManager.Instance.Clear())
                CommandManager.Instance.PrintError("Failed to clear");
        }

        public override string? GetDescription(DescriptionMode mode = DescriptionMode.Basic)
        {
            if (mode != DescriptionMode.HTML)
                return "clear console.";

            return null;
        }
    }
}
