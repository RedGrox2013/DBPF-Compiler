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
    }
}
