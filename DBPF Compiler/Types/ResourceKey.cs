using DBPF_Compiler.DBPF;

namespace DBPF_Compiler.Types
{
    public struct ResourceKey(uint instanceID, uint typeID = 0, uint groupID = 0)
    {
        public uint InstanceID { get; set; } = instanceID;
        public uint TypeID { get; set; } = typeID;
        public uint GroupID { get; set; } = groupID;

        public static bool operator ==(ResourceKey left, ResourceKey right)
            => left.Equals(right);
        public static bool operator !=(ResourceKey left, ResourceKey right)
            => !left.Equals(right);

        public override readonly bool Equals(object? obj)
        {
            if (obj is IndexEntry entry)
                return entry.InstanceID == InstanceID &&
                    entry.TypeID == TypeID &&
                    entry.GroupID == GroupID;
            if (obj is ResourceKey key)
                return key.InstanceID == InstanceID &&
                    key.TypeID == TypeID &&
                    key.GroupID == GroupID;

            return false;
        }

        public override readonly int GetHashCode()
            => base.GetHashCode();

        public override readonly string ToString()
            => new StringResourceKey(this).ToString();
    }
}
