namespace DBPF_Compiler.Types
{
    public struct Transform
    {
        public short Flags { get; set; }
        public short TransformCount { get; set; }
        public Vector3 Offset { get; set; }
        public float Scale { get; set; }
        internal Matrix Rotate { get; set; }
        public readonly Vector3 RotateXYZ
        {
            get => Rotate.ToEulerDegrees();
            set => Rotate.Rotate(value);
        }

        public const int SIZE = 56;

        public Transform()
        {
            Offset = new();
            Scale = 1.0f;
            Rotate = new();
        }
    }
}
