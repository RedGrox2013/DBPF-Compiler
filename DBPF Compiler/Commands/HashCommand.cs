using DBPF_Compiler.ArgScript;
using DBPF_Compiler.FNV;

namespace DBPF_Compiler.Commands
{
    internal class HashCommand : ASCommand
    {
        public override void ParseLine(Line line)
        {
            //if (line[2].Equals("fnv", StringComparison.InvariantCultureIgnoreCase))
            //{
            //    CommandManager.Instance.WriteLine(FNVHash.ToString(FNVHash.Compute(line[1])));
            //    return;
            //}

            //CommandManager.Instance.WriteLine(FNVHash.ToString(NameRegistryManager.Instance.GetHash(name, regName)));

            string? name = line.GetOption("-name", 1)?[0] ?? line.GetOption("n", 1)?[0];
            if (string.IsNullOrEmpty(name))
            {
                CommandManager.Instance.PrintError("Required argument missing: <name>");
                return;
            }

            string? regName = line.GetOption("-registry", 1)?[0];
            if (regName != null && regName.Equals("fnv", StringComparison.InvariantCultureIgnoreCase))
            {
                CommandManager.Instance.WriteLine(FNVHash.ToString(FNVHash.Compute(name)));
                return;
            }

            CommandManager.Instance.WriteLine(FNVHash.ToString(NameRegistryManager.Instance.GetHash(name, regName)));
        }

        public override string? GetDescription(DescriptionMode mode = DescriptionMode.Basic)
        {
            // TODO: Добавить нормальное описание
            if (mode == DescriptionMode.Basic || mode == DescriptionMode.Complete)
                return "example: hash -n <name> --registry <registry>";

            return null;
        }
    }
}
