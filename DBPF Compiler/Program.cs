using DBPF_Compiler.ArgScript;
using DBPF_Compiler.Commands;
using DBPF_Compiler.DBPF;
using DBPF_Compiler.FileTypes.Prop;
using DBPF_Compiler.FNV;
using DBPF_Compiler.Types;
using System.Diagnostics;

Console.WriteLine("Spore Database Packed File Compiler");

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

CommandManager manager = CommandManager.Initialize();
manager.Out = Console.Out;
manager.PrintErrorEvent += PrintError;
Line line = new(args);

try
{
    if (args[0].Equals("--help") || args[0].Equals("-h"))
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
        args[0].Equals("--unpack") || args[0].Equals("-u"))
        manager.ParseLine(line);
    else if (args[0].Equals("--encode") || args[0].Equals("-e"))
        Encode(args[1], args.Length >= 3 ? args[2] : null);
    else if (args[0].Equals("--decode") || args[0].Equals("-d"))
        Decode(args[1], args.Length >= 3 ? args[2] : null);
    else if (args[0].Equals("--hash"))
        Hash(args[1], args.Length >= 3 ? args[2] : "all");
    else if (args[0].Equals("--name-by-hash"))
        NameByHash(args[1], args.Length >= 3 ? args[2] : "all");
}
catch (Exception e)
{
    PrintError(e.Message);
}


static void Encode(string inputPath, string? outputPath)
{
    var prop = PropertyListJsonSerializer.Deserialize(File.ReadAllText(inputPath));

    using FileStream stream = File.Create(outputPath + "\\" + Path.GetFileNameWithoutExtension(inputPath));
    prop.Encode(stream);
}

static void Decode(string inputPath, string? outputPath)
{
    using FileStream stream = File.OpenRead(inputPath);
    string json = PropertyListJsonSerializer.DecodePropertyListToJson(stream);
    Console.WriteLine(json);
    using StreamWriter writer = File.CreateText(outputPath ?? inputPath + ".json");
    writer.Write(json);
}

static void Hash(string name, string regName)
{
    if (regName.Equals("fnv", StringComparison.InvariantCultureIgnoreCase))
    {
        Console.WriteLine(FNVHash.ToString(FNVHash.Compute(name)));
        return;
    }

    Console.WriteLine(FNVHash.ToString(NameRegistryManager.Instance.GetHash(name, regName)));
}

static void NameByHash(string strHash, string regName)
{
    if (!FNVHash.TryParse(strHash, out var hash))
    {
        PrintError($"\"{strHash}\" is not hash.");
        return;
    }

    string name;
    if (regName.Equals("all", StringComparison.InvariantCultureIgnoreCase))
    {
        name = NameRegistryManager.Instance.GetName(hash);
        if (name.StartsWith("0x"))
            Console.WriteLine($"\"{strHash}\" not found.");
        else
            Console.WriteLine(name);

        return;
    }

    var reg = NameRegistryManager.Instance.GetRegistry(regName);
    if (reg == null)
    {
        PrintError($"\"{regName}\" not found.");
        return;
    }
    if (!reg.GetName(hash, out name))
    {
        PrintError($"\"{strHash}\" not found.");
        return;
    }

    Console.WriteLine(name);
}

static void PrintError(object? message)
{
    var oldColor = Console.ForegroundColor;
    Console.ForegroundColor = ConsoleColor.Red;
    Console.Error.WriteLine(message);
    Console.ForegroundColor = oldColor;
}