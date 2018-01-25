using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.collections.api;
using aima.net.learning.framework;
using aima.net.learning.inductive;

namespace aima.net.test.unit.learning.inductive
{
    [TestClass]
    public class DLTestTest
    {
        [TestMethod]
        public void testDecisionList()
        {
            DataSet ds = DataSetFactory.getRestaurantDataSet();
            ICollection<aima.net.learning.inductive.DecisionListTest> dlTests = new DecisionListTestFactory()
                       .createDLTestsWithAttributeCount(ds, 1);
            Assert.AreEqual(26, dlTests.Size());
        }

        [TestMethod]
        public void testDLTestMatchSucceedsWithMatchedExample()
        {
            DataSet ds = DataSetFactory.getRestaurantDataSet();
            Example e = ds.getExample(0);
            aima.net.learning.inductive.DecisionListTest test = new aima.net.learning.inductive.DecisionListTest();
            test.add("type", "French");
            Assert.IsTrue(test.matches(e));
        }

        [TestMethod]
        public void testDLTestMatchFailsOnMismatchedExample()
        {
            DataSet ds = DataSetFactory.getRestaurantDataSet();
            Example e = ds.getExample(0);
            aima.net.learning.inductive.DecisionListTest test = new aima.net.learning.inductive.DecisionListTest();
            test.add("type", "Thai");
            Assert.IsFalse(test.matches(e));
        }

        [TestMethod]
        public void testDLTestMatchesEvenOnMismatchedTargetAttributeValue()


        {
            DataSet ds = DataSetFactory.getRestaurantDataSet();
            Example e = ds.getExample(0);
            aima.net.learning.inductive.DecisionListTest test = new aima.net.learning.inductive.DecisionListTest();
            test.add("type", "French");
            Assert.IsTrue(test.matches(e));
        }

        [TestMethod]
        public void testDLTestReturnsMatchedAndUnmatchedExamplesCorrectly()


        {
            DataSet ds = DataSetFactory.getRestaurantDataSet();
            aima.net.learning.inductive.DecisionListTest test = new aima.net.learning.inductive.DecisionListTest();
            test.add("type", "Burger");

            DataSet matched = test.matchedExamples(ds);
            Assert.AreEqual(4, matched.size());

            DataSet unmatched = test.unmatchedExamples(ds);
            Assert.AreEqual(8, unmatched.size());
        }
    } 
}
