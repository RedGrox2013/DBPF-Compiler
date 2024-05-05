using DBPF_Compiler.FNV;

namespace DBPF_Compiler.Types
{
    public struct StringLocalizedString(string tableID, string instanceID, string? placeholderText = null)
    {
        public string TableID { get; set; } = tableID;
        public string InstanceID { get; set; } = instanceID;
        public string? PlaceholderText { get; set; } = placeholderText;

        public StringLocalizedString(LocalizedString text)
            : this(FNVHash.ToString(text.TableID), FNVHash.ToString(text.InstanceID), text.PlaceholderText) { }

        public readonly override string ToString()
            => $"({TableID}!{InstanceID}) \"{PlaceholderText}\"";
    }
}
