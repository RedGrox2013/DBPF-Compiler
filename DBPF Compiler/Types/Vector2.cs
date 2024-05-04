namespace DBPF_Compiler.Types
{
    public struct Vector2(float x, float y)
    {
        public float X { get; set; } = x;
        public float Y { get; set; } = y;

        public Vector2(Vector4 vector) : this(vector.X, vector.Y) { }

        public readonly override string ToString()
            => $"({X}, {Y})";
    }
}
