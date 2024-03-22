using DBPF_Compiler.DBPF;

namespace DBPF_Compiler
{
    internal static class StreamHelpers
    {
        public static void WriteUInt32(this Stream stream, uint? value)
        {
            if (value !=  null)
                stream.Write(BitConverter.GetBytes((uint)value));
        }
        public static void WriteInt32(this Stream stream, int? value)
        {
            if (value != null)
                stream.Write(BitConverter.GetBytes((uint)value));
        }
        public static void WriteUInt16(this Stream stream, ushort? value)
        {
            if (value != null)
                stream.Write(BitConverter.GetBytes((ushort)value));
        }
        public static void WriteBoolean(this Stream stream, bool? value)
        {
            if (value == null)
                return;

            if ((bool)value)
                stream.WriteByte(1);
            else
                stream.WriteByte(0);
        }

        public static void WriteIndexEntry(this Stream stream, IndexEntry entry)
        {
            stream.WriteUInt32(entry.TypeID);
            stream.WriteUInt32(entry.GroupID);
            stream.WriteUInt32(entry.UnknownID);
            stream.WriteUInt32(entry.InstanceID);
            stream.WriteUInt32(entry.Offset);
            stream.WriteUInt32(entry.CompressedSize | 0x80000000);
            stream.WriteUInt32(entry.UncompressedSize);
            stream.WriteUInt16(entry.IsCompressed ? (ushort)0xFFFF : (ushort)0);
            stream.WriteBoolean(entry.IsSaved);
            stream.WriteByte(0);
        }
    }
}
