using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.environment.nqueens;
using aima.net.search.framework.problem;
using aima.net.search.local;
using aima.net.search.local.api;

namespace aima.net.test.unit.environment.nqueens
{
    [TestClass]
    public class NQueensGenAlgoUtilTest
    { 
        private IFitnessFunction<int> fitnessFunction;
        private GoalTest<Individual<int>> goalTest;

        [TestInitialize]
        public void setUp()
        {
            fitnessFunction = NQueensGenAlgoUtil.getFitnessFunction();
            goalTest = NQueensGenAlgoUtil.getGoalTest();
        }

        [TestMethod]
        public void test_getValue()
        {
            Assert.IsTrue(0.0 == fitnessFunction.apply(
                new Individual<int>(CollectionFactory.CreateQueue<int>(new[] { 0, 0, 0, 0, 0, 0, 0, 0 }))));
            Assert.IsTrue(0.0 == fitnessFunction.apply(
                new Individual<int>(CollectionFactory.CreateQueue<int>(new[] { 0, 1, 2, 3, 4, 5, 6, 7 }))));
            Assert.IsTrue(0.0 == fitnessFunction.apply(
                new Individual<int>(CollectionFactory.CreateQueue<int>(new[] { 7, 6, 5, 4, 3, 2, 1, 0 })))); 
            Assert.IsTrue(23.0 == fitnessFunction.apply(
                new Individual<int>(CollectionFactory.CreateQueue<int>(new[] { 5, 6, 1, 3, 6, 4, 7, 7 }))));
            Assert.IsTrue(28.0 == fitnessFunction.apply(
                new Individual<int>(CollectionFactory.CreateQueue<int>(new[] { 0, 4, 7, 5, 2, 6, 1, 3 }))));
        }

        [TestMethod]
        public void test_isGoalState()
        {
            Assert.IsTrue(goalTest(new Individual<int>(
                    CollectionFactory.CreateQueue<int>(new[] { 0, 4, 7, 5, 2, 6, 1, 3 }))));
            Assert.IsFalse(goalTest(new Individual<int>(
                   CollectionFactory.CreateQueue<int>(new[] { 0, 0, 0, 0, 0, 0, 0, 0 }))));
            Assert.IsFalse(goalTest(new Individual<int>(
                   CollectionFactory.CreateQueue<int>(new[] { 5, 6, 1, 3, 6, 4, 7, 7 }))));
        }

        [TestMethod]
        public void test_getBoardForIndividual()
        {
            NQueensBoard board = NQueensGenAlgoUtil
                    .getBoardForIndividual(new Individual<int>(CollectionFactory.CreateQueue<int>(new[] { 5, 6, 1, 3, 6, 4, 7, 7 })));
            Assert.AreEqual(" -  -  -  -  -  -  -  - \n"
                    + " -  -  Q  -  -  -  -  - \n" + " -  -  -  -  -  -  -  - \n"
                    + " -  -  -  Q  -  -  -  - \n" + " -  -  -  -  -  Q  -  - \n"
                    + " Q  -  -  -  -  -  -  - \n" + " -  Q  -  -  Q  -  -  - \n"
                    + " -  -  -  -  -  -  Q  Q \n", board.getBoardPic());

            Assert.AreEqual("--------\n" + "--Q-----\n" + "--------\n"
                    + "---Q----\n" + "-----Q--\n" + "Q-------\n" + "-Q--Q---\n"
                    + "------QQ\n", board.ToString());
        }

        [TestMethod]
        public void test_generateRandomIndividual()
        {
            for (int i = 2; i <= 40; ++i)
            {
                Individual<int> individual = NQueensGenAlgoUtil.generateRandomIndividual(i);
                Assert.AreEqual(i, individual.length());
            }
        }

        [TestMethod]
        public void test_getFiniteAlphabet()
        {
            for (int i = 2; i <= 40; ++i)
            {
                ICollection<int> fab = NQueensGenAlgoUtil.getFiniteAlphabetForBoardOfSize(i);
                Assert.AreEqual(i, fab.Size());
            }
        }
    }
}
