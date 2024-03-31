namespace DBPF_Compiler.FileTypes
{
    public interface ISporeFile
    {
        uint TypeID { get; }

        bool Decode(byte[] data);
        byte[] Encode();
        //void ToXML();
        //string ToArgScript();
    }
}
