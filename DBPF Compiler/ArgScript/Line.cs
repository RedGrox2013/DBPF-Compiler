using System.Text;

namespace DBPF_Compiler.ArgScript
{
    public class Line(string[] arguments)
    {
        private readonly string[] _args = arguments;

        public int ArgumentCount => _args.Length;
        public int LinePosition { get; set; } = 0;

        public static Line Empty => new(0);

        private Line(int count) : this(new string[count]) { }
        public Line(IEnumerable<string> arguments) : this(arguments.ToArray()) { }

        public static bool IsNullOrEmpty(Line? line)
        {
            if (line == null || line.ArgumentCount == 0)
                return true;

            foreach (var arg in line._args)
                if (!string.IsNullOrWhiteSpace(arg))
                    return false;

            return true;
        }

        public int Find(string argumentName)
            => Find(a => a.Equals(argumentName, StringComparison.InvariantCultureIgnoreCase));

        public int Find(Predicate<string> predicate)
        {
            for (int i = 0; i < _args.Length; ++i)
                if (predicate(_args[i]))
                    return i;

            return -1;
        }

        public bool HasFlag(string flagName) => Find("-" + flagName) != -1;

        public string[]? GetOption(string optionName, int count)
        {
            int index = Find("-" + optionName) + 1;
            if (index == 0)
                return null;

            if (index + count - 1 >= _args.Length)
                throw new IndexOutOfRangeException(optionName + " does not have as many arguments: " + count);

            string[] result = new string[count];
            Array.Copy(_args, index, result, 0, count);

            return result;
        }

        public override string ToString()
        {
            StringBuilder sb = new();
            foreach (var arg in _args)
            {
                if (arg.Contains(' '))
                {
                    sb.Append('"');
                    sb.Append(arg);
                    sb.Append('"');
                }
                else
                    sb.Append(arg);
                sb.Append(' ');
            }

            return sb.ToString();
        }

        public string this[int index] => _args[index];
    }
}
