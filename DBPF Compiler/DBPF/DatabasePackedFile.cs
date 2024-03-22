/*
 * Всю инфу брал от сюда:
 * https://web.archive.org/web/20090130111849/http://blog.spore.vg/file-formats/package/
 */

using System.Drawing;

namespace DBPF_Compiler.DBPF
{
    public class DatabasePackedFile : IDisposable
    {
        public delegate void PackingHandler(object? message);

        private PackingHandler? _onHeaderWriting;
        public event PackingHandler OnHeaderWriting
        {
            add => _onHeaderWriting += value;
            remove => _onHeaderWriting -= value;
        }
        private PackingHandler? _onDataWriting;
        public event PackingHandler OnDataWriting
        {
            add => _onDataWriting += value;
            remove => _onDataWriting -= value;
        }
        private PackingHandler? _onIndexWriting;
        public event PackingHandler OnIndexWriting
        {
            add => _onIndexWriting += value;
            remove => _onIndexWriting -= value;
        }

        private readonly FileStream _stream;
        private bool _disposed = false;
        private bool _headerWrited = false;
        private bool _indexWrited = false;

        public const uint HeaderSize = 96u;
        public readonly uint HeaderOffset;

        #region DBPF Header
        /// <summary>
        /// DBPF
        /// </summary>
        public const uint Magic = 0x46504244;
        /// <summary>
        /// Always 2 or 3
        /// </summary>
        public int MajorVersion
        {
            get => _majorVersion;
            set
            {
                if (value < 2)
                    _majorVersion = 2;
                else if (value > 3)
                    _majorVersion = 3;
                else
                    _majorVersion = value;
            }
        }
        private int _majorVersion = 3;
        /// <summary>
        /// Always 0
        /// </summary>
        public const int MinorVersion = 0;
        /// <summary>
        /// Number of index entries in the package.
        /// </summary>
        public int IndexCount => _index.Entries.Count;
        /// <summary>
        /// The total size in bytes of index entries.
        /// </summary>
        public int IndexSize { get; private set; } = 8;
        /// <summary>
        /// Observed as always being 3.
        /// </summary>
        public const int Unknown3C = 3;
        /// <summary>
        /// Absolute offset in package to the index header.
        /// </summary>
        public uint IndexOffset { get; private set; }
        #endregion

        private readonly DBPFIndex _index = new();

        public DatabasePackedFile(FileStream stream)
        {
            HeaderOffset = (uint)stream.Position;
            stream.Seek(HeaderSize, SeekOrigin.Current);
            IndexOffset = HeaderOffset + HeaderSize;
            _stream = stream;
        }

        public void WriteHeader()
        {
            if (_headerWrited)
                return;

            _stream.Position = HeaderOffset;
            _onHeaderWriting?.Invoke(null);

            _stream.WriteUInt32(Magic);
            _stream.WriteInt32(MajorVersion);
            _stream.WriteInt32(MinorVersion);
            _stream.Seek(sizeof(int) * 6, SeekOrigin.Current);  // Unknown values
            _stream.WriteInt32(IndexCount);
            _stream.Seek(sizeof(int), SeekOrigin.Current);      // Unknown values
            _stream.WriteInt32(IndexSize);
            _stream.Seek(12, SeekOrigin.Current);               // Unknown byte array
            _stream.WriteInt32(Unknown3C);
            _stream.WriteUInt32(IndexOffset);
            //_stream.Seek(sizeof(int) + 24, SeekOrigin.Current); // Unknown value + unknown byte array

            _stream.Position = IndexOffset;
            _headerWrited = true;
        }
        public async Task WriteHeaderAsync()
            => await Task.Run(WriteHeader);

        public void WriteData(byte[] data, uint instanceID, uint typeID, uint groupID)
        {
            _headerWrited = _indexWrited = false;

            uint size = (uint)data.LongLength;
            var entry = new IndexEntry
            {
                TypeID = typeID,
                InstanceID = instanceID,
                GroupID = groupID,
                Offset = IndexOffset,
                CompressedSize = size,
                UncompressedSize = size,
            };
            _index.Entries.Add(entry);
            _onDataWriting?.Invoke(entry);
            _stream.Write(data);

            IndexSize += IndexEntry.EntrySize;
            IndexOffset += size;
        }
        public async Task WriteDataAsync(byte[] data, uint instanceID, uint typeID, uint groupID)
            => await Task.Run(() => WriteData(data, instanceID, typeID, groupID));

        public void CopyFromStream(Stream stream, uint instanceID, uint typeID, uint groupID)
        {
            _headerWrited = _indexWrited = false;

            var entry = new IndexEntry
            {
                TypeID = typeID,
                InstanceID = instanceID,
                GroupID = groupID,
                Offset = IndexOffset,
                CompressedSize = (uint)stream.Length,
                UncompressedSize = (uint)stream.Length,
            };
            _index.Entries.Add(entry);
            _onDataWriting?.Invoke(entry);
            stream.CopyTo(_stream);

            IndexSize += IndexEntry.EntrySize;
            IndexOffset += (uint)stream.Length;
        }
        public async Task CopyFromStreamAsync(Stream stream, uint instanceID, uint typeID, uint groupID)
            => await Task.Run(() => CopyFromStream(stream, instanceID, typeID, groupID));

        public void WriteIndex()
        {
            if (_indexWrited)
                return;

            _onIndexWriting?.Invoke(IndexCount);
            _stream.WriteUInt32(_index.ValuesFlag);
            _stream.WriteUInt32(_index.TypeID);
            _stream.WriteUInt32(_index.GroupID);
            _stream.WriteUInt32(DBPFIndex.UnknownID);

            foreach (var entry in _index.Entries)
                _stream.WriteIndexEntry(entry);

            _indexWrited = true;
        }
        public void WriteIndexAsync()
            => Task.Run(WriteIndex);

        #region IDisposable realization
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            WriteIndex();
            WriteHeader();

            if (disposing)
                _stream.Dispose();
            _disposed = true;
        }
        #endregion

        ~DatabasePackedFile() => Dispose(false);
    }
}
