namespace DBPF_Compiler.ArgScript
{
    public static class Lexer
    {
        public static Line LineToArgs(string? line)
            => Line.Parse(line);
    }
}
