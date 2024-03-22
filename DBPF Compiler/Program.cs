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

using FileStream fs = File.Create("data.package");
using DatabasePackedFile dbpf = new(fs);
dbpf.WriteData(Encoding.Default.GetBytes("Кто прочитал, тот лох"), 0xBC2C0BE9, 0xBC2C0BE9, 0xBC2C0BE9);
