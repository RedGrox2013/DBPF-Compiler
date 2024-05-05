﻿using DBPF_Compiler.FNV;
using DBPF_Compiler.Types;
using System.Text.Json.Serialization;
using System.Xml;

namespace DBPF_Compiler.FileTypes.Prop
{
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
        bools = 0x00010030,

        // Integer
        int8 = 0x00050000,
        int8s = 0x00050030,
        uint8 = 0x00060000,
        uint8s = 0x00060030,
        int16 = 0x00070000,
        int16s = 0x00070030,
        uint16 = 0x00080000,
        uint16s = 0x00080030,
        int32 = 0x00090000,
        int32s = 0x00090030,
        uint32 = 0x000A0000,
        uint32s = 0x000A0030,
        int64 = 0x000B0000,
        int64s = 0x000B0030,
        uint64 = 0x000C0000,
        uint64s = 0x000C0030,

        // Float
        @float = 0x000D0000,
        floats = 0x000D0030,

        // String
        string8 = 0x00120000,
        string8s = 0x00120030,
        string16 = 0x00130000,
        string16s = 0x00130030,

        // ResourceKey
        key = 0x00200000,
        keys = 0x00200030,

        // Vector
        vector2 = 0x00300000,
        vector2s = 0x00300030,
        vector3 = 0x00310000,
        vector3s = 0x00310030,
        colorRGB = 0x00320000,
        colorRGBs = 0x00320030,
        vector4 = 0x00330000,
        vector4s = 0x00330030,
        colorRGBA = 0x00340000,
        colorRGBAs = 0x00340030,

        // LocalizedString
        text = 0x00220000,
        texts = 0x00220030,

        // BoundingBox
        bbox = 0x00390000,
        bboxes = 0x00390030,
        bboxs = 0x00390030,

        // Transform
        transform = 00380000,
        transforms = 00380030
    }
}
