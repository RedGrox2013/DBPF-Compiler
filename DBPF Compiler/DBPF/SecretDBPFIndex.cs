using System.Text;

namespace DBPF_Compiler.DBPF
{
    internal class SecretDBPFIndex(string groupName)
    {
        public string GroupName { get; set; } = groupName;
        public int IndexCount => Entries.Count;

        public readonly List<SecretIndexEntry> Entries = [];

        public SecretDBPFIndex() : this("secret") { }
    }
}
