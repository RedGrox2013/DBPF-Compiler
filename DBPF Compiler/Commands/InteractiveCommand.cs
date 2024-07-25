using DBPF_Compiler.ArgScript;

namespace DBPF_Compiler.Commands
{
    internal class InteractiveCommand : ASCommand
    {
        private static bool _run = false;

        public override void ParseLine(Line line)
        {
            if (_run)
                return;

            _run = true;
            line = Line.Empty;
            string[]? args;
            CommandManager.Instance.WriteLine("To exit, enter \"exit\"");

            do
            {
                CommandManager.Instance.ParseLine(line);
                CommandManager.Instance.Write(">>> ");
                args = CommandManager.Instance.ReadLine()?.Split();
                line = args == null ? Line.Empty : new(args);
            } while (args != null && !line[0].Equals("exit", StringComparison.OrdinalIgnoreCase));

            _run = false;
        }

        public override string? GetDescription(DescriptionMode mode = DescriptionMode.Basic)
        {
            return null;
        }
    }
}
