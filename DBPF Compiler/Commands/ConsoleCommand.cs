using DBPF_Compiler.ArgScript;

namespace DBPF_Compiler.Commands
{
    public abstract class ConsoleCommand : ASCommand
    {
        public bool NotDisplayDescription { get; set; } = false;
        public TextWriter? Out { get; set; }
        public Action<object?>? PrintError { get; set; }
        public Action? Clear { get; set; }

        public override string? GetDescription(DescriptionMode mode = DescriptionMode.Basic)
            => null;

        public void Write(object? message) => Out?.Write(message);
        public void WriteLine(object? message) => Out?.WriteLine(message);
    }
}
