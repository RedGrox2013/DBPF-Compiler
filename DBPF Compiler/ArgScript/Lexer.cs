namespace DBPF_Compiler.ArgScript
{
    internal static class Lexer
    {
        public static Line[] Analyze(string argscript)
        {
            string[] lines = argscript.Split('\n');
            Line[] result = new Line[lines.Length];
            for (int i = 0; i < lines.Length; i++)
                result[i] = AnalyzeLine(lines[i], i);

            return result;
        }

        // TODO: оптимизировать, добавить поддержку комментариев
        public static Line AnalyzeLine(string? argscriptLine, int linePosition = 0)
        {
            if (string.IsNullOrWhiteSpace(argscriptLine))
                return Line.Empty;

            argscriptLine = argscriptLine.Trim();
            int argIndex = 0, bracketsLevel = 0;
            bool quotesOpen = false;
            List<string> args = [];
            for (int i = 0; i < argscriptLine.Length; i++)
            {
                if (argscriptLine[i] == '"')
                {
                    quotesOpen = !quotesOpen;
                    argIndex = i + 1;
                }
                else if (argscriptLine[i] == '(' && bracketsLevel >= 0)
                {
                    if (bracketsLevel == 0)
                        argIndex = i + 1;
                    ++bracketsLevel;
                }
                else if (argscriptLine[i] == ')')
                {
                    --bracketsLevel;
                    if (bracketsLevel < 0)
                        throw new ArgScriptException(null, linePosition, i, "No opening bracket");
                }
                else if ((argscriptLine[i] == ' ' || argscriptLine[i] == '\t') && !quotesOpen && bracketsLevel == 0)
                {
                    args.Add(argscriptLine[argIndex..i].TrimEnd('"', ')'));
                    argIndex = i + 1;
                }
            }
            args.Add(argscriptLine[argIndex..]);

            return new Line(args) { LinePosition = linePosition };
        }
    }
}
