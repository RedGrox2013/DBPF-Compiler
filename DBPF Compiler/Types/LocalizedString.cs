﻿namespace DBPF_Compiler.Types
{
    public struct LocalizedString(uint tableID, uint instanceID, string? placeholderText = null)
    {
        public uint TableID { get; set; } = tableID;
        public uint InstanceID { get; set; } = instanceID;
        public string? PlaceholderText { get; set; } = placeholderText;

        public const int PLACEHOLDER_SIZE = 512;

        public LocalizedString() : this(0xFFFFFFFF, 0xFFFFFFFF, null) { }
        public LocalizedString(string placeholderText) : this(0xFFFFFFFF, 0xFFFFFFFF, placeholderText) { }

        public readonly override string ToString()
            => new StringLocalizedString(this).ToString();
    }
}
