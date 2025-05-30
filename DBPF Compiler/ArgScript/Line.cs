﻿using DBPF_Compiler.ArgScript.Syntax;
using System.Runtime.InteropServices;
using System.Text;

namespace DBPF_Compiler.ArgScript
{
    public class Line
    {
        private readonly string[] _args;

        public int ArgumentCount => _args.Length;
        public int LinePosition { get; set; } = 0;

        public static Line Empty => new(0);

        private Line(int count) : this(new string[count]) { }
        public Line(IEnumerable<string> arguments) : this(arguments.ToArray()) { }
        Line(string[] arguments)
        {
            _args = arguments;
        }
        internal Line(IEnumerable<Token> tokens)
        {
            List<string> args = [];
            string? prefix = null;
            foreach (var token in tokens)
            {
                if (string.IsNullOrWhiteSpace(token.Text))
                    continue;

                if (token.Type == TokenType.STR)
                    args.Add(token.Text.Trim('"'));
                else if (token.Type == TokenType.BRACEEXPR)
                    args.Add(token.Text.TrimStart('(', '{').TrimEnd(')', '}'));
                else if (token.Type == TokenType.MINUS ||
                    token.Type == TokenType.DEVIDE ||
                    //token.Type == TokenType.VARIABLE ||
                    token.Type == TokenType.MOD ||
                    token.Type == TokenType.PLUS ||
                    token.Type == TokenType.MULTIPLY ||
                    token.Type == TokenType.POWER)
                    prefix += token.Text;
                else
                {
                    args.Add(prefix + token.Text);
                    prefix = null;
                }
            }

            _args = [.. args];
        }

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

        [Obsolete("Представляет функцию CommandLineToArgvW из shell32.dll. Для полной поддержки ArgScript лучше использовать Lexer.LineToArgs")]
        /// <summary>
        /// Парсит строку и преобразует её в <see cref="Line"/><br/>
        /// Представляет функцию <c>CommandLineToArgvW</c> из shell32.dll
        /// </summary>
        /// <param name="line"></param>
        /// <returns>Строка в виде объекта <see cref="Line"/></returns>
        public static Line Parse(string? line)
        {
            if (string.IsNullOrWhiteSpace(line))
                return Empty;

            IntPtr argv = CommandLineToArgvW(line, out int argc);
            if (argv == IntPtr.Zero)
                return Empty;

            try
            {
                Line result = new(argc);
                for (int i = 0; i < argc; i++)
                {
                    IntPtr pStr = Marshal.ReadIntPtr(argv, i * IntPtr.Size);
                    result._args[i] = Marshal.PtrToStringUni(pStr) ?? string.Empty;
                }

                return result;
            }
            finally
            {
                Marshal.FreeHGlobal(argv);
            }
        }

        [Obsolete("Представляет функцию CommandLineToArgvW из shell32.dll. Для полной поддержки ArgScript лучше использовать Lexer.LineToArgs")]
        public static bool TryParse(string? line, out Line? result)
        {
            try
            {
                result = Parse(line);
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
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

        [DllImport("shell32.dll", SetLastError = true)]
        private static extern IntPtr CommandLineToArgvW([MarshalAs(UnmanagedType.LPWStr)] string lpCmdLine, out int pNumArgs);
    }
}
