using NLua;

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
    }
}
