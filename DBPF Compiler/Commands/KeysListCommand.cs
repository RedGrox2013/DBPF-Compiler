using DBPF_Compiler.ArgScript;
using DBPF_Compiler.DBPF;
using DBPF_Compiler.FNV;

namespace DBPF_Compiler.Commands
{
    internal class KeysListCommand : ConsoleCommand
    {
        public override void ParseLine(Line line)
        {
            if (line.ArgumentCount < 2)
            {
                PrintError("Missing <input> argument.");
                return;
            }

            using FileStream file = File.OpenRead(line[1]);
            using DatabasePackedFile dbpf = new(file);
            var keys = dbpf.ReadDBPFInfo();

            if (!line.HasFlag("-no-header"))
                WriteLine($@"Database packed file header:
Major version: {dbpf.MajorVersion}
Index count:   {dbpf.IndexCount}
Index size:    {dbpf.IndexSize} bytes
Index offset:  {dbpf.IndexOffset} bytes

Resources:");

            if (line.HasFlag("b"))
            {
                foreach (var key in keys)
                    WriteLine(NameRegistryManager.Instance.GetStringResourceKey(key));

                return;
            }

            Dictionary<string, List<string>> groups = [];
            foreach (var k in keys)
            {
                var key = NameRegistryManager.Instance.GetStringResourceKey(k);
                string groupID = key.GroupID ?? "animations~";
                if (!groups.TryGetValue(groupID, out var group))
                {
                    group = [];
                    groups.Add(groupID, group);
                }
                string name = key.InstanceID;
                if (key.TypeID != null)
                    name += "." + key.TypeID;
                group.Add(name);
            }

            foreach (var group in groups)
            {
                WriteLine(group.Key);
                for (int i = 0; i < group.Value.Count - 1; i++)
                    WriteLine(" ├ " + group.Value[i]);
                WriteLine(" └ " + group.Value[^1]);
            }
        }

        public override string? GetDescription(DescriptionMode mode = DescriptionMode.Basic)
        {
            if (mode == DescriptionMode.Basic)
                return "get a list of resources in DBPF.";
            if (mode == DescriptionMode.Complete)
                return @"get a list of resources in DBPF.
Usage:  keys <filePath> [--no-header] [-b]
    <filePath>  path to DBPF
    --no-header use to not display file header information
    -b          use to display a list of resources in
                groupID!instanceID.typeID format
                without displaying them in a tree view";

            return base.GetDescription(mode);
        }
    }
}
