namespace DBPF_Compiler.FNV
{
    /// <summary>
    /// Реестр имён
    /// </summary>
    /// <param name="name">Имя реестра имён</param>
    public class NameRegistry(string name)
    {
        /// <summary>
        /// Имя реестра имён
        /// </summary>
        public string Name { get; protected set; } = name;

        private readonly Dictionary<string, uint> _reg = [];

        public void Add(string name, uint hash) => _reg.TryAdd(name, hash);

        public uint GetHash(string name)
        {
            if (_reg.TryGetValue(name, out uint hash))
                return hash;
            return FNVHash.Compute(name);
        }

        public bool GetName(uint hash, out string name)
        {
            foreach (var i in _reg)
                if (i.Value == hash)
                {
                    name = i.Key;
                    return true;
                }

            name = string.Empty;
            return false;
        }

        public string GetNameOrConvertToString(uint hash)
        {
            if (GetName(hash, out string name))
                return name;

            return "0x" + Convert.ToString(hash, 16);
        }
    }
}
