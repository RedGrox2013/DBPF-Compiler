using DBPF_Compiler.Types;
using System.Text;

namespace DBPF_Compiler.DBPF
{
    internal class SecretIndexEntry
    {
        public StringResourceKey Key { get; set; }
        public uint Offset { get; set; }
        public uint Size { get; set; }
    }
}
