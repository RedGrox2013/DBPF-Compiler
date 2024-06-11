namespace DBPF_Compiler.Types
{
    public struct LocalizedString
    {
        public uint TableID { get; set; }
        public uint InstanceID { get; set; }
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

        public const int PLACEHOLDER_SIZE = 512;

        public LocalizedString() : this(0, 0) { }
        public LocalizedString(string placeholderText) : this(0, 0, placeholderText) { }
        public LocalizedString(uint tableID, uint instanceID, string? placeholderText = null)
        {
            TableID = tableID;
            InstanceID = instanceID;
            PlaceholderText = placeholderText;
        }

        public readonly override string ToString()
            => new StringLocalizedString(this).ToString();
    }
}
