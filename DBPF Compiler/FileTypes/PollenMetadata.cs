using DBPF_Compiler.Types;
using System.Text;
using System.Xml.Serialization;

namespace DBPF_Compiler.FileTypes
{
    [Serializable]
    public class PollenMetadata : ISporeFile
    {
        [XmlIgnore]
        public uint TypeID => 0x030BDEE3;

        [XmlIgnore]
        public uint DataSize
        {
            get
            {
                int size = sizeof(int) * 7 + sizeof(long) * 9 + sizeof(ulong) * 2;
                if (!HasLocale)
                    size += sizeof(long) + sizeof(int) +
                        Encoding.Unicode.GetByteCount(AuthorName ?? string.Empty) +
                        Encoding.Unicode.GetByteCount(Name ?? string.Empty) +
                        Encoding.Unicode.GetByteCount(Description ?? string.Empty);
                else
                    size += sizeof(uint) * 3;

                if (HasAuthors)
                    size += sizeof(int) + Encoding.ASCII.GetByteCount(Authors ?? string.Empty);
                if (HasTags)
                    size += sizeof(int) + Encoding.Unicode.GetByteCount(Tags ?? string.Empty);
                if (HasConsequenceTraits)
                    size += sizeof(uint);

                return (uint)size;
            }
        }

        /// <summary>
        /// Поддерживается пока что только 13 версия
        /// </summary>
        public int MetadataVersion { get; set; } = 13;
        public long AssetID { get; set; }
        public ResourceKey AssetKey { get; set; }
        public ResourceKey ParentAssetKey { get; set; } = new(0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF);
        public long ParentAssetID { get; set; } = 0xFFFFFFFF;
        public long OriginalParentAssetID { get; set; } = 0xFFFFFFFF;
        public ulong TimeCreated { get; set; } = 0;
        public ulong TimeDownloaded { get; set;} = 0;

        /// <summary>
        /// 4 байта:<br/>
        /// 0xFFFFFFFF - если есть локализации<br/>
        /// 0 - если их нет
        /// </summary>
        [XmlIgnore]
        public bool HasLocale => LocaleTableID != null;

        /// <summary>
        /// Имеется только если нет локализаций
        /// </summary>
        public long AuthorID { get; set; } = 0xFFFFFFFF;

        /// <summary>
        /// Имеется только если нет локализаций
        /// </summary>
        public string? AuthorName { get; set; }

        /// <summary>
        /// Имеется только если нет локализаций
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Имеется только если нет локализаций
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Имеется только если есть локализации
        /// </summary>
        public uint? LocaleTableID { get; set; }

        /// <summary>
        /// Имеется только если есть локализации
        /// </summary>
        public uint AuthorNameLocale { get; set; }

        /// <summary>
        /// Имеется только если есть локализации
        /// </summary>
        public uint DescriptionLocale { get; set; } = 0xFFFFFFFF;

        /// <summary>
        /// 4 байта
        /// </summary>
        [XmlIgnore]
        public bool HasAuthors => !string.IsNullOrEmpty(Authors);

        public string? Authors { get; set; }

        /// <summary>
        /// Хз что это такое, вроде равно нулю
        /// </summary>
        [XmlIgnore]
        public int UnknownValue { get; set; } = 0;

        /// <summary>
        /// 4 байта
        /// </summary>
        [XmlIgnore]
        public bool HasTags => !string.IsNullOrEmpty(Tags);

        public string? Tags { get; set; }

        /// <summary>
        /// 4 байта:<br/>
        /// 0xFFFFFFFF - если <c>true</c><br/>
        /// 0 - если <c>false</c>
        /// </summary>
        public bool IsShareable { get; set; } = true;

        /// <summary>
        /// 4 байта
        /// </summary>
        [XmlIgnore]
        public bool HasConsequenceTraits => ConsequenceTraits != null;

        public uint? ConsequenceTraits { get; set; }

