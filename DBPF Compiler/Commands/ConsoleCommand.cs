using DBPF_Compiler.ArgScript;

namespace DBPF_Compiler.Commands
{
    public abstract class ConsoleCommand : ArgScriptCommand
    {
        public bool NotDisplayDescription { get; set; } = false;
        public virtual TraceConsole? Console { get; set; }
        public virtual Action<object?>? PrintErrorAction { get; set; }
        public virtual Action? ClearAction { get; set; }

        public override string? GetDescription(DescriptionMode mode = DescriptionMode.Basic)
            => null;

        protected virtual void Write(object? message) => Console?.Write(message);
        protected virtual void WriteLine(object? message) => Console?.WriteLine(message);
        protected virtual void WriteLine() => Console?.WriteLine();
        protected virtual void PrintError(object? message) => PrintErrorAction?.Invoke(message);
    }
}
