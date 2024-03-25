namespace DBPF_Compiler.Types
{
    public struct StringResourceKey(string instanceID, string? typeID = null, string? groupID = null)
    {
        public string InstanceID { get; set; } = instanceID;
        public string? TypeID { get; set; } = typeID;
        public string? GroupID { get; set; } = groupID;

        public StringResourceKey(ResourceKey key) : this("0x" + Convert.ToString(key.InstanceID, 16),
            key.TypeID != 0 ? "0x" + Convert.ToString(key.TypeID, 16) : null,
            key.GroupID != 0 ? "0x" + Convert.ToString(key.GroupID, 16) : null)
        { }

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
