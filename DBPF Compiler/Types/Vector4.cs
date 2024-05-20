namespace DBPF_Compiler.Types
{
    public struct Vector4(float x, float y, float z, float a)
    {
        public float X { get; set; } = x;
        public float Y { get; set; } = y;
        public float Z { get; set;} = z;
        public float A { get; set; } = a;

        public readonly override string ToString() => $"({X}, {Y}, {Z}, {A})";

        public static explicit operator Vector2(Vector4 v) => new(v);
        public static explicit operator Vector3(Vector4 v) => new(v);
    }
}
