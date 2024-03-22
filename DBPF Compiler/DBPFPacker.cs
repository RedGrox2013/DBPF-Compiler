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
                    if (!FNVHash.TryParse(fileName, out var instanceID))
                        instanceID = FNVHash.Compute(fileName);
                    var extension = file.Extension.Remove(0, 1);
                    if (!FNVHash.TryParse(extension, out var typeID))
                        typeID = FNVHash.Compute(extension);
                    if (!FNVHash.TryParse(dir.Name, out var groupID))
                        groupID = FNVHash.Compute(dir.Name);

                    using FileStream f = file.OpenRead();
                    output.CopyFromStream(f, instanceID, typeID, groupID);
                }
            }
            output.WriteIndex();
            output.WriteHeader();
        }
    }
}
