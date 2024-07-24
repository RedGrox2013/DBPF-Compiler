using DBPF_Compiler.ArgScript;
using DBPF_Compiler.FileTypes.Prop;

namespace DBPF_Compiler.Commands
{
    internal class EncodeCommand : ASCommand
    {
        public override void ParseLine(Line line)
        {
            if (line.ArgumentCount < 2)
            {
                CommandManager.Instance.PrintError("Missing <input> and <output> arguments.");
                return;
            }

            var prop = PropertyListJsonSerializer.Deserialize(File.ReadAllText(line[1]));
            string? outPath = line.ArgumentCount > 2 ? (line[2] + "\\") : null;
            using FileStream stream = File.Create(outPath + Path.GetFileNameWithoutExtension(line[1]));
            prop.Encode(stream);
        }

        public override string? GetDescription(DescriptionMode mode = DescriptionMode.Basic)
        {
            if (mode == DescriptionMode.Basic)
                return "encode file.";
            if (mode == DescriptionMode.Complete)
                return "encode file. This command takes the following arguments: <input> and <output>.";

            return null;
        }
    }
}
