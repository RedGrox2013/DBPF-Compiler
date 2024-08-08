using DBPF_Compiler.DBPF;

namespace DBPF_Compiler.ModsTemplates
{
    public interface IModTemplate
    {
        void BuildMod(DatabasePackedFile dbpf, DBPFPackerHelper helper);
    }
}
