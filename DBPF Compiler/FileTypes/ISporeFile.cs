namespace DBPF_Compiler.FileTypes
{
    public interface ISporeFile
    {
        uint TypeID { get; }

        void ReadFromStream(Stream stream);
        void WriteToStream(Stream stream);
    }
}
