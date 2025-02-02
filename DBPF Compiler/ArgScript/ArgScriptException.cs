namespace DBPF_Compiler.ArgScript
{
    public class ArgScriptException : Exception
    {
        public int Position { get; set; }

        public ArgScriptException(int position)
        {
            Position = position;
        }
        public ArgScriptException(string? message, int position) : base(message)
        {
            Position = position;
        }
        public ArgScriptException(string? message, int position, Exception? innerException) : base(message, innerException)
        {
            Position = position;
        }
    }
}
