using DBPF_Compiler.ArgScript;
using DBPF_Compiler.FNV;

namespace DBPF_Compiler.Commands
{
    internal class HashToNameCommand : ASCommand
    {
        public override void ParseLine(Line line)
        {
            if (line.ArgumentCount < 2)
            {
                CommandManager.Instance.PrintError("Required arguments are missing");
                return;
            }

            string strHash = line.GetOption("-hash", 1)?[0] ?? line[1];
            if (!FNVHash.TryParse(strHash, out var hash))
            {
                CommandManager.Instance.PrintError($"\"{strHash}\" is not hash.");
                return;
            }

            string name;
            string? regName = line.GetOption("-registry", 1)?[0];
            if (regName == null || regName.Equals("all", StringComparison.InvariantCultureIgnoreCase))
            {
                name = NameRegistryManager.Instance.GetName(hash);
                if (name.StartsWith("0x"))
                {
                    CommandManager.Instance.PrintError($"\"{strHash}\" not found.");
                    return;
                }
            }
            else
            {
                var reg = NameRegistryManager.Instance.GetRegistry(regName);
                if (reg == null)
                {
                    CommandManager.Instance.PrintError($"\"{regName}\" not found.");
                    return;
                }
                if (!reg.GetName(hash, out name))
                {
                    CommandManager.Instance.PrintError($"\"{strHash}\" not found.");
                    return;
                }
            }

            CommandManager.Instance.WriteLine($"{strHash} ----[{regName ?? "all"}]----> {name}");
        }

        public override string? GetDescription(DescriptionMode mode = DescriptionMode.Basic)
        {
            if (mode == DescriptionMode.Basic)
                return "get name by hash.";
            if (mode == DescriptionMode.Complete)
                return @"get name by hash.
Usage:     hash-to-name --hash <hash> [--registry <registry>]
<hash>     hash by which the name will be searched
<registry> The registry from which the name will be searched.
           This parameter accepts the following values (default is all):
                all
                file
                property
                type
                simulator";

            return null;
        }
    }
}
