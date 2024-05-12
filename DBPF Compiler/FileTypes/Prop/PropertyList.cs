﻿using DBPF_Compiler.FNV;
using DBPF_Compiler.Types;
using System.Text;
using System.Text.Json.Serialization;

namespace DBPF_Compiler.FileTypes.Prop
{
    public class PropertyList : ISporeFile
    {
        [JsonIgnore]
        public TypeIDs TypeID => TypeIDs.prop;

        [JsonInclude]
        public List<Property> Properties
        {
            get => _properties;
            set => _properties = new List<Property>(value);
        }
        private List<Property> _properties;

        [JsonIgnore]
        private readonly NameRegistryManager _regManager = NameRegistryManager.Instance;

        public PropertyList() => _properties = [];
        public PropertyList(IEnumerable<Property> properties)
            => _properties = new List<Property>(properties);

        public bool Decode(Stream input)
        {
            int count = input.ReadInt32(true);
            for (int i = 0; i < count; i++)
            {
                Property property = new(_regManager.GetName(input.ReadUInt32(true), "property"))
                {
                    PropertyType = (PropertyType)input.ReadInt32(true),
                };
                switch (property.PropertyType)
                {
                    case PropertyType.@bool:
                        property.Value = input.ReadByte() == 1;
                        break;
                    case PropertyType.bools:
                        property.Value = input.ReadBoolArray(true);
                        break;
                    case PropertyType.int8:
                        property.Value = (sbyte)input.ReadByte();
                        break;
                    case PropertyType.int8s:
                        property.Value = input.ReadInt8Array(true);
                        break;
                    case PropertyType.uint8:
                        property.Value = (byte)input.ReadByte();
                        break;
                    case PropertyType.uint8s:
                        property.Value = input.ReadUInt8Array(true);
                        break;
                    case PropertyType.int16:
                        property.Value = input.ReadInt16(true);
                        break;
                    case PropertyType.int16s:
                        property.Value = input.ReadInt16Array(true);
                        break;
                    case PropertyType.uint16:
                        property.Value = input.ReadUInt16(true);
                        break;
                    case PropertyType.int32:
                        property.Value = input.ReadInt32(true);
                        break;
                    case PropertyType.int32s:
                        property.Value = input.ReadInt32Array(true);
                        break;
                    case PropertyType.uint32:
                        property.Value = GetUInt32Name(input.ReadUInt32(true));
                        break;
                    case PropertyType.uint32s:
                    case PropertyType.uint32 + 0x9C:
                        property.Value = input.ReadUInt32Array(true).Select(GetUInt32Name);
                        break;
                    case PropertyType.int64:
                        property.Value = input.ReadInt64(true);
                        break;
                    case PropertyType.uint64:
                        property.Value = input.ReadUInt64(true);
                        break;
                    case PropertyType.@float:
                        property.Value = input.ReadFloat(true);
                        break;
                    case PropertyType.floats:
                        property.Value = input.ReadFloatArray(true);
                        break;
                    case PropertyType.string8:
                        property.Value = input.ReadString8(true);
                        break;
                    case PropertyType.string8s:
                        property.Value = input.ReadString8Array(true);
                        break;
                    case PropertyType.string16:
                        property.Value = input.ReadString16(true);
                        break;
                    case PropertyType.string16s:
                        property.Value = input.ReadString16Array(true);
                        break;
                    case PropertyType.key:
                        property.Value = _regManager.GetStringResourceKey(input.ReadResourceKey());
                        break;
                    case PropertyType.keys:
                        property.Value = input.ReadResourceKeyArray(true).Select(_regManager.GetStringResourceKey);
                        break;
                    case PropertyType.vector2:
                        property.Value = new Vector2(input.ReadVector());
                        break;
                    case PropertyType.vector2s:
                        property.Value = input.ReadVector2Array(true);
                        break;
                    case PropertyType.vector3:
                        property.Value = new Vector3(input.ReadVector());
                        break;
                    case PropertyType.vector3s:
                        property.Value = input.ReadVector3Array(true);
                        break;
                    case PropertyType.colorRGB:
                        property.Value = new ColorRGB(input.ReadVector());
                        break;
                    case PropertyType.colorRGBs:
                        property.Value = input.ReadVector3Array(true).Select(v => new ColorRGB(v));
                        break;
                    case PropertyType.vector4:
                        property.Value = input.ReadVector();
                        break;
                    case PropertyType.vector4s:
                        property.Value = input.ReadVector4Array(true);
                        break;
                    case PropertyType.colorRGBA:
                        property.Value = new ColorRGBA(input.ReadVector());
                        break;
                    case PropertyType.colorRGBAs:
                        property.Value = input.ReadVector4Array(true).Select(v => new ColorRGBA(v));
                        break;
                    case PropertyType.texts:
                    case PropertyType.text + 0x1C:
                        property.Value = input.ReadLocalizedStringArray(true).Select(t => new StringLocalizedString(
                            _regManager.GetName(t.TableID, "file"),
                            FNVHash.ToString(t.InstanceID), t.PlaceholderText));
                        break;
                    case PropertyType.bboxes:
                        property.Value = input.ReadBBoxArray(true);
                        break;
                    case PropertyType.transforms:
                        property.Value = input.ReadTransformArray(true);
                        break;
                    default:
                        break;
                }

                Add(property);
            }

            return true;
        }

        public uint Encode(Stream output)
        {
            throw new NotImplementedException();
        }

        private string GetUInt32Name(uint value)
        {
            var regFile = _regManager.GetRegistry("file");
            if (regFile != null && regFile.GetName(value, out string name))
                return $"hash({name})";

            return FNVHash.ToString(value);
        }

        public object? GetValue(string propertyName, PropertyType type)
            => _properties.Find(p
                => p.Name.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase) && p.PropertyType == type)?.Value;

        public void Add(Property property) => _properties.Add(property);

        public override string ToString()
        {
            StringBuilder sb = new();
            foreach (var prop in _properties)
            {
                sb.Append(prop);
                sb.Append('\n');
            }

            return sb.ToString();
        }
    }
}
