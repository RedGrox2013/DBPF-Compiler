using DBPF_Compiler.ArgScript;
using DBPF_Compiler.DBPF;
using DBPF_Compiler.FNV;
using DBPF_Compiler.Types;

namespace DBPF_Compiler.Commands
{
    internal class ExtractCommand : ConsoleCommand
    {
        public override void ParseLine(Line line)
        {
            string? resource = line.GetOption("k", 1)?[0];
            if (resource == null)
            {
                PrintError("Required argument missing: <key>");
                return;
            }

            string? dbpfPath = line.GetOption("p", 1)?[0];
            if (dbpfPath == null)
            {
                PrintError("Required argument missing: <DBPF_path>");
                return;
            }

            var skey = StringResourceKey.Parse(resource);
            var key = NameRegistryManager.Instance.GetResourceKey(skey);
            string output = line.GetOption("o", 1)?[0] ?? (skey.InstanceID + "." + skey.TypeID);

            using FileStream file = File.OpenRead(dbpfPath);
            using DatabasePackedFile dbpf = new(file);
            
            if (!dbpf.ReadDBPFInfo().Contains(key))
            {
                PrintError(skey + " not found");
                return;
            }

            using FileStream outputFile = File.Create(output);
            dbpf.CopyResourceTo(outputFile, key);
        }

        public override string? GetDescription(DescriptionMode mode = DescriptionMode.Basic)
        {
            if (mode == DescriptionMode.Basic)
                return "extract resource from DBPF.";
            if (mode == DescriptionMode.Complete)
                return @"extract resource from DBPF.
Usage:  extract -p <DBPF_path> -k <key> [-o <output>]
<DBPF_path> path to DBPF
<key>       resource key
<output>    path to the output file";

            return base.GetDescription(mode);
        }
    }
}
