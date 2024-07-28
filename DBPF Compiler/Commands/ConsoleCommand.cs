using DBPF_Compiler.ArgScript;

namespace DBPF_Compiler.Commands
{
    public abstract class ConsoleCommand : ASCommand
    {
        public bool NotDisplayDescription { get; set; } = false;

        public override string? GetDescription(DescriptionMode mode = DescriptionMode.Basic)
            => null;
    }
}
