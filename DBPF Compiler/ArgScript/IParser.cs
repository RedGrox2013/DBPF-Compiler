namespace DBPF_Compiler.ArgScript
{
    public interface IParser
    {
        public FormatParser? FormatParser { get; set; }

        void ParseLine(Line line);
        string? GetDescription(DescriptionMode mode = DescriptionMode.Basic);
    }

    public enum DescriptionMode
    {
        Basic,
        Complete,
        HTML
    }
}
