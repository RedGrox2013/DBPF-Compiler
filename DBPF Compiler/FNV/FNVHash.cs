namespace DBPF_Compiler.FNV
{
    public static class FNVHash
    {
        public const uint FNV_32_PRIME = 0x01000193;
        public const uint FNV_BASIS = 0x811c9dc5;

        public static uint Compute(string input)
        {
            uint hash = FNV_BASIS;

            foreach (var c in input.ToLowerInvariant())
            {
                hash *= FNV_32_PRIME;
                hash ^= c;
            }

            return hash;
        }

        public static uint Parse(string input)
        {
            if (TryParse(input, out uint hash))
                return hash;

            throw new FormatException();
        }

        public static bool TryParse(string? input, out uint hash)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                hash = 0;
                return false;
            }

            if (!input.StartsWith("0x") && !input.StartsWith('#'))
                return uint.TryParse(input, out hash);

            try
            {
                hash = Convert.ToUInt32(input.TrimStart('#'), 16);
            }
            catch
            {
                hash = 0;
                return false;
            }

            return true;
        }

        public static string ToString(uint hash) => $"0x{hash:X08}";
    }
}
