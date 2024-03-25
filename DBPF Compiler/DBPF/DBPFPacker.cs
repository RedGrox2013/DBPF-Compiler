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

        public void Pack(DatabasePackedFile output)
        {
            foreach (var dir in Directory.GetDirectories())
            {
                foreach (var file in dir.GetFiles())
                {
                    string fileName = file.Name.Split('.')[0];
                    ResourceKey key = new(ParseHashOrCompute(fileName),
                        ParseHashOrCompute(file.Extension.Remove(0, 1)),
                        ParseHashOrCompute(dir.Name));

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
                var sr = new StringResourceKey(resource);
                var path = Directory.FullName + "\\" + (string.IsNullOrWhiteSpace(sr.GroupID) ?
                    "0x0" : sr.GroupID);
                if (!System.IO.Directory.Exists(path))
                    System.IO.Directory.CreateDirectory(path);
                using FileStream file = File.Create(path + "\\" + sr.InstanceID + "." + sr.TypeID ?? "0x0");
                dbpf.CopyResourceTo(file, resource);
            }

        }

        private static uint ParseHashOrCompute(string input)
        {
            if (FNVHash.TryParse(input, out uint hash))
                return hash;
            return FNVHash.Compute(input);
        }
    }
}
