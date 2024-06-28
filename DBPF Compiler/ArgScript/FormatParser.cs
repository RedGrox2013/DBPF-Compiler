namespace DBPF_Compiler.ArgScript
{
    public class FormatParser
    {
        public ParserState State => States.Count == 0 ? ParserState.None : States.Peek();
        protected Stack<ParserState> States { get; set; }
        protected Stack<IParser> CurrentParsers { get; set; }

        protected Dictionary<string, IParser> Parsers { get; set; }
        protected Dictionary<string, IBlock> Blocks { get; set; }
        protected Dictionary<string, ISpecialBlock> SpecialBlocks { get; set; }

        public FormatParser()
        {
            Parsers = [];
            States = new();
            CurrentParsers = new();
            Blocks = [];
            SpecialBlocks = [];
        }

        public void AddParser(string keyword, IParser parser)
        {
            parser.FormatParser = this;
            Parsers.Add(keyword, parser);
        }

        public IParser GetParser(string keyword)
            => Parsers[keyword];

        public void AddBlock(string keyword, IBlock block)
        {
            block.FormatParser = this;
            Blocks.Add(keyword, block);
        }

        public IBlock GetBlock(string keyword)
            => Blocks[keyword];

        public void ParseLines(IEnumerable<Line> lines)
        {
            foreach (Line line in lines)
                ParseLine(line);
        }

        public void ParseLine(Line line)
        {
            if (line.ArgumentCount == 0)
                return;

            if (State == ParserState.Block)
            {
                if (line[0].Equals("end", StringComparison.InvariantCultureIgnoreCase))
                {
                    IBlock? block = CurrentParsers.Pop() as IBlock;
                    States.Pop();
                    block?.OnBlockEnd();
                }
                else
                    CurrentParsers.Peek().ParseLine(line);

                return;
            }

            if (State == ParserState.SpecialBlock)
            {
                ISpecialBlock? block = CurrentParsers.Peek() as ISpecialBlock;
                if (line[0].Equals(block?.EndKeyword, StringComparison.InvariantCultureIgnoreCase))
                {
                    CurrentParsers.Pop();
                    States.Pop();
                    block.OnBlockEnd();
                }
                else
                    block?.ParseLine(line);

                return;
            }

            if (Parsers.TryGetValue(line[0], out IParser? parser) && parser != null)
                parser.ParseLine(line);
            else if (Blocks.TryGetValue(line[0], out IBlock? block) && block != null)
            {
                CurrentParsers.Push(block);
                States.Push(ParserState.Block);
                block.OnBlockStart(line);
            }
            else if (SpecialBlocks.TryGetValue(line[0], out ISpecialBlock? specBlock) && specBlock != null)
            {
                CurrentParsers.Push(specBlock);
                States.Push(ParserState.SpecialBlock);
                specBlock.OnBlockStart(line);
            }
            else
                throw new ArgScriptException(line, line.LinePosition, 0, $"Unknown keyword \"{line[0]}\"");
        }
    }

    public enum ParserState
    {
        None,
        Block,
        SpecialBlock
    }
}
