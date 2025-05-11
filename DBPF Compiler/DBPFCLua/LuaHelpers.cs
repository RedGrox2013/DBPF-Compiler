using NLua;
using System.Reflection;

namespace DBPF_Compiler.DBPFCLua
{
    public static class LuaHelpers
    {
        public static void RegisterEnum<T>(this Lua lua) where T : Enum =>
            RegisterEnum<T>(lua, typeof(T).Name);
        public static void RegisterEnum<T>(this Lua lua, string path) where T : Enum
        {
            var type = Enum.GetUnderlyingType(typeof(T));
            var dict = Enum.GetValues(typeof(T)).Cast<T>().ToDictionary(k => k.ToString(), v => Convert.ChangeType(v, type));
            RegisterDictionary(lua, dict, path);
        }

        public static void RegisterDictionary<TKey, TValue>(
            this Lua lua,
            Dictionary<TKey, TValue> dict,
            string path) where TKey : notnull
        {
            lua.NewTable(path);
            var table = lua.GetTable(path);
            foreach (var i in dict)
                table[i.Key.ToString()] = i.Value;
        }

        // public static void RegisterClass<T>(this Lua lua) => RegisterClass(lua, typeof(T));
        // public static void RegisterClass(this Lua lua, Type classType)
        // {
        //     RegisterClass(lua, classType, BindingFlags.Public | BindingFlags.Instance);

        //     lua.DoString($"function {classType.Name}.new() return setmetatable({{}}, {classType.Name}) end",
        //         $"{classType.Name} constructor");
        // }

        public static void RegisterStaticClass(this Lua lua, Type classType) =>
            RegisterClass(lua, classType, BindingFlags.Public | BindingFlags.Static);

        private static void RegisterClass(Lua lua, Type classType, BindingFlags bindingFlags)
        {
            lua.DoString($"{classType.Name} = {{}} {classType.Name}.__index = {classType.Name}");
            foreach (var member in classType.GetMembers(bindingFlags))
            {
                string path = $"{classType.Name}.{member.Name}";
                switch (member.MemberType)
                {
                    case MemberTypes.Method:
                        if (bindingFlags.HasFlag(BindingFlags.Static))
                            lua.RegisterFunction(path, (MethodBase)member);
                        else
                            lua.RegisterFunction($"{classType.Name}:{member.Name}", (MethodBase)member);
                        break;
                    case MemberTypes.Property:
                        lua[path] = ((PropertyInfo)member).GetValue(null);
                        break;
                    case MemberTypes.Field:
                        lua[path] = ((FieldInfo)member).GetValue(null);
                        break;
                    default:
                        continue;
                }
            }
        }

        public static object[] DoEmbeddedScript(this Lua lua, string resourcePath) =>
            DoEmbeddedScript(lua, resourcePath, Assembly.GetExecutingAssembly());

        public static object[] DoEmbeddedScript(this Lua lua, string resourcePath, Assembly assembly)
        {
            using var stream = assembly.GetManifestResourceStream(resourcePath) ??
                throw new NullReferenceException("Embedded script not found.");

            using var reader = new StreamReader(stream);
            return lua.DoString(reader.ReadToEnd());
        }
    }
}
