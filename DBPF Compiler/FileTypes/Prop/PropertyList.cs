using DBPF_Compiler.FNV;
using DBPF_Compiler.Types;
using System.Text;
using System.Text.Json;
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
        private static readonly NameRegistryManager _regManager = NameRegistryManager.Instance;

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
                    case PropertyType.@bool + 0x9C:
                    case PropertyType.@bool + 0x1C:
                        property.PropertyType = PropertyType.bools;
                        property.Value = input.ReadBoolArray(true);
                        break;
                    case PropertyType.int8:
                        property.Value = (sbyte)input.ReadByte();
                        break;
                    case PropertyType.int8s:
                    case PropertyType.int8 + 0x9C:
                    case PropertyType.int8 + 0x1C:
                        property.PropertyType = PropertyType.int8s;
                        property.Value = input.ReadInt8Array(true);
                        break;
                    case PropertyType.uint8:
                        property.Value = (byte)input.ReadByte();
                        break;
                    case PropertyType.uint8s:
                    case PropertyType.uint8 + 0x9C:
                    case PropertyType.uint8 + 0x1C:
                        property.PropertyType = PropertyType.uint8s;
                        property.Value = input.ReadUInt8Array(true);
                        break;
                    case PropertyType.int16:
                        property.Value = input.ReadInt16(true);
                        break;
                    case PropertyType.int16s:
                    case PropertyType.int16 + 0x9C:
                    case PropertyType.int16 + 0x1C:
                        property.PropertyType = PropertyType.int16s;
                        property.Value = input.ReadInt16Array(true);
                        break;
                    case PropertyType.uint16:
                        property.Value = input.ReadUInt16(true);
                        break;
                    case PropertyType.int32:
                        property.Value = input.ReadInt32(true);
                        break;
                    case PropertyType.int32s:
                    case PropertyType.int32 + 0x9C:
                    case PropertyType.int32 + 0x1C:
                        property.PropertyType = PropertyType.int32s;
                        property.Value = input.ReadInt32Array(true);
                        break;
                    case PropertyType.uint32:
                        property.Value = GetUInt32Name(input.ReadUInt32(true));
                        break;
                    case PropertyType.uint32s:
                    case PropertyType.uint32 + 0x9C:
                    case PropertyType.uint32 + 0x1C:
                        property.PropertyType = PropertyType.uint32s;
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
                    case PropertyType.@float + 0x9C:
                    case PropertyType.@float + 0x1C:
                        property.PropertyType = PropertyType.floats;
                        property.Value = input.ReadFloatArray(true);
                        break;
                    case PropertyType.string8:
                    case PropertyType.string8 + 0xD:
                        property.PropertyType = PropertyType.string8;
                        property.Value = input.ReadString8(true);
                        break;
                    case PropertyType.string8s:
                    case PropertyType.string8 + 0x9C:
                    case PropertyType.string8 + 0x1C:
                        property.PropertyType = PropertyType.string8s;
                        property.Value = input.ReadString8Array(true);
                        break;
                    case PropertyType.string16:
                    case PropertyType.string16 + 0xD:
                        property.PropertyType = PropertyType.string16;
                        property.Value = input.ReadString16(true);
                        break;
                    case PropertyType.string16s:
                    case PropertyType.string16 + 0x9C:
                    case PropertyType.string16 + 0x1C:
                        property.PropertyType = PropertyType.string16s;
                        property.Value = input.ReadString16Array(true);
                        break;
                    case PropertyType.key:
                        property.Value = _regManager.GetStringResourceKey(input.ReadResourceKey());
                        break;
                    case PropertyType.keys:
                    case PropertyType.key + 0x9C:
                    case PropertyType.key + 0x1C:
                        property.PropertyType = PropertyType.keys;
                        property.Value = input.ReadResourceKeyArray(true).Select(_regManager.GetStringResourceKey);
                        break;
                    case PropertyType.vector2:
                        property.Value = new Vector2(input.ReadVector());
                        break;
                    case PropertyType.vector2s:
                    case PropertyType.vector2 + 0x9C:
                    case PropertyType.vector2 + 0x1C:
                        property.PropertyType = PropertyType.vector2s;
                        property.Value = input.ReadVector2Array(true);
                        break;
                    case PropertyType.vector3:
                        property.Value = new Vector3(input.ReadVector());
                        break;
                    case PropertyType.vector3s:
                    case PropertyType.vector3 + 0x9C:
                    case PropertyType.vector3 + 0x1C:
                        property.PropertyType = PropertyType.vector3s;
                        property.Value = input.ReadVector3Array(true);
                        break;
                    case PropertyType.colorRGB:
                        property.Value = new ColorRGB(input.ReadVector());
                        break;
                    case PropertyType.colorRGBs:
                    case PropertyType.colorRGB + 0x9C:
                    case PropertyType.colorRGB + 0x1C:
                        property.PropertyType = PropertyType.colorRGBs;
                        property.Value = input.ReadVector3Array(true).Select(v => new ColorRGB(v));
                        break;
                    case PropertyType.vector4:
                        property.Value = input.ReadVector();
                        break;
                    case PropertyType.vector4s:
                    case PropertyType.vector4 + 0x9C:
                    case PropertyType.vector4 + 0x1C:
                        property.PropertyType = PropertyType.vector4s;
                        property.Value = input.ReadVector4Array(true);
                        break;
                    case PropertyType.colorRGBA:
                        property.Value = new ColorRGBA(input.ReadVector());
                        break;
                    case PropertyType.colorRGBAs:
                    case PropertyType.colorRGBA + 0x9C:
                    case PropertyType.colorRGBA + 0x1C:
                        property.PropertyType = PropertyType.colorRGBAs;
                        property.Value = input.ReadVector4Array(true).Select(v => new ColorRGBA(v));
                        break;
                    case PropertyType.texts:
                    case PropertyType.text + 0x9C:
                    case PropertyType.text + 0x1C:
                        property.PropertyType = PropertyType.texts;
                        property.Value = input.ReadLocalizedStringArray(true).Select(t => new StringLocalizedString(
                            _regManager.GetName(t.TableID, "file"),
                            FNVHash.ToString(t.InstanceID), t.PlaceholderText));
                        break;
                    case PropertyType.bboxes:
                    case PropertyType.bbox + 0x9C:
                    case PropertyType.bbox + 0x1C:
                        property.PropertyType = PropertyType.bboxs;
                        property.Value = input.ReadBBoxArray(true);
                        break;
                    case PropertyType.transforms:
                    case PropertyType.transform + 0x9C:
                    case PropertyType.transform + 0x1C:
                        property.PropertyType = PropertyType.transforms;
                        property.Value = input.ReadTransformArray(true);
                        break;
                    default:
                        break;
                }

                Add(property);
            }

            return true;
        }

        public void Encode(Stream output)
        {
            _properties.Sort();
            output.WriteInt32(_properties.Count, true);

            // Переписать это говно (но мне будет лень)
            foreach (var p in _properties)
            {
                output.WriteUInt32(p.Id, true);
                output.WriteInt32((int)p.PropertyType, true);
                switch (p.PropertyType)
                {
                    case PropertyType.@bool:
                        output.WriteBoolean((bool)(p.Value ?? false));
                        break;
                    case PropertyType.bools:
                        {
                            var arr = p.Value as IEnumerable<bool>;
                            var count = arr?.Count() ?? 0;
                            output.WriteInt32(count, true);
                            output.WriteInt32(sizeof(bool), true);
                            if (arr != null)
                                foreach (var i in arr)
                                    output.WriteBoolean(i);
                        }
                        break;
                    case PropertyType.int8s:
                        {
                            var arr = p.Value as IEnumerable<byte>;
                            var count = arr?.Count() ?? 0;
                            output.WriteInt32(count, true);
                            output.WriteInt32(sizeof(byte), true);
                            if (arr != null)
                                foreach (var i in arr)
                                    output.WriteByte(i);
                        }
                        break;
                    case PropertyType.uint8:
                    case PropertyType.int8:
                        output.WriteByte((byte)(p.Value ?? 0));
                        break;
                    case PropertyType.uint8s:
                        {
                            var arr = p.Value as IEnumerable<sbyte>;
                            var count = arr?.Count() ?? 0;
                            output.WriteInt32(count, true);
                            output.WriteInt32(sizeof(sbyte), true);
                            if (arr != null)
                                foreach (var i in arr)
                                    output.WriteByte((byte)i);
                        }
                        break;
                    case PropertyType.int16s:
                        {
                            var arr = p.Value as IEnumerable<short>;
                            var count = arr?.Count() ?? 0;
                            output.WriteInt32(count, true);
                            output.WriteInt32(sizeof(short), true);
                            if (arr != null)
                                foreach (var i in arr)
                                    output.WriteInt16(i, true);
                        }
                        break;
                    case PropertyType.uint16:
                    case PropertyType.int16:
                        output.WriteUInt16((ushort)(p.Value ?? 0), true);
                        break;
                    case PropertyType.uint16s:
                        {
                            var arr = p.Value as IEnumerable<ushort>;
                            var count = arr?.Count() ?? 0;
                            output.WriteInt32(count, true);
                            output.WriteInt32(sizeof(ushort), true);
                            if (arr != null)
                                foreach (var i in arr)
                                    output.WriteUInt16(i, true);
                        }
                        break;
                    case PropertyType.int32:
                        output.WriteInt32((int)(p.Value ?? 0), true);
                        break;
                    case PropertyType.int32s:
                        {
                            var arr = p.Value as IEnumerable<int>;
                            var count = arr?.Count() ?? 0;
                            output.WriteInt32(count, true);
                            output.WriteInt32(sizeof(int), true);
                            if (arr != null)
                                foreach (var i in arr)
                                    output.WriteInt32(i, true);
                        }
                        break;
                    case PropertyType.uint32:
                        {
                            uint? value = p.Value as uint?;
                            if (value != null)
                                output.WriteUInt32((uint)value, true);
                            else
                                output.WriteUInt32(ParseUInt32Name(p.Value?.ToString()), true);
                        }
                        break;
                    case PropertyType.uint32s:
                        {
                            if (p.Value is IEnumerable<uint> uintArr)
                            {
                                var count = uintArr.Count();
                                output.WriteInt32(count, true);
                                output.WriteInt32(sizeof(uint), true);
                                foreach (var i in uintArr)
                                    output.WriteUInt32(i, true);
                            }
                            else
                            {
                                var arr = p.Value as IEnumerable<string>;
                                var count = arr?.Count() ?? 0;
                                output.WriteInt32(count, true);
                                output.WriteInt32(sizeof(uint), true);
                                if (arr != null)
                                    foreach (var i in arr)
                                        output.WriteUInt32(ParseUInt32Name(i), true);
                            }
                        }
                        break;
                    //case PropertyType.int64:
                    //    break;
                    //case PropertyType.int64s:
                    //    break;
                    //case PropertyType.uint64:
                    //    break;
                    //case PropertyType.uint64s:
                    //    break;
                    case PropertyType.@float:
                        output.WriteFloat((float)(p.Value ?? 0), true);
                        break;
                    case PropertyType.floats:
                        {
                            var arr = p.Value as IEnumerable<float>;
                            var count = arr?.Count() ?? 0;
                            output.WriteInt32(count, true);
                            output.WriteInt32(sizeof(float), true);
                            if (arr != null)
                                foreach (var i in arr)
                                    output.WriteFloat(i, true);
                        }
                        break;
                    case PropertyType.string8:
                        output.WriteString((string?)p.Value, Encoding.ASCII, true);
                        break;
                    case PropertyType.string8s:
                    case PropertyType.string16s:
                        {
                            var arr = p.Value as IEnumerable<string>;
                            var count = arr?.Count() ?? 0;
                            output.WriteInt32(count, true);
                            output.Write([0, 0, 0, 10]);
                            Encoding encoding = p.PropertyType == PropertyType.string8 ? Encoding.ASCII : Encoding.Unicode;
                            if (arr != null)
                                foreach (var i in arr)
                                    output.WriteString(i, encoding, true);
                        }
                        break;
                    case PropertyType.string16:
                        output.WriteString((string?)p.Value, Encoding.Unicode, true);
                        break;
                    case PropertyType.key:
                        output.WriteResourceKey(GetResourceKey(p.Value));
                        break;
                    case PropertyType.keys:
                        {
                            var arr = p.Value as IEnumerable<object>;
                            var count = arr?.Count() ?? 0;
                            output.WriteInt32(count, true);
                            output.WriteInt32(sizeof(uint) * 3, true);
                            if (arr != null)
                                foreach (var i in arr)
                                    output.WriteResourceKey(GetResourceKey(i));
                        }
                        break;
                    case PropertyType.vector2s:
                        {
                            var arr = p.Value as IEnumerable<Vector2>;
                            var count = arr?.Count() ?? 0;
                            output.WriteInt32(count, true);
                            output.WriteInt32(sizeof(uint) * 2, true);
                            if (arr != null)
                                foreach (var i in arr)
                                {
                                    output.WriteFloat(i.X);
                                    output.WriteFloat(i.Y);
                                }
                        }
                        break;
                    case PropertyType.vector3s:
                        {
                            var arr = p.Value as IEnumerable<Vector3>;
                            var count = arr?.Count() ?? 0;
                            output.WriteInt32(count, true);
                            output.WriteInt32(sizeof(uint) * 3, true);
                            if (arr != null)
                                foreach (var i in arr)
                                {
                                    output.WriteFloat(i.X);
                                    output.WriteFloat(i.Y);
                                    output.WriteFloat(i.Z);
                                }
                        }
                        break;
                    case PropertyType.colorRGBs:
                        {
                            var arr = p.Value as IEnumerable<ColorRGB>;
                            var count = arr?.Count() ?? 0;
                            output.WriteInt32(count, true);
                            output.WriteInt32(sizeof(uint) * 3, true);
                            if (arr != null)
                                foreach (var i in arr)
                                {
                                    output.WriteFloat(i.R);
                                    output.WriteFloat(i.G);
                                    output.WriteFloat(i.B);
                                }
                        }
                        break;
                    case PropertyType.vector2:
                        output.WriteVector(p.Value as Vector2? ?? new());
                        break;
                    case PropertyType.vector3:
                        output.WriteVector(p.Value as Vector3? ?? new());
                        break;
                    case PropertyType.colorRGB:
                        output.WriteVector(p.Value as ColorRGB? ?? new());
                        break;
                    case PropertyType.vector4:
                        output.WriteVector(p.Value as Vector4? ?? new());
                        break;
                    case PropertyType.colorRGBA:
                        output.WriteVector(p.Value as ColorRGBA? ?? new());
                        break;
                    case PropertyType.vector4s:
                        {
                            var arr = p.Value as IEnumerable<Vector4>;
                            var count = arr?.Count() ?? 0;
                            output.WriteInt32(count, true);
                            output.WriteInt32(sizeof(uint) * 4, true);
                            if (arr != null)
                                foreach (var i in arr)
                                    output.WriteVector(i);
                        }
                        break;
                    case PropertyType.colorRGBAs:
                        {
                            var arr = p.Value as IEnumerable<ColorRGBA>;
                            var count = arr?.Count() ?? 0;
                            output.WriteInt32(count, true);
                            output.WriteInt32(sizeof(uint) * 4, true);
                            if (arr != null)
                                foreach (var i in arr)
                                {
                                    output.WriteFloat(i.R);
                                    output.WriteFloat(i.G);
                                    output.WriteFloat(i.B);
                                    output.WriteFloat(i.A);
                                }
                        }
                        break;
                    //case PropertyType.text:
                    //    break;
                    case PropertyType.texts:
                        {
                            var arr = p.Value as IEnumerable<StringLocalizedString>;
                            var count = arr?.Count() ?? 0;
                            output.WriteInt32(count, true);
                            output.WriteInt32(sizeof(uint) * 2 + LocalizedString.PLACEHOLDER_SIZE, true);
                            if (arr != null)
                                foreach (var i in arr)
                                    output.WriteLocalizedString(new(
                                        i.TableID != null ? _regManager.GetHash(i.TableID, "file") : 0,
                                        i.InstanceID != null ? _regManager.GetHash(i.InstanceID) : 0,
                                        i.PlaceholderText));
                        }
                        break;
                    //case PropertyType.bbox:
                    //    break;
                    case PropertyType.bboxes:
                        {
                            var arr = p.Value as IEnumerable<BoundingBox>;
                            var count = arr?.Count() ?? 0;
                            output.WriteInt32(count, true);
                            output.WriteInt32(sizeof(float) * 6, true);
                            if (arr != null)
                                foreach (var i in arr)
                                    output.WriteBBox(i);
                        }
                        break;
                    //case PropertyType.transform:
                    //    break;
                    case PropertyType.transforms:
                        {
                            var arr = p.Value as IEnumerable<Transform>;
                            var count = arr?.Count() ?? 0;
                            output.WriteInt32(count, true);
                            output.WriteInt32(Transform.SIZE, true);
                            if (arr != null)
                                foreach (var i in arr)
                                    output.WriteTransform(i);
                        }
                        break;
                    default:
                        throw new NotSupportedException(p.PropertyType + " not supported");
                }
            }
        }

        public string SerializeToJson(JsonSerializerOptions? options = null)
            => JsonSerializer.Serialize(this, options);

        public bool DeserializeFromJson(string json, JsonSerializerOptions? options = null)
        {
            var prop = JsonSerializer.Deserialize<PropertyList>(json, options);
            if (prop == null)
                return false;

            _properties = prop._properties;
            for (int i = 0; i < _properties.Count; i++)
            {
                if (_properties[i].Value is not JsonElement element)
                    continue;

                switch (_properties[i].PropertyType)
                {
                    case PropertyType.@bool:
                        _properties[i].Value = element.Deserialize<bool>(options);
                        break;
                    case PropertyType.int32:
                        _properties[i].Value = element.Deserialize<int>(options);
                        break;
                    case PropertyType.uint32:
                    case PropertyType.string8:
                    case PropertyType.string16:
                        _properties[i].Value = element.Deserialize<string>(options);
                        break;
                    case PropertyType.int8:
                        _properties[i].Value = element.Deserialize<sbyte>(options);
                        break;
                    case PropertyType.uint8:
                        _properties[i].Value = element.Deserialize<byte>(options);
                        break;
                    case PropertyType.@float:
                        _properties[i].Value = element.Deserialize<float>(options);
                        break;
                    case PropertyType.bboxes:
                        _properties[i].Value = element.Deserialize<IEnumerable<BoundingBox>>(options);
                        break;
                    case PropertyType.transforms:
                        _properties[i].Value = element.Deserialize<IEnumerable<Transform>>(options);
                        break;
                    case PropertyType.bools:
                        _properties[i].Value = element.Deserialize<IEnumerable<bool>>(options);
                        break;
                    case PropertyType.int8s:
                        _properties[i].Value = element.Deserialize<IEnumerable<sbyte>>(options);
                        break;
                    case PropertyType.uint8s:
                        _properties[i].Value = element.Deserialize<IEnumerable<byte>>(options);
                        break;
                    case PropertyType.int16s:
                        _properties[i].Value = element.Deserialize<IEnumerable<short>>(options);
                        break;
                    case PropertyType.uint16s:
                        _properties[i].Value = element.Deserialize<IEnumerable<ushort>>(options);
                        break;
                    case PropertyType.int32s:
                        _properties[i].Value = element.Deserialize<IEnumerable<int>>(options);
                        break;
                    case PropertyType.int64s:
                        _properties[i].Value = element.Deserialize<IEnumerable<long>>(options);
                        break;
                    case PropertyType.uint64s:
                        _properties[i].Value = element.Deserialize<IEnumerable<ulong>>(options);
                        break;
                    case PropertyType.floats:
                        _properties[i].Value = element.Deserialize<IEnumerable<float>>(options);
                        break;
                    case PropertyType.string8s:
                    case PropertyType.string16s:
                    case PropertyType.uint32s:
                        _properties[i].Value = element.Deserialize<IEnumerable<string>>(options);
                        break;
                    case PropertyType.key:
                        _properties[i].Value = element.Deserialize<StringResourceKey>(options);
                        break;
                    case PropertyType.keys:
                        _properties[i].Value = element.Deserialize<IEnumerable<StringResourceKey>>(options);
                        break;
                    case PropertyType.vector2:
                        _properties[i].Value = element.Deserialize<Vector2>(options);
                        break;
                    case PropertyType.vector2s:
                        _properties[i].Value = element.Deserialize<IEnumerable<Vector2>>(options);
                        break;
                    case PropertyType.vector3:
                        _properties[i].Value = element.Deserialize<Vector3>(options);
                        break;
                    case PropertyType.vector3s:
                        _properties[i].Value = element.Deserialize<IEnumerable<Vector3>>(options);
                        break;
                    case PropertyType.colorRGB:
                        _properties[i].Value = element.Deserialize<ColorRGB>(options);
                        break;
                    case PropertyType.colorRGBs:
                        _properties[i].Value = element.Deserialize<IEnumerable<ColorRGB>>(options);
                        break;
                    case PropertyType.vector4:
                        _properties[i].Value = element.Deserialize<Vector4>(options);
                        break;
                    case PropertyType.vector4s:
                        _properties[i].Value = element.Deserialize<IEnumerable<Vector4>>(options);
                        break;
                    case PropertyType.colorRGBA:
                        _properties[i].Value = element.Deserialize<ColorRGBA>(options);
                        break;
                    case PropertyType.colorRGBAs:
                        _properties[i].Value = element.Deserialize<IEnumerable<ColorRGBA>>(options);
                        break;
                    case PropertyType.texts:
                        _properties[i].Value = element.Deserialize<IEnumerable<StringLocalizedString>>(options);
                        break;
                }
            }

            return true;
        }

        private string GetUInt32Name(uint value)
        {
            var regFile = _regManager.GetRegistry("file");
            if (regFile != null && regFile.GetName(value, out string name))
                return $"hash({name})";

            return FNVHash.ToString(value);
        }

        private static uint ParseUInt32Name(string? name)
        {
            if (FNVHash.TryParse(name, out uint hash))
                return hash;

            name = name?.TrimEnd(')');
            if (name != null && (name.StartsWith("hash(") || name.StartsWith('$')))
                return _regManager.GetHash(name.Replace("hash(", null).Replace("$", null), "file");

            return 0;
        }

        private static ResourceKey GetResourceKey(object? obj)
        {
            var key = obj as ResourceKey?;
            if (key == null)
            {
                StringResourceKey stringKey = (obj as StringResourceKey?) ?? new();
                key = _regManager.GetResourceKey(stringKey);
            }

            return (ResourceKey)key;
        }

        public object? GetValue(string propertyName, PropertyType type)
            => _properties.Find(p
                => p.Name.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase) && p.PropertyType == type)?.Value;

        public void Add(Property property) => _properties.Add(property);
    }
}
