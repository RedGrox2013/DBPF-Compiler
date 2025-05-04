using DBPF_Compiler.ArgScript;
using NLua;

namespace DBPF_Compiler.Commands
{
    internal class LuaCommand(Lua luaInterpreter) : ConsoleCommand
    {
        public Lua LuaInterpreter { get; set; } = luaInterpreter;

        public override void ParseLine(Line line)
        {
            if (line.ArgumentCount > 1)
            {
                //string path = Path.Combine(new FileInfo(line[1]).DirectoryName ?? string.Empty, "?.LuaInterpreter");
                //LuaInterpreter.DoString($"package.path = package.path .. \";{path.Replace("\\", "\\\\")}\"");

                LuaInterpreter.NewTable("arg");
                for (int i = 0; i < line.ArgumentCount; i++)
                    ((LuaTable)LuaInterpreter["arg"])[i - 1] = line[i];
                LuaInterpreter.DoFile(line[1]);

                return;
            }

            WriteLine(LuaInterpreter["_VERSION"] +
                ".7  Copyright © 1994–2024 Lua.org, PUC-Rio\nTo exit, enter a blank line");
            string? l;
            do
            {
                Write("> ");
                l = Console?.ReadLine();

                try
                {
                    var results = LuaInterpreter.DoString(l);

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
