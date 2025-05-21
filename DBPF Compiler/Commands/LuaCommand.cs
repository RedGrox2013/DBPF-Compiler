using DBPF_Compiler.ArgScript;
using DBPF_Compiler.DBPFCLua;
using NLua;

namespace DBPF_Compiler.Commands
{
    internal class LuaCommand : ConsoleCommand
    {
        private readonly LuaCreator _luaCreator = new();

        public override TraceConsole? Console
        {
            get => base.Console;
            set => base.Console = _luaCreator.Console = value;
        }

        public override void ParseLine(Line line)
        {
            using var lua = _luaCreator.CreateLua();

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

            lua.DoString("""

                         __isLuaCommandRunning__ = true

                         function exit()
                             exit = nil
                             __isLuaCommandRunning__ = false
                         end

                         """);

            WriteLine(lua["_VERSION"] +
                ".7  Copyright © 1994–2024 Lua.org, PUC-Rio\nTo exit, call exit()");
            string? l;
            do
            {
                Write("> ");
                l = Console?.ReadLine();

                try
                {
                    var results = lua.DoString(l, nameof(LuaCommand));

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
            } while ((bool)lua["__isLuaCommandRunning__"]);
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
