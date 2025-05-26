using DBPF_Compiler.ArgScript;
using DBPF_Compiler.Commands;
using DBPF_Compiler.FileTypes.Prop;
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
        if (type == typeof(LuaUserData))
            return "userdata";
        return type == typeof(ProxyType) ? ((ProxyType)value).UnderlyingSystemType.Name : type.Name;
    }

    public static object? New(string className, params object?[]? args)
    {
        var type = Type.GetType(className);
        return type == null ? null : New(type, args);
    }

    // public static object? NewGeneric(string genericClassName, LuaTable types, params object?[]? args)
    // {
    //     var genericType = Type.GetType(genericClassName);
    //     if (genericType == null) return null;
    //
    //     var convertedTypes = new Type[types.Values.Count];
    //     int i = 0;
    //     foreach (var type in types.Values)
    //     {
    //         if (type is Type t)
    //             convertedTypes[i] = t;
    //         else
    //         {
    //             string? stype = type.ToString();
    //             if (stype == null)
    //                 throw new NullReferenceException("Type not found");
    //             
    //             convertedTypes[i] = Type.GetType(stype) ?? throw new NullReferenceException("Type not found");
    //         }
    //     
    //         ++i;
    //     }
    //     
    //     return New(genericType.MakeGenericType(convertedTypes), args);
    // }

    private static object? New(Type type, params object?[]? args)
    {
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

    public static PropertyList TableToPropertyList(LuaTable table) =>
        new(table.Values.OfType<Property>());
}
