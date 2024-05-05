using System.Text;

namespace DBPF_Compiler.DBPF
{
    internal class SecretDBPFIndex(string groupName)
    {
        public int GroupNameLength => Encoding.Unicode.GetByteCount(GroupName);
        public string GroupName { get; set; } = groupName;
        public int IndexCount => Entries.Count;

        public readonly List<SecretIndexEntry> Entries = [];

        public int SizeWithoutEntries => sizeof(int) + GroupNameLength;

        public SecretDBPFIndex() : this("secret") { }
    }
}
