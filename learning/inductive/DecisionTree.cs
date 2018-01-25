using aima.net.api;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.exceptions;
using aima.net.text;
using aima.net.text.api;
using aima.net.learning.framework;
using aima.net.util;

namespace aima.net.learning.inductive
{
    public class DecisionTree : IStringable
    {
        private string attributeName;

        // each node modelled as a hash of attribute_value/decisiontree
        private IMap<string, DecisionTree> nodes;

        protected DecisionTree()
        { }

        public DecisionTree(string attributeName)
        {
            this.attributeName = attributeName;
            nodes = CollectionFactory.CreateInsertionOrderedMap<string, DecisionTree>();
        }

        public virtual void addLeaf(string attributeValue, string decision)
        {
            nodes.Put(attributeValue, new ConstantDecisonTree(decision));
        }

        public virtual void addNode(string attributeValue, DecisionTree tree)
        {
            nodes.Put(attributeValue, tree);
        }

        public virtual object predict(Example e)
        {
            string attrValue = e.getAttributeValueAsString(attributeName);
            if (nodes.ContainsKey(attrValue))
            {
                return nodes.Get(attrValue).predict(e);
            }
            else
            {
                throw new RuntimeException("no node exists for attribute value " + attrValue);
            }
        }

        public static DecisionTree getStumpFor(DataSet ds, string attributeName,
                string attributeValue, string returnValueIfMatched,
                ICollection<string> unmatchedValues, string returnValueIfUnmatched)
        {
            DecisionTree dt = new DecisionTree(attributeName);
            dt.addLeaf(attributeValue, returnValueIfMatched);
            foreach (string unmatchedValue in unmatchedValues)
            {
                dt.addLeaf(unmatchedValue, returnValueIfUnmatched);
            }
            return dt;
        }

        public static ICollection<DecisionTree> getStumpsFor(DataSet ds,
                string returnValueIfMatched, string returnValueIfUnmatched)
        {
            ICollection<string> attributes = ds.getNonTargetAttributes();
            ICollection<DecisionTree> trees = CollectionFactory.CreateQueue<DecisionTree>();
            foreach (string attribute in attributes)
            {
                ICollection<string> values = ds.getPossibleAttributeValues(attribute);
                foreach (string value in values)
                {
                    ICollection<string> unmatchedValues = Util.removeFrom(ds.getPossibleAttributeValues(attribute), value);

                    DecisionTree tree = getStumpFor(ds, attribute, value,
                            returnValueIfMatched, unmatchedValues,
                            returnValueIfUnmatched);
                    trees.Add(tree);

                }
            }
            return trees;
        }

        /**
         * @return Returns the attributeName.
         */
        public virtual string getAttributeName()
        {
            return attributeName;
        }

        public override string ToString()
        {
            return ToString(1, TextFactory.CreateStringBuilder());
        }

        public virtual string ToString(int depth, IStringBuilder buf)
        {
            if (attributeName != null)
            {
                buf.Append(Util.ntimes("\t", depth));
                buf.Append(Util.ntimes("***", 1));
                buf.Append(attributeName + " \n");
                foreach (string attributeValue in nodes.GetKeys())
                {
                    buf.Append(Util.ntimes("\t", depth + 1));
                    buf.Append("+" + attributeValue);
                    buf.Append("\n");
                    DecisionTree child = nodes.Get(attributeValue);
                    buf.Append(child.ToString(depth + 1, TextFactory.CreateStringBuilder()));
                }
            }

            return buf.ToString();
        }
    }
}
