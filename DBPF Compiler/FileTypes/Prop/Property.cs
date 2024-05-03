using DBPF_Compiler.FNV;
using DBPF_Compiler.Types;
using System.Text.Json.Serialization;
using System.Xml;

namespace DBPF_Compiler.FileTypes.Prop
{
    [JsonSerializable(typeof(Property))]
    public class Property(string propertyName)
    {
        public string Name { get; set; } = propertyName;
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PropertyType PropertyType { get; set; }
        public object? Value { get; set; }

        public XmlElement ToXml(XmlDocument xml)
        {
            XmlElement property = xml.CreateElement(PropertyType.ToString());
            XmlAttribute name = xml.CreateAttribute("name");
            name.Value = Name;
            property.Attributes.Append(name);

            switch (PropertyType)
            {
                case PropertyType.key:
                    XmlAttribute instance = xml.CreateAttribute("instanceid");
                    if (Value is StringResourceKey key)
                        instance.Value = key.InstanceID;
                    else if (Value is ResourceKey rk)
                    {
                        key = NameRegistryManager.Instance.GetStringResourceKey(rk);
                        instance.Value = key.InstanceID;
                    }
                    else
                    {
                        instance.Value = Value?.ToString();
                        key = new();
                    }

                    if (!string.IsNullOrWhiteSpace(key.GroupID))
                    {
                        XmlAttribute group = xml.CreateAttribute("groupid");
                        group.Value = key.GroupID;
                        property.Attributes.Append(group);
                    }
                    property.Attributes.Append(instance);
                    if (!string.IsNullOrWhiteSpace(key.TypeID))
                    {
                        XmlAttribute type = xml.CreateAttribute("typeid");
                        type.Value = key.TypeID;
                        property.Attributes.Append(type);
                    }
                    break;
                default:
                    property.InnerText = Value?.ToString() ?? string.Empty;
                    break;
            }

            return property;
        }
    }

    public enum PropertyType
    {
        // Boolean
        @bool = 0x00010000,

        // Integer
        int8 = 0x00050000,
        uint8 = 0x00060000,
        int16 = 0x00070000,
        uint16 = 0x00080000,
        int32 = 0x00090000,
        uint32 = 0x000A0000,
        int64 = 0x000B0000,
        uint64 = 0x000C0000,

        // Float
        @float = 0x000D0000,

        // String
        string8 = 0x00120000,
        string16 = 0x00130000,

        // ResourceKey
        key = 0x00200000,

        // Vector
        vector2 = 0x00300000,
        vector3 = 0x00310000,
        colorRGB = 0x00320000,
        vector4 = 0x00330000,
        colorRGBA = 0x00340000,

        // LocalizedString
        text = 0x00220000,

        // BoundingBox
        bbox = 0x00390000,

        // Transform
        transform = 00380000
    }
}
