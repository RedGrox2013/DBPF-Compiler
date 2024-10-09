using DBPF_Compiler.ArgScript;
//using DBPF_Compiler.DBPF;
//using DBPF_Compiler.FNV;
//using DBPF_Compiler.Types;
using DBPF_Compiler.FileTypes;

namespace DBPF_Compiler.Commands
{
    internal class TestCommand : ConsoleCommand
    {
        public override void ParseLine(Line line)
        {
            var test = Lexer.LineToArgs(line[1]);
            for (int i = 0; i < test.ArgumentCount; i++)
                WriteLine($"{i + 1}: {test[i]}");
        }


        /*public override void ParseLine(Line line)
        {
            if (line.ArgumentCount < 2)
            {
                PrintError("Missing <filePath> argument.");
                return;
            }

            using FileStream file = File.OpenRead(line[1]);
            using DatabasePackedFile dbpf = new(file);
            var keys = dbpf.ReadDBPFInfo();

            if (!line.HasFlag("-no-header"))
                WriteLine($@"Database packed file header:
Version:       {dbpf.MajorVersion}.{dbpf.MinorVersion}
Index count:   {dbpf.IndexCount}
Index size:    {dbpf.IndexSize} bytes
Index offset:  {dbpf.IndexOffset} bytes

Resources:");

            if (!line.HasFlag("b"))
            {
                WriteLine(AssetsPath.GetFileName(file.Name));
                PrintTree(dbpf, keys);
                return;
            }

            foreach (var key in keys)
                WriteLine(NameRegistryManager.Instance.GetStringResourceKey(key));
        }

        private void PrintTree(DatabasePackedFile dbpf, IEnumerable<ResourceKey> keys, int level = 1)
        {
            Dictionary<string, List<ResourceKey>> groups = [];
            foreach (var key in keys)
            {
                string groupID = NameRegistryManager.Instance.GetName(key.GroupID, "file");
                if (!groups.TryGetValue(groupID, out var group))
                {
                    group = [];
                    groups.Add(groupID, group);
                }
                group.Add(key);
            }

            for (int i = 0; i < groups.Count; i++)
            {
                for (int k = 1; k < level; k++)
                    Write(" | ");
                if (level > 1)
                    Write(" | ");

                var group = groups.ElementAt(i);
                if (i < groups.Count - 1)
                    Write(" ├ ");
                else
                    Write(" └ ");
                WriteLine(group.Key);

                for (int j = 0; j < group.Value.Count; j++)
                {
                    for (int k = 0; k < level; k++)
                    {
                        Write(" | ");
                    }
                    if (level > 1)
                        Write(" | ");

                    if (j < group.Value.Count - 1)
                        Write(" ├ ");
                    else
                        Write(" └ ");
                    
                    string name = NameRegistryManager.Instance.GetName(group.Value[j].InstanceID, "file");
                    uint typeID = group.Value[j].TypeID;
                    if (typeID != 0)
                        name += "." + NameRegistryManager.Instance.GetName(typeID, "file");

                    WriteLine(name);

                    if (typeID == (uint)TypeIDs.package)
                    {
                        using MemoryStream stream = new();
                        dbpf.CopyResourceTo(stream, group.Value[j]);
                        using DatabasePackedFile package = new(stream);
                        PrintTree(package, package.ReadDBPFInfo(), level + 1);
                    }
                }
            }
        }*/
    }
}
