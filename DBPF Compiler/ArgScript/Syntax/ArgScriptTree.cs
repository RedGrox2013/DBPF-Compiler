using System.Collections;

namespace DBPF_Compiler.ArgScript.Syntax
{
    //public interface IArgScriptTreeNode { }

    //public class ArgScriptTree : IArgScriptTreeNode, IEnumerable<IArgScriptTreeNode>
    //{
    //    private readonly List<IArgScriptTreeNode> _argScript = [];

    //    public void AddNode(IArgScriptTreeNode node)
    //        => _argScript.Add(node);

    //    public IEnumerator<IArgScriptTreeNode> GetEnumerator()
    //        => ((IEnumerable<IArgScriptTreeNode>)_argScript).GetEnumerator();

    //    IEnumerator IEnumerable.GetEnumerator()
    //        => ((IEnumerable)_argScript).GetEnumerator();
    //}

    //internal record class ArgumentNode(Token Argument) : IArgScriptTreeNode;

    //internal record class NumberNode(Token Number) : IArgScriptTreeNode;

    //internal record class BinOperatorNode(Token Operator, IArgScriptTreeNode Left, IArgScriptTreeNode Right) : IArgScriptTreeNode;

    //internal record class UnarOperatorNode(Token Operator, IArgScriptTreeNode Operand) : IArgScriptTreeNode;

    //internal class CommandNode(Token command) : IArgScriptTreeNode
    //{
    //    public Token Command { get; set; } = command;

    //}

    //public class ArgScriptTree
    //{
    //    private ArgScriptNode? _root;

    //    public void Add(ArgScriptNode node)
    //    {
    //        if (_root == null)
    //        {
    //            _root = node;
    //            return;
    //        }

    //        // доделать
    //    }
    //}

    public abstract class ArgScriptNode(Token token)
    {   
        public ArgScriptNode? Left { get; set; }
        public ArgScriptNode? Right { get; set; }
        //public int Priority { get; set; }

        public Token Token { get; set; } = token;
    }

    public class NumberNode(Token number) : ArgScriptNode(number);
    public class ArgumentNode(Token argument) : ArgScriptNode(argument);

    public class BinOperatorNode : ArgScriptNode
    {
        public BinOperatorNode(Token @operator, ArgScriptNode left, ArgScriptNode right) : base(@operator)
        {
            Left = left;
            Right = right;
        }
    }

    public class UnarOperatorNode : ArgScriptNode
    {
        public UnarOperatorNode(Token @operator, ArgScriptNode operand) : base(@operator) =>
            Left = operand;
    }
}
