using DBPF_Compiler.FileTypes.Prop;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace DBPF_Compiler.FileTypes.Converters
{
    public class JsonPropConverter(JsonSerializerOptions jsonSerializerOptions) : IConverter
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions = jsonSerializerOptions;

        public JsonPropConverter() : this(new JsonSerializerOptions()
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        }) { }

        public ISporeFile Convert(Stream stream)
        {
            PropertyList prop = new();
            using var reader = new StreamReader(stream);
            prop.DeserializeFromJson(reader.ReadToEnd(), _jsonSerializerOptions);

            return prop;
        }
    }
}
