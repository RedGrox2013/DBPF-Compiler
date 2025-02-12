using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBPF_Compiler.ArgScript
{
    public abstract class ArgScriptTree { }

    internal class ArgScriptTreeRoot : ArgScriptTree
    {
        private readonly List<ArgScriptTree> _argScript = [];

        public void AddNode(ArgScriptTree node)
            => _argScript.Add(node);

        public void ForEach(Action<ArgScriptTree> action)
            => _argScript.ForEach(action);
    }
}
