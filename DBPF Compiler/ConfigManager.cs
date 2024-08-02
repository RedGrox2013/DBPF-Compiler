using System.Text.Json;
using System.Text.Json.Serialization;

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

                return _instance ?? new ConfigManager(DEFAULT_CONFIGS_PATH);
            }
        }

        public const string DEFAULT_CONFIGS_PATH = "configs.json";
        [JsonIgnore]
        public string ConfigsPath { get; set; }
        public string RegistriesPath { get; set; } = "Registries";

        public static async Task LoadAsync(string path = DEFAULT_CONFIGS_PATH, CancellationToken cancellationToken = default)
        {
            if (File.Exists(path))
                _instance = JsonSerializer.Deserialize<ConfigManager>(
                    await File.ReadAllTextAsync(path, cancellationToken));
            _instance ??= new ConfigManager(DEFAULT_CONFIGS_PATH);
        }

        public static void Load(string path = DEFAULT_CONFIGS_PATH)
            => LoadAsync(path).Wait();

        private ConfigManager(string configPath) => ConfigsPath = configPath;
    }
}
