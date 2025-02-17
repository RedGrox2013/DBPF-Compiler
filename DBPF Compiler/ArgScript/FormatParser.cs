using DBPF_Compiler.ArgScript.Syntax;

namespace DBPF_Compiler.ArgScript
{
    public class FormatParser(Lexer lexer)
    {
        private readonly Lexer _lexer = lexer;

        private List<Token>? _tokens;
        private int _position;

        private readonly Dictionary<string, IParser> _parsers = [];

        public string CurrentScope { get; private set; } = string.Empty;
        private readonly Dictionary<string, Dictionary<string, object>> _scopes = new()
        {
            {string.Empty, []} // global
        };

        public FormatParser() : this(new Lexer()) { }

        public void SetGlobalVariable(string name, object value)
            => _scopes[string.Empty][name] = value;
        public void SetVariable(string name, object value)
            => _scopes[CurrentScope][name] = value;
        public object GetVariable(string name)
        {
            foreach (var scope in _scopes)
                if (scope.Value.TryGetValue(name, out var value))
                    return value;

            throw new KeyNotFoundException();
        }
        public T GetVariable<T>(string name)
            => (T)GetVariable(name);

        public void ClearScopes()
        {
            CurrentScope = string.Empty;
            _scopes.Clear();
            _scopes.Add(string.Empty, []);
        }

        public void AddParser(string keyword, IParser parser)
        {
            parser.SetData(this, _tokens);
            _parsers.Add(keyword, parser);

            _lexer.AddKeyword(keyword);
        }

        public IParser GetParser(string keyword)
            => _parsers[keyword];

        public ArgScriptTree Parse(string argScript)
        {
            _tokens = _lexer.Tokenize(argScript);
            ClearScopes();

            _position = 0;
            var tree = new ArgScriptTree();
            while (_position < _tokens.Count)
            {
                var node = ParseCommand();
                //if (node == null)
                //    continue;
                //Require(TokenType.ENDL);
                tree.AddNode(node);
            }

            return tree;
        }

        private ArgScriptTree ParseCommand()
        {
            if (_tokens == null)
                throw new NullReferenceException("No tokens");

            var command = new ArgScriptTree();
            while (Match(TokenType.ENDL) == null)
            {
                var keyword = Match(TokenType.ARGUMENT);
                if (keyword != null)
                    command.AddNode(new KeywordNode(keyword));
                else
                    command.AddNode(ParseFormula());
            }

            return command;
        }

        private IArgScriptTreeNode ParseFormula()
        {
            var left = ParseParentheses();

            throw new NotImplementedException();
        }

        private IArgScriptTreeNode ParseParentheses()
        {
            if (Match(TokenType.LPAR) == null)
                return ParseValue();

            var node = ParseFormula();
            Require(TokenType.RPAR);

            return node;
        }

        private IArgScriptTreeNode ParseValue()
        {
            throw new NotImplementedException();
        }

        private Token? Match(params IEnumerable<TokenType> expected)
        {
            if (_tokens != null && _position < _tokens.Count && expected.Contains(_tokens[_position].Type))
                return _tokens[_position++];

            return null;
        }

        private Token Require(params IEnumerable<TokenType> expected)
            => Match(expected) ??
            throw new ArgScriptException($"Token expected: {string.Join(" or ", expected)}", _position);
    }
}
