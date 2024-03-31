using DBPF_Compiler.Types;

namespace DBPF_Compiler.FileTypes
{
    public class PollenMetadata : ISporeFile
    {
        public uint TypeID => 0x030BDEE3;

        public int MetadataVersion { get; set; } = 13;
        public ulong AssetID { get; set; }
        public ResourceKey AssetKey { get; set; }
        public ResourceKey ParentAssetKey { get; set; } = new(0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF);
        public long ParentAssetID { get; set; } = 0xFFFFFFFF;
        public long OriginalParentAssetID { get; set; } = 0xFFFFFFFF;
        public ulong TimeCreated { get; set; } = 0;
        public ulong TimeDownloaded { get; set;} = 0;

        /*
         * Дальше идут 4 байта:
         * 0xFFFFFFFF - если есть локализации
         * 0 - если их нет
         */

        /// <summary>
        /// Имеется только если нет локализаций
        /// </summary>
        public long? AuthorID { get; set; } = 0xFFFFFFFF;

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
        public uint DescriptionLocale { get; set; }

        public bool Decode(byte[] data)
        {
            throw new NotImplementedException();
        }

        public byte[] Encode()
        {
            throw new NotImplementedException();
        }
    }
}
