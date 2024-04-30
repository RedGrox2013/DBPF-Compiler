using DBPF_Compiler.FNV;
using DBPF_Compiler.Types;

namespace DBPF_Compiler.DBPF
{
    public class DBPFPacker
    {
        public DirectoryInfo UnpackedDataDirectory { get; set; }

        public DBPFPacker(DirectoryInfo unpackedDataDirectory)
            => UnpackedDataDirectory = unpackedDataDirectory;
        public DBPFPacker(string unpackedDataPath)
            => UnpackedDataDirectory = new DirectoryInfo(unpackedDataPath);

        private readonly NameRegistryManager _regManager = NameRegistryManager.Instance;

        public void Pack(DatabasePackedFile output)
        {
            foreach (var group in UnpackedDataDirectory.GetDirectories())
            {
                foreach (var d in group.GetDirectories())
                {
                    if (!d.Name.EndsWith(".package.unpacked"))
                        continue;

                    DBPFPacker packer = new(d);
                    using MemoryStream stream = new();
                    using DatabasePackedFile package = new(stream);
                    packer.Pack(package);

                    string folderName = d.Name.Split('.')[0];
                    ResourceKey key = new(
                        _regManager.GetHash(folderName, "file"),
                        (uint)TypeIDs.package,
                        _regManager.GetHash(group.Name, "file")
                        );
                    stream.Position = 0;
                    output.CopyFromStream(stream, key);
                }

                foreach (var file in group.GetFiles())
                {
                    string fileName = file.Name.Split('.')[0];
                    ResourceKey key = new(
                        _regManager.GetHash(fileName, "file"),
                        _regManager.GetHash(file.Extension.Remove(0, 1), "type"),
                        _regManager.GetHash(group.Name, "file")
                        );

                    using FileStream f = file.OpenRead();
                    output.CopyFromStream(f, key);
                }
            }

            output.WriteIndex();
            output.WriteHeader();
        }

        public void Unpack(DatabasePackedFile dbpf, EncodeFlags flags = EncodeFlags.All)
        {
            foreach (var resource in dbpf.ReadDBPFInfo())
            {
                var key = _regManager.GetStringResourceKey(resource);
                var path = UnpackedDataDirectory.FullName + "\\" + key.GroupID;
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                path += "\\" + key.InstanceID + "." + key.TypeID;

                if (resource.TypeID == (uint)TypeIDs.package && flags.HasFlag(EncodeFlags.Package))
                {
                    DBPFPacker unpacker = new(path + ".unpacked");
                    using MemoryStream stream = new();
                    dbpf.CopyResourceTo(stream, resource);
                    DatabasePackedFile package = new(stream);
                    unpacker.Unpack(package);
                }
                else
                {
                    using FileStream file = File.Create(path);
                    dbpf.CopyResourceTo(file, resource);
                }
            }
        }
    }

    [Flags]
    public enum EncodeFlags
    {
        All = -1,
        None = 0,
        Package = 0b1,
    }
}
