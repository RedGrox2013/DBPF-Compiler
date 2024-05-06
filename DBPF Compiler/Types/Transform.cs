namespace DBPF_Compiler.Types
{
    public struct Transform
    {
        public uint UnknownID { get; set; }
        public Vector3 Offset { get; set; }
        public float Scale { get; set; }
        public uint[] UnknownData { get; set; }

        public const int SIZE = 56;
    }
}
