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

            var tokens = Tokenize(line);
            List<string> args = [];
            string? prefix = null;
            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i].Type == TokenType.STR)
                    args.Add(tokens[i].Text.Trim('"'));
                else if (tokens[i].Type == TokenType.MINUS)
                    prefix += tokens[i].Text;
                else
                {
                    args.Add(prefix + tokens[i].Text);
                    prefix = null;
                }
            }

            return new Line(args);
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
