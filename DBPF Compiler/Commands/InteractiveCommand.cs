using DBPF_Compiler.ArgScript;

namespace DBPF_Compiler.Commands
{
    internal class InteractiveCommand(TextReader input) : ConsoleCommand
    {
        public static bool IsRunning { get; private set; } = false;
        public TextReader In { get; set; } = input;

        public override void ParseLine(Line line)
        {
            if (IsRunning)
            {
                PrintError("Interactive mode is already running");
                return;
            }

            string? cmdLine;
            IsRunning = true;
            line = Line.Empty;
            Out?.WriteLine("To exit, enter \"exit\"");

            do
            {
                try
                {
                    CommandManager.Instance.ParseLine(line);

                    Out?.Write("dbpfc>");
                    cmdLine = In.ReadLine();
                    line = Lexer.LineToArgs(cmdLine);
                }
                catch (Exception e)
                {
                    PrintError(e.Message);
                    cmdLine = string.Empty;
                }
            } while (cmdLine != null &&
                (Line.IsNullOrEmpty(line) || !line[0].Equals("exit", StringComparison.OrdinalIgnoreCase)));

            IsRunning = false;
        }

        public override string? GetDescription(DescriptionMode mode = DescriptionMode.Basic)
        {
            if (mode != DescriptionMode.HTML)
                return "enter interactive mode.";

            return null;
        }
    }
}
