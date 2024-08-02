namespace DBPF_Compiler.ArgScript
{
    public abstract class ArgScriptCommand : IParser
    {
        public FormatParser FormatParser { get; protected set; } = new();
        public object? Data { get; protected set; }

        public abstract string? GetDescription(DescriptionMode mode = DescriptionMode.Basic);
        public abstract void ParseLine(Line line);

        public void SetData(FormatParser parser, object? data)
        {
            FormatParser = parser;
            Data = data;
        }
    }
}
