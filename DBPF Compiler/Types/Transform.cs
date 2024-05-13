namespace DBPF_Compiler.Types
{
    public struct Transform
    {
        internal TransformFlags Flags { get; set; }
        internal short TransformCount { get; set; }
        private Vector3 _offset;
        public Vector3 Offset
        {
            readonly get => _offset;
            set
            {
                _offset = value;
                Flags |= TransformFlags.Offset;
            }
        }
        private float _scale;
        public float Scale
        {
            readonly get => _scale;
            set
            {
                _scale = value;
                Flags |= TransformFlags.Scale;
            }
        }
        private Matrix _rotate;
        internal Matrix Rotate
        {
            readonly get => _rotate;
            set
            {
                _rotate = value;
                Flags |= TransformFlags.Rotate;
            }
        }
        public Vector3 RotateXYZ
        {
            readonly get => _rotate.ToEulerDegrees();
            set
            {
                _rotate.Rotate(value);
                Flags |= TransformFlags.Rotate;
            }
        }

        public const int SIZE = 56;

        public Transform()
        {
            TransformCount = 1;
            _offset = new();
            _scale = 1.0f;
            _rotate = Matrix.CreateIdentity();
        }
    }

    [Flags]
    internal enum TransformFlags : short
    {
        Scale = 1,
        Rotate = 2,
        Offset = 4
    }
}
