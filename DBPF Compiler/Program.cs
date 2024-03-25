using DBPF_Compiler;
using DBPF_Compiler.DBPF;
using DBPF_Compiler.Types;
using System.Diagnostics;
using System.Text;

Console.WriteLine("Spore Database Packed File Compiler");

if (args.Length == 0)
    return;

if (args[0].Equals("--help") || args[0].Equals("-h"))
    Console.WriteLine(@"
--help, -h:                    show help
--pack, -p <input> <output>:   pack the contents of a folder into DBPF
--unpack, -u <input> <output>: unpack DBPF to a specified directory
");
else if ((args[0].Equals("--pack") || args[0].Equals("-p")) && CheckArguments(args))
    Pack(args[1], args[2]);
else if ((args[0].Equals("--unpack") || args[0].Equals("-u")) && CheckArguments(args))
    Unpack(args[1], args[2]);

static void Pack(string inputPath, string outputPath)
{
    DBPFPacker packer = new(inputPath);
    Stopwatch stopwatch = Stopwatch.StartNew();

    const string STR_DATA = "Я люблю кринжовник";
    byte[] data = Encoding.Default.GetBytes(STR_DATA);
    uint dataID = FNVHash.Compute(STR_DATA);

    using FileStream fs = File.Create(outputPath);
    using DatabasePackedFile dbpf = new(fs);
    dbpf.OnHeaderWriting += msg => Console.WriteLine("Writing header");
    dbpf.OnDataWriting += DisplayDataWritingMessage;
    dbpf.OnIndexWriting += msg => Console.WriteLine("Writing index");
    dbpf.WriteData(data, new ResourceKey(dataID, dataID, dataID));

    packer.Pack(dbpf);

    stopwatch.Stop();
    var ts = stopwatch.Elapsed;
    Console.WriteLine($"The file was packed in {ts.Seconds}:{ts.Milliseconds}:{ts.Nanoseconds} sec.");
}

static void Unpack(string inputPath, string outputPath)
{
    DirectoryInfo dir = new(outputPath);
    using FileStream fs = new(inputPath, FileMode.Open, FileAccess.Read);
    using DatabasePackedFile dbpf = new(fs);
    Stopwatch stopwatch = Stopwatch.StartNew();

    foreach (var resource in dbpf.ReadDBPFInfo())
    {
        var path = outputPath + "\\0x" + Convert.ToString(resource.GroupID, 16);
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        using FileStream file = File.Create(path + "\\0x" +
            Convert.ToString(resource.InstanceID, 16) + ".0x" +
            Convert.ToString(resource.TypeID, 16));
        dbpf.CopyResourceTo(file, resource);
    }

    stopwatch.Stop();
    var ts = stopwatch.Elapsed;
    Console.WriteLine($"The file was unpacked in {ts.Seconds}:{ts.Milliseconds}:{ts.Nanoseconds} sec.");
}

static void DisplayDataWritingMessage(object? message)
{
    if (message is ResourceKey key)
        Console.WriteLine("Writing data: {0}", key);
}

static bool CheckArguments(string[] args)
{
    if (args.Length == 1)
    {
        Console.WriteLine("Missing <input> and <output> arguments.");
        return false;
    }
    if (args.Length == 2)
    {
        Console.WriteLine("Missing <output> argument.");
        return false;
    }

    return true;
}