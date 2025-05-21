using DBPF_Compiler.ArgScript;
using DBPF_Compiler.Commands;
using NLua;

namespace DBPF_Compiler.DBPFCLua;

internal static class LuaFunctions
{
    public static void ExecuteCommand(string argumentLine)
    {
        var cmd = DBPFCServices.GetService<CommandManager>() ??
                  throw new NullReferenceException("Command manager not found");

        cmd.ParseLine(Lexer.LineToArgs(argumentLine));
    }

    public static string TypeOf(object? value)
    {
        if (value == null)
            return "nil";

        var type = value.GetType();
        if (type == typeof(int) ||
            type == typeof(float) ||
            type == typeof(double) ||
            type == typeof(byte) ||
            type == typeof(long) ||
            type == typeof(uint) ||
            type == typeof(ulong) ||
            type == typeof(decimal) ||
            type == typeof(short) ||
            type == typeof(ushort) ||
            type == typeof(sbyte))
            return "number";
        if (type == typeof(bool))
            return "boolean";
        if (type == typeof(string))
            return "string";
        if (type == typeof(LuaFunction))
            return "function";
        if (type == typeof(LuaTable))
            return "table";
        if (type == typeof(LuaThread))
            return "thread";

        return type == typeof(LuaUserData) ? "userdata" : type.FullName ?? type.Name;
    }

    public static object? New(string className, params object?[]? args)
    {
        var type = Type.GetType(className);
        if (type == null)
            return null;

        if (args == null || args.Length == 0)
            return Activator.CreateInstance(type);
        
        var constructors = type.GetConstructors();
        var ctor = constructors.FirstOrDefault(c => c.GetParameters().Length == args.Length);
        if (ctor == null)
            throw new MissingMethodException($"No constructor with {args.Length} parameters found.");
        
        var parameters = ctor.GetParameters();
        var convertedArgs = new object?[args.Length];
        
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] is not long l)
            {
                convertedArgs[i] = args[i];
                continue;
            }
            
            var paramType = parameters[i].ParameterType;
            
            if (paramType == typeof(int)) convertedArgs[i] = Convert.ToInt32(l);
            else if (paramType == typeof(uint)) convertedArgs[i] = Convert.ToUInt32(l);
            else if (paramType == typeof(double)) convertedArgs[i] = Convert.ToInt64(l);
            else if (paramType == typeof(ulong)) convertedArgs[i] = Convert.ToUInt64(l);
            else if (paramType == typeof(short)) convertedArgs[i] = Convert.ToInt16(l);
            else if (paramType == typeof(ushort)) convertedArgs[i] = Convert.ToUInt16(l);
            else if (paramType == typeof(float)) convertedArgs[i] = Convert.ToSingle(l);
            else if (paramType == typeof(decimal)) convertedArgs[i] = Convert.ToDecimal(l);
            else if (paramType == typeof(byte)) convertedArgs[i] = Convert.ToByte(l);
            else if (paramType == typeof(sbyte)) convertedArgs[i] = Convert.ToSByte(l);
            else convertedArgs[i] = args[i];
        }
        
        return Activator.CreateInstance(type, convertedArgs);
    }
}
