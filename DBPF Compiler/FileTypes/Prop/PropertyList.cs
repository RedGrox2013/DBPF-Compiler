using System.Text.Json.Serialization;
using System.Xml;

namespace DBPF_Compiler.FileTypes.Prop
{
    [JsonSerializable(typeof(PropertyList))]
    public class PropertyList : ISporeFile
    {
        [JsonIgnore]
        public TypeIDs TypeID => TypeIDs.prop;

        [JsonInclude]
        public readonly List<Property> Properties = [];

        public bool Decode(byte[]? data)
        {
            throw new NotImplementedException();
        }

        public uint WriteToStream(Stream stream)
        {
            throw new NotImplementedException();
        }

        public XmlDocument ToXml()
        {
            XmlDocument xml = new();
            var declaration = xml.CreateXmlDeclaration("1.0", "utf-8", null);
            xml.AppendChild(declaration);
            XmlElement root = xml.CreateElement("properties");
            xml.AppendChild(root);

            foreach (var property in Properties)
                root.AppendChild(property.ToXml(xml));

            return xml;
        }

        public object? GetValue(string propertyName, PropertyType type)
            => Properties.Find(p
                => p.Name.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase) && p.PropertyType == type)?.Value;

        public void Add(Property property) => Properties.Add(property);
    }
}
