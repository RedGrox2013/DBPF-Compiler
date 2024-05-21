using DBPF_Compiler.FNV;

namespace DBPF_Compiler.Types
{
    public struct StringLocalizedString
    {
        public string TableID { get; set; }
        public string InstanceID { get; set; }
        private string? _placeholderText;
        public string? PlaceholderText
        {
            readonly get => _placeholderText;
            set
            {
                if (value != null && value.Length > PLACEHOLDER_SIZE / 2)
                    throw new ArgumentException("Placeholder text exceeds the permissible length (" + PLACEHOLDER_SIZE / 2 + ").");

                _placeholderText = value;
            }
        }

        public const int PLACEHOLDER_SIZE = LocalizedString.PLACEHOLDER_SIZE;

        public StringLocalizedString(LocalizedString text)
            : this(FNVHash.ToString(text.TableID), FNVHash.ToString(text.InstanceID), text.PlaceholderText) { }
        public StringLocalizedString(string tableID, string instanceID, string? placeholderText = null)
        {
            TableID = tableID;
            InstanceID = instanceID;
            PlaceholderText = placeholderText;
        }

        public readonly override string ToString()
            => $"({TableID}!{InstanceID}) \"{PlaceholderText}\"";
    }
}
