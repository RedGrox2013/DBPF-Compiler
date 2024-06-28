namespace DBPF_Compiler.ArgScript
{
    public interface IBlock : IParser
    {
        void OnBlockStart(Line startLine);
        void OnBlockEnd();
        void AddParser(string keyword, IParser parser);
        IParser GetParser(string keyword);
    }
}
