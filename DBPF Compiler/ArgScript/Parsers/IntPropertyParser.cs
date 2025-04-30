using DBPF_Compiler.FileTypes.Prop;

namespace DBPF_Compiler.ArgScript.Parsers
{
    class IntPropertyParser : PropertyParser
    {
        public IntPropertyParser() =>
            Description = "Represents integer values.";

        public override void ParseLine(Line line)
        {
            var prop = ParseProperty(line);

            switch (prop.PropertyType)
            {
                case PropertyType.int8:
                    prop.Value = (sbyte)FormatParser.ParseInteger(line[2]);
                    break;
                case PropertyType.int16:
                    prop.Value = (short)FormatParser.ParseInteger(line[2]);
                    break;
                case PropertyType.int32:
                    prop.Value = FormatParser.ParseInteger(line[2]);
                    break;
                case PropertyType.int64:
                    prop.Value = (long)FormatParser.ParseInteger(line[2]);
                    break;
                case PropertyType.uint8:
                    prop.Value = (byte)FormatParser.ParseUInteger(line[2]);
                    break;
                case PropertyType.uint16:
                    prop.Value = (ushort)FormatParser.ParseUInteger(line[2]);
                    break;
                case PropertyType.uint32:
                    prop.Value = FormatParser.ParseUInteger(line[2]);
                    break;
                case PropertyType.uint64:
                    prop.Value = (ulong)FormatParser.ParseUInteger(line[2]);
                    break;
            }

            PropList.Add(prop);
        }
    }
}
