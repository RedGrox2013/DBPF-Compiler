namespace DBPF_Compiler
{
    public class TraceConsole(TextWriter output, TextReader input)
    {
        public TextWriter Out { get; set; } = output;
        public TextReader In { get; set; } = input;

        public void Write(object? message) => Out.Write(message);
        public void WriteLine(object? message) => Out.WriteLine(message);
        public void WriteLine() => Out.WriteLine();
        public string? ReadLine() => In.ReadLine();
    }
}
