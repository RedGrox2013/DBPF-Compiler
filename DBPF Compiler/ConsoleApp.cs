﻿using DBPF_Compiler.ArgScript;
using DBPF_Compiler.Commands;

namespace DBPF_Compiler
{
    public class ConsoleApp : IDisposable
    {
        private bool _disposed;

        public CommandManager CommandManager { get; protected set; }

        public Action<object?>? PrintErrorAction
        {
            get => CommandManager.PrintErrorAction;
            set => CommandManager.PrintErrorAction = value;
        }

        public TraceConsole? Console
        {
            get => CommandManager.Console;
            set => CommandManager.Console = value;
        }

        public ConsoleApp() : this(new CommandManager()) {}
        public ConsoleApp(CommandManager cmd)
        {
            CommandManager = cmd;

            cmd.AddCommand("help", new HelpCommand(cmd));
            cmd.AddCommand("pack", new PackCommand());
            cmd.AddCommand("unpack", new UnpackCommand());
            cmd.AddCommand("encode", new EncodeCommand());
            cmd.AddCommand("decode", new DecodeCommand());
            cmd.AddCommand("hash", new HashCommand());
            cmd.AddCommand("hashToName", new HashToNameCommand());
            cmd.AddCommand("keys", new KeysListCommand());
            cmd.AddCommand("extract", new ExtractCommand());
            cmd.AddCommand("configs", new ConfigsCommand());
            cmd.AddCommand("createProject", new CreateProjectCommand());
            cmd.AddCommand("lua", new LuaCommand());
            //cmd.AddCommand("create-template", new CreateTemplateCommand());
            cmd.AddCommand("clear", new ClearCommand());
        }

        public bool Run(Line line)
        {
            try
            {
                CommandManager.ParseLine(line);
            }
            catch (Exception e)
            {
                PrintErrorAction?.Invoke(e.Message);
                return false;
            }

            return true;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // TODO: освободить управляемое состояние (управляемые объекты)
            }

            // TODO: освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить метод завершения
            // TODO: установить значение NULL для больших полей
            _disposed = true;
        }

        ~ConsoleApp()
        {
            // Не изменяйте этот код. Разместите код очистки в методе "Dispose(bool disposing)".
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Не изменяйте этот код. Разместите код очистки в методе "Dispose(bool disposing)".
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
