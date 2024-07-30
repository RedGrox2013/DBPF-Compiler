using DBPF_Compiler.ArgScript;
using DBPF_Compiler.Types;

namespace DBPF_Compiler.Commands
{
    internal class TestCommand : ConsoleCommand
    {
        public override void ParseLine(Line line)
        {
            StringResourceKey key = StringResourceKey.Parse(line[1]);
            WriteLine(
@$"Group:    {key.GroupID}
Instance: {key.InstanceID}
Type:     {key.TypeID}");
        }
    }
}
