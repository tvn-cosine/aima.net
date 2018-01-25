using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.learning.framework;
using aima.net.learning.inductive;

namespace aima.net.test.unit.learning.inductive
{
    [TestClass]
    public class DecisionListTest
    {

        [TestMethod]
        public void testDecisonListWithNoTestsReturnsDefaultValue() 
        {
            DecisionList dlist = new DecisionList("Yes", "No");
            DataSet ds = DataSetFactory.getRestaurantDataSet();
            Assert.AreEqual("No", dlist.predict(ds.getExample(0)));
        }

        [TestMethod]
        public void testDecisionListWithSingleTestReturnsTestValueIfTestSuccessful()


        {
            DecisionList dlist = new DecisionList("Yes", "No");
            DataSet ds = DataSetFactory.getRestaurantDataSet();

            aima.net.learning.inductive.DecisionListTest test = new aima.net.learning.inductive.DecisionListTest();
            test.add("type", "French");

            dlist.add(test, "test1success");

            Assert.AreEqual("test1success", dlist.predict(ds.getExample(0)));
        }

        [TestMethod]
        public void testDecisionListFallsThruToNextTestIfOneDoesntMatch()


        {
            DecisionList dlist = new DecisionList("Yes", "No");
            DataSet ds = DataSetFactory.getRestaurantDataSet();

            aima.net.learning.inductive.DecisionListTest test1 = new aima.net.learning.inductive.DecisionListTest();
            test1.add("type", "Thai"); // doesn't match first example
            dlist.add(test1, "test1success");

            aima.net.learning.inductive.DecisionListTest test2 = new aima.net.learning.inductive.DecisionListTest();
            test2.add("type", "French");
            dlist.add(test2, "test2success");// matches first example

            Assert.AreEqual("test2success", dlist.predict(ds.getExample(0)));
        }

        [TestMethod]
        public void testDecisionListFallsThruToDefaultIfNoTestMatches()


        {
            DecisionList dlist = new DecisionList("Yes", "No");
            DataSet ds = DataSetFactory.getRestaurantDataSet();

            aima.net.learning.inductive.DecisionListTest test1 = new aima.net.learning.inductive.DecisionListTest();
            test1.add("type", "Thai"); // doesn't match first example
            dlist.add(test1, "test1success");

            aima.net.learning.inductive.DecisionListTest test2 = new aima.net.learning.inductive.DecisionListTest();
            test2.add("type", "Burger");
            dlist.add(test2, "test2success");// doesn't match first example

            Assert.AreEqual("No", dlist.predict(ds.getExample(0)));
        }

        [TestMethod]
        public void testDecisionListHandlesEmptyDataSet()
        {
            // tests first base case of recursion
            DecisionList dlist = new DecisionList("Yes", "No");

            aima.net.learning.inductive.DecisionListTest test1 = new aima.net.learning.inductive.DecisionListTest();
            test1.add("type", "Thai"); // doesn't match first example
            dlist.add(test1, "test1success");
        }

        [TestMethod]
        public void testDecisionListMerge()
        {
            DecisionList dlist1 = new DecisionList("Yes", "No");
            DecisionList dlist2 = new DecisionList("Yes", "No");
            DataSet ds = DataSetFactory.getRestaurantDataSet();

            aima.net.learning.inductive.DecisionListTest test1 = new aima.net.learning.inductive.DecisionListTest();
            test1.add("type", "Thai"); // doesn't match first example
            dlist1.add(test1, "test1success");

            aima.net.learning.inductive.DecisionListTest test2 = new aima.net.learning.inductive.DecisionListTest();
            test2.add("type", "French");
            dlist2.add(test2, "test2success");// matches first example

            DecisionList dlist3 = dlist1.mergeWith(dlist2);
            Assert.AreEqual("test2success", dlist3.predict(ds.getExample(0)));
        }
    } 
}
