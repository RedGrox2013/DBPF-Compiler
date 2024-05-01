using DBPF_Compiler.DBPF;
using DBPF_Compiler.FileTypes.Prop;
using DBPF_Compiler.FNV;
using DBPF_Compiler.Types;
using System.Diagnostics;
using System.Text;
using System.Xml;

Console.WriteLine("Spore Database Packed File Compiler");

if (args.Length == 0)
    return;

DirectoryInfo regDir = new("Registries");
if (regDir.Exists)
{
    foreach (var file in regDir.GetFiles())
        if (file.Name.StartsWith("reg_"))
            NameRegistryManager.Instance.AddRegistryFromFileAsync(file.FullName).Wait();
}

if (args[0].Equals("--help") || args[0].Equals("-h"))
    Console.WriteLine(@"
--help, -h:                    show help
--pack, -p <input> <output>:   pack the contents of a folder into DBPF
--unpack, -u <input> <output>: unpack DBPF to a specified directory
--encode, -e:                  encode file
");
else if ((args[0].Equals("--pack") || args[0].Equals("-p")) && CheckArguments(args))
    Pack(args[1], args[2]);
else if ((args[0].Equals("--unpack") || args[0].Equals("-u")) && CheckArguments(args))
    Unpack(args[1], args[2]);
else if (args[0].Equals("--encode") || args[0].Equals("-e"))
    Encode();

static void Pack(string inputPath, string outputPath, string? secretFolder = null)
{
    DBPFPacker packer = new(inputPath);
    Stopwatch stopwatch = Stopwatch.StartNew();

    const string STR_DATA = "Со мной воюет сатана 😈";
    byte[] data = Encoding.Default.GetBytes(STR_DATA);
    uint dataID = FNVHash.Compute(STR_DATA);

    using FileStream fs = File.Create(outputPath);
    using DatabasePackedFile dbpf = new(fs);
    dbpf.OnHeaderWriting += msg => Console.WriteLine("Writing header . . .");
    dbpf.OnDataWriting += DisplayDataWritingMessage;
    dbpf.OnIndexWriting += msg => Console.WriteLine("Writing index . . .");
    dbpf.WriteData(data, new ResourceKey(dataID, FNVHash.Compute("txt"), dataID));

    //dbpf.WriteSecretData(Encoding.Default.GetBytes("Уууу секретики"), new("Секретик", "txt"));

    packer.Pack(dbpf);

    stopwatch.Stop();
    var ts = stopwatch.Elapsed;
    Console.WriteLine($"The file was packed in {ts.Seconds}:{ts.Milliseconds}:{ts.Nanoseconds} sec.");
}

static void Unpack(string inputPath, string outputPath)
{
    Stopwatch stopwatch = Stopwatch.StartNew();
    using FileStream fs = new(inputPath, FileMode.Open, FileAccess.Read);
    using DatabasePackedFile dbpf = new(fs);

    dbpf.OnHeaderReading += msg => Console.WriteLine("Reading header . . .");
    dbpf.OnDataReading += DisplayDataReadingMessage;
    dbpf.OnIndexReading += msg => Console.WriteLine("Reading index . . . Index offset: " + (msg as uint?));

    DBPFPacker unpacker = new(outputPath);

    unpacker.Unpack(dbpf);

    stopwatch.Stop();
    var ts = stopwatch.Elapsed;
    Console.WriteLine($"The file was unpacked in {ts.Seconds}:{ts.Milliseconds}:{ts.Nanoseconds} sec.");
}

static void Encode()
{
    PropertyList prop = new();
    prop.Add(new Property
    {
        Name = "Test",
        PropertyType = PropertyType.@bool,
        Value = true,
    });
    prop.Add(new Property
    {
        Name = "Test2",
        PropertyType = PropertyType.int32,
        Value = "1000-7",
    });
    prop.Add(new Property
    {
        Name = "Test3",
        PropertyType = PropertyType.key,
        Value = new StringResourceKey("instance", "type", "group"),
    });
    prop.Add(new Property
    {
        Name = "Test4",
        PropertyType = PropertyType.string8,
        Value = "Hello world!",
    });
    prop.Add(new Property
    {
        Name = "Test5",
        PropertyType = PropertyType.string16,
        Value = "Привет мир!",
    });

    prop.ToXml().Save("test.prop.xml");
}

static void DisplayDataWritingMessage(object? message)
{
    if (message is ResourceKey key)
        Console.WriteLine("Writing data: {0} . . .", key);
}
static void DisplayDataReadingMessage(object? message)
{
    if (message is ResourceKey key)
        Console.WriteLine("Reading data: {0} . . .", key);
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