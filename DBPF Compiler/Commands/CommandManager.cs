﻿using DBPF_Compiler.ArgScript;

namespace DBPF_Compiler.Commands
{
    public class CommandManager
    {
        private static CommandManager? _instance;
        public static CommandManager Instance => _instance ??= new CommandManager();

        private Action<object?>? _printError;
        public event Action<object?> PrintErrorEvent { add => _printError += value; remove => _printError -= value; }
        public TextWriter? Out { get; set; }

        private readonly Dictionary<string, ASCommand> _commands = [];

        private CommandManager() { }

        public void Write(object? message) => Out?.Write(message);
        public void WriteLine(object? message) => Out?.WriteLine(message);
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
            _instance.AddCommand(new PackCommand(),   "pack",   "--pack",   "-p");
            _instance.AddCommand(new UnpackCommand(), "unpack", "--unpack", "-u");
            _instance.AddCommand(new EncodeCommand(), "encode", "--encode", "-e");
            _instance.AddCommand(new DecodeCommand(), "decode", "--decode", "-d");
            _instance.AddCommand(new HashCommand(),   "hash", "--hash");

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
            if (Out == null)
                return;

            if (string.IsNullOrWhiteSpace(commandName))
            {
                foreach (var command in _commands)
                    Out.WriteLine(command.Key + ":\t\t" + (command.Value.GetDescription() ?? "no description"));
                return;
            }

            if (!_commands.TryGetValue(commandName, out var cmd))
                _printError?.Invoke(commandName + " is not found");
            else
                Out.WriteLine(commandName + ":\t" +
                    (cmd.GetDescription(DescriptionMode.Complete) ?? cmd.GetDescription() ?? "no description"));
        }
    }
}