        public bool Decode(byte[]? data)
        {
            if (data == null)
                return false;

            Array.Reverse(data);
            int offset = data.Length - sizeof(int);
            MetadataVersion = BitConverter.ToInt32(data, offset);
            if (MetadataVersion != 13)
                return false;

            offset -= sizeof(long);
            AssetID = BitConverter.ToInt64(data, offset);
            offset -= sizeof(uint);
            var typeID = BitConverter.ToUInt32(data, offset);
            offset -= sizeof(uint);
            var groupID = BitConverter.ToUInt32(data, offset);
            offset -= sizeof(uint);
            var instanceID = BitConverter.ToUInt32(data, offset);
            AssetKey = new(instanceID, typeID, groupID);
            offset -= sizeof(uint);
            typeID = BitConverter.ToUInt32(data, offset);
            offset -= sizeof(uint);
            groupID = BitConverter.ToUInt32(data, offset);
            offset -= sizeof(uint);
            instanceID = BitConverter.ToUInt32(data, offset);
            ParentAssetKey = new(instanceID, typeID, groupID);
            offset -= sizeof(long);
            ParentAssetID = BitConverter.ToInt64(data, offset);
            offset -= sizeof(long);
            OriginalParentAssetID = BitConverter.ToInt64(data, offset);
            offset -= sizeof(ulong);
            TimeCreated = BitConverter.ToUInt64(data, offset);
            offset -= sizeof(ulong);
            TimeDownloaded = BitConverter.ToUInt64(data, offset);

            offset -= sizeof(int);
            if (BitConverter.ToUInt32(data, offset) != 0xFFFFFFFF)
            {
                offset -= sizeof(long);
                AuthorID = BitConverter.ToInt64(data, offset);
                AuthorName = GetUnicodeString(data, ref offset);
                Name = GetUnicodeString(data, ref offset);
                Description = GetUnicodeString(data, ref offset);
            }
            else
            {
                offset -= sizeof(uint);
                LocaleTableID = BitConverter.ToUInt32(data, offset);
                offset -= sizeof(uint);
                AuthorNameLocale = BitConverter.ToUInt32(data, offset);
                offset -= sizeof(uint);
                DescriptionLocale = BitConverter.ToUInt32(data, offset);
            }

            offset -= sizeof(int);
            if (BitConverter.ToInt32(data, offset) == 1)
            {
                offset -= sizeof(int);
                int len = BitConverter.ToInt32(data, offset);
                offset -= len;
                Array.Reverse(data, offset, len);
                Authors = Encoding.ASCII.GetString(data, offset, len);
            }

            offset -= sizeof(int);
            UnknownValue = BitConverter.ToInt32(data, offset);
            offset -= sizeof(int);
            if (BitConverter.ToInt32(data, offset) == 1)
                Tags = GetUnicodeString(data, ref offset);
            offset -= sizeof(uint);
            IsShareable = BitConverter.ToUInt32(data, offset) == 0xFFFFFFFF;
            offset -= sizeof(int);
            if (BitConverter.ToInt32(data, offset) == 1)
            {
                offset -= sizeof(uint);
                ConsequenceTraits = BitConverter.ToUInt32(data, offset);
            }

            return true;
        }

        private static string GetUnicodeString(byte[] data, ref int offset)
        {
            offset -= sizeof(int);
            int len = BitConverter.ToInt32(data, offset);
            offset -= len;
            Array.Reverse(data, offset, len);

            return Encoding.Unicode.GetString(data, offset, len);
        }

        public byte[] Encode()
        {
            /*byte[] data = new byte[DataSize];
            int index = 0;
            if (HasConsequenceTraits)
            {
                Array.Copy(BitConverter.GetBytes(ConsequenceTraits ?? 0), data, sizeof(uint));
                index += sizeof(uint);
                Array.Copy(BitConverter.GetBytes(1), 0, data, index, sizeof(int));
            }
            else
                Array.Copy(BitConverter.GetBytes(0), data, sizeof(int));
            index += sizeof(int);
            for (int i = 0; i < sizeof(uint); i++)
                data[index + i] = IsShareable ? (byte)0xFF : (byte)0;
            index += sizeof(uint);
            if (HasTags)
            {
                var tags = Encoding.Unicode.GetBytes(Tags ?? string.Empty);
                Array.Reverse(tags);
                Array.Copy(tags, 0, data, index, tags.Length);
                index += tags.Length;
                Array.Copy(BitConverter.GetBytes(tags.Length), 0, data, index, sizeof(int));
                index += sizeof(int);
                Array.Copy(BitConverter.GetBytes(1), 0, data, index, sizeof(int));
            }
            else
                Array.Copy(BitConverter.GetBytes(0), 0, data, index, sizeof(int));
            index += sizeof(int);

            Array.Copy(BitConverter.GetBytes(UnknownValue), 0, data, index, sizeof(int));
            index += sizeof(int);
            if (HasAuthors)
            {
                var authorsData = Encoding.ASCII.GetBytes(Authors ?? string.Empty);
                Array.Reverse(authorsData);
                Array.Copy(authorsData, 0, data, index, authorsData.Length);
                index += authorsData.Length;
                Array.Copy(BitConverter.GetBytes(authorsData.Length), 0, data, index, sizeof(int));
                index += sizeof(int);
                Array.Copy(BitConverter.GetBytes(1), 0, data, index, sizeof(int));
            }
            else
                Array.Copy(BitConverter.GetBytes(0), 0, data, index, sizeof(int));
            index += sizeof(int);

            Array.Reverse(data);
            return data;*/

            throw new NotImplementedException();
        }
    }
}
