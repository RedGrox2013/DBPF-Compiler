using DBPF_Compiler;
using DBPF_Compiler.ArgScript;
using DBPF_Compiler.Commands;
using DBPF_Compiler.FNV;

Console.WriteLine("Spore Database Packed File Compiler\n");

Line line = await Initialize(args);

try
{
    CommandManager.Instance.ParseLine(line);
    await ConfigManager.SaveAsync();
}
catch (Exception e)
{
    PrintError(e.Message);
    Environment.Exit(-1);
}


static void PrintError(object? message)
{
    var oldColor = Console.ForegroundColor;
    Console.ForegroundColor = ConsoleColor.Red;
    Console.Error.WriteLine(message);
    Console.ForegroundColor = oldColor;
}

static async Task<Line> Initialize(string[] args)
{
    await ConfigManager.LoadAsync();
    var configs = ConfigManager.Instance;

    if (!await NameRegistryManager.LoadAsync(new DirectoryInfo(configs.RegistriesPath)))
        PrintError("No registers found. Translating hashes is not possible.");
    if (string.IsNullOrWhiteSpace(configs.EALayer3Path) || !File.Exists(configs.EALayer3Path))
    {
        PrintError("EALayer3 not found. MP3 files will not be converted to SNR.");
        Console.Write("To resolve this issue, do the following:\n\t1. Download EALayer3: ");
        var oldColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("https://github.com/driftyz700/ealayer3-nfsw/releases");
        Console.ForegroundColor = oldColor;
        Console.Write("\t2. Use the command ");
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write("configs set ");
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write(nameof(configs.EALayer3Path));
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine(" <path>");
        Console.ForegroundColor = oldColor;
    }

    CommandManager cmd = CommandManager.Instance;
    cmd.Out = Console.Out;
    cmd.PrintErrorAction = PrintError;
    cmd.ClearAction = Console.Clear;
    cmd.AddCommand("interactive", new InteractiveCommand(Console.In));
    cmd.AddCommand("test", new TestCommand() { NotDisplayDescription = true });

    if (args.Length > 0 &&
        (args[0].Equals("-h", StringComparison.OrdinalIgnoreCase) ||
        args[0].Equals("--help", StringComparison.OrdinalIgnoreCase)))
        args[0] = "help";

    return new Line(args);
}