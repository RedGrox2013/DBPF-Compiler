using DBPF_Compiler.ArgScript.Parsers;
using DBPF_Compiler.ArgScript.Syntax;
using DBPF_Compiler.FNV;

namespace DBPF_Compiler.ArgScript
{
    public class FormatParser
    {
        //private readonly Lexer _lexer = lexer;

        private readonly Dictionary<string, IParser> _parsers = [];

        public string CurrentScope { get; private set; } = string.Empty;
        private readonly Dictionary<string, Dictionary<string, object>> _scopes = new()
        {
            {string.Empty, []} // global
        };

        public TextWriter? TraceWriter { get; set; }

        //public FormatParser() : this(new Lexer()) { }

        public void SetGlobalVariable(string name, object value)
            => _scopes[string.Empty][name.ToLower()] = value;
        public void SetVariable(string name, object value)
            => _scopes[CurrentScope][name.ToLower()] = value;
        public object GetVariable(string name)
        {
            name = name.ToLower();
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
            parser.SetData(this, null);
            _parsers.Add(keyword.ToLower(), parser);
            
            //_lexer.AddKeyword(keyword);
        }

        public IParser? GetParser(string keyword) => _parsers.GetValueOrDefault(keyword.ToLower());

        public T Parse<T>(string argScript) where T : new() => Parse<T>(Lexer.Tokenize(argScript));

        public T Parse<T>(List<Token> tokenizedArgScript) where T : new()
        {
            var tokens = tokenizedArgScript;
            ClearScopes();

            int position = 0;
            T result = new();
            while (position < tokens.Count)
            {
                if (Match(tokens, ref position, TokenType.ENDL) != null)
                    continue;

                var keyword = Require(tokens, ref position, TokenType.ARGUMENT, TokenType.HASH);
                IParser parser = GetParser(keyword.Text) ?? throw new ArgScriptException("Unknown token: " + keyword.Text, position);

                List<Token> args = [keyword];
                while (Match(tokens, ref position, TokenType.ENDL) == null)
                    args.Add(tokens[position++]);
                
                parser.SetData(this, result);
                parser.ParseLine(new Line(args));
            }

            return result;
        }

        private /*ArgScriptTree*/ ArgScriptNode ParseExpression(string expression)
        {
            var tokens = Lexer.Tokenize(expression, TokenType.ExpressionsTokens);

            throw new NotImplementedException();
        }

        public string ParseString(string arg) => arg;

        public float ParseFloat(string expression)
        {
            return float.Parse(expression, System.Globalization.CultureInfo.InvariantCulture);
        }

        public int ParseInteger(string expression) => (int)ParseFloat(expression);
        public uint ParseUInteger(string expression) => FNVHash.Parse(expression);

        //public ArgScriptTree Parse(string argScript)
        //{
        //    _tokens = _lexer.Tokenize(argScript);
        //    ClearScopes();

        //    _position = 0;
        //    var tree = new ArgScriptTree();
        //    while (_position < _tokens.Count)
        //    {
        //        var node = ParseCommand();
        //        //if (node == null)
        //        //    continue;
        //        //Require(TokenType.ENDL);
        //        tree.AddNode(node);
        //    }

        //    return tree;
        //}

        //private ArgScriptTree ParseCommand()
        //{
        //    if (_tokens == null)
        //        throw new NullReferenceException("No tokens");

        //    var command = new ArgScriptTree();
        //    while (Match(TokenType.ENDL) == null)
        //    {
        //        var keyword = Match(TokenType.ARGUMENT);
        //        if (keyword != null)
        //            command.AddNode(new ArgumentNode(keyword));
        //        else
        //            command.AddNode(ParseFormula());
        //    }

        //    return command;
        //}

        //private IArgScriptTreeNode ParseFormula()
        //{
        //    var left = ParseParentheses();

        //    throw new NotImplementedException();
        //}

        //private IArgScriptTreeNode ParseParentheses()
        //{
        //    if (Match(TokenType.LPAR) == null)
        //        return ParseValue();

        //    var node = ParseFormula();
        //    Require(TokenType.RPAR);

        //    return node;
        //}

        //private IArgScriptTreeNode ParseValue()
        //{
        //    throw new NotImplementedException();
        //}

        private static Token? Match(List<Token> tokens, ref int position, params IEnumerable<TokenType> expected)
        {
            if (tokens != null && position < tokens.Count && expected.Contains(tokens[position].Type))
                return tokens[position++];

            return null;
        }

        private static Token Require(List<Token> tokens, ref int position, params IEnumerable<TokenType> expected) =>
            Match(tokens, ref position, expected) ??
            throw new ArgScriptException($"Token expected: {string.Join(" or ", expected)}", position);
    }
}
