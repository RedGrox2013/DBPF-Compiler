namespace DBPF_Compiler.Types
{
    public struct Vector3(float x, float y, float z)
    {
        public float X { get; set; } = x;
        public float Y { get; set; } = y;
        public float Z { get; set; } = z;

        public Vector3(Vector4 vector) : this(vector.X, vector.Y, vector.Z) { }

        public readonly override string ToString() => $"({X}, {Y}, {Z})";

        public static explicit operator Vector2(Vector3 v) => new(v.X, v.Y);
        public static implicit operator Vector4(Vector3 v) => new(v.X, v.Y, v.Z, 0);
    }
}
