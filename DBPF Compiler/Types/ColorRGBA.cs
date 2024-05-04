namespace DBPF_Compiler.Types
{
    public struct ColorRGBA(Vector4 color)
    {
        private Vector4 _color = color;

        public ColorRGBA(float r, float g, float b, float a = 1) : this(new Vector4(r, g, b, a)) { }
        public ColorRGBA(ColorRGB color) : this(color.R, color.G, color.B) { }

        public float R { readonly get => _color.X; set => _color.X = value; }
        public float G { readonly get => _color.Y; set => _color.Y = value; }
        public float B { readonly get => _color.Z; set => _color.Z = value; }
        public float A { readonly get => _color.A; set => _color.A = value; }

        public readonly override string ToString()
            => $"({R}, {G}, {B}, {A})";
    }
}
