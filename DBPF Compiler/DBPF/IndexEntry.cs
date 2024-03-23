namespace DBPF_Compiler.DBPF
{
    public class IndexEntry
    {
        public uint? TypeID { get; set; } = null;
        public uint? GroupID { get; set; } = null;
        public uint? UnknownID { get; set; } = null;
        public uint InstanceID { get; set; }
        public uint Offset { get; set; }
        public uint CompressedSize { get; set; }
        public uint UncompressedSize { get; set; }
        public bool IsCompressed { get; set; } = false;
        public bool IsSaved { get; set; } = true;

        public int EntrySize
        {
            get
            {
                int size = sizeof(uint) * 4 + sizeof(ushort) * 2;
                if (TypeID != null)
                    size += sizeof(uint);
                if (GroupID != null)
                    size += sizeof(uint);
                if (UnknownID != null)
                    size += sizeof(uint);

                return size;
            }
        }
    }
}
