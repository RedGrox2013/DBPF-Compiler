namespace DBPF_Compiler.FileTypes.Prop
{
    public struct Property
    {
        public string Name { get; set; }
        public PropertyType PropertyType { get; set; }
        public object Value { get; set; }
    }

    public enum PropertyType
    {
        Boolean = 0x10000,
        Int8 = 0x50000,
        Int16 = 0x70000,
        Int32 = 0x90000,
    }
}
