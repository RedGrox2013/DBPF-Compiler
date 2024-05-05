using DBPF_Compiler.DBPF;
using DBPF_Compiler.Types;
using System.Text;

namespace DBPF_Compiler
{
    public static class StreamHelpers
    {
        private static byte[]? _buffer = null;

        private static byte[] ReadData(this Stream stream, int size, bool bigEndian)
        {
            if (_buffer == null || _buffer.Length < size)
                _buffer = new byte[size];

            stream.Read(_buffer, 0, size);
            if (bigEndian)
                Array.Reverse(_buffer, 0, size);

            return _buffer;
        }

        internal static void WriteUInt64(this Stream stream, ulong? value)
        {
            if (value != null)
                stream.Write(BitConverter.GetBytes((ulong)value));
        }
        internal static void WriteUInt32(this Stream stream, uint? value)
        {
            if (value !=  null)
                stream.Write(BitConverter.GetBytes((uint)value));
        }
        internal static void WriteInt32(this Stream stream, int? value)
        {
            if (value != null)
                stream.Write(BitConverter.GetBytes((uint)value));
        }
        internal static void WriteUInt16(this Stream stream, ushort? value)
        {
            if (value != null)
                stream.Write(BitConverter.GetBytes((ushort)value));
        }
        internal static void WriteBoolean(this Stream stream, bool? value)
        {
            if (value == null)
                return;

            if ((bool)value)
                stream.WriteByte(1);
            else
                stream.WriteByte(0);
        }

        internal static short ReadInt16(this Stream stream, bool bigEndian = false)
            => BitConverter.ToInt16(stream.ReadData(sizeof(short), bigEndian), 0);
        internal static ushort ReadUInt16(this Stream stream, bool bigEndian = false)
            => BitConverter.ToUInt16(stream.ReadData(sizeof(ushort), bigEndian), 0);
        internal static int ReadInt32(this Stream stream, bool bigEndian = false)
            => BitConverter.ToInt32(stream.ReadData(sizeof(int), bigEndian), 0);
        internal static uint ReadUInt32(this Stream stream, bool bigEndian = false)
            => BitConverter.ToUInt32(stream.ReadData(sizeof(uint), bigEndian), 0);
        internal static long ReadInt64(this Stream stream, bool bigEndian = false)
            => BitConverter.ToInt64(stream.ReadData(sizeof(long), bigEndian), 0);
        internal static ulong ReadUInt64(this Stream stream, bool bigEndian = false)
            => BitConverter.ToUInt64(stream.ReadData(sizeof(ulong), bigEndian), 0);

        internal static float ReadFloat(this Stream stream, bool bigEndian = false)
            => BitConverter.ToSingle(stream.ReadData(sizeof(float), bigEndian), 0);

        internal static string ReadString8(this Stream stream, bool bigEndianLength = false)
        {
            int len = stream.ReadInt32(bigEndianLength);

            return Encoding.ASCII.GetString(stream.ReadData(len, false), 0, len);
        }
        internal static string ReadString16(this Stream stream, bool bigEndianLength = false, bool bigEndianString = false)
        {
            int len = stream.ReadInt32(bigEndianLength) * 2;

            return Encoding.Unicode.GetString(stream.ReadData(len, bigEndianString), 0, len);
        }

        internal static ResourceKey ReadResourceKey(this Stream stream, bool bigEndian = false)
        {
            ResourceKey key = new(stream.ReadUInt32(bigEndian), stream.ReadUInt32(bigEndian), stream.ReadUInt32(bigEndian));
            stream.Seek(sizeof(uint), SeekOrigin.Current); // wildcard

            return key;
        }

        internal static bool[] ReadPropertyBoolArray(this Stream stream, bool bigEndianSize = false)
        {
            var buffer = stream.ReadPropertyUInt8Array(bigEndianSize);
            bool[] array = new bool[buffer.Length];
            for (int i = 0; i < buffer.Length; i++)
                array[i] = buffer[i] == 1;

            return array;
        }
        internal static byte[] ReadPropertyUInt8Array(this Stream stream, bool bigEndianSize = false)
        {
            int size = stream.ReadInt32(bigEndianSize);
            int elementSize = stream.ReadInt32(bigEndianSize);

            return stream.ReadData(size * elementSize, false);
        }
        internal static sbyte[] ReadPropertyInt8Array(this Stream stream, bool bigEndianSize = false)
        {
            byte[] buffer = stream.ReadPropertyUInt8Array(bigEndianSize);
            sbyte[] array = new sbyte[buffer.Length];
            for (int i = 0; i < buffer.Length; i++)
                array[i] = (sbyte)buffer[i];

            return array;
        }

