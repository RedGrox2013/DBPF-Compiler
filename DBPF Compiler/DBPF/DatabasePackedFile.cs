/*
 * Всю инфу брал от сюда:
 * https://web.archive.org/web/20090130111849/http://blog.spore.vg/file-formats/package/
 */

using DBPF_Compiler.FileTypes;
using DBPF_Compiler.Types;
using System.Text;

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
        private PackingHandler? _onHeaderReading;
        public event PackingHandler OnHeaderReading
        {
            add => _onHeaderReading += value;
            remove => _onHeaderReading -= value;
        }
        private PackingHandler? _onDataReading;
        public event PackingHandler OnDataReading
        {
            add => _onDataReading += value;
            remove => _onDataReading -= value;
        }
        private PackingHandler? _onIndexReading;
        public event PackingHandler OnIndexReading
        {
            add => _onIndexReading += value;
            remove => _onIndexReading -= value;
        }

        private readonly Stream _stream;
        private bool _disposed = false;
        private bool _headerWritten = false;
        private bool _indexWritten = false;

        public const uint HeaderSize = 96u;

        private const uint COMPRESSED_OR = 0x80000000;

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
        public int IndexSize { get; private set; }
        /// <summary>
        /// Observed as always being 3.
        /// </summary>
        public const int Unknown3C = 3;
        /// <summary>
        /// Absolute offset in package to the index header.
        /// </summary>
        public uint IndexOffset { get; private set; }

        ////////// Secret data //////////

        public uint SecretIndexOffset => (uint)(IndexOffset + IndexSize);
        #endregion

        private DBPFIndex _index = new();

        private SecretDBPFIndex? _secretIndex = null;

        public string? SecretGroupName
        {
            get => _secretIndex?.GroupName;
            set
            {
                if (_secretIndex != null && !string.IsNullOrWhiteSpace(value))
                    _secretIndex.GroupName = value;
            }
        }

        public DatabasePackedFile(Stream stream)
        {
            stream.Seek(HeaderSize, SeekOrigin.Current);
            IndexOffset = HeaderSize;
            IndexSize = _index.SizeWithoutEntries;
            _stream = stream;
        }

        public void WriteHeader()
        {
            if (_headerWritten)
                return;

            _stream.Position = 0;
            Task.Run(() => _onHeaderWriting?.Invoke(null));

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
            _headerWritten = true;
        }
        public async Task WriteHeaderAsync()
            => await Task.Run(WriteHeader);

        public void WriteData(byte[] data, ResourceKey key)
        {
            Task.Run(() => _onDataWriting?.Invoke(key));
            _headerWritten = _indexWritten = false;

            uint size = (uint)data.LongLength;
            var entry = new IndexEntry
            {
                TypeID = key.TypeID,
                InstanceID = key.InstanceID,
                GroupID = key.GroupID,
                Offset = IndexOffset,
                CompressedSize = size | COMPRESSED_OR,
                UncompressedSize = size,
            };
            _index.Entries.Add(entry);
            _stream.Write(data);

            IndexSize += entry.EntrySize;
            IndexOffset += size;
        }
        public async Task WriteDataAsync(byte[] data, ResourceKey key)
            => await Task.Run(() => WriteData(data, key));

        public void WriteSporeFile(ISporeFile file, ResourceKey key)
        {
            Task.Run(() => _onDataWriting?.Invoke(key));
            _headerWritten = _indexWritten = false;

            uint size = file.Encode(_stream);
            var entry = new IndexEntry
            {
                TypeID = key.TypeID,
                InstanceID = key.InstanceID,
                GroupID = key.GroupID,
                Offset = IndexOffset,
                CompressedSize = size | COMPRESSED_OR,
                UncompressedSize = size,
            };
            _index.Entries.Add(entry);
            IndexSize += entry.EntrySize;
            IndexOffset += size;
        }
        public async Task WriteSporeFileAsync(ISporeFile file, ResourceKey key)
            => await Task.Run(() => WriteSporeFile(file, key));

        public void CopyFromStream(Stream stream, ResourceKey key)
        {
            Task.Run(() => _onDataWriting?.Invoke(key));
            _headerWritten = _indexWritten = false;

            uint size = (uint)(stream.Length - stream.Position);
            var entry = new IndexEntry
            {
                TypeID = key.TypeID,
                InstanceID = key.InstanceID,
                GroupID = key.GroupID,
                Offset = IndexOffset,
                CompressedSize = size | COMPRESSED_OR,
                UncompressedSize = size,
            };
            _index.Entries.Add(entry);
            stream.CopyTo(_stream);

            IndexSize += entry.EntrySize;
            IndexOffset += size;
        }
        public async Task CopyFromStreamAsync(Stream stream, ResourceKey key)
            => await Task.Run(() => CopyFromStream(stream, key));

        public void WriteIndex()
        {
            if (_indexWritten)
                return;

            Task.Run(() => _onIndexWriting?.Invoke(IndexCount));
            _stream.WriteUInt32(_index.ValuesFlag);
            if (_index.GetFlagAt(0))
                _stream.WriteUInt32(_index.TypeID);
            if (_index.GetFlagAt(1))
                _stream.WriteUInt32(_index.GroupID);
            if (_index.GetFlagAt(2))
                _stream.WriteUInt32(_index.UnknownID);

            foreach (var entry in _index.Entries)
                _stream.WriteIndexEntry(entry);

            _indexWritten = true;
        }
        public async Task WriteIndexAsync()
            => await Task.Run(WriteIndex);

        public void WriteSecretIndex()
        {
            if (_secretIndex == null)
                return;

            if (!_indexWritten)
                WriteIndex();

            _stream.Position = SecretIndexOffset;
            using BinaryWriter writer = new(_stream, Encoding.Unicode, true);
            writer.Write(_secretIndex.GroupName);
            writer.Write(_secretIndex.IndexCount);

            foreach (var entry in _secretIndex.Entries)
            {
                writer.Write(entry.Key.InstanceID);
                writer.Write(entry.Key.TypeID ?? string.Empty);
                writer.Write(entry.Offset);
                writer.Write(entry.Size);
            }
        }
        public async Task WriteSecretIndexAsync()
            => await Task.Run(WriteSecretIndex);

        public void WriteSecretData(byte[] data, StringResourceKey key)
        {
            _secretIndex ??= new();
            Task.Run(() => _onDataWriting?.Invoke(key));
            _headerWritten = _indexWritten = false;

            var entry = new SecretIndexEntry
            {
                Key = key,
                Offset = IndexOffset,
                Size = (uint)data.LongLength
            };
            _secretIndex.Entries.Add(entry);
            _stream.Write(data);

            IndexOffset += entry.Size;
        }

        public ResourceKey[] ReadDBPFInfo()
        {
            Task.Run(() => _onHeaderReading?.Invoke(null));
            _stream.Position = 0;
            using BinaryReader reader = new(_stream, Encoding.Default, true);
            var magic = reader.ReadUInt32();
            if (magic != Magic)
                throw new NotSupportedException(Encoding.ASCII.GetString(BitConverter.GetBytes(magic)) +
                    " is not supported.");

            MajorVersion = reader.ReadInt32();
            _stream.Seek(7 * sizeof(int), SeekOrigin.Current);
            var indexCount = reader.ReadInt32();
            _stream.Seek(sizeof(int), SeekOrigin.Current);
            //_stream.Read(buffer);
            //IndexSize = BitConverter.ToInt32(buffer);
            //_stream.Seek(12 + sizeof(int), SeekOrigin.Current);
            _stream.Seek(12 + sizeof(int) * 2, SeekOrigin.Current);
            IndexSize = 0;
            _stream.Position = IndexOffset = reader.ReadUInt32();

            Task.Run(() => _onIndexReading?.Invoke(IndexOffset));
            _index.Entries.Clear();
            ResourceKey[] keys = new ResourceKey[indexCount];
            _index.ValuesFlag = reader.ReadUInt32();
            if (_index.GetFlagAt(0))
                _index.TypeID = reader.ReadUInt32();
            if (_index.GetFlagAt(1))
                _index.GroupID = reader.ReadUInt32();
            if (_index.GetFlagAt(2))
                _index.UnknownID = reader.ReadUInt32();

            for (int i = 0; i < indexCount; i++)
            {
                var entry = new IndexEntry();
                if (!_index.GetFlagAt(0))
                {
                    entry.TypeID = reader.ReadUInt32();
                }
                else entry.TypeID = _index.TypeID;
                if (!_index.GetFlagAt(1))
                {
                    entry.GroupID = reader.ReadUInt32();
                }
                else entry.GroupID = _index.GroupID;
                if (!_index.GetFlagAt(2))
                    _stream.Seek(sizeof(uint), SeekOrigin.Begin);

                entry.InstanceID = reader.ReadUInt32();
                entry.Offset = reader.ReadUInt32();
                entry.CompressedSize = reader.ReadUInt32();
                entry.UncompressedSize = reader.ReadUInt32();
                entry.IsCompressed = reader.ReadUInt16() == 0xFFFF;
                entry.IsSaved = reader.ReadByte() == 1;
                _stream.Seek(1, SeekOrigin.Current);
                _index.Entries.Add(entry);
                keys[i] = new ResourceKey(entry.InstanceID, entry.TypeID ?? 0, entry.GroupID ?? 0);
                IndexSize += entry.EntrySize;
            }

            _stream.Position = IndexOffset;
            _index.ValuesFlag = 4;
            IndexSize += _index.SizeWithoutEntries;

            return keys;
        }
        public async Task<ResourceKey[]> ReadDBPFInfoAsync()
            => await Task.Run(ReadDBPFInfo);

        public StringResourceKey[]? ReadSecretIndex()
        {
            if (SecretIndexOffset >= _stream.Length)
                return null;

            _stream.Position = SecretIndexOffset;
            using BinaryReader reader = new(_stream, Encoding.Unicode, true);
            _secretIndex = new(reader.ReadString());
            int count = reader.ReadInt32();

            StringResourceKey[] keys = new StringResourceKey[count];
            for (int i = 0; i < count; i++)
            {
                keys[i] = new(instanceID: reader.ReadString(), typeID: reader.ReadString(), _secretIndex.GroupName);
                SecretIndexEntry entry = new()
                {
                    Key = keys[i],
                    Offset = reader.ReadUInt32(),
                    Size = reader.ReadUInt32()
                };
                _secretIndex.Entries.Add(entry);
            }

            return keys;
        }

        public byte[]? ReadSecretData(StringResourceKey key)
        {
            if (_secretIndex == null)
                return null;

            Task.Run(() => _onDataReading?.Invoke(key));
            foreach (var entry in _secretIndex.Entries)
            {
                if (!key.Equals(entry))
                    continue;

                byte[] data = new byte[entry.Size];
                var oldPosition = _stream.Position;
                _stream.Position = entry.Offset;
                _stream.Read(data);
                _stream.Position = oldPosition;

                return data;
            }

            return null;
        }
        public async Task<byte[]?> ReadSecretDataAsync(StringResourceKey key)
            => await Task.Run(() => ReadSecretData(key));

        public bool CopySecretDataTo(Stream destination, StringResourceKey key)
        {
            var buffer = ReadSecretData(key);
            if (buffer != null)
            {
                destination.Write(buffer);
                return true;
            }

            return false;
        }
        public async Task<bool> CopySecretDataToAsync(Stream destination, StringResourceKey key)
            => await Task.Run(() => CopySecretDataTo(destination, key));

        public void CopySecretDataFromStream(Stream input, StringResourceKey key)
        {
            _secretIndex ??= new();
            Task.Run(() => _onDataWriting?.Invoke(key));
            _headerWritten = _indexWritten = false;

            var entry = new SecretIndexEntry
            {
                Key = key,
                Offset = IndexOffset,
                Size = (uint)input.Length
            };
            _secretIndex.Entries.Add(entry);
            input.CopyTo(_stream);

            IndexOffset += entry.Size;
        }
        public async Task CopySecretDataFromStreamAsync(Stream input, StringResourceKey key)
            => await Task.Run(() => CopySecretDataFromStream(input, key));

        public byte[]? ReadResource(ResourceKey key, bool decompress = true)
        {
            Task.Run(() => _onDataReading?.Invoke(key));
            foreach (var entry in _index.Entries)
            {
                if (!key.Equals(entry))
                    continue;

                var oldPosition = _stream.Position;
                _stream.Position = entry.Offset;
                bool compressed = (entry.UncompressedSize | COMPRESSED_OR) != entry.CompressedSize;
                byte[] data;
                if (!decompress || !compressed)
                {
                    data = new byte[compressed ? entry.CompressedSize : entry.UncompressedSize];
                    _stream.Read(data);
                }
                else
                    data = _stream.RefPackDecompress(entry.CompressedSize, entry.UncompressedSize);

                _stream.Position = oldPosition;
                return data;
            }

            return null;
        }
        public async Task<byte[]?> ReadResourceAsync(ResourceKey key, bool decompress = true)
            => await Task.Run(() => ReadResource(key, decompress));

        public bool CopyResourceTo(Stream destination, ResourceKey key, bool decompress = true)
        {
            var buffer = ReadResource(key, decompress);
            if (buffer == null)
                return false;

            destination.Write(buffer);
            return true;
        }
        public async Task<bool> CopyResourceToAsync(Stream destination, ResourceKey key, bool decompress = true)
            => await Task.Run(() => CopyResourceTo(destination, key, decompress));

        #region IDisposable realization
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (_stream.CanWrite)
            {
                WriteIndex();
                WriteHeader();
            }

            if (disposing)
                _stream.Dispose();
            _disposed = true;
        }
        #endregion

        ~DatabasePackedFile() => Dispose(false);
    }
}
