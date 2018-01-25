using aima.net.collections.api;
using aima.net.learning.framework;
using aima.net.learning.framework.api;
using aima.net.learning.inductive;
using aima.net.util;

namespace aima.net.learning.learners
{ 
    public class DecisionTreeLearner : ILearner
    {
        private DecisionTree tree;
        private string defaultValue;

        public DecisionTreeLearner()
        {
            this.defaultValue = "Unable To Classify"; 
        }

        // used when you have to test a non induced tree (eg: for testing)
        public DecisionTreeLearner(DecisionTree tree, string defaultValue)
        {
            this.tree = tree;
            this.defaultValue = defaultValue;
        }
         
        /// <summary>
        /// Induces the decision tree from the specified set of examples
        /// </summary>
        /// <param name="ds">a set of examples for constructing the decision tree</param>
        public virtual void Train(DataSet ds)
        {
            ICollection<string> attributes = ds.getNonTargetAttributes();
            this.tree = decisionTreeLearning(ds, attributes, new ConstantDecisonTree(defaultValue));
        }

        public virtual string Predict(Example e)
        {
            return (string)tree.predict(e);
        }

        public virtual int[] Test(DataSet ds)
        {
            int[] results = new int[] { 0, 0 };

            foreach (Example e in ds.examples)
            {
                if (e.targetValue().Equals(tree.predict(e)))
                {
                    results[0] = results[0] + 1;
                }
                else
                {
                    results[1] = results[1] + 1;
                }
            }
            return results;
        }
        
        /// <summary>
        /// Returns the decision tree of this decision tree learner
        /// </summary>
        /// <returns>the decision tree of this decision tree learner</returns>
        public virtual DecisionTree getDecisionTree()
        {
            return tree;
        }
         
        private DecisionTree decisionTreeLearning(DataSet ds, ICollection<string> attributeNames, ConstantDecisonTree defaultTree)
        {
            if (ds.size() == 0)
            {
                return defaultTree;
            }
            if (allExamplesHaveSameClassification(ds))
            {
                return new ConstantDecisonTree(ds.getExample(0).targetValue());
            }
            if (attributeNames.Size() == 0)
            {
                return majorityValue(ds);
            }
            string chosenAttribute = chooseAttribute(ds, attributeNames);

            DecisionTree tree = new DecisionTree(chosenAttribute);
            ConstantDecisonTree m = majorityValue(ds);

            ICollection<string> values = ds.getPossibleAttributeValues(chosenAttribute);
            foreach (string v in values)
            {
                DataSet filtered = ds.matchingDataSet(chosenAttribute, v);
                ICollection<string> newAttribs = Util.removeFrom(attributeNames, chosenAttribute);
                DecisionTree subTree = decisionTreeLearning(filtered, newAttribs, m);
                tree.addNode(v, subTree);
            }

            return tree;
        }

        private ConstantDecisonTree majorityValue(DataSet ds)
        {
            ILearner learner = new MajorityLearner();
            learner.Train(ds);
            return new ConstantDecisonTree(learner.Predict(ds.getExample(0)));
        }

        private string chooseAttribute(DataSet ds, ICollection<string> attributeNames)
        {
            double greatestGain = 0.0;
            string attributeWithGreatestGain = attributeNames.Get(0);
            foreach (string attr in attributeNames)
            {
                double gain = ds.calculateGainFor(attr);
                if (gain > greatestGain)
                {
                    greatestGain = gain;
                    attributeWithGreatestGain = attr;
                }
            }

            return attributeWithGreatestGain;
        }

        private bool allExamplesHaveSameClassification(DataSet ds)
        {
            string classification = ds.getExample(0).targetValue();
            foreach (Example element in ds)
            {
                if (!(element.targetValue().Equals(classification)))
                {
                    return false;
                }
            }
            return true;
        }
    } 
}
