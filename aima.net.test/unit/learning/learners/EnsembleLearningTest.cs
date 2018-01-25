using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.learning.framework;
using aima.net.learning.framework.api;
using aima.net.learning.inductive;
using aima.net.learning.learners;

namespace aima.net.test.unit.learning.learners
{
    [TestClass]
    public class EnsembleLearningTest
    {

        private static readonly string YES = "Yes";

        [TestMethod]
        public void testAdaBoostEnablesCollectionOfStumpsToClassifyDataSetAccurately()


        {
            DataSet ds = DataSetFactory.getRestaurantDataSet();
            ICollection<DecisionTree> stumps = DecisionTree.getStumpsFor(ds, YES, "No");
            ICollection<ILearner> learners = CollectionFactory.CreateQueue<ILearner>();
            foreach (object stump in stumps)
            {
                DecisionTree sl = (DecisionTree)stump;
                StumpLearner stumpLearner = new StumpLearner(sl, "No");
                learners.Add(stumpLearner);
            }
            AdaBoostLearner learner = new AdaBoostLearner(learners, ds);
            learner.Train(ds);
            int[] result = learner.Test(ds);
            Assert.AreEqual(12, result[0]);
            Assert.AreEqual(0, result[1]);
        }
    }
}
