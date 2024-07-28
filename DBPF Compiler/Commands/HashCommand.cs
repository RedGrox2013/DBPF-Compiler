using DBPF_Compiler.ArgScript;
using DBPF_Compiler.FNV;

namespace DBPF_Compiler.Commands
{
    internal class HashCommand : ConsoleCommand
    {
        public override void ParseLine(Line line)
        {
            string? name = line.ArgumentCount == 2 ? line[1] :
                line.GetOption("-name", 1)?[0] ?? line.GetOption("n", 1)?[0];
            if (string.IsNullOrEmpty(name))
            {
                PrintError?.Invoke("Required argument missing: <name>");
                return;
            }

            string? regName = line.GetOption("-registry", 1)?[0];
            string hash;
            if (regName != null && regName.Equals("fnv", StringComparison.InvariantCultureIgnoreCase))
                hash = FNVHash.ToString(FNVHash.Compute(name));
            else
                hash = FNVHash.ToString(NameRegistryManager.Instance.GetHash(name, regName));

            Out?.WriteLine($"{name} ----[{regName ?? "all"}]----> {hash}");
        }

        public override string? GetDescription(DescriptionMode mode = DescriptionMode.Basic)
        {
            if (mode == DescriptionMode.Basic)
                return "get hash by name.";
            if (mode == DescriptionMode.Complete)
                return @"get hash by name.
Usage:  hash -n <name> [--registry <registry>]
<name>     name to hash
<registry> The registry from which the hash will be searched.
           If the hash is not found, the name will be hashed.
           This parameter accepts the following values (default is all):
                all
                fnv
                file
                property
                type
                simulator";

            return null;
        }
    }
}
