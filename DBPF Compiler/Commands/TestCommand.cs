using DBPF_Compiler.ArgScript;
using DBPF_Compiler.ArgScript.Syntax;
using DBPF_Compiler.FileTypes.Prop;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace DBPF_Compiler.Commands
{
    internal class TestCommand : ConsoleCommand
    {
        public override void ParseLine(Line line)
        {
            var tokens = Lexer.Tokenize(File.ReadAllText("example.argscript"), TokenType.MainTokens);
            foreach (var token in tokens)
                WriteLine($"{token.Type}: {token.Text} ({token.Position})");

            var prop = new FormatParserBuilder().AddPropertyListParsers().Build().Parse<PropertyList>(tokens);
            WriteLine(prop?.SerializeToJson(new System.Text.Json.JsonSerializerOptions()
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            }));
        }
    }
}
