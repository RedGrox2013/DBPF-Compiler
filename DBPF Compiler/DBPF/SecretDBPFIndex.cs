using System.Text;

namespace DBPF_Compiler.DBPF
{
    internal struct SecretDBPFIndex(string groupName)
    {
        public readonly int GroupNameLength => Encoding.UTF8.GetByteCount(GroupName);
        public string GroupName { get; set; } = groupName;
        public readonly int IndexCount => Entries.Count;

        public readonly List<SecretIndexEntry> Entries = [];

        public readonly int SizeWithoutEntries => sizeof(int) + GroupNameLength;

        public SecretDBPFIndex() : this("secret") { }
    }
}
