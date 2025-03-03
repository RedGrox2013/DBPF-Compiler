﻿using DBPF_Compiler.DBPF;
using DBPF_Compiler.Types;
using System.Text;

namespace DBPF_Compiler
{
    public static class StreamHelpers
    {
        private static byte[] ReadData(this Stream stream, int size, bool bigEndian)
        {
            var buffer = new byte[size];

            stream.ReadExactly(buffer);
            if (bigEndian)
                Array.Reverse(buffer, 0, size);

            return buffer;
        }

        internal static void WriteUInt64(this Stream stream, ulong? value, bool bigEndian = false)
        {
            if (value == null)
                return;

            var buffer = BitConverter.GetBytes((ulong)value);
            if (bigEndian)
                Array.Reverse(buffer);

            stream.Write(buffer);
        }
        internal static void WriteUInt32(this Stream stream, uint? value, bool bigEndian = false)
        {
            if (value == null)
                return;

            var buffer = BitConverter.GetBytes((uint)value);
            if (bigEndian)
                Array.Reverse(buffer);

            stream.Write(buffer);
        }
        internal static void WriteInt32(this Stream stream, int? value, bool bigEndian = false)
        {
            if (value == null)
                return;

            var buffer = BitConverter.GetBytes((int)value);
            if (bigEndian)
                Array.Reverse(buffer);

            stream.Write(buffer);
        }
        internal static void WriteUInt16(this Stream stream, ushort? value, bool bigEndian = false)
        {
            if (value == null)
                return;

            var buffer = BitConverter.GetBytes((ushort)value);
            if (bigEndian)
                Array.Reverse(buffer);

            stream.Write(buffer);
        }
        internal static void WriteInt16(this Stream stream, short? value, bool bigEndian = false)
        {
            if (value == null)
                return;

            var buffer = BitConverter.GetBytes((short)value);
            if (bigEndian)
                Array.Reverse(buffer);

            stream.Write(buffer);
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
        internal static void WriteFloat(this Stream stream, float? value, bool bigEndian = false)
        {
            if (value == null)
                return;

            var buffer = BitConverter.GetBytes((float)value);
            if (bigEndian)
                Array.Reverse(buffer);

            stream.Write(buffer);
        }

        internal static void WriteString(this Stream stream, string? value, Encoding encoding, bool bigEndianLength = false, bool bigEndian = false)
        {
            var buffer = encoding.GetBytes(value ?? string.Empty);
            if (bigEndian)
                Array.Reverse(buffer);

            stream.WriteInt32(value?.Length ?? 0, bigEndianLength);
            stream.Write(buffer);
        }

        internal static void WriteLocalizedString(this Stream stream, LocalizedString value, bool bigEndian = false)
        {
            stream.WriteUInt32(value.TableID, bigEndian);
            stream.WriteUInt32(value.InstanceID, bigEndian);

            var placeholder = Encoding.Unicode.GetBytes(value.PlaceholderText ?? string.Empty);
            stream.Write(placeholder);
            if (placeholder.Length == LocalizedString.PLACEHOLDER_SIZE)
                return;

            stream.Seek(LocalizedString.PLACEHOLDER_SIZE - 1 - placeholder.Length, SeekOrigin.Current);
            stream.WriteByte(0);
        }

        internal static void WriteBBox(this Stream stream, BoundingBox value, bool bigEndian = false)
        {
            stream.WriteFloat(value.Min.X, bigEndian);
            stream.WriteFloat(value.Min.Y, bigEndian);
            stream.WriteFloat(value.Min.Z, bigEndian);
            stream.WriteFloat(value.Max.X, bigEndian);
            stream.WriteFloat(value.Max.Y, bigEndian);
            stream.WriteFloat(value.Max.Z, bigEndian);
        }

        internal static void WriteResourceKey(this Stream stream, ResourceKey value, bool bigEndian = false)
        {
            stream.WriteUInt32(value.InstanceID, bigEndian);
            stream.WriteUInt32(value.TypeID, bigEndian);
            stream.WriteUInt32(value.GroupID, bigEndian);
            stream.Write([0, 0, 0, 0]); // wildcard
        }

        internal static void WriteVector(this Stream stream, Vector4 value, bool bigEndian = false)
        {
            stream.WriteFloat(value.X, bigEndian);
            stream.WriteFloat(value.Y, bigEndian);
            stream.WriteFloat(value.Z, bigEndian);
            stream.WriteFloat(value.A, bigEndian);
        }

        internal static void WriteTransform(this Stream stream, Transform value, bool bigEndian = false)
        {
            stream.WriteInt16((short)value.Flags, bigEndian);
            stream.WriteInt16(value.TransformCount, bigEndian);
            stream.WriteFloat(value.Offset.X, bigEndian);
            stream.WriteFloat(value.Offset.Y, bigEndian);
            stream.WriteFloat(value.Offset.Z, bigEndian);
            stream.WriteFloat(value.Scale, bigEndian);

            for (int i = 0; i < Matrix.SIZE; i++)
                for (int j = 0; j < Matrix.SIZE; j++)
                    stream.WriteFloat(value.Rotate[i, j]);
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

        internal static bool[] ReadBoolArray(this Stream stream, bool bigEndianSize = false)
        {
            var buffer = stream.ReadUInt8Array(bigEndianSize);
            bool[] array = new bool[buffer.Length];
            for (int i = 0; i < buffer.Length; i++)
                array[i] = buffer[i] == 1;

            return array;
        }
        internal static byte[] ReadUInt8Array(this Stream stream, bool bigEndianSize = false)
        {
            int size = stream.ReadInt32(bigEndianSize);
            int elementSize = stream.ReadInt32(bigEndianSize);

            return stream.ReadData(size * elementSize, false);
        }
        internal static sbyte[] ReadInt8Array(this Stream stream, bool bigEndianSize = false)
        {
            byte[] buffer = stream.ReadUInt8Array(bigEndianSize);
            sbyte[] array = new sbyte[buffer.Length];
            for (int i = 0; i < buffer.Length; i++)
                array[i] = (sbyte)buffer[i];

            return array;
        }

        internal static short[] ReadInt16Array(this Stream stream, bool bigEndian = false)
        {
            byte[] buffer = stream.ReadUInt8Array(bigEndian);
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
        internal static ushort[] ReadUInt16Array(this Stream stream, bool bigEndian = false)
        {
            byte[] buffer = stream.ReadUInt8Array(bigEndian);
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
        internal static int[] ReadInt32Array(this Stream stream, bool bigEndian = false)
        {
            byte[] buffer = stream.ReadUInt8Array(bigEndian);
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
        internal static uint[] ReadUInt32Array(this Stream stream, bool bigEndian = false)
        {
            byte[] buffer = stream.ReadUInt8Array(bigEndian);
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
        internal static float[] ReadFloatArray(this Stream stream, bool bigEndian = false)
        {
            byte[] buffer = stream.ReadUInt8Array(bigEndian);
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

        internal static string[] ReadString8Array(this Stream stream, bool bigEndianLength = false)
        {
            string[] array = new string[stream.ReadInt32(bigEndianLength)];
            stream.Seek(4, SeekOrigin.Current);
            for (int i = 0; i < array.Length; i++)
                array[i] = stream.ReadString8(bigEndianLength);

            return array;
        }
        internal static string[] ReadString16Array(this Stream stream, bool bigEndianLength = false, bool bigEndianString = false)
        {
            string[] array = new string[stream.ReadInt32(bigEndianLength)];
            stream.Seek(4, SeekOrigin.Current);
            for (int i = 0; i < array.Length; i++)
                array[i] = stream.ReadString16(bigEndianLength, bigEndianString);

            return array;
        }

        internal static ResourceKey[] ReadResourceKeyArray(this Stream stream, bool bigEndianSize = false)
        {
            byte[] buffer = stream.ReadUInt8Array(bigEndianSize);
            ResourceKey[] array = new ResourceKey[buffer.Length / (sizeof(uint) * 3)];
            for (int i = 0, j = 0; i < array.Length; i++)
                array[i] = new(BitConverter.ToUInt32(buffer, j++ * sizeof(uint)),
                    BitConverter.ToUInt32(buffer, j++ * sizeof(uint)),
                    BitConverter.ToUInt32(buffer, j++ * sizeof(uint)));

            return array;
        }

        internal static Vector2[] ReadVector2Array(this Stream stream, bool bigEndian = false)
        {
            byte[] buffer = stream.ReadUInt8Array(bigEndian);
            Vector2[] array = new Vector2[buffer.Length / (sizeof(float) * 2)];
            for (int i = 0, j = 0; i < array.Length; i++)
                array[i] = new(BitConverter.ToSingle(buffer, j++ * sizeof(float)),
                    BitConverter.ToSingle(buffer, j++ * sizeof(float)));

            return array;
        }
        internal static Vector3[] ReadVector3Array(this Stream stream, bool bigEndian = false)
        {
            byte[] buffer = stream.ReadUInt8Array(bigEndian);
            Vector3[] array = new Vector3[buffer.Length / (sizeof(float) * 3)];
            for (int i = 0, j = 0; i < array.Length; i++)
                array[i] = new(BitConverter.ToSingle(buffer, j++ * sizeof(float)),
                    BitConverter.ToSingle(buffer, j++ * sizeof(float)),
                    BitConverter.ToSingle(buffer, j++ * sizeof(float)));

            return array;
        }
        internal static Vector4[] ReadVector4Array(this Stream stream, bool bigEndian = false)
        {
            byte[] buffer = stream.ReadUInt8Array(bigEndian);
            Vector4[] array = new Vector4[buffer.Length / (sizeof(float) * 4)];
            for (int i = 0, j = 0; i < array.Length; i++)
                array[i] = new(BitConverter.ToSingle(buffer, j++ * sizeof(float)),
                    BitConverter.ToSingle(buffer, j++ * sizeof(float)),
                    BitConverter.ToSingle(buffer, j++ * sizeof(float)),
                    BitConverter.ToSingle(buffer, j++ * sizeof(float)));

            return array;
        }

        internal static LocalizedString[] ReadLocalizedStringArray(this Stream stream, bool bigEndianSize = false, bool bigEndianKey = false)
        {
            byte[] buffer = stream.ReadUInt8Array(bigEndianSize);
            LocalizedString[] array = new LocalizedString[buffer.Length / (LocalizedString.PLACEHOLDER_SIZE + sizeof(uint) * 2)];
            for (int i = 0; i < array.Length; i++)
            {
                int bufferIndex = i * (LocalizedString.PLACEHOLDER_SIZE + sizeof(uint) * 2);
                if (bigEndianKey)
                {
                    Array.Reverse(buffer, bufferIndex, sizeof(uint));
                    Array.Reverse(buffer, bufferIndex + sizeof(uint), sizeof(uint));
                }

                uint tableID = BitConverter.ToUInt32(buffer, bufferIndex);
                bufferIndex += sizeof(uint);
                uint instanceID = BitConverter.ToUInt32(buffer, bufferIndex);
                bufferIndex += sizeof(uint);
                string placeholder = Encoding.Unicode.GetString(buffer, bufferIndex, LocalizedString.PLACEHOLDER_SIZE);

                array[i] = new(tableID, instanceID, placeholder.TrimEnd('\0'));
            }

            return array;
        }

        internal static BoundingBox[] ReadBBoxArray(this Stream stream, bool bigEndianSize = false)
        {
            byte[] buffer = stream.ReadUInt8Array(bigEndianSize);
            BoundingBox[] array = new BoundingBox[buffer.Length / (sizeof(float) * 3 * 2)];
            for (int i = 0, j = 0; i < array.Length; i++)
                array[i] = new()
                {
                    Min = new(BitConverter.ToSingle(buffer, j++ * sizeof(float)),
                    BitConverter.ToSingle(buffer, j++ * sizeof(float)),
                    BitConverter.ToSingle(buffer, j++ * sizeof(float))),
                    Max = new(BitConverter.ToSingle(buffer, j++ * sizeof(float)),
                    BitConverter.ToSingle(buffer, j++ * sizeof(float)),
                    BitConverter.ToSingle(buffer, j++ * sizeof(float)))
                };

            return array;
        }

        internal static Transform[] ReadTransformArray(this Stream stream, bool bigEndianSize = false, bool bigEndian = false)
        {
            int count = stream.ReadInt32(bigEndianSize);
            stream.Seek(sizeof(int), SeekOrigin.Current); // element size
            Transform[] array = new Transform[count];
            for (int i = 0; i < count; i++)
            {
                var flags = (TransformFlags)stream.ReadInt16(bigEndian);
                array[i] = new Transform
                {
                    //Flags = (TransformFlags)stream.ReadInt16(bigEndian),
                    TransformCount = stream.ReadInt16(bigEndian)
                };

                if (flags.HasFlag(TransformFlags.Offset))
                    array[i].Offset = new(stream.ReadFloat(bigEndian), stream.ReadFloat(bigEndian), stream.ReadFloat(bigEndian));
                else
                    stream.Seek(sizeof(float) * 3, SeekOrigin.Current);
                if (flags.HasFlag(TransformFlags.Scale))
                    array[i].Scale = stream.ReadFloat(bigEndian);
                else
                    stream.Seek(sizeof(float), SeekOrigin.Current);
                if (flags.HasFlag(TransformFlags.Rotate))
                    array[i].Rotate = Matrix.ReadMatrix(stream, bigEndian);
                else
                    stream.Seek(sizeof(float) * Matrix.SIZE * Matrix.SIZE, SeekOrigin.Current);
            }

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
            stream.ReadExactly(header);

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
                stream.ReadExactly(outputData);
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
                    stream.ReadExactly(extra);
                    plainSize = (uint)(prefix & 3);
                    copySize = (uint)((((prefix & 0x0C) << 6) | extra[2]) + 5);
                    copyOffset = (uint)((((((prefix & 0x10) << 4) | extra[0]) << 8) | extra[1]) + 1);
                }
                else if (prefix >= 0x80)
                {
                    byte[] extra = new byte[2];
                    stream.ReadExactly(extra);
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
                    stream.ReadExactly(outputData, (int)offset, (int)plainSize);
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