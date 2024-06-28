namespace DBPF_Compiler.ArgScript
{
    public class ArgScriptException : Exception
    {
        public Line? TargetLine { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }

        public ArgScriptException(Line? targetLine, int row, int column)
        {
            TargetLine = targetLine;
            Row = row;
            Column = column;
        }
        public ArgScriptException(Line? targetLine, int row, int column, string? message) : base(message)
        {
            TargetLine = targetLine;
            Row = row;
            Column = column;
        }
    }
}
