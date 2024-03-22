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
    }
}
