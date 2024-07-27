using DBPF_Compiler.ArgScript;

namespace DBPF_Compiler.Commands
{
    internal class InteractiveCommand : ASCommand
    {
        public static bool IsRunning { get; private set; } = false;

        public override void ParseLine(Line line)
        {
            if (IsRunning)
                return;

            string? cmdLine;
            IsRunning = true;
            line = Line.Empty;
            CommandManager.Instance.WriteLine("To exit, enter \"exit\"");

            try
            {
                do
                {
                    CommandManager.Instance.ParseLine(line);

                    CommandManager.Instance.Write(">>> ");
                    cmdLine = CommandManager.Instance.ReadLine();
                    line = Lexer.LineToArgs(cmdLine);
                } while (cmdLine != null &&
                    (Line.IsNullOrEmpty(line) || !line[0].Equals("exit", StringComparison.OrdinalIgnoreCase)));
            }
            finally
            {
                IsRunning = false;
            }
        }

        public override string? GetDescription(DescriptionMode mode = DescriptionMode.Basic)
        {
            if (mode != DescriptionMode.HTML)
                return "enter interactive mode.";

            return null;
        }
    }
}
