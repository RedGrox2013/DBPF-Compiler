using DBPF_Compiler.FNV;
using NLua;
using System.Reflection;

namespace DBPF_Compiler.DBPFCLua;

public class LuaCreator
{
    public TraceConsole? Console { get; set; }

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
", "configuration");

        var assembly = Assembly.GetExecutingAssembly();
        lua.DoEmbeddedScript("DBPF_Compiler.scripts.initLua.lua", assembly);
        lua.DoEmbeddedScript("DBPF_Compiler.scripts.classes.DBPFCObject.lua", assembly);
        lua.DoEmbeddedScript("DBPF_Compiler.scripts.classes.Key.lua", assembly);
        lua.DoEmbeddedScript("DBPF_Compiler.scripts.classes.Property.lua", assembly);
        lua.DoEmbeddedScript("DBPF_Compiler.scripts.classes.PropertyList.lua", assembly);

        //foreach (var resource in assembly.GetManifestResourceNames())
        //    if (resource.StartsWith("DBPF_Compiler.scripts.classes"))
        //        lua.DoEmbeddedScript(resource, assembly);
        
        var version = assembly.GetName().Version;
        lua["DBPFC_VERSION.major"]    = version?.Major;
        lua["DBPFC_VERSION.minor"]    = version?.Minor;
        lua["DBPFC_VERSION.build"]    = version?.Build;
        lua["DBPFC_VERSION.revision"] = version?.Revision;

        return lua;
    }
}
