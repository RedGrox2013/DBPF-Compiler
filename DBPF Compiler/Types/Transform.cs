namespace DBPF_Compiler.Types
{
    public struct Transform
    {
        public uint UnknownValue { get; set; }
        public Vector3 Offset { get; set; }
        public float Scale { get; set; }
        public uint[] UnknownData { get; set; }

        public const int SIZE = 56;

        public Transform()
        {
            UnknownData = new uint[9];
            Offset = new Vector3();
            Scale = 1.0f;
        }
    }
}
