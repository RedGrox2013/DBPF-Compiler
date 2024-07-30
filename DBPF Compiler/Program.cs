﻿using DBPF_Compiler.ArgScript;
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

CommandManager cmd = CommandManager.Instance;
cmd.Out = Console.Out;
cmd.PrintErrorAction = PrintError;
cmd.ClearAction = Console.Clear;
cmd.AddCommand("interactive", new InteractiveCommand(Console.In));
cmd.AddCommand("test", new TestCommand() { NotDisplayDescription = true });

if (args[0].Equals("-h", StringComparison.OrdinalIgnoreCase) || args[0].Equals("--help", StringComparison.OrdinalIgnoreCase))
    args[0] = "help";

try
{
    cmd.ParseLine(new Line(args));
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