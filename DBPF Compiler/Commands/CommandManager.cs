using DBPF_Compiler.ArgScript;

namespace DBPF_Compiler.Commands
{
    public class CommandManager : IParser
    {
        private static CommandManager? _instance;
        public static CommandManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CommandManager();

                    _instance.AddCommand("help", new HelpCommand());
                    _instance.AddCommand("pack", new PackCommand());
                    _instance.AddCommand("unpack", new UnpackCommand());
                    _instance.AddCommand("encode", new EncodeCommand());
                    _instance.AddCommand("decode", new DecodeCommand());
                    _instance.AddCommand("hash", new HashCommand());
                    _instance.AddCommand("hash-to-name", new HashToNameCommand());
                    _instance.AddCommand("keys", new KeysListCommand());
                    _instance.AddCommand("extract", new ExtractCommand());
                    _instance.AddCommand("configs", new ConfigsCommand());
                    _instance.AddCommand("create-project", new CreateProjectCommand());
                    _instance.AddCommand("create-template", new CreateTemplateCommand());
                    _instance.AddCommand("clear", new ClearCommand());
                }

                return _instance;
            }
        }

        public FormatParser FormatParser { get; private set; } = new();
        public object? Data { get; private set; }

        private Action<object?>? _printErrorAction;
        public Action<object?>? PrintErrorAction
        {
            get => _printErrorAction;
            set
            {
                _printErrorAction = value;
                foreach (var command in _commands)
                    command.Value.PrintErrorAction = value;
            }
        }
        private Action? _clearAction;
        public Action? ClearAction
        {
            get => _clearAction;
            set
            {
                _clearAction = value;
                foreach (var command in _commands)
                    command.Value.ClearAction = value;
            }
        }
        private TextWriter? _out;
        public TextWriter? Out
        {
            get => _out;
            set
            {
                _out = value;
                foreach (var command in _commands)
                    command.Value.Out = value;
            }
        }

        private readonly Dictionary<string, ConsoleCommand> _commands = [];

        private CommandManager() { }

        public void AddCommand(string keyword, ConsoleCommand command)
        {
            command.SetData(FormatParser, Data);
            command.Out = _out;
            command.ClearAction = _clearAction;
            command.PrintErrorAction = _printErrorAction;
            _commands.Add(keyword, command);
        }
        public ConsoleCommand GetCommand(string keyword) => _commands[keyword];

        public void SetData(FormatParser formatParser, object? data)
        {
            FormatParser = formatParser;
            Data = data;

            foreach (var command in _commands)
                command.Value.SetData(formatParser, data);
        }

        public void ParseLine(Line? line)
        {
            if (Line.IsNullOrEmpty(line))
                return;

#pragma warning disable CS8602 // Разыменование вероятной пустой ссылки.
            string keyword = line[0];
#pragma warning restore CS8602 // Разыменование вероятной пустой ссылки.

            if (!_commands.TryGetValue(keyword, out var command))
                PrintErrorAction?.Invoke(keyword + ": unknown command");
            else
                command.ParseLine(line);
        }

        public string? GetDescription(DescriptionMode mode = DescriptionMode.Basic)
            => null;

        public void PrintHelp(string? commandName = null)
        {
            if (Out == null)
                return;

            if (string.IsNullOrWhiteSpace(commandName))
            {
                foreach (var command in _commands)
                {
                    if (!command.Value.NotDisplayDescription)
                        Out.WriteLine(command.Key + "\t\t\t" + (command.Value.GetDescription() ?? "no description"));
                }
                return;
            }

            if (!_commands.TryGetValue(commandName, out var cmd))
                PrintErrorAction?.Invoke(commandName + " is not found");
            else
                Out.WriteLine(commandName + "\t" +
                    (cmd.GetDescription(DescriptionMode.Complete) ?? cmd.GetDescription() ?? "no description"));
        }
    }
}
