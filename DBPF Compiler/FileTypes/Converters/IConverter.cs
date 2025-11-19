namespace DBPF_Compiler.FileTypes.Converters
{
    public interface IConverter
    {
        ISporeFile Convert(Stream stream);
    }
}
