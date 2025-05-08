using DBPF_Compiler.FNV;
using NLua;

namespace DBPF_Compiler.DBPFCLua;

public class LuaCreator(TraceConsole console)
{
    public TraceConsole Console { get; set; } = console;

    public Lua CreateLua()
    {
        var lua = new Lua();
        lua.State.Encoding = System.Text.Encoding.UTF8;

        lua.RegisterEnum<TypeIDs>();
        lua.RegisterEnum<GroupIDs>();

        lua.RegisterFunction("__trace__", Console, typeof(TraceConsole).GetMethod("WriteLine", [typeof(object)]));
        lua.RegisterFunction("__write__", Console, typeof(TraceConsole).GetMethod("Write"));
        lua.RegisterFunction("readline", Console, typeof(TraceConsole).GetMethod("ReadLine"));
        lua.RegisterFunction("__hash__", NameRegistryManager.Instance, typeof(NameRegistryManager).GetMethod("GetHash"));
        lua.RegisterFunction("hashtoname", NameRegistryManager.Instance, typeof(NameRegistryManager).GetMethod("GetName"));
        lua.RegisterFunction("getProgramDirectory", typeof(Directory).GetMethod("GetCurrentDirectory"));
        lua.RegisterFunction("executeCommand", typeof(LuaFunctions).GetMethod("ExecuteCommand"));

        lua.RegisterStaticClass(typeof(FNVHash));

        lua.DoString(@$"package.path = package.path ..
    "";{Path.Combine(Directory.GetCurrentDirectory(), "scripts", "?.lua").Replace("\\", "\\\\")}""

local versionmt = {{
    __tostring = function(self)
        return self.major .. '.' .. self.minor .. '.' .. self.build .. '.' .. self.revision
    end
}}
versionmt.__index = versionmt
DBPFC_VERSION = setmetatable({{}}, versionmt)

local _trace = __trace__
local _write = __write__
local _hash  = __hash__
__trace__, __write__, __hash__ = nil, nil, nil
function write(v) _write(tostring(v)) end
function hash (v) return _hash (tostring(v)) end
function trace(...)
    local args = table.pack(...)
    if args.n == 1 then
        _trace(tostring(args[1]))
        return
    end

    for i = 1, args.n do
        write(args[i])
        _write('\t')
    end
    _write('\n')
end

-- для лучшей совместимости с DBPFC некоторые функции будут переопределены
print    = trace
io.write = write
io.read  = readline
os.exit  = nil -- добавить потом норм метод

-- чтобы не было путаницы с регистром имён функций
readLine   = readline
hashToName = hashtoname
", "configuration");

        var version = GetType().Assembly.GetName().Version;
        lua["DBPFC_VERSION.major"]    = version?.Major;
        lua["DBPFC_VERSION.minor"]    = version?.Minor;
        lua["DBPFC_VERSION.build"]    = version?.Build;
        lua["DBPFC_VERSION.revision"] = version?.Revision;

        return lua;
    }
}
