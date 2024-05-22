using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace DBPF_Compiler.FileTypes.Prop
{
    public static class PropertyListJsonSerializer
    {
        private readonly static JsonSerializerOptions _jsonSerializerOptions = new()
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            UnknownTypeHandling = System.Text.Json.Serialization.JsonUnknownTypeHandling.JsonNode
        };

        public static string DecodePropertyListToJson(Stream propListStream)
        {
            PropertyList prop = new();
            prop.Decode(propListStream);

            return prop.SerializeToJson(_jsonSerializerOptions);
        }

        public static PropertyList Deserialize(string json)
        {
            PropertyList prop = new();
            prop.DeserializeFromJson(json);

            return prop;
        }
    }
}
