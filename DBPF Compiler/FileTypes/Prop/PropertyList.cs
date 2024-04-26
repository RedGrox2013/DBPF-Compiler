using DBPF_Compiler.FNV;

namespace DBPF_Compiler.FileTypes.Prop
{
    public class PropertyList : ISporeFile
    {
        public uint TypeID => 0x00B1B104;

        public bool Decode(byte[]? data)
        {
            throw new NotImplementedException();
        }

        public uint WriteToStream(Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}
