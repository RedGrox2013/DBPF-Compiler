using DBPF_Compiler;
using DBPF_Compiler.DBPF;
using System.Text;

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
Console.WriteLine("0x" + Convert.ToString(dataID, 16));

using FileStream fs = File.Create("data.package");
using DatabasePackedFile dbpf = new(fs);
dbpf.WriteData(data, dataID, dataID, dataID);
