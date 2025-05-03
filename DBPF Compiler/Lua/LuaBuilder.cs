using System.Reflection;
using System.Text;

namespace DBPF_Compiler.Lua
{
    internal class LuaBuilder
    {
        private List<LuaFunctionInfo>? _functions;
        private readonly List<string> _configScripts = [@"
print    = trace
io.write = write
io.read  = readline
"];
        public TraceConsole? Console { get; set; }

        public Encoding Encoding { get; set; } = Encoding.UTF8;

        private readonly StringBuilder _packagePath = new(Path.Combine(Directory.GetCurrentDirectory(), "scripts", "?.lua"));
        private bool _hasDefaultPath = false;

        public LuaBuilder AddPackagePath(string path)
        {
            _packagePath.Append(';').Append(path);
            return this;
        }

        public LuaBuilder AddFunction(string path, object target, MethodBase function)
        {
            _functions ??= [];
            _functions.Add(new LuaFunctionInfo(path, target, function));

            return this;
        }
        public LuaBuilder AddFunction(string path, MethodBase function)
        {
            _functions ??= [];
            _functions.Add(new LuaFunctionInfo(path, null, function));

            return this;
        }

        public LuaBuilder AddConfigurationScript(string script)
        {
            _configScripts.Add(script);
            return this;
        }
        public LuaBuilder AddConfigurationScriptFromFile(string path) =>
            AddConfigurationScript(File.ReadAllText(path));

        public NLua.Lua Build()
        {
            var lua = new NLua.Lua();
            lua.State.Encoding = Encoding;

            if (!_hasDefaultPath)
            {
                _hasDefaultPath = true;
                _packagePath.Insert(0, ';').Insert(0, lua.GetTable("package")["path"] as string).Insert(0, "package.path = \"");
            }
            lua.DoString(_packagePath.Append('"').Replace("\\", "\\\\").ToString());
            _packagePath.Remove(_packagePath.Length - 1, 1).Replace("\\\\", "\\");

            if (Console != null)
            {
                lua.RegisterFunction("trace", Console, typeof(TraceConsole).GetMethod("WriteLine", [typeof(object)]));
                lua.RegisterFunction("write", Console, typeof(TraceConsole).GetMethod("Write"));
                lua.RegisterFunction("readline", Console, typeof(TraceConsole).GetMethod("ReadLine"));
                lua.RegisterFunction("hash", FNV.NameRegistryManager.Instance, typeof(FNV.NameRegistryManager).GetMethod("GetHash"));
            }

            if (_functions != null)
            {
                foreach (var func in _functions)
                {
                    if (func.Target == null)
                        lua.RegisterFunction(func.Path, func.Function);
                    else
                        lua.RegisterFunction(func.Path, func.Target, func.Function);
                }
            }

            foreach (var script in _configScripts)
                    lua.DoString(script);

            return lua;
        }
    }

    internal record class LuaFunctionInfo(string Path, object? Target, MethodBase? Function);
}
