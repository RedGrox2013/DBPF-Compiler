namespace DBPF_Compiler.DBPF
{
    internal struct IndexEntry
    {
        public uint TypeId { get; set; }
        public uint GroupId { get; set; }
        public readonly uint? UnknownID = null;
        public uint InstanceId { get; set; }
        public uint Offset { get; set; }
        public uint CompressedSize { get; set; }
        public uint UncompressedSize { get; set; }
        public bool IsCompressed { get; set; } = false;
        public bool IsSaved { get; private set; } = true;

        public const int EntrySize = 4 + 4 + 4 + 4 + 2 + 2 + sizeof(uint) + sizeof(uint);

        public IndexEntry() { }
    }
}
