using DBPF_Compiler;
using DBPF_Compiler.DBPF;
using System.Text;

static void DisplayDataWritingMessage(object? message)
{
    if (message is IndexEntry entry)
        Console.WriteLine("Writing data: 0x{0}!0x{1}.0x{2}",
            Convert.ToString(entry.GroupID, 16),
            Convert.ToString(entry.InstanceID, 16),
            Convert.ToString(entry.TypeID, 16));
}

Console.WriteLine("Database Packed File Compiler");

//string path;
//if (args.Length == 0)
//{
//    Console.Write("Enter data path: ");
//    path = Console.ReadLine() ?? string.Empty;
//}
//else
//    path = args[0];

const string STR_DATA = "Кто прочитал, тот лох";
byte[] data = Encoding.Default.GetBytes(STR_DATA);
uint dataID = FNVHash.Compute(STR_DATA);

using FileStream fs = File.Create("data.package");
using DatabasePackedFile dbpf = new(fs);
dbpf.OnHeaderWriting += msg => Console.WriteLine("Writing header");
dbpf.OnDataWriting += DisplayDataWritingMessage;
dbpf.OnIndexWriting += msg => Console.WriteLine("Writing index");
dbpf.WriteData(data, dataID, dataID, dataID);
