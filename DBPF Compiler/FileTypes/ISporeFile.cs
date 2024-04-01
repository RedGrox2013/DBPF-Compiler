namespace DBPF_Compiler.FileTypes
{
    public interface ISporeFile
    {
        uint TypeID { get; }
        uint DataSize { get; }

        bool Decode(byte[]? data);
        List<byte> Encode();
    }
}
