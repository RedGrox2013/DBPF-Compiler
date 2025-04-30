using DBPF_Compiler.FileTypes.Prop;

namespace DBPF_Compiler.ArgScript.Parsers
{
    abstract class PropertyParser : ArgScriptCommand
    {
        public string? Description { get; protected set; }

        protected PropertyList PropList
        {
            get
            {
                PropertyList prop;
                if (Data == null)
                    Data = prop = new PropertyList();
                else
                    prop = Data as PropertyList ?? throw new Exception("Failed to get property list");

                return prop;
            }
        }

        protected Property ParseProperty(Line line, Func<string, object>? parseValue = null) =>
            new(FormatParser.ParseString(line[1]))
            {
                PropertyType = Enum.Parse<PropertyType>(FormatParser.ParseString(line[0]), true),
                Value = parseValue?.Invoke(line[2])
            };

        public override string? GetDescription(DescriptionMode mode = DescriptionMode.Basic) =>
            mode != DescriptionMode.HTML ? Description : null;
    }
}
