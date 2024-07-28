using DBPF_Compiler.ArgScript;
using DBPF_Compiler.DBPF;
using DBPF_Compiler.Types;
using System.Diagnostics;

namespace DBPF_Compiler.Commands
{
    internal class UnpackCommand : ConsoleCommand
    {
        public override void ParseLine(Line line)
        {
            if (line.ArgumentCount == 1)
            {
                PrintError?.Invoke("Missing <input> and <output> arguments.");
                return;
            }
            if (line.ArgumentCount == 2)
            {
                PrintError?.Invoke("Missing <output> argument.");
                return;
            }

            Stopwatch stopwatch = Stopwatch.StartNew();
            using FileStream fs = new(line[1], FileMode.Open, FileAccess.Read);
            using DatabasePackedFile dbpf = new(fs);

            dbpf.OnHeaderReading += msg => Out?.WriteLine("Reading header . . .");
            dbpf.OnDataReading += DisplayDataReadingMessage;
            dbpf.OnIndexReading += msg => Out?.WriteLine("Reading index . . . Index offset: " + (msg as uint?));

            DBPFPacker unpacker = new(line[2]);

            unpacker.Unpack(dbpf);
            unpacker.UnpackSecret(dbpf);

            stopwatch.Stop();
            var ts = stopwatch.Elapsed;
            Out?.WriteLine($"The file was unpacked in {ts.Seconds}:{ts.Milliseconds}:{ts.Nanoseconds} sec.");
        }

        public override string? GetDescription(DescriptionMode mode = DescriptionMode.Basic)
        {
            if (mode == DescriptionMode.Basic)
                return "unpack DBPF to a specified directory.";
            if (mode == DescriptionMode.Complete)
                return
    @"unpack DBPF to a specified directory.
Usage:  unpack <input> <output>
    <input> - path to DBPF
    <output> - path to output directory";

            return null;
        }

        private void DisplayDataReadingMessage(object? message)
        {
            if (message is ResourceKey key)
                Out?.WriteLine($"Writing data: {key} . . .");
        }
    }
}
