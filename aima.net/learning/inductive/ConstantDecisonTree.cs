using aima.net.exceptions;
using aima.net.text.api;
using aima.net.learning.framework;
using aima.net.util;

namespace aima.net.learning.inductive
{ 
    public class ConstantDecisonTree : DecisionTree
    {
        // represents leaf nodes like "Yes" or "No"
        private string value;

        public ConstantDecisonTree(string value)
        {
            this.value = value;
        }

        public override void addLeaf(string attributeValue, string decision)
        {
            throw new RuntimeException("cannot add Leaf to ConstantDecisonTree");
        }

        public override void addNode(string attributeValue, DecisionTree tree)
        {
            throw new RuntimeException("cannot add Node to ConstantDecisonTree");
        }

        public override object predict(Example e)
        {
            return value;
        }

        public override string ToString()
        {
            return "DECISION -> " + value;
        }

        public override string ToString(int depth, IStringBuilder buf)
        {
            buf.Append(Util.ntimes("\t", depth + 1));
            buf.Append("DECISION -> " + value + "\n");
            return buf.ToString();
        }
    }
}
