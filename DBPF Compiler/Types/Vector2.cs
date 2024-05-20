namespace DBPF_Compiler.Types
{
    public struct Vector2(float x, float y)
    {
        public float X { get; set; } = x;
        public float Y { get; set; } = y;

        public Vector2(Vector4 vector) : this(vector.X, vector.Y) { }

        public readonly override string ToString() => $"({X}, {Y})";

        public static implicit operator Vector3(Vector2 v) => new(v.X, v.Y, 0);
        public static implicit operator Vector4(Vector2 v) => new(v.X, v.Y, 0, 0);
    }
}
