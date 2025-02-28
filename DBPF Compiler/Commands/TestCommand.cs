using DBPF_Compiler.ArgScript;
using DBPF_Compiler.ArgScript.Syntax;
using DBPF_Compiler.FileTypes.Prop;

namespace DBPF_Compiler.Commands
{
    internal class TestCommand : ConsoleCommand
    {
        public override void ParseLine(Line line)
        {
            var tokens = Lexer.Tokenize(File.ReadAllText("example.argscript"), TokenType.AllTypes);
            foreach (var token in tokens)
                WriteLine($"{token.Type}: {token.Text} ({token.Position})");

            var prop = FormatParser.PropertyListParser.Parse(tokens) as PropertyList;
            WriteLine(prop?.SerializeToJson());
        }
    }
}
