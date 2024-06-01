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

        public bool GetHash(string name, out uint hash)
        {
            if (FNVHash.TryParse(name, out hash))
                return true;

            return _reg.TryGetValue(name, out hash);
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

            return FNVHash.ToString(hash);
        }
    }
}
