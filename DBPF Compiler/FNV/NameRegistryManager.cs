using DBPF_Compiler.Types;

namespace DBPF_Compiler.FNV
{
    public class NameRegistryManager
    {
        private static NameRegistryManager? _instance;
        public static NameRegistryManager Instance => _instance ??= new NameRegistryManager();

        private readonly List<NameRegistry> _regs;

        private NameRegistryManager()
        {
            _regs = [];
        }

        /// <summary>
        /// Доступные имена реестров имён
        /// </summary>
        public string[] RegistriesNames
            => (from r in _regs select r.Name).ToArray();

        public NameRegistry? GetRegistry(string? regName)
        {
            foreach (var reg in _regs)
                if (reg.Name.Equals(regName, StringComparison.InvariantCultureIgnoreCase))
                    return reg;

            return null;
        }

        public void AddRegistry(NameRegistry reg)
            => _regs.Add(reg);

        public async Task AddRegistryFromFileAsync(string filePath)
        {
            NameRegistry reg = new(Path.GetFileNameWithoutExtension(filePath).Replace("reg_", null));
            using TextReader reader = File.OpenText(filePath);
            string text = await reader.ReadToEndAsync();

            foreach (string line in text.Split('\n'))
            {
                string l = line.Trim();
                if (string.IsNullOrWhiteSpace(l) || l.StartsWith('#'))
                    continue;

                string[] pair = l.Split('\t');
                if (pair.Length == 1 || !FNVHash.TryParse(pair[1], out uint hash))
                    hash = FNVHash.Compute(pair[0]);
                reg.Add(pair[0], hash);
            }

            AddRegistry(reg);
        }

        public string GetName(uint hash, string? regName = "all", params string[]? ignoreRegNames)
        {
            if (regName != "all")
            {
                var reg = GetRegistry(regName);
                if (reg != null && reg.GetName(hash, out string name))
                    return name;
            }

            foreach (var reg in _regs)
            {
                bool ignore = false;
                if (ignoreRegNames != null)
                    foreach (var ignoreName in ignoreRegNames)
                        if (reg.Name == ignoreName)
                        {
                            ignore = true;
                            break;
                        }

                if (!ignore && reg.Name != regName && reg.GetName(hash, out string name))
                    return name;
            }

            return FNVHash.ToString(hash);
        }

        public uint GetHash(string name, string? regName = "all")
        {
            if (regName != "all")
            {
                var reg = GetRegistry(regName);
                if (reg != null && reg.GetHash(name, out uint hash))
                    return hash;
            }

            foreach (var reg in _regs)
                if (reg.Name != regName && reg.GetHash(name, out uint hash))
                    return hash;

            return FNVHash.Compute(name);
        }

        public StringResourceKey GetStringResourceKey(ResourceKey key) => new(
            GetName(key.InstanceID, "file"),
            key.TypeID != 0 ? GetName(key.TypeID, "type") : null,
            key.GroupID != 0 ? GetName(key.GroupID, "file") : null);

        public ResourceKey GetResourceKey(StringResourceKey key)
            => GetResourceKey(key.InstanceID, key.TypeID, key.GroupID);
        public ResourceKey GetResourceKey(string instance, string? type = null, string? group = null)
        {
            if (!FNVHash.TryParse(instance, out var hash))
                hash = instance.EndsWith('~') ? GetHash(instance, "file") : FNVHash.Compute(instance);
            ResourceKey key = new(hash);

            if (FNVHash.TryParse(group, out hash))
                key.GroupID = hash;
            else if (!string.IsNullOrWhiteSpace(group))
                key.GroupID = group.EndsWith('~') ? GetHash(group, "file") : FNVHash.Compute(group);

            if (!string.IsNullOrWhiteSpace(type))
                key.TypeID = GetHash(type, "type");

            return key;
        }
    }
}
