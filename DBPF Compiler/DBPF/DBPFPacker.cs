using DBPF_Compiler.FileTypes.Prop;
using DBPF_Compiler.FNV;
using DBPF_Compiler.Types;

namespace DBPF_Compiler.DBPF
{
    public class DBPFPacker
    {
        public delegate void DBPFPackerEventHandler(object? sender, StringResourceKey key);

        public DirectoryInfo UnpackedDataDirectory { get; set; }

        private DBPFPackerEventHandler? _packHandler = null;
        public event DBPFPackerEventHandler? PackHandler { add => _packHandler += value; remove => _packHandler -= value; }
        private DBPFPackerEventHandler? _unpackHandler = null;
        public event DBPFPackerEventHandler? UnpackHandler { add => _unpackHandler += value; remove => _unpackHandler -= value; }

        public DBPFPacker(DirectoryInfo unpackedDataDirectory)
            => UnpackedDataDirectory = unpackedDataDirectory;
        public DBPFPacker(string unpackedDataPath)
            => UnpackedDataDirectory = new DirectoryInfo(unpackedDataPath);

        private readonly NameRegistryManager _regManager = NameRegistryManager.Instance;

        public void Pack(DatabasePackedFile output, string? secretFolder = null)
        {
            foreach (var group in UnpackedDataDirectory.GetDirectories())
            {
                if (group.Name == secretFolder)
                {
                    output.SecretGroupName = secretFolder;
                    foreach (var secret in group.GetFiles())
                    {
                        StringResourceKey key = new(secret.Name, secret.Extension.Remove(0, 1));
                        _packHandler?.Invoke(this, key);
                        using FileStream f = secret.OpenRead();
                        output.CopySecretDataFromStream(f, key);
                    }

                    continue;
                }

                foreach (var d in group.GetDirectories())
                {
                    if (!d.Name.EndsWith(".package.unpacked"))
                        continue;

                    DBPFPacker packer = new(d)
                    {
                        _packHandler = _packHandler
                    };
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
                    string[] splitFileName = file.Name.Split('.');
                    string fileName = splitFileName[0];
                    string extension = splitFileName.Length > 1 ? splitFileName[1] : string.Empty;
                    _packHandler?.Invoke(this, new(fileName, extension, group.Name));
                    ResourceKey key = _regManager.GetResourceKey(fileName, extension, group.Name);

                    if (file.Name.EndsWith(".prop.json", StringComparison.InvariantCultureIgnoreCase) ||
                        file.Name.EndsWith(".soundProp.json", StringComparison.InvariantCultureIgnoreCase))
                    {
                        PropertyList prop = PropertyListJsonSerializer.Deserialize(File.ReadAllText(file.FullName));
                        output.WriteSporeFile(prop, key);
                    }
                    else
                    {
                        using FileStream f = file.OpenRead();
                        output.CopyFromStream(f, key);
                    }
                }
            }

            output.WriteIndex();
            output.WriteSecretIndex();
            output.WriteHeader();
        }

        public void Unpack(DatabasePackedFile input, EncodeFlags flags = EncodeFlags.All)
        {
            foreach (var resource in input.ReadDBPFInfo())
            {
                var key = _regManager.GetStringResourceKey(resource);
                var path = UnpackedDataDirectory.FullName + "\\" + (key.GroupID ?? "animations~");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                path += "\\" + key.InstanceID + "." + key.TypeID;
                _unpackHandler?.Invoke(this, key);

                if (resource.TypeID == (uint)TypeIDs.package && flags.HasFlag(EncodeFlags.Package))
                {
                    DBPFPacker unpacker = new(path + ".unpacked");
                    using MemoryStream stream = new();
                    input.CopyResourceTo(stream, resource);
                    DatabasePackedFile package = new(stream);
                    unpacker.Unpack(package);
                }
                else if ((resource.TypeID == (uint)TypeIDs.prop || resource.TypeID == (uint)TypeIDs.soundProp) &&
                    flags.HasFlag(EncodeFlags.PropertyList))
                {
                    byte[]? buffer = input.ReadResource(resource);
                    if (buffer == null)
                        continue;

                    using MemoryStream stream = new(buffer);
                    using StreamWriter writer = new(path + ".json");
                    writer.WriteLine(PropertyListJsonSerializer.DecodePropertyListToJson(stream));
                }
                else
                {
                    using FileStream file = File.Create(path);
                    input.CopyResourceTo(file, resource);
                }
            }
        }

        public bool UnpackSecret(DatabasePackedFile input)
        {
            var secrets = input.ReadSecretIndex();
            if (secrets == null || secrets.Length == 0)
                return false;

            string outputPath = UnpackedDataDirectory.FullName + "\\" + secrets[0].GroupID + "\\";
            if (!Directory.Exists(outputPath))
                Directory.CreateDirectory(outputPath);

            foreach (var secret in secrets)
            {
                string path = outputPath + secret.InstanceID;
                if (!string.IsNullOrEmpty(secret.TypeID))
                    path += "." + secret.TypeID;

                using FileStream file = File.Create(path);
                input.CopySecretDataTo(file, secret);
            }

            return true;
        }
    }

    //public class DBPFPackerEventArgs
    //{
    //    public StringResourceKey CurrentKey { get; set; }
    //    public int KeysCount { get; set; }
    //}

    [Flags]
    public enum EncodeFlags
    {
        All = -1,
        None = 0,
        Package = 0b1,
        PropertyList = 0b10
    }
}
