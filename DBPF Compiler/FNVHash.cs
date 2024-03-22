namespace DBPF_Compiler
{
    public static class FNVHash
    {
        private const uint FNV_32_PRIME = 0x01000193;
        private const uint FNV_BASIS = 0x811c9dc5;

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

        public static bool TryParse(string? input, out uint hash)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                hash = 0;
                return false;
            }

            if (!input.StartsWith("0x") && !input.StartsWith('#'))
                return uint.TryParse(input.Replace("0x", null).Replace("#", null), out hash);

            try
            {
                hash = Convert.ToUInt32(input, 16);
            }
            catch
            {
                hash = 0;
                return false;
            }

            return true;
        }
    }
}
