using DBPF_Compiler.Types;
using System.Text;

namespace DBPF_Compiler.DBPF
{
    internal class SecretIndexEntry
    {
        public int NameLength => Encoding.UTF8.GetByteCount(Key.InstanceID);
        public StringResourceKey Key { get; set; }
        public int TypeLength => Key.TypeID != null ? Encoding.UTF8.GetByteCount(Key.TypeID) : 0;
        public uint Offset { get; set; }
        public uint Size { get; set; }

        public int EntrySize => NameLength + TypeLength + sizeof(uint) * 2 + sizeof(int) * 2;
    }
}
