using aima.net.collections.api;
using aima.net.exceptions;
using aima.net.learning.framework;
using aima.net.learning.framework.api;
using aima.net.learning.inductive;

namespace aima.net.learning.learners
{
    public class DecisionListLearner : ILearner
    {
        public const string FAILURE = "Failure";

        private DecisionList decisionList;
        private string positive, negative;
        private DecisionListTestFactory testFactory;

        public DecisionListLearner(string positive, string negative, DecisionListTestFactory testFactory)
        {
            this.positive = positive;
            this.negative = negative;
            this.testFactory = testFactory;
        }

        /// <summary>
        /// Induces the decision list from the specified set of examples
        /// </summary>
        /// <param name="ds">a set of examples for constructing the decision list</param>
        public void Train(DataSet ds)
        {
            this.decisionList = decisionListLearning(ds);
        }

        public string Predict(Example e)
        {
            if (decisionList == null)
            {
                throw new RuntimeException("learner has not been trained with dataset yet!");
            }
            return decisionList.predict(e);
        }

        public int[] Test(DataSet ds)
        {
            int[] results = new int[] { 0, 0 };

            foreach (Example e in ds.examples)
            {
                if (e.targetValue().Equals(decisionList.predict(e)))
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
        /// Returns the decision list of this decision list learner
        /// </summary>
        /// <returns>the decision list of this decision list learner</returns>
        public DecisionList getDecisionList()
        {
            return decisionList;
        }

        private DecisionList decisionListLearning(DataSet ds)
        {
            if (ds.size() == 0)
            {
                return new DecisionList(positive, negative);
            }
            ICollection<DecisionListTest> possibleTests = testFactory.createDLTestsWithAttributeCount(ds, 1);
            DecisionListTest test = getValidTest(possibleTests, ds);
            if (test == null)
            {
                return new DecisionList(null, FAILURE);
            }
            // at this point there is a test that classifies some subset of examples
            // with the same target value
            DataSet matched = test.matchedExamples(ds);
            DecisionList list = new DecisionList(positive, negative);
            list.add(test, matched.getExample(0).targetValue());
            return list.mergeWith(decisionListLearning(test.unmatchedExamples(ds)));
        }

        private DecisionListTest getValidTest(ICollection<DecisionListTest> possibleTests, DataSet ds)
        {
            foreach (DecisionListTest test in possibleTests)
            {
                DataSet matched = test.matchedExamples(ds);
                if (!(matched.size() == 0))
                {
                    if (allExamplesHaveSameTargetValue(matched))
                    {
                        return test;
                    }
                }

            }
            return null;
        }

        private bool allExamplesHaveSameTargetValue(DataSet matched)
        {
            // assumes at least i example in dataset
            string targetValue = matched.getExample(0).targetValue();
            foreach (Example e in matched.examples)
            {
                if (!(e.targetValue().Equals(targetValue)))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
