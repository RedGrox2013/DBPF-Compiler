using DBPF_Compiler.FileTypes.Prop;

namespace DBPF_Compiler.ArgScript.Parsers
{
    class FloatPropertyParser : ArgScriptCommand
    {
        public override void ParseLine(Line line)
        {
            PropertyList prop;
            if (Data == null)
                Data = prop = new PropertyList();
            else
                prop = Data as PropertyList ?? throw new Exception();

            prop.Add(new Property(line[1])
            {
                PropertyType = PropertyType.@float,
                Value = FormatParser.ParseFloat(line[2])
            });
        }

        public override string? GetDescription(DescriptionMode mode = DescriptionMode.Basic) => null;
    }
}
