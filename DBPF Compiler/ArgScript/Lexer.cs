using DBPF_Compiler.ArgScript.Syntax;
using System.Text.RegularExpressions;

namespace DBPF_Compiler.ArgScript
{
    public static class Lexer
    {
        //private List<TokenType> _tokenTypes = [.. TokenType.MainTokens];

        public static Line LineToArgs(string? line)
        {
            if (string.IsNullOrWhiteSpace(line))
                return Line.Empty;

            return new Line(Tokenize(line, TokenType.MainTokens));
        }

        internal static List<Token> Tokenize(string argScript, IEnumerable<TokenType> tokenTypes)
        {
            List<Token> tokens = [];
            int pos = 0;
            while (pos < argScript.Length)
            {
                bool success = false;
                foreach (var type in tokenTypes)
                {
                    var result = Regex.Match(argScript[pos..], "^" + type.Regex,
                        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
                    success = !string.IsNullOrEmpty(result.Value);
                    if (success)
                    {
                        if (type != TokenType.SPACE &&
                            type != TokenType.MULTILCOMMENT &&
                            type != TokenType.COMMENT)
                            tokens.Add(new Token(type, result.Value, pos));
                        pos += result.Value.Length;

                        break;
                    }
                }

                if (!success)
                    throw new ArgScriptException($"Unknown token in {pos}", pos);
            }

            if (tokens.Count > 0 && tokens[^1].Type != TokenType.ENDL)
                tokens.Add(new Token(TokenType.ENDL, "\n", pos));

            //return from t in tokens
            //       where t.Type != TokenType.SPACE &&
            //       t.Type != TokenType.MULTILCOMMENT &&
            //       t.Type != TokenType.COMMENT
            //       select t;
            return tokens;
        }

        //internal List<Token> Tokenize(string argScript) => Tokenize(argScript, _tokenTypes);
        internal static List<Token> Tokenize(string argScript) => Tokenize(argScript, TokenType.MainTokens);

        //public void AddKeyword(string keyword) =>
        //    _tokenTypes.Add(new TokenType(keyword, string.Format(@"\b{0}\b", keyword)));

        //public void Clear() => _tokenTypes = [.. TokenType.MainTokens];
    }
}
