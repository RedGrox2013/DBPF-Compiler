using DBPF_Compiler.Types;
using System.Numerics;
using System.Text;

namespace DBPF_Compiler.FileTypes
{
    public class PollenMetadata : ISporeFile
    {
        public uint TypeID => 0x030BDEE3;

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
        public bool HasLocale { get; set; } = false;

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
        public uint LocaleTableID { get; set; }

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
        public bool HasAuthors => !string.IsNullOrEmpty(AuthorName);

        public string? Authors { get; set; }

        /// <summary>
        /// Хз что это такое, вроде равно нулю
        /// </summary>
        public int UnknownValue { get; set; } = 0;

        /// <summary>
        /// 4 байта
        /// </summary>
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
        public bool HasConsequenceTraits { get; set; } = false;

        public uint ConsequenceTraits { get; set; }

        public bool Decode(byte[] data)
        {
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
            HasLocale = BitConverter.ToUInt32(data, offset) == 0xFFFFFFFF;

            if (!HasLocale)
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
                // Я устал(
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
            throw new NotImplementedException();
        }
    }
}
