using DBPF_Compiler.ArgScript;

namespace DBPF_Compiler.Commands
{
    internal class HelpCommand(CommandManager cmd) : ConsoleCommand
    {
        public CommandManager CommandManager { get; set; } = cmd;

        public override void ParseLine(Line line)
        {
            if (line.ArgumentCount > 1)
                for (int i = 1; i < line.ArgumentCount; i++)
                    CommandManager.PrintHelp(line[i]);
            else
                CommandManager.PrintHelp();
        }

        public override string? GetDescription(DescriptionMode mode = DescriptionMode.Basic)
        {
            if (mode == DescriptionMode.Basic)
                return "get help.";
            if (mode == DescriptionMode.Complete)
                return @"get help.
Usage:  help [<commands>]
    <commands>  commands for which you want help.
                If you want a short description of all commands,
                use without arguments.";

            return null;
        }
    }
}
