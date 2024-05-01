using DBPF_Compiler.FNV;
using DBPF_Compiler.Types;
using System.Xml;

namespace DBPF_Compiler.FileTypes.Prop
{
    public struct Property
    {
        public string Name { get; set; }
        public PropertyType PropertyType { get; set; }
        public object Value { get; set; }

        public readonly XmlElement ToXml(XmlDocument xml)
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
                        instance.Value = Value.ToString();
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
                    property.InnerText = Value.ToString() ?? string.Empty;
                    break;
            }

            return property;
        }
    }

    public enum PropertyType
    {
        // Boolean
        @bool = 0x10000,

        // Integer
        int8 = 0x50000,
        uint8 = 0x60000,
        int16 = 0x70000,
        uint16 = 0x80000,
        int32 = 0x90000,
        uint32 = 0xA00,

        // Float
        @float = 0xD00,

        // String
        string8 = 0x1200,
        string16 = 0x1300,

        // ResourceKey
        key = 0x2000,

        // Vector
        vector2 = 0x3000,
        vector3 = 0x3100,
        colorRGB = 0x3200,
        vector4 = 0x3300,
        colorRGBA = 0x3400,

        // LocalizedString
        //text = 
    }
}
