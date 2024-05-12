namespace DBPF_Compiler.Types
{
    internal struct Matrix
    {
        public const int SIZE = 3;

        private readonly float[,] _matrix;

        public Matrix()
            => _matrix = new float[SIZE, SIZE];
        public Matrix(Matrix other) : this()
        {
            for (int i = 0; i < SIZE; i++)
                for (int j = 0; j < SIZE; j++)
                    _matrix[i, j] = other[i, j];
        }

        public readonly Vector3 ToDegreesRotation() => new(
            (float)(180 / Math.PI * Math.Atan2(_matrix[2, 1], _matrix[2, 2])),
            (float)(180 / Math.PI * -Math.Asin(_matrix[2, 0])),
            (float)(180 / Math.PI * Math.Atan2(_matrix[1, 0], _matrix[0, 0]))
            );

        public readonly void Rotate(Vector3 degreesRotation) => Rotate(
            Math.PI / 180 * degreesRotation.X,
            Math.PI / 180 * degreesRotation.Y,
            Math.PI / 180 * degreesRotation.Z
            );
        public readonly void Rotate(double radiansX, double radiansY, double radiansZ)
        {
            double
                cosx = Math.Cos(radiansX),
                cosy = Math.Cos(radiansY),
                cosz = Math.Cos(radiansZ),
                sinx = Math.Sin(radiansX),
                siny = Math.Sin(radiansY),
                sinz = Math.Sin(radiansZ);

            double
                coscos = cosx * cosz,
                cossin = cosx * sinz,
                sincos = sinx * cosz,
                sinsin = sinx * sinz;

            _matrix[0, 0] = (float)(cosy * cosz);
            _matrix[0, 1] = (float)(siny * sincos - cossin);
            _matrix[0, 2] = (float)(siny * coscos + sinsin);
            _matrix[1, 0] = (float)(cosy * sinz);
            _matrix[1, 1] = (float)(siny * sinsin + coscos);
            _matrix[1, 2] = (float)(siny * cossin - sincos);
            _matrix[2, 0] = (float)-siny;
            _matrix[2, 1] = (float)(cosy * sinx);
            _matrix[2, 2] = (float)(cosy * cosx);
        }

        public readonly float this[int row, int column]
        {
            get => _matrix[row, column];
            set => _matrix[row, column] = value;
        }
    }
}
