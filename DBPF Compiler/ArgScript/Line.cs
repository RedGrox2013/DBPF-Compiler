namespace DBPF_Compiler.ArgScript
{
    internal class Line(string[] arguments)
    {
        private readonly string[] _args = arguments;

        public int Count => _args.Length;

        private Line(int count) : this(new string[count]) { }
        public Line(IEnumerable<string> arguments) : this(arguments.ToArray()) { }

        public int Find(string argumentName)
        {
            for (int i = 0; i < _args.Length; i++)
                if (argumentName.Equals(_args[i], StringComparison.InvariantCultureIgnoreCase))
                    return i;

            return -1;
        }

        public bool HasFlag(string flagName) => Find("-" + flagName) != -1;

        public Line? GetOption(string optionName, int count)
        {
            int index = Find("-" + optionName) + 1;
            if (index == 0)
                return null;

            if (index + count >= _args.Length)
                throw new IndexOutOfRangeException(optionName + " does not have as many arguments: " + count);

            Line option = new(count);
            Array.Copy(_args, index, option._args, 0, count);

            return option;
        }

        public override string ToString() => string.Join(' ', _args);

        public string this[int index] => _args[index];
    }
}
