using DBPF_Compiler.DBPF;
using DBPF_Compiler.FNV;
using System.Text;

namespace DBPF_Compiler.Types
{
    public struct StringResourceKey(string instanceID, string? typeID = null, string? groupID = null)
    {
        public string InstanceID { get; set; } = instanceID;
        public string? TypeID { get; set; } = typeID;
        public string? GroupID { get; set; } = groupID;

        public StringResourceKey(ResourceKey key) : this(
            FNVHash.ToString(key.InstanceID),
            key.TypeID != 0 ? FNVHash.ToString(key.TypeID) : null,
            key.GroupID != 0 ? FNVHash.ToString(key.GroupID) : null
            ) { }

        public static StringResourceKey Parse(string key)
        {
            string[] parts = key.Split('!');
            string? group;
            if (parts.Length == 1)
                group = null;
            else
            {
                group = parts[0];
                key = string.Empty;
                for (int i = 1; i < parts.Length; i++)
                    key += parts[i];
            }

            int index = key.LastIndexOf('.');
            if (index == -1)
                return new StringResourceKey(key, groupID: group);
            return new StringResourceKey(key[..index], key[(index + 1)..], group);
        }

        public static bool operator==(StringResourceKey left, StringResourceKey right)
            => left.Equals(right);
        public static bool operator !=(StringResourceKey left, StringResourceKey right)
            => !left.Equals(right);

        public override readonly bool Equals(object? obj)
        {
            if (obj is SecretIndexEntry entry)
                return entry.Key.InstanceID == InstanceID && entry.Key.TypeID == TypeID;
            if (obj is StringResourceKey key)
                return key.InstanceID == InstanceID && key.GroupID == GroupID && key.TypeID == TypeID;

            return false;
        }

        public override readonly int GetHashCode()
            => base.GetHashCode();

        public override readonly string ToString()
        {
            string s = string.IsNullOrWhiteSpace(GroupID) ?
                InstanceID : GroupID + "!" + InstanceID;
            if (!string.IsNullOrWhiteSpace(TypeID))
                s += "." + TypeID;

            return s;
        }
    }
}
