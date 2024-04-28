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
