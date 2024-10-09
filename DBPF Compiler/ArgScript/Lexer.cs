using System.Runtime.InteropServices;

namespace DBPF_Compiler.ArgScript
{
    public static class Lexer
    {
        public static Line LineToArgs(string? line)
        {
            if (string.IsNullOrWhiteSpace(line))
                return Line.Empty;

            IntPtr argv = CommandLineToArgvW(line, out int argc);
            if (argv == IntPtr.Zero)
                return Line.Empty;

            try
            {
                string[] args = new string[argc];
                for (int i = 0; i < argc; i++)
                {
                    IntPtr pStr = Marshal.ReadIntPtr(argv, i * IntPtr.Size);
                    args[i] = Marshal.PtrToStringUni(pStr) ?? string.Empty;
                }

                return new Line(args);
            }
            finally
            {
                Marshal.FreeHGlobal(argv);
            }
        }

        [DllImport("shell32.dll", SetLastError = true)]
        private static extern IntPtr CommandLineToArgvW([MarshalAs(UnmanagedType.LPWStr)] string lpCmdLine, out int pNumArgs);
    }
}
