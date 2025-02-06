using DBPF_Compiler.ArgScript.Syntax;

namespace DBPF_Compiler.ArgScript
{
    public class FormatParser
    {
        private List<Token>? _tokens;
        private int _position;

        private readonly Dictionary<string, object> _variables = [];

        public FormatParser() { }
        internal FormatParser(IEnumerable<Token> tokens)
        {
            _tokens = new(tokens);
        }

        public void SetVariable(string name, object value)
            => _variables[name] = value;
        public object GetVariable(string name)
            => _variables[name];
        public T GetVariable<T>(string name)
            => (T)_variables[name];

        internal void Parse()
        {
            throw new NotImplementedException();
        }

        public void Parse(string argScript)
        {
            _tokens = Lexer.Tokenize(argScript);
            _variables.Clear();
            _position = 0;

            Parse();
        }

        private Token? Match(params TokenType[] expected)
        {
            if (_tokens != null && _position < _tokens.Count && expected.Contains(_tokens[_position].Type))
                return _tokens[_position++];

            return null;
        }

        private Token Require(params TokenType[] expected)
            => Match(expected) ??
            throw new ArgScriptException($"Token expected: {string.Join(" or ", expected)}", _position);
    }
}
