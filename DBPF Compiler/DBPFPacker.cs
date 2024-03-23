using DBPF_Compiler.DBPF;

namespace DBPF_Compiler
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
                    var instanceID = ParseHashOrCompute(fileName);
                    var typeID = ParseHashOrCompute(file.Extension.Remove(0, 1));
                    var groupID = ParseHashOrCompute(dir.Name);

                    using FileStream f = file.OpenRead();
                    output.CopyFromStream(f, instanceID, typeID, groupID);
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
