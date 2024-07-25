using DBPF_Compiler.ArgScript;
using DBPF_Compiler.Commands;
using DBPF_Compiler.FNV;

Console.WriteLine("Spore Database Packed File Compiler\n");

if (args.Length == 0)
    return;

DirectoryInfo regDir = new("Registries");
if (regDir.Exists)
{
    foreach (var file in regDir.GetFiles())
        if (file.Name.StartsWith("reg_"))
            await NameRegistryManager.Instance.AddRegistryFromFileAsync(file.FullName);
}
else
    PrintError("No registers found. Translating hashes is not possible.");

CommandManager cmd = CommandManager.Initialize();
cmd.Out = Console.Out;
cmd.In = Console.In;
cmd.PrintErrorEvent += PrintError;
cmd.AddCommand(new InteractiveCommand(), "interactive");
Line line = new(args);

try
{
    /*if (args[0].Equals("--help") || args[0].Equals("-h"))
        Console.WriteLine(@"
--help, -h:                           show help
--pack, -p <input> <output> <secret>: pack the contents of a folder into DBPF. <secret> - name of the folder whose contents are hidden in the DBPF
--unpack, -u <input> <output>:        unpack DBPF to a specified directory
--encode, -e <input> <output>:        encode file
--decode, -d <input> <output>:        decode file
--hash <name> <registry>:             get hash by name
--name-by-hash <name> <registry>:     get name by hash
");
    else if (args[0].Equals("--pack") || args[0].Equals("-p") ||
        args[0].Equals("--unpack") || args[0].Equals("-u") ||
        args[0].Equals("--encode") || args[0].Equals("-e") ||
        args[0].Equals("--help") || args[0].Equals("-h") ||
        args[0].Equals("--decode") || args[0].Equals("-d") ||
        args[0].Equals("--hash"))
        cmd.ParseLine(line);
    else if (args[0].Equals("--name-by-hash"))
        NameByHash(args[1], args.Length >= 3 ? args[2] : "all");*/

    //if (line.HasFlag("-interactive"))
    //{
    //    do
    //    {
    //        cmd.Write(">>> ");
    //        args = Console.ReadLine()?.Split();
    //        line = args == null ? Line.Empty : new(args);
    //        cmd.ParseLine(line);
    //    } while (!line[0].Equals("^Z") && !line[0].Equals("exit", StringComparison.OrdinalIgnoreCase));
    //}
    //else
    //    cmd.ParseLine(line);

    cmd.ParseLine(line);
}
catch (Exception e)
{
    PrintError(e.Message);
}


static void PrintError(object? message)
{
    var oldColor = Console.ForegroundColor;
    Console.ForegroundColor = ConsoleColor.Red;
    Console.Error.WriteLine(message);
    Console.ForegroundColor = oldColor;
}