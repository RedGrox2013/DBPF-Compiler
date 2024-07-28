using DBPF_Compiler.ArgScript;
using DBPF_Compiler.FNV;

namespace DBPF_Compiler.Commands
{
    internal class HashToNameCommand : ConsoleCommand
    {
        public override void ParseLine(Line line)
        {
            if (line.ArgumentCount < 2)
            {
                PrintError?.Invoke("Required arguments are missing");
                return;
            }

            string strHash = line.GetOption("-hash", 1)?[0] ?? line[1];
            if (!FNVHash.TryParse(strHash, out var hash))
            {
                PrintError?.Invoke($"\"{strHash}\" is not hash.");
                return;
            }

            string name;
            string? regName = line.GetOption("-registry", 1)?[0];
            if (regName == null || regName.Equals("all", StringComparison.InvariantCultureIgnoreCase))
            {
                name = NameRegistryManager.Instance.GetName(hash);
                if (name.StartsWith("0x"))
                {
                    PrintError?.Invoke($"\"{strHash}\" not found.");
                    return;
                }
            }
            else
            {
                var reg = NameRegistryManager.Instance.GetRegistry(regName);
                if (reg == null)
                {
                    PrintError?.Invoke($"\"{regName}\" not found.");
                    return;
                }
                if (!reg.GetName(hash, out name))
                {
                    PrintError?.Invoke($"\"{strHash}\" not found.");
                    return;
                }
            }

            Out?.WriteLine($"{strHash} ----[{regName ?? "all"}]----> {name}");
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
