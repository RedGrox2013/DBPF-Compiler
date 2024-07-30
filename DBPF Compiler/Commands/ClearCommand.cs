using DBPF_Compiler.ArgScript;

namespace DBPF_Compiler.Commands
{
    internal class ClearCommand : ConsoleCommand
    {
        public override void ParseLine(Line line)
        {
            if (ClearAction == null)
                PrintErrorAction?.Invoke("Failed to clear");
            else
                ClearAction();
        }

        public override string? GetDescription(DescriptionMode mode = DescriptionMode.Basic)
        {
            if (mode != DescriptionMode.HTML)
                return "clear console.";

            return null;
        }
    }
}
