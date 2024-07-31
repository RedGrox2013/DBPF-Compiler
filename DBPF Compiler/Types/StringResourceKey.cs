using DBPF_Compiler.DBPF;
using DBPF_Compiler.FNV;

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
            int groupIndex = key.IndexOf('!');
            string? group = groupIndex == -1 ? null : key[..groupIndex];

            int typeIndex = key.LastIndexOf('.');
            if (typeIndex <= groupIndex)
                return new StringResourceKey(key[(groupIndex + 1)..], groupID: group);

            return new StringResourceKey(key[(groupIndex + 1)..typeIndex], key[(typeIndex + 1)..], group);
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
