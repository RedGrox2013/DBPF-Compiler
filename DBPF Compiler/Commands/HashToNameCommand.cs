using DBPF_Compiler.ArgScript;
using DBPF_Compiler.FNV;

namespace DBPF_Compiler.Commands
{
    internal class HashToNameCommand : ASCommand
    {
        public override void ParseLine(Line line)
        {
            if (!FNVHash.TryParse(line[1], out var hash))
            {
                CommandManager.Instance.PrintError($"\"{line[1]}\" is not hash.");
                return;
            }

            string name;
            string? regName = line.GetOption("-registry", 1)?[0];
            if (regName == null || regName.Equals("all", StringComparison.InvariantCultureIgnoreCase))
            {
                name = NameRegistryManager.Instance.GetName(hash);
                if (name.StartsWith("0x"))
                    CommandManager.Instance.WriteLine($"\"{line[1]}\" not found.");
                else
                    CommandManager.Instance.WriteLine(name);

                return;
            }

            var reg = NameRegistryManager.Instance.GetRegistry(regName);
            if (reg == null)
                CommandManager.Instance.PrintError($"\"{regName}\" not found.");
            else if (!reg.GetName(hash, out name))
                CommandManager.Instance.PrintError($"\"{line[1]}\" not found.");
            else
                CommandManager.Instance.WriteLine($"{hash} ----[{regName ?? "all"}]----> {name}");
        }

        public override string? GetDescription(DescriptionMode mode = DescriptionMode.Basic)
        {
            return null;
        }
    }
}
