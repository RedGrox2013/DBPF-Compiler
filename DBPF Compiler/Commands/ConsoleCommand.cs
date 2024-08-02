using DBPF_Compiler.ArgScript;

namespace DBPF_Compiler.Commands
{
    public abstract class ConsoleCommand : ArgScriptCommand
    {
        public bool NotDisplayDescription { get; set; } = false;
        public TextWriter? Out { get; set; }
        public Action<object?>? PrintErrorAction { get; set; }
        public Action? ClearAction { get; set; }

        public override string? GetDescription(DescriptionMode mode = DescriptionMode.Basic)
            => null;

        protected virtual void Write(object? message) => Out?.Write(message);
        protected virtual void WriteLine(object? message) => Out?.WriteLine(message);
        protected virtual void PrintError(object? message) => PrintErrorAction?.Invoke(message);
    }
}
