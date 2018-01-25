using System.Collections.Generic;
using aima.net.text.patternmatching.api;

namespace aima.net.text.patternmatching.ahocorasick
{
    internal class Node
    {
        internal Node(char value, bool isRootNode)
        {
            GotoStateDictionary = new Dictionary<char, Node>();
            Output = new HashSet<IPattern>();
            IsRootNode = isRootNode;
            Value = value;
        }

        internal IDictionary<char, Node> GotoStateDictionary { get; }
        internal ISet<IPattern> Output { get; }
        internal bool IsRootNode { get; }
        internal char Value { get; }

        private Node failure;
        internal virtual Node Failure
        {
            get
            {
                if (IsRootNode)
                {
                    return this; //// a rootnode fails towards itself
                }
                return failure;
            }
            set
            {
                if (IsRootNode)
                {
                    throw new System.AccessViolationException("Trying to set failure of a root node.");
                }

                if (failure != value)
                {
                    failure = value;
                }
            }
        } 
    }
}
