using DBPF_Compiler.DBPF;

namespace DBPF_Compiler.ModsTemplates
{
    public interface IModTemplate
    {
        ModTemplateType TemplateType { get; }
        void BuildMod(DatabasePackedFile dbpf, DBPFPackerHelper helper);
    }

    public enum ModTemplateType
    {
        Project,
        Music
    }
}
