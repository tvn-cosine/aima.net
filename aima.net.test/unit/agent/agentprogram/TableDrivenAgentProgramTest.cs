using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.agent.api;
using aima.net.agent;
using aima.net.agent.agentprogram;
using aima.net.collections.api;
using aima.net.collections;

namespace aima.net.test.unit.agent.agentprogram
{
    [TestClass]
    public class TableDrivenAgentProgramTest
    { 
        private static readonly IAction ACTION_1 = new DynamicAction("action1");
        private static readonly IAction ACTION_2 = new DynamicAction("action2");
        private static readonly IAction ACTION_3 = new DynamicAction("action3");

        private DynamicAgent agent;

        [TestInitialize]
        public void setUp()
        {
            IMap<ICollection<IPercept>, IAction> perceptSequenceActions = CollectionFactory.CreateInsertionOrderedMap<ICollection<IPercept>, IAction>();
            perceptSequenceActions.Put(createPerceptSequence(
                new DynamicPercept("key1", "value1")), ACTION_1);
            perceptSequenceActions.Put(createPerceptSequence(
                new DynamicPercept("key1", "value1"),
                new DynamicPercept("key1", "value2")), ACTION_2);
            perceptSequenceActions.Put(createPerceptSequence(
                new DynamicPercept("key1", "value1"),
                new DynamicPercept("key1", "value2"),
                new DynamicPercept("key1", "value3")), ACTION_3);

            agent = new DynamicAgent(new TableDrivenAgentProgram(perceptSequenceActions));
        }

        [TestMethod]
        public void testExistingSequences()
        {
            Assert.AreEqual(ACTION_1,
                    agent.Execute(new DynamicPercept("key1", "value1")));
            Assert.AreEqual(ACTION_2,
                    agent.Execute(new DynamicPercept("key1", "value2")));
            Assert.AreEqual(ACTION_3,
                    agent.Execute(new DynamicPercept("key1", "value3")));
        }

        [TestMethod]
        public void testNonExistingSequence()
        {
            Assert.AreEqual(ACTION_1,
                    agent.Execute(new DynamicPercept("key1", "value1")));
            Assert.AreEqual(aima.net.agent.DynamicAction.NO_OP,
                    agent.Execute(new DynamicPercept("key1", "value3")));
        }

        private static ICollection<IPercept> createPerceptSequence(params IPercept[] percepts)
        {
            ICollection<IPercept> perceptSequence = CollectionFactory.CreateQueue<IPercept>();

            foreach (IPercept p in percepts)
            {
                perceptSequence.Add(p);
            }

            return perceptSequence;
        }
    }

}
