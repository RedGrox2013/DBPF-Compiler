namespace DBPF_Compiler.ArgScript.Syntax
{
    internal class Token(TokenType type, string text, int position)
    {
        public readonly TokenType Type = type;
        public readonly string Text = text;
        public readonly int Position = position;
    }
}
