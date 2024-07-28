namespace DBPF_Compiler.ArgScript
{
    public interface IParser
    {
        FormatParser FormatParser { get; }
        object? Data { get; }

        void ParseLine(Line line);
        string? GetDescription(DescriptionMode mode = DescriptionMode.Basic);
        void SetData(FormatParser parser, object? data);
    }

    public enum DescriptionMode
    {
        Basic,
        Complete,
        HTML
    }
}
