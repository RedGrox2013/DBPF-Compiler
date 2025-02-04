using DBPF_Compiler.ArgScript.Syntax;

namespace DBPF_Compiler.ArgScript
{
    public class FormatParser
    {
        private List<Token>? _tokens;

        private readonly Dictionary<string, object> _variables = [];

        public FormatParser()
        {
            
        }
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
            _variables.Clear();
            _tokens = Lexer.Tokenize(argScript);

            Parse();
        }
    }
}
