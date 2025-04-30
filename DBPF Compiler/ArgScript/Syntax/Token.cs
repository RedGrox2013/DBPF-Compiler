namespace DBPF_Compiler.ArgScript.Syntax
{
    public class Token(TokenType type, string text, int position)
    {
        public readonly TokenType Type = type;
        public readonly string Text = text;
        public readonly int Position = position;
    }
}
