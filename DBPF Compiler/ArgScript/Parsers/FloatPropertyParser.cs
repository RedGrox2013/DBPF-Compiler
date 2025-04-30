namespace DBPF_Compiler.ArgScript.Parsers
{
    class FloatPropertyParser : PropertyParser
    {
        public FloatPropertyParser() =>
            Description = "Represents a floating-point real number.";

        public override void ParseLine(Line line) =>
            PropList.Add(ParseProperty(line, arg => FormatParser.ParseFloat(arg)));
    }
}
