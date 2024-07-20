using DBPF_Compiler.ArgScript;

namespace DBPF_Compiler.Commands
{
    public class CommandManager
    {
        private static CommandManager? _instance;
        public static CommandManager Instance => _instance ??= new CommandManager();

        private Action<object?>? _write, _writeLine, _printError;
        public event Action<object?> WriteEvent { add => _write += value; remove => _write -= value; }
        public event Action<object?> WriteLineEvent { add => _writeLine += value; remove => _writeLine -= value; }
        public event Action<object?> PrintErrorEvent { add => _printError += value; remove => _printError -= value; }

        private readonly Dictionary<string, ASCommand> _commands = [];

        private CommandManager() { }

        public void Write(object? message) => _write?.Invoke(message);
        public void WriteLine(object? message) => _writeLine?.Invoke(message);
        public void PrintError(object? message) => _printError?.Invoke(message);

        public void AddCommand(ASCommand command, params string[] keywords)
        {
            foreach (string keyword in keywords)
                _commands.Add(keyword, command);
        }
        public ASCommand GetCommand(string keyword) => _commands[keyword];

        public static CommandManager Initialize()
        {
            _instance = new CommandManager();
            _instance.AddCommand(new PackCommand(), "-p", "--pack", "pack");

            return _instance;
        }

        public void ParseLine(Line? line)
        {
            if (Line.IsNullOrEmpty(line))
                return;

#pragma warning disable CS8602 // Разыменование вероятной пустой ссылки.
            string keyword = line[0];
#pragma warning restore CS8602 // Разыменование вероятной пустой ссылки.

            if (keyword.Equals("-h", StringComparison.OrdinalIgnoreCase) ||
                keyword.Equals("--help", StringComparison.OrdinalIgnoreCase) ||
                keyword.Equals("help", StringComparison.OrdinalIgnoreCase))
            {
                if (line.ArgumentCount > 1)
                    for (int i = 1; i < line.ArgumentCount; i++)
                        PrintHelp(line[i]);
                else
                    PrintHelp();

                return;
            }

            if (!_commands.TryGetValue(keyword, out var command))
                _printError?.Invoke(keyword + ": unknown command");
            else
                command.ParseLine(line);
        }

        public void PrintHelp(string? commandName = null)
        {
            if (_writeLine == null)
                return;

            if (string.IsNullOrWhiteSpace(commandName))
            {
                foreach (var command in _commands)
                    _writeLine(command.Key + ":\t" + (command.Value.GetDescription() ?? "no description"));
                return;
            }

            if (!_commands.TryGetValue(commandName, out var cmd))
                _printError?.Invoke(commandName + " is not found");
            else
                _writeLine(commandName + ":\t" +
                    (cmd.GetDescription(DescriptionMode.Complete) ?? cmd.GetDescription() ?? "no description"));
        }
    }
}
