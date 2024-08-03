using DBPF_Compiler.DBPF;
using System.Text.Json.Serialization;

namespace DBPF_Compiler
{
    public class ModProject
    {
        [JsonIgnore]
        public DBPFPacker? Packer { get; set; }
        [JsonIgnore]
        public string? Path => Packer?.UnpackedDataDirectory.FullName;

        public ModProject() { }
    }
}
