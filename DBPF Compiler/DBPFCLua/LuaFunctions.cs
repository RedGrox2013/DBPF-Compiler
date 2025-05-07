using DBPF_Compiler.ArgScript;
using DBPF_Compiler.Commands;

namespace DBPF_Compiler.DBPFCLua;

internal static class LuaFunctions
{
    public static void ExecuteCommand(string argumentLine)
    {
        var cmd = DBPFCServices.GetService<CommandManager>() ??
            throw new NullReferenceException("Command manager not found");

        cmd.ParseLine(Lexer.LineToArgs(argumentLine));
    }
}