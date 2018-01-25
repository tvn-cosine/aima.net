﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.learning.framework;
using aima.net.learning.inductive;
using aima.net.learning.learners;
using aima.net.util;

namespace aima.net.test.unit.learning.learners
{
    [TestClass]
    public class DecisionTreeTest
    {
        private static readonly string YES = "Yes";

        private static readonly string NO = "No";

        [TestMethod]
        public void testActualDecisionTreeClassifiesRestaurantDataSetCorrectly() 
        {
            DecisionTreeLearner learner = new DecisionTreeLearner(
                    createActualRestaurantDecisionTree(), "Unable to clasify");
            int[] results = learner.Test(DataSetFactory.getRestaurantDataSet());
            Assert.AreEqual(12, results[0]);
            Assert.AreEqual(0, results[1]);
        }

        [TestMethod]
        public void testInducedDecisionTreeClassifiesRestaurantDataSetCorrectly() 
        {
            DecisionTreeLearner learner = new DecisionTreeLearner(
                    createInducedRestaurantDecisionTree(), "Unable to clasify");
            int[] results = learner.Test(DataSetFactory.getRestaurantDataSet());
            Assert.AreEqual(12, results[0]);
            Assert.AreEqual(0, results[1]);
        }

        [TestMethod]
        public void testStumpCreationForSpecifiedAttributeValuePair() 
        {
            DataSet ds = DataSetFactory.getRestaurantDataSet();
            ICollection<string> unmatchedValues = CollectionFactory.CreateQueue<string>();
            unmatchedValues.Add(NO);
            DecisionTree dt = DecisionTree.getStumpFor(ds, "alternate", YES, YES,
                    unmatchedValues, NO);
            Assert.IsNotNull(dt);
        }

        [TestMethod]
        public void testStumpCreationForDataSet()
        {
            DataSet ds = DataSetFactory.getRestaurantDataSet();
            ICollection<DecisionTree> dt = DecisionTree.getStumpsFor(ds, YES,
                           "Unable to classify");
            Assert.AreEqual(26, dt.Size());
        }

        [TestMethod]
        public void testStumpPredictionForDataSet()
        {
            DataSet ds = DataSetFactory.getRestaurantDataSet();

            ICollection<string> unmatchedValues = CollectionFactory.CreateQueue<string>();
            unmatchedValues.Add(NO);
            DecisionTree tree = DecisionTree.getStumpFor(ds, "hungry", YES, YES,
                    unmatchedValues, "Unable to Classify");
            DecisionTreeLearner learner = new DecisionTreeLearner(tree,
                    "Unable to Classify");
            int[] result = learner.Test(ds);
            Assert.AreEqual(5, result[0]);
            Assert.AreEqual(7, result[1]);
        }

        //
        // PRIVATE METHODS
        //
        private static DecisionTree createInducedRestaurantDecisionTree()
        {
            // from AIMA 2nd ED
            // Fig 18.6
            // friday saturday node
            DecisionTree frisat = new DecisionTree("fri/sat");
            frisat.addLeaf(Util.YES, Util.YES);
            frisat.addLeaf(Util.NO, Util.NO);

            // type node
            DecisionTree type = new DecisionTree("type");
            type.addLeaf("French", Util.YES);
            type.addLeaf("Italian", Util.NO);
            type.addNode("Thai", frisat);
            type.addLeaf("Burger", Util.YES);

            // hungry node
            DecisionTree hungry = new DecisionTree("hungry");
            hungry.addLeaf(Util.NO, Util.NO);
            hungry.addNode(Util.YES, type);

            // patrons node
            DecisionTree patrons = new DecisionTree("patrons");
            patrons.addLeaf("None", Util.NO);
            patrons.addLeaf("Some", Util.YES);
            patrons.addNode("Full", hungry);

            return patrons;
        }

        private static DecisionTree createActualRestaurantDecisionTree()
        {
            // from AIMA 2nd ED
            // Fig 18.2

            // raining node
            DecisionTree raining = new DecisionTree("raining");
            raining.addLeaf(Util.YES, Util.YES);
            raining.addLeaf(Util.NO, Util.NO);

            // bar node
            DecisionTree bar = new DecisionTree("bar");
            bar.addLeaf(Util.YES, Util.YES);
            bar.addLeaf(Util.NO, Util.NO);

            // friday saturday node
            DecisionTree frisat = new DecisionTree("fri/sat");
            frisat.addLeaf(Util.YES, Util.YES);
            frisat.addLeaf(Util.NO, Util.NO);

            // second alternate node to the right of the diagram below hungry
            DecisionTree alternate2 = new DecisionTree("alternate");
            alternate2.addNode(Util.YES, raining);
            alternate2.addLeaf(Util.NO, Util.YES);

            // reservation node
            DecisionTree reservation = new DecisionTree("reservation");
            frisat.addNode(Util.NO, bar);
            frisat.addLeaf(Util.YES, Util.YES);

            // first alternate node to the left of the diagram below waitestimate
            DecisionTree alternate1 = new DecisionTree("alternate");
            alternate1.addNode(Util.NO, reservation);
            alternate1.addNode(Util.YES, frisat);

            // hungry node
            DecisionTree hungry = new DecisionTree("hungry");
            hungry.addLeaf(Util.NO, Util.YES);
            hungry.addNode(Util.YES, alternate2);

            // wait estimate node
            DecisionTree waitEstimate = new DecisionTree("wait_estimate");
            waitEstimate.addLeaf(">60", Util.NO);
            waitEstimate.addNode("30-60", alternate1);
            waitEstimate.addNode("10-30", hungry);
            waitEstimate.addLeaf("0-10", Util.YES);

            // patrons node
            DecisionTree patrons = new DecisionTree("patrons");
            patrons.addLeaf("None", Util.NO);
            patrons.addLeaf("Some", Util.YES);
            patrons.addNode("Full", waitEstimate);

            return patrons;
        }
    }

}
