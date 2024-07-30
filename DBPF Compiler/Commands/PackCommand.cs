using DBPF_Compiler.ArgScript;
using DBPF_Compiler.DBPF;
using DBPF_Compiler.Types;
using System.Diagnostics;

namespace DBPF_Compiler.Commands
{
    internal class PackCommand : ConsoleCommand
    {
        public override void ParseLine(Line line)
        {
            if (line.ArgumentCount == 1)
            {
                PrintErrorAction?.Invoke("Missing <input> and <output> arguments.");
                return;
            }
            if (line.ArgumentCount == 2)
            {
                PrintErrorAction?.Invoke("Missing <output> argument.");
                return;
            }

            DBPFPacker packer = new(line[1]);
            Stopwatch stopwatch = Stopwatch.StartNew();
        
            using FileStream fs = File.Create(line[2]);
            using DatabasePackedFile dbpf = new(fs);
            dbpf.OnHeaderWriting += msg => Out?.WriteLine("Writing header . . .");
            dbpf.OnDataWriting += DisplayDataWritingMessage;
            dbpf.OnIndexWriting += msg => Out?.WriteLine("Writing index . . .");

            packer.Pack(dbpf, line.ArgumentCount > 3 ? line[3] : null);

            stopwatch.Stop();
            var ts = stopwatch.Elapsed;
            Out?.WriteLine($"The file was packed in {ts.Seconds}:{ts.Milliseconds}:{ts.Nanoseconds} sec.");
        }

        public override string? GetDescription(DescriptionMode mode = DescriptionMode.Basic)
        {
            if (mode == DescriptionMode.Basic)
                return "pack the contents of a folder into DBPF.";
            if (mode == DescriptionMode.Complete)
                return
    @"pack the contents of a folder into DBPF.
Usage:  pack <input> <output> [<secret>]
    <input> - path to source data
    <output> - path to the output file
    <secret> - name of the folder whose contents are hidden in the DBPF";

            return null;
        }

        private void DisplayDataWritingMessage(object? message)
        {
            if (message is ResourceKey key)
                Out?.WriteLine($"Writing data: {key} . . .");
        }
    }
}
