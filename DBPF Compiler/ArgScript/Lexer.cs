namespace DBPF_Compiler.ArgScript
{
    internal static class Lexer
    {
        // TODO: оптимизировать, добавить поддержку комментариев
        public static Line[] Analyze(string argscript)
        {
            string[] lines = argscript.Split('\n');
            Line[] result = new Line[lines.Length];

            for (int i = 0; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                {
                    result[i] = Line.Empty;
                    continue;
                }

                string line = lines[i].Trim();
                int argIndex = 0, bracketsLevel = 0;
                bool quotesOpen = false; //, isComment = false;
                List<string> args = [];
                for (int j = 0; j < line.Length; j++)
                {
                    //if (line[j] == '#' && !isComment && j + 1 < line.Length && line[j + 1] == '<')
                    //{
                    //    isComment = true;
                    //    //args.Add(line[argIndex..(j)].TrimEnd('"', ')', ' ', '\t', '#'));
                    //}
                    //else if (line[j] == '#' && isComment && j + 1 < line.Length && line[j + 1] == '>')
                    //    isComment = false;
                    //else if (line[j] == '#')
                    //    break;

                    //if (isComment)
                    //    continue;

                    if (line[j] == '"')
                    {
                        quotesOpen = !quotesOpen;
                        argIndex = j + 1;
                    }
                    else if (line[j] == '(' && bracketsLevel >= 0)
                    {
                        if (bracketsLevel == 0)
                            argIndex = j + 1;
                        ++bracketsLevel;
                    }
                    else if (line[j] == ')')
                    {
                        --bracketsLevel;
                        if (bracketsLevel < 0)
                            throw new ArgScriptException(null, i, j, "No opening bracket");
                    }
                    else if ((line[j] == ' ' || line[j] == '\t') && !quotesOpen && bracketsLevel == 0)
                    {
                        args.Add(line[argIndex..j].TrimEnd('"', ')', ' ', '\t'));
                        argIndex = j + 1;
                    }
                }

                args.Add(line[argIndex..]);
                result[i] = new Line(args) { LinePosition = i };
            }

            return result;
        }

        //public static Line AnalyzeLine(string? line, int linePosition = 0)
        //{
        //    if (string.IsNullOrWhiteSpace(line))
        //        return Line.Empty;

        //    line = line.Trim();
        //    int argIndex = 0, bracketsLevel = 0;
        //    bool quotesOpen = false, isComment = false;
        //    List<string> args = [];
        //    for (int j = 0; j < line.Length; j++)
        //    {
        //        if (line[j] == '#' && !isComment && j + 1 < line.Length && line[j + 1] == '<')
        //            isComment = true;
        //        else if (line[j] == '#' && isComment && j + 1 < line.Length && line[j + 1] == '>')
        //            isComment = false;
        //        else if (line[j] == '#')
        //            break;

        //        if (isComment)
        //            continue;

        //        if (line[j] == '"')
        //        {
        //            quotesOpen = !quotesOpen;
        //            argIndex = j + 1;
        //        }
        //        else if (line[j] == '(' && bracketsLevel >= 0)
        //        {
        //            if (bracketsLevel == 0)
        //                argIndex = j + 1;
        //            ++bracketsLevel;
        //        }
        //        else if (line[j] == ')')
        //        {
        //            --bracketsLevel;
        //            if (bracketsLevel < 0)
        //                throw new ArgScriptException(null, linePosition, j, "No opening bracket");
        //        }
        //        else if ((line[j] == ' ' || line[j] == '\t') && !quotesOpen && bracketsLevel == 0)
        //        {
        //            args.Add(line[argIndex..j].TrimEnd('"', ')'));
        //            argIndex = j + 1;
        //        }
        //    }
        //    args.Add(line[argIndex..]);

        //    return new Line(args) { LinePosition = linePosition };
        //}
    }
}
