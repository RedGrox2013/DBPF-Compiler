using DBPF_Compiler.DBPF;

namespace DBPF_Compiler
{
    public static class StreamHelpers
    {
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
        private static void ReadRefPackCompressionHeader(this Stream stream)
        {
            byte[] header = new byte[2];
            stream.Read(header, 0, header.Length);

            //bool stop = false;

            // hdr & 0x3EFF) == 0x10FB 
            if ((header[0] & 0x3E) != 0x10 || (header[1] != 0xFB))
            {
                // stream is not compressed 
                //stop = true;
            }

            // read destination (uncompressed) length 
            bool isLong = ((header[0] & 0x80) != 0);
            bool hasMore = ((header[0] & 0x01) != 0);

            byte[] data = new byte[(isLong ? 4 : 3) * (hasMore ? 2 : 1)];
            stream.Read(data, 0, data.Length);

            UInt32 realLength = (uint)((((data[0] << 8) + data[1]) << 8) + data[2]);
            if (isLong)
            {
                realLength = (realLength << 8) + data[3];
            }
        }

        public static byte[] RefPackDecompress(this Stream stream, uint compressedSize, uint decompressedSize)
        {
            long baseOffset = stream.Position;
            byte[] outputData = new byte[decompressedSize];
            uint offset = 0;

            stream.ReadRefPackCompressionHeader();

            while (stream.Position < baseOffset + compressedSize)
            {
                bool stop = false;
                UInt32 plainSize = 0;
                UInt32 copySize = 0;
                UInt32 copyOffset = 0;

                byte prefix = (byte)stream.ReadByte();

                if (prefix >= 0xC0)
                {
                    if (prefix >= 0xE0)
                    {
                        if (prefix >= 0xFC)
                        {
                            plainSize = (uint)(prefix & 3);
                            stop = true;
                        }
                        else
                        {
                            plainSize = (uint)(((prefix & 0x1F) + 1) * 4);
                        }
                    }
                    else
                    {
                        byte[] extra = new byte[3];
                        stream.Read(extra, 0, extra.Length);
                        plainSize = (uint)(prefix & 3);
                        copySize = (uint)((((prefix & 0x0C) << 6) | extra[2]) + 5);
                        copyOffset = (uint)((((((prefix & 0x10) << 4) | extra[0]) << 8) | extra[1]) + 1);
                    }
                }
                else
                {
                    if (prefix >= 0x80)
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
                }

                if (plainSize > 0)
                {
                    stream.Read(outputData, (int)offset, (int)plainSize);
                    offset += plainSize;
                }

                if (copySize > 0)
                {
                    for (uint i = 0; i < copySize; i++)
                    {
                        outputData[offset + i] = outputData[(offset - copyOffset) + i];
                    }

                    offset += copySize;
                }

                if (stop)
                {
                    break;
                }
            }

            return outputData;
        }
    }
}