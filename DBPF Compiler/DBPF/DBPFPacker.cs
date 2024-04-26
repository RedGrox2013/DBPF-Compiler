using DBPF_Compiler.FNV;
using DBPF_Compiler.Types;

namespace DBPF_Compiler.DBPF
{
    public class DBPFPacker
    {
        public DirectoryInfo Directory { get; set; }

        public DBPFPacker(DirectoryInfo directory)
            => Directory = directory;
        public DBPFPacker(string path)
            => Directory = new DirectoryInfo(path);

        private readonly NameRegistryManager _regManager = NameRegistryManager.Instance;

        public void Pack(DatabasePackedFile output)
        {
            foreach (var dir in Directory.GetDirectories())
            {
                foreach (var file in dir.GetFiles())
                {
                    string fileName = file.Name.Split('.')[0];
                    ResourceKey key = new(
                        _regManager.GetHash(fileName, "file"),
                        _regManager.GetHash(file.Extension.Remove(0, 1), "type"),
                        _regManager.GetHash(dir.Name, "file")
                        );

                    using FileStream f = file.OpenRead();
                    output.CopyFromStream(f, key);
                }
            }
            output.WriteIndex();
            output.WriteHeader();
        }

        public void Unpack(DatabasePackedFile dbpf)
        {
            foreach (var resource in dbpf.ReadDBPFInfo())
            {
                var key = new StringResourceKey(
                    _regManager.GetName(resource.InstanceID, "file"),
                    _regManager.GetName(resource.TypeID, "type"),
                    _regManager.GetName(resource.GroupID, "file")
                    );
                var path = Directory.FullName + "\\" + (string.IsNullOrWhiteSpace(key.GroupID) ?
                    "animations~" : key.GroupID);
                if (!System.IO.Directory.Exists(path))
                    System.IO.Directory.CreateDirectory(path);
                using FileStream file = File.Create(path + "\\" + key.InstanceID + "." + key.TypeID ?? "0x0");
                dbpf.CopyResourceTo(file, resource);
            }

        }
    }
}
