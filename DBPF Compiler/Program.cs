using DBPF_Compiler;
using DBPF_Compiler.DBPF;
using System.Diagnostics;
using System.Text;

Console.WriteLine("Database Packed File Compiler");

string path;
if (args.Length == 0)
{
    Console.Write("Enter data path: ");
    path = Console.ReadLine() ?? string.Empty;
}
else
    path = args[0];

DBPFPacker packer = new(path);
Stopwatch stopwatch = Stopwatch.StartNew();

const string STR_DATA = "Я люблю кринжовник";
byte[] data = Encoding.Default.GetBytes(STR_DATA);
uint dataID = FNVHash.Compute(STR_DATA);

using FileStream fs = File.Create("output.package");
using DatabasePackedFile dbpf = new(fs);
dbpf.OnHeaderWriting += msg => Console.WriteLine("Writing header");
dbpf.OnDataWriting += DisplayDataWritingMessage;
dbpf.OnIndexWriting += msg => Console.WriteLine("Writing index");
dbpf.WriteData(data, dataID, dataID, dataID);

packer.Pack(dbpf);

stopwatch.Stop();
var ts = stopwatch.Elapsed;

Console.WriteLine($"\nIndex size: {dbpf.IndexSize}, index offset {dbpf.IndexOffset}\nReading header");
dbpf.ReadDBPFInfo();
Console.WriteLine(@$"Index size: {dbpf.IndexSize}, index offset {dbpf.IndexOffset}
The file was packed in {ts.Seconds}:{ts.Milliseconds}:{ts.Nanoseconds} sec.");


static void DisplayDataWritingMessage(object? message)
{
    if (message is not IndexEntry entry)
        return;
    string instance = Convert.ToString(entry.InstanceID, 16);
    if (entry.GroupID == null || entry.TypeID == null)
    {
        Console.WriteLine("Writing data: " + instance);
        return;
    }

    Console.WriteLine("Writing data: 0x{0}!0x{1}.0x{2}",
        Convert.ToString((uint)entry.GroupID, 16),
        instance,
        Convert.ToString((uint)entry.TypeID, 16));
}
