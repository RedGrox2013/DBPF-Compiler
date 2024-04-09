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
                    size += sizeof(uint) * 4;

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
        public uint  NameLocale { get; set; }

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

        public void ReadFromStream(Stream stream)
        {
            throw new NotImplementedException();
        }

        public void WriteToStream(Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}
