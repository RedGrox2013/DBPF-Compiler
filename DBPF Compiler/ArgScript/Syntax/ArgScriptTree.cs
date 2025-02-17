using System.Collections;

namespace DBPF_Compiler.ArgScript.Syntax
{
    public interface IArgScriptTreeNode { }

    public class ArgScriptTree : IArgScriptTreeNode, IEnumerable<IArgScriptTreeNode>
    {
        private readonly List<IArgScriptTreeNode> _argScript = [];

        public void AddNode(IArgScriptTreeNode node)
            => _argScript.Add(node);

        public IEnumerator<IArgScriptTreeNode> GetEnumerator()
            => ((IEnumerable<IArgScriptTreeNode>)_argScript).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => ((IEnumerable)_argScript).GetEnumerator();
    }

    internal record class KeywordNode(Token Keyword) : IArgScriptTreeNode;

    internal record class NumberNode(Token Number) : IArgScriptTreeNode;

    internal record class BinOperatorNode(Token Operator, IArgScriptTreeNode Left, IArgScriptTreeNode Right) : IArgScriptTreeNode;

    internal record class UnarOperatorNode(Token Operator, IArgScriptTreeNode Operand) : IArgScriptTreeNode;

    //internal class CommandNode(Token command) : IArgScriptTreeNode
    //{
    //    public Token Command { get; set; } = command;

    //}
}
