using DBPF_Compiler.ArgScript.Syntax;
using System.Text.RegularExpressions;

namespace DBPF_Compiler.ArgScript
{
    public static class Lexer
    {
        public static Line LineToArgs(string? line)
        {
            if (string.IsNullOrWhiteSpace(line))
                return Line.Empty;

            return new Line(Tokenize(line));
        }

        internal static List<Token> Tokenize(string argScript)
        {
            List<Token> tokens = [];
            int pos = 0;
            while (pos < argScript.Length)
            {
                bool success = false;
                foreach (var type in TokenType.TokenTypes)
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

            //return from t in tokens
            //       where t.Type != TokenType.SPACE &&
            //       t.Type != TokenType.MULTILCOMMENT &&
            //       t.Type != TokenType.COMMENT
            //       select t;
            return tokens;
        }
    }
}
