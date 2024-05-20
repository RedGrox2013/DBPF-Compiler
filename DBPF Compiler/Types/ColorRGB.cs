namespace DBPF_Compiler.Types
{
    public struct ColorRGB(Vector3 color)
    {
        private Vector3 _color = color;

        public ColorRGB(float r, float g, float b) : this(new Vector3(r, g, b)) { }
        public ColorRGB(Vector4 color) : this(new Vector3(color)) { }

        public float R { readonly get => _color.X; set => _color.X = value; }
        public float G { readonly get => _color.Y; set => _color.Y = value; }
        public float B { readonly get => _color.Z; set => _color.Z = value; }

        public readonly override string ToString() => $"({R}, {G}, {B})";

        public static implicit operator Vector3(ColorRGB color) => color._color;
        public static implicit operator Vector4(ColorRGB color) => (Vector4)color._color;
    }
}