        internal static short[] ReadPropertyInt16Array(this Stream stream, bool bigEndian = false)
        {
            byte[] buffer = stream.ReadPropertyUInt8Array(bigEndian);
            short[] array = new short[buffer.Length / sizeof(short)];
            for (int i = 0; i < array.Length; i++)
            {
                int bufferIndex = i * sizeof(short);
                if (bigEndian)
                    Array.Reverse(buffer, bufferIndex, sizeof(short));

                array[i] = BitConverter.ToInt16(buffer, bufferIndex);
            }

            return array;
        }
        internal static ushort[] ReadPropertyUInt16Array(this Stream stream, bool bigEndian = false)
        {
            byte[] buffer = stream.ReadPropertyUInt8Array(bigEndian);
            ushort[] array = new ushort[buffer.Length / sizeof(ushort)];
            for (int i = 0; i < array.Length; i++)
            {
                int bufferIndex = i * sizeof(ushort);
                if (bigEndian)
                    Array.Reverse(buffer, bufferIndex, sizeof(ushort));

                array[i] = BitConverter.ToUInt16(buffer, bufferIndex);
            }

            return array;
        }
        internal static int[] ReadPropertyInt32Array(this Stream stream, bool bigEndian = false)
        {
            byte[] buffer = stream.ReadPropertyUInt8Array(bigEndian);
            int[] array = new int[buffer.Length / sizeof(int)];
            for (int i = 0; i < array.Length; i++)
            {
                int bufferIndex = i * sizeof(int);
                if (bigEndian)
                    Array.Reverse(buffer, bufferIndex, sizeof(int));

                array[i] = BitConverter.ToInt32(buffer, bufferIndex);
            }

            return array;
        }
        internal static uint[] ReadPropertyUInt32Array(this Stream stream, bool bigEndian = false)
        {
            byte[] buffer = stream.ReadPropertyUInt8Array(bigEndian);
            uint[] array = new uint[buffer.Length / sizeof(uint)];
            for (int i = 0; i < array.Length; i++)
            {
                int bufferIndex = i * sizeof(uint);
                if (bigEndian)
                    Array.Reverse(buffer, bufferIndex, sizeof(uint));

                array[i] = BitConverter.ToUInt32(buffer, bufferIndex);
            }

            return array;
        }
        internal static float[] ReadPropertyFloatArray(this Stream stream, bool bigEndian = false)
        {
            byte[] buffer = stream.ReadPropertyUInt8Array(bigEndian);
            float[] array = new float[buffer.Length / sizeof(float)];
            for (int i = 0; i < array.Length; i++)
            {
                int bufferIndex = i * sizeof(float);
                if (bigEndian)
                    Array.Reverse(buffer, bufferIndex, sizeof(float));

                array[i] = BitConverter.ToSingle(buffer, bufferIndex);
            }

            return array;
        }

        internal static string[] ReadPropertyString8Array(this Stream stream, bool bigEndianLength = false)
        {
            string[] array = new string[stream.ReadInt32(bigEndianLength)];
            stream.Seek(4, SeekOrigin.Current);
            for (int i = 0; i < array.Length; i++)
                array[i] = stream.ReadString8(bigEndianLength);

            return array;
        }
        internal static string[] ReadPropertyString16Array(this Stream stream, bool bigEndianLength = false, bool bigEndianString = false)
        {
            string[] array = new string[stream.ReadInt32(bigEndianLength)];
            stream.Seek(4, SeekOrigin.Current);
            for (int i = 0; i < array.Length; i++)
                array[i] = stream.ReadString16(bigEndianLength, bigEndianString);

            return array;
        }

        internal static ResourceKey[] ReadPropertyResourceKeyArray(this Stream stream, bool bigEndianSize = false, bool bigEndian = false)
        {
            byte[] buffer = stream.ReadPropertyUInt8Array(bigEndianSize);
            ResourceKey[] array = new ResourceKey[buffer.Length / (sizeof(uint) * 3)];
            for (int i = 0, j = 0; i < array.Length; i++)
                array[i] = new(BitConverter.ToUInt32(buffer, j++ * sizeof(uint)),
                    BitConverter.ToUInt32(buffer, j++ * sizeof(uint)),
                    BitConverter.ToUInt32(buffer, j++ * sizeof(uint)));

            return array;
        }

