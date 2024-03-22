using DBPF_Compiler.DBPF;

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
