using DBPF_Compiler.Commands;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace DBPF_Compiler
{
    public class ConfigManager
    {
        private static ConfigManager? _instance;
        public static ConfigManager Instance
        {
            get
            {
                if (_instance == null)
                    Load();

                return _instance ??= new ConfigManager();
            }
        }

        public const string DEFAULT_CONFIGS_PATH = "configs.json";
        private readonly static JsonSerializerOptions _jsonSerializerOptions = new()
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        };
        [JsonIgnore]
        [ConfigsCommandIgnore]
        public string ConfigsPath { get; set; } = DEFAULT_CONFIGS_PATH;

        public string RegistriesPath { get; set; } = "Registries";
        public string? EALayer3Path { get; set; }

        public static async Task LoadAsync(string path = DEFAULT_CONFIGS_PATH, CancellationToken cancellationToken = default)
        {
            if (File.Exists(path))
                _instance = JsonSerializer.Deserialize<ConfigManager>(
                    await File.ReadAllTextAsync(path, cancellationToken));

            _instance ??= new ConfigManager();
            _instance.ConfigsPath = path;
        }
        public static void Load(string path = DEFAULT_CONFIGS_PATH)
            => LoadAsync(path).Wait();

        public static async Task SaveAsync(CancellationToken cancellationToken = default)
        {
            if (_instance == null)
                return;

            using var file = File.Create(_instance.ConfigsPath);
            await JsonSerializer.SerializeAsync(file, _instance, _jsonSerializerOptions, cancellationToken);
        }
        public static void Save() => SaveAsync().Wait();
    }
}
