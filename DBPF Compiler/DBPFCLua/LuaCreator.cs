﻿using DBPF_Compiler.FNV;
using NLua;
using System.Reflection;

namespace DBPF_Compiler.DBPFCLua;

public class LuaCreator
{
    public TraceConsole? Console { get; set; }

    public Lua CreateLua(bool loadCLR = true)
    {
        var lua = new Lua();
        lua.State.Encoding = System.Text.Encoding.UTF8;

        lua.RegisterEnum<TypeIDs>();
        lua.RegisterEnum<GroupIDs>();

        if (loadCLR)
        {
            lua.LoadCLRPackage();
            lua.DoString($"""
                            import('dbpfc', 'DBPF_Compiler.FNV')
                            import('dbpfc', 'DBPF_Compiler.FileTypes.Prop')
                         """, "imports");
        }

        lua.RegisterFunction("__trace__", Console, typeof(TraceConsole).GetMethod("WriteLine", [typeof(object)]));
        lua.RegisterFunction("__write__", Console, typeof(TraceConsole).GetMethod("Write"));
        lua.RegisterFunction("readline", Console, typeof(TraceConsole).GetMethod("ReadLine"));
        lua.RegisterFunction("__hash__", NameRegistryManager.Instance, typeof(NameRegistryManager).GetMethod("GetHash"));
        lua.RegisterFunction("hashtoname", NameRegistryManager.Instance, typeof(NameRegistryManager).GetMethod("GetName"));
        lua.RegisterFunction("getProgramDirectory", typeof(Directory).GetMethod("GetCurrentDirectory"));
        lua.RegisterFunction("executeCommand", typeof(LuaFunctions).GetMethod("ExecuteCommand"));
        lua.RegisterFunction("typeof", typeof(LuaFunctions).GetMethod("TypeOf"));
        lua.RegisterFunction("new", typeof(LuaFunctions).GetMethod("New"));
        // lua.RegisterFunction("newGeneric", typeof(LuaFunctions).GetMethod("NewGeneric"));
        // lua.RegisterFunction("getType", typeof(Type).GetMethod("GetType", [typeof(string)]));

        //lua.RegisterStaticClass(typeof(FNVHash));

        lua.DoString($"""
                      package.path = package.path ..
                          ";{Path.Combine(Directory.GetCurrentDirectory(), "scripts", "?.lua").Replace("\\", @"\\")}"

                      """, "package.path configuration");

        var assembly = Assembly.GetExecutingAssembly();
        lua.DoEmbeddedScript("DBPF_Compiler.scripts.initLua.lua", assembly);

        lua["DBPFC_VERSION"] = assembly.GetName().Version;

        lua.DoEmbeddedScript("DBPF_Compiler.scripts.Prop.lua", assembly);
        lua.RegisterFunction("Prop.toPropList", typeof(LuaFunctions).GetMethod("TableToPropertyList"));
        
        // lua.DoEmbeddedScript("DBPF_Compiler.scripts.classes.DBPFCObject.lua", assembly);
        // lua.DoEmbeddedScript("DBPF_Compiler.scripts.classes.Key.lua", assembly);
        // lua.DoEmbeddedScript("DBPF_Compiler.scripts.classes.Vector2.lua", assembly);
        // lua.DoEmbeddedScript("DBPF_Compiler.scripts.classes.Property.lua", assembly);
        // lua.DoEmbeddedScript("DBPF_Compiler.scripts.classes.PropertyList.lua", assembly);

        //foreach (var resource in assembly.GetManifestResourceNames())
        //    if (resource.StartsWith("DBPF_Compiler.scripts.classes"))
        //        lua.DoEmbeddedScript(resource, assembly);
        
        return lua;
    }
}
