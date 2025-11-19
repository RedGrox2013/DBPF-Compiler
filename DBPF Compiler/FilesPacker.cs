using DBPF_Compiler.DBPF;
using DBPF_Compiler.FileTypes;
using DBPF_Compiler.FileTypes.Converters;
using DBPF_Compiler.FNV;
using DBPF_Compiler.Types;

namespace DBPF_Compiler
{
    public class FilesPacker
    {
        public delegate void FilesPackerEventHandler(FilesPacker? sender, FilesPackerEventArgs e);

        private readonly Dictionary<string, IConverter> _converters = [];

        public const string IGNORE_FOLDERS_EXTENSION = ".dbpfc_ignore";

        public event FilesPackerEventHandler? OnFilePacked;

        private static readonly NameRegistryManager _regManager = NameRegistryManager.Instance;

        public void AddConverter(string type, IConverter encoder) => _converters.Add(type, encoder);

        public void Pack(string inputPath, DatabasePackedFile output, string? secretFolder = null) =>
            Pack(new DirectoryInfo(inputPath), output, secretFolder);

        public void Pack(DirectoryInfo inputDirectory, DatabasePackedFile output, string? secretFolder = null)
        {
            DBPFPackerHelper helper = new();
            if (ModProject.TryLoad(inputDirectory.FullName, out var project))
                project?.BuildMod(output, helper);

            foreach (var group in inputDirectory.GetDirectories())
            {
                if (group.Name.EndsWith(IGNORE_FOLDERS_EXTENSION))
                    continue;

                if (group.Name == secretFolder)
                {
                    output.SecretGroupName = secretFolder;
                    foreach (var secret in group.GetFiles())
                    {
                        StringResourceKey key = new(secret.Name, secret.Extension[1..]);
                        OnFilePacked?.Invoke(this, new(key));
                        using FileStream f = secret.OpenRead();
                        output.CopySecretDataFromStream(f, key);
                    }

                    continue;
                }

                foreach (var d in group.GetDirectories())
                {
                    if (!d.Name.EndsWith(".package.unpacked"))
                        continue;

                    using MemoryStream stream = new();
                    using DatabasePackedFile package = new(stream);
                    Pack(d, package);

                    string folderName = Path.GetFileNameWithoutExtension(d.Name);
                    ResourceKey key = new(
                        _regManager.GetHash(folderName, "file"),
                        (uint)TypeIDs.package,
                        _regManager.GetHash(group.Name, "file")
                        );
                    stream.Position = 0;
                    output.CopyFromStream(stream, key);
                }

                foreach (var file in group.GetFiles())
                {
                    string[] splitFileName = file.Name.Split('.');
                    string fileName = splitFileName[0];
                    string extension = splitFileName.Length > 1 ? splitFileName[1] : string.Empty;
                    StringResourceKey strKey = new(fileName, extension, group.Name);
                    ResourceKey key = _regManager.GetResourceKey(strKey);

                    using FileStream f = file.OpenRead();
                    ISporeFile? sporeFile = ConvertToSporeFile(f,
                        splitFileName.Length > 1 ? extension + "." + splitFileName[^1] : string.Empty);

                    if (sporeFile != null)
                        output.WriteSporeFile(sporeFile, key);
                    else
                        output.CopyFromStream(f, key);

                    OnFilePacked?.Invoke(this, new(strKey));
                }
            }

            helper.WriteHelperData(output);

            output.WriteIndex();
            output.WriteSecretIndex();
            output.WriteHeader();
        }

        private ISporeFile? ConvertToSporeFile(Stream stream, string type)
        {
            if (_converters.TryGetValue(type, out var converter))
                return converter.Convert(stream);

            return null;
        }

        public record class FilesPackerEventArgs(StringResourceKey Key);
    }
}
