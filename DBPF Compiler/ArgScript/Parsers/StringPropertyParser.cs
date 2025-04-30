namespace DBPF_Compiler.ArgScript.Parsers
{
    class StringPropertyParser : PropertyParser
    {
        public StringPropertyParser() =>
            Description = "Represents a text value.";

        public override void ParseLine(Line line) =>
            PropList.Add(ParseProperty(line, FormatParser.ParseString));
    }
}
