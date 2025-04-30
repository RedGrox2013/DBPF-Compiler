using DBPF_Compiler.ArgScript.Parsers;

namespace DBPF_Compiler.ArgScript
{
    public class FormatParserBuilder
    {
        private bool _propParsersAdded = false,
                     _mathAdded        = false;

        private readonly Dictionary<string, IParser> _parsers = [];

        public FormatParser Build()
        {
            FormatParser result = new();
            foreach (var parser in _parsers)
                result.AddParser(parser.Key, parser.Value);

            return result;
        }

        public FormatParserBuilder Clear()
        {
            _parsers.Clear();
            _propParsersAdded = _mathAdded = false;

            return this;
        }

        public FormatParserBuilder AddParser(string keyword, IParser parser)
        {
            _parsers.Add(keyword, parser);
            return this;
        }

        public FormatParserBuilder AddPropertyListParsers(bool mathFunctionsSupport = true)
        {
            if (_propParsersAdded)
                return this;

            _propParsersAdded = true;
            IParser strParser = new StringPropertyParser(),
                    intParser = new IntPropertyParser();

            _parsers.Add("string8",  strParser);
            _parsers.Add("string16", strParser);
            _parsers.Add("int8",     intParser);
            _parsers.Add("int16",    intParser);
            _parsers.Add("int32",    intParser);
            _parsers.Add("int64",    intParser);
            _parsers.Add("uint8",    intParser);
            _parsers.Add("uint16",   intParser);
            _parsers.Add("uint32",   intParser);
            _parsers.Add("uint64",   intParser);
            _parsers.Add("float",    new FloatPropertyParser());

            return mathFunctionsSupport ? AddMath() : this;
        }

        public FormatParserBuilder AddMath()
        {
            if (_mathAdded)
                return this;

            _mathAdded = true;

            // доделать

            return this;
        }
    }
}
