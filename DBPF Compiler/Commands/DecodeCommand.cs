﻿using DBPF_Compiler.ArgScript;
using DBPF_Compiler.FileTypes.Prop;

namespace DBPF_Compiler.Commands
{
    internal class DecodeCommand : ASCommand
    {
        public override void ParseLine(Line line)
        {
            if (line.ArgumentCount < 2)
            {
                CommandManager.Instance.PrintError("Missing <input> and <output> arguments.");
                return;
            }

            using FileStream stream = File.OpenRead(line[1]);
            string json = PropertyListJsonSerializer.DecodePropertyListToJson(stream);
            CommandManager.Instance.WriteLine(json);
            using StreamWriter writer = File.CreateText(line.ArgumentCount > 3 ? line[3] : (line[1] + ".json"));
            writer.Write(json);
        }

        public override string? GetDescription(DescriptionMode mode = DescriptionMode.Basic)
        {
            if (mode == DescriptionMode.Basic)
                return "decode file.";
            if (mode == DescriptionMode.Complete)
                return "decode file. This command takes the following arguments: <input> and <output>.";

            throw new NotImplementedException();
        }
    }
}