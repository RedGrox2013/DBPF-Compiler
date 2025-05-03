using DBPF_Compiler.ArgScript;
using DBPF_Compiler.Lua;
using NLua;

namespace DBPF_Compiler.Commands
{
    internal class LuaCommand : ConsoleCommand
    {
        public override TraceConsole? Console
        {
            get => base.Console;
            set
            {
                base.Console = value;
                _luaBuilder.Console = value;
            }
        }

        private readonly LuaBuilder _luaBuilder;

        public LuaCommand()
        {
            _luaBuilder = new LuaBuilder() { Console = Console };
        }

        public override void ParseLine(Line line)
        {
            using var lua = _luaBuilder.Build();
            if (line.ArgumentCount > 1)
            {
                //string path = Path.Combine(new FileInfo(line[1]).DirectoryName ?? string.Empty, "?.lua");
                //lua.DoString($"package.path = package.path .. \";{path.Replace("\\", "\\\\")}\"");

                lua.NewTable("arg");
                for (int i = 0; i < line.ArgumentCount; i++)
                    ((LuaTable)lua["arg"])[i - 1] = line[i];
                lua.DoFile(line[1]);

                return;
            }

            WriteLine("To exit, enter a blank line");
            string? l;
            do
            {
                Write("> ");
                l = Console?.ReadLine();

                try
                {
                    var results = lua.DoString(l);

                    if (results != null && results.Length > 0)
                    {
                        foreach (var res in results)
                            Write((res ?? "nil") + "\t");
                        WriteLine();
                    }
                }
                catch (Exception e)
                {
                    PrintError(e.Message);
                }
            } while (!string.IsNullOrWhiteSpace(l));
        }

        public override string? GetDescription(DescriptionMode mode = DescriptionMode.Basic)
        {
            if (mode == DescriptionMode.Basic)
                return "run lua script.";
            if (mode == DescriptionMode.Complete)
                return @"run lua script.
Usage:       lua [<scriptPath>] [<args>]
<scriptPath> path to the file with the script
<args>       command line arguments";

            return base.GetDescription(mode);
        }
    }
}
