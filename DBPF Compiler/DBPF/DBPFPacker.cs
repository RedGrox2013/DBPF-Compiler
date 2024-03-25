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

        private static uint ParseHashOrCompute(string input)
        {
            if (FNVHash.TryParse(input, out uint hash))
                return hash;
            return FNVHash.Compute(input);
        }
    }
}
