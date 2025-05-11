using DBPF_Compiler;
using DBPF_Compiler.ArgScript;
using DBPF_Compiler.Commands;
using DBPF_Compiler.DBPFCLua;
using DBPF_Compiler.FNV;

#region ASCII art
const ConsoleColor ART_COLOR = ConsoleColor.Green;
var oldColor = Console.ForegroundColor;
Console.ForegroundColor = ART_COLOR;

//Console.Write(@"

//░▒▓███████▓▒░░▒▓███████▓▒░░▒▓███████▓▒░░▒▓████████▓▒░▒▓██████▓▒░  
//░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░     ░▒▓█▓▒░░▒▓█▓▒░ 
//░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░     ░▒▓█▓▒░        
//░▒▓█▓▒░░▒▓█▓▒░▒▓███████▓▒░░▒▓███████▓▒░░▒▓██████▓▒░░▒▓█▓▒░        
//░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░      ░▒▓█▓▒░     ░▒▓█▓▒░        
//░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░      ░▒▓█▓▒░     ░▒▓█▓▒░░▒▓█▓▒░ 
//░▒▓███████▓▒░░▒▓███████▓▒░░▒▓█▓▒░      ░▒▓█▓▒░      ░▒▓██████▓▒░  



//");
Console.Write(@"

$$$$$$$\  $$$$$$$\  $$$$$$$\  $$$$$$$$\  $$$$$$\  
$$  __$$\ $$  __$$\ $$  __$$\ $$  _____|$$  __$$\ 
$$ |  $$ |$$ |  $$ |$$ |  $$ |$$ |      $$ /  \__|
$$ |  $$ |$$$$$$$\ |$$$$$$$  |$$$$$\    $$ |      
$$ |  $$ |$$  __$$\ $$  ____/ $$  __|   $$ |      
$$ |  $$ |$$ |  $$ |$$ |      $$ |      $$ |  $$\ 
$$$$$$$  |$$$$$$$  |$$ |      $$ |      \$$$$$$  |
\_______/ \_______/ \__|      \__|       \______/ 




");
//Console.Write(@"

//  ____    ____    ____     _____    ____  
// |  _""\U | __"")uU|  _""\ u |"" ___|U /""___| 
///| | | |\|  _ \/\| |_) |/U| |_  u\| | u   
//U| |_| |\| |_) | |  __/  \|  _|/  | |/__  
// |____/ u|____/  |_|      |_|      \____| 
//  |||_  _|| \\_  ||>>_    )(\\,-  _// \\  
// (__)_)(__) (__)(__)__)  (__)(_/ (__)(__) 

//");
//Console.Write(@"

//    ____  ____  ____  ____________
//   / __ \/ __ )/ __ \/ ____/ ____/
//  / / / / __  / /_/ / /_  / /     
// / /_/ / /_/ / ____/ __/ / /___   
///_____/_____/_/   /_/    \____/   


//");
//Console.Write(@"

//__/\\\\\\\\\\\\_____/\\\\\\\\\\\\\____/\\\\\\\\\\\\\____/\\\\\\\\\\\\\\\________/\\\\\\\\\_        
// _\/\\\////////\\\__\/\\\/////////\\\_\/\\\/////////\\\_\/\\\///////////______/\\\////////__       
//  _\/\\\______\//\\\_\/\\\_______\/\\\_\/\\\_______\/\\\_\/\\\_______________/\\\/___________      
//   _\/\\\_______\/\\\_\/\\\\\\\\\\\\\\__\/\\\\\\\\\\\\\/__\/\\\\\\\\\\\______/\\\_____________     
//    _\/\\\_______\/\\\_\/\\\/////////\\\_\/\\\/////////____\/\\\///////______\/\\\_____________    
//     _\/\\\_______\/\\\_\/\\\_______\/\\\_\/\\\_____________\/\\\_____________\//\\\____________   
//      _\/\\\_______/\\\__\/\\\_______\/\\\_\/\\\_____________\/\\\______________\///\\\__________  
//       _\/\\\\\\\\\\\\/___\/\\\\\\\\\\\\\/__\/\\\_____________\/\\\________________\////\\\\\\\\\_ 
//        _\////////////_____\/////////////____\///______________\///____________________\/////////__

//");
//Console.Write(@"

//   ___     ___      ___     ___    ___   
//  |   \   | _ )    | _ \   | __|  / __|  
//  | |) |  | _ \    |  _/   | _|  | (__   
//  |___/   |___/   _|_|_   _|_|_   \___|  
//_|""""""""""|_|""""""""""|_| """""" |_| """""" |_|""""""""""| 
//""`-0-0-'""`-0-0-'""`-0-0-'""`-0-0-'""`-0-0-' 

//");
//Console.Write(@"

// ________  ________  ________  ________ ________     
//|\   ___ \|\   __  \|\   __  \|\  _____\\   ____\    
//\ \  \_|\ \ \  \|\ /\ \  \|\  \ \  \__/\ \  \___|    
// \ \  \ \\ \ \   __  \ \   ____\ \   __\\ \  \       
//  \ \  \_\\ \ \  \|\  \ \  \___|\ \  \_| \ \  \____  
//   \ \_______\ \_______\ \__\    \ \__\   \ \_______\
//    \|_______|\|_______|\|__|     \|__|    \|_______|




//");

//Console.Write("Spore Database Packed File Compiler ");

Console.ForegroundColor = oldColor;
Console.Write("Spore ");
Console.ForegroundColor = ART_COLOR;
Console.Write('D');
Console.ForegroundColor = oldColor;
Console.Write("ata");
Console.ForegroundColor = ART_COLOR;
Console.Write('b');
Console.ForegroundColor = oldColor;
Console.Write("ase ");
Console.ForegroundColor = ART_COLOR;
Console.Write('P');
Console.ForegroundColor = oldColor;
Console.Write("acked ");
Console.ForegroundColor = ART_COLOR;
Console.Write('F');
Console.ForegroundColor = oldColor;
Console.Write("ile ");
Console.ForegroundColor = ART_COLOR;
Console.Write('C');
Console.ForegroundColor = oldColor;
Console.Write("ompiler ");

Console.WriteLine(typeof(Program).Assembly.GetName().Version);
Console.WriteLine();
#endregion

Line line = await Initialize(args);

var console = new TraceConsole(Console.Out, Console.In);
CommandManager cmd = new()
{
    Console = console,
    PrintErrorAction = PrintError,
    ClearAction = Console.Clear
};
DBPFCServices.AddService(cmd);
DBPFCServices.AddService(console);
using var app = new ConsoleApp(cmd);

cmd.AddCommand("interactive", new InteractiveCommand(cmd));
#if DEBUG
cmd.AddCommand("test", new TestCommand() { NotDisplayDescription = true });
#endif

if (!app.Run(line))
    Environment.ExitCode = -1;

await ConfigManager.SaveAsync();


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
    DBPFCServices.AddService(configs);

    if (!await NameRegistryManager.LoadAsync(new DirectoryInfo(configs.RegistriesPath)))
        PrintError("No registers found. Translating hashes is not possible.");
    else
        DBPFCServices.AddService(NameRegistryManager.Instance);

    /*if (string.IsNullOrWhiteSpace(configs.EALayer3Path) || !File.Exists(configs.EALayer3Path))
    {
        PrintErrorAction("EALayer3 not found. MP3 files will not be converted to SNR.");
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
    }*/

    if (args.Length > 0 &&
        (args[0].Equals("-h", StringComparison.OrdinalIgnoreCase) ||
        args[0].Equals("--help", StringComparison.OrdinalIgnoreCase)))
        args[0] = "help";

    return new Line(args);
}