        internal static Vector4 ReadVector(this Stream stream, bool bigEndian = false)
            => new(stream.ReadFloat(bigEndian), stream.ReadFloat(bigEndian), stream.ReadFloat(bigEndian), stream.ReadFloat(bigEndian));


        internal static void WriteIndexEntry(this Stream stream, IndexEntry entry)
        {
            stream.WriteUInt32(entry.TypeID);
            stream.WriteUInt32(entry.GroupID);
            stream.WriteUInt32(entry.UnknownID);
            stream.WriteUInt32(entry.InstanceID);
            stream.WriteUInt32(entry.Offset);
            stream.WriteUInt32(entry.CompressedSize);
            stream.WriteUInt32(entry.UncompressedSize);
            stream.WriteUInt16(entry.IsCompressed ? (ushort)0xFFFF : (ushort)0);
            stream.WriteBoolean(entry.IsSaved);
            stream.WriteByte(0);
        }


        /*
         * Украл реализацию разжатия из SporeMaster'а
         * https://github.com/dptetc/SporeMaster/blob/master/SporeMaster/Gibbed.Spore/Helpers/StreamHelpers.cs
         */
        private static bool ReadRefPackCompressionHeader(this Stream stream)
        {
            byte[] header = new byte[2];
            stream.Read(header, 0, header.Length);

            if ((header[0] & 0x3E) != 0x10 || (header[1] != 0xFB))
            {
                // stream is not compressed 
                return false;
            }

            stream.Seek(((header[0] & 0x80) != 0 ? 4 : 3) * ((header[0] & 0x01) != 0 ? 2 : 1), SeekOrigin.Current);
            return true;
        }

        public static byte[] RefPackDecompress(this Stream stream, uint compressedSize, uint decompressedSize)
        {
            long baseOffset = stream.Position;
            byte[] outputData = new byte[decompressedSize];

            if (!stream.ReadRefPackCompressionHeader())
            {
                stream.Position = baseOffset;
                stream.Read(outputData);
                return outputData;
            }

            uint offset = 0;
            bool stop = false;
            while (stream.Position < baseOffset + compressedSize && !stop)
            {
                uint plainSize, copySize = 0, copyOffset = 0;
                byte prefix = (byte)stream.ReadByte();

                if (prefix >= 0xFC)
                {
                    plainSize = (uint)(prefix & 3);
                    stop = true;
                }
                else if (prefix >= 0xE0)
                    plainSize = (uint)(((prefix & 0x1F) + 1) * 4);
                else if (prefix >= 0xC0)
                {
                    byte[] extra = new byte[3];
                    stream.Read(extra, 0, extra.Length);
                    plainSize = (uint)(prefix & 3);
                    copySize = (uint)((((prefix & 0x0C) << 6) | extra[2]) + 5);
                    copyOffset = (uint)((((((prefix & 0x10) << 4) | extra[0]) << 8) | extra[1]) + 1);
                }
                else if (prefix >= 0x80)
                {
                    byte[] extra = new byte[2];
                    stream.Read(extra, 0, extra.Length);
                    plainSize = (uint)(extra[0] >> 6);
                    copySize = (uint)((prefix & 0x3F) + 4);
                    copyOffset = (uint)((((extra[0] & 0x3F) << 8) | extra[1]) + 1);
                }
                else
                {
                    byte extra = (byte)stream.ReadByte();
                    plainSize = (uint)(prefix & 3);
                    copySize = (uint)(((prefix & 0x1C) >> 2) + 3);
                    copyOffset = (uint)((((prefix & 0x60) << 3) | extra) + 1);
                }

                if (plainSize > 0)
                {
                    stream.Read(outputData, (int)offset, (int)plainSize);
                    offset += plainSize;
                }

                if (copySize > 0)
                {
                    for (uint i = 0; i < copySize; i++)
                        outputData[offset + i] = outputData[(offset - copyOffset) + i];

                    offset += copySize;
                }
            }

            return outputData;
        }
    }
}