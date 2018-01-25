using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.environment.wumpusworld;
using aima.net.environment.wumpusworld.action;
using aima.net.logic.propositional.inference;

namespace aima.net.test.unit.environment.wumpusworld
{
    [TestClass]
    public class WumpusKnowledgeBaseTest
    {
        private DPLL dpll = new DPLLSatisfiable();
       // private DPLL dpll = new OptimizedDPLL();

        private void step(WumpusKnowledgeBase KB, AgentPercept percept, int t)
        {
            KB.tellTemporalPhysicsSentences(t);
            KB.makePerceptSentence(percept, t);
        }
           
        [TestMethod]
        public void testAskCurrentPosition()
        {
            WumpusKnowledgeBase KB = new WumpusKnowledgeBase(dpll, 2); // Create very small cave in order to make inference for tests faster.
                                                                       // NOTE: in the 2x2 cave for this set of assertion tests, 
                                                                       // we are going to have no pits and the wumpus in [2,2]
                                                                       // this needs to be correctly set up in order to keep the KB consistent.
            int t = 0;
            AgentPosition current;
            step(KB, new AgentPercept(false, false, false, false, false), t);
            current = KB.askCurrentPosition(t);
            Assert.AreEqual(new AgentPosition(1, 1, AgentPosition.Orientation.FACING_EAST), current);
            KB.makeActionSentence(new Forward(current), t);

            t++;
            step(KB, new AgentPercept(true, false, false, false, false), t);
            current = KB.askCurrentPosition(t);
            Assert.AreEqual(new AgentPosition(2, 1, AgentPosition.Orientation.FACING_EAST), current);
            KB.makeActionSentence(new TurnLeft(current.getOrientation()), t);

            t++;
            step(KB, new AgentPercept(true, false, false, false, false), t);
            current = KB.askCurrentPosition(t);
            Assert.AreEqual(new AgentPosition(2, 1, AgentPosition.Orientation.FACING_NORTH), current);
            KB.makeActionSentence(new TurnLeft(current.getOrientation()), t);

            t++;
            step(KB, new AgentPercept(true, false, false, false, false), t);
            current = KB.askCurrentPosition(t);
            Assert.AreEqual(new AgentPosition(2, 1, AgentPosition.Orientation.FACING_WEST), current);
            KB.makeActionSentence(new Forward(current), t);

            t++;
            step(KB, new AgentPercept(false, false, false, false, false), t);
            current = KB.askCurrentPosition(t);
            Assert.AreEqual(new AgentPosition(1, 1, AgentPosition.Orientation.FACING_WEST), current);
            KB.makeActionSentence(new Forward(current), t);

            t++;
            step(KB, new AgentPercept(false, false, false, true, false), t);
            current = KB.askCurrentPosition(t);
            Assert.AreEqual(new AgentPosition(1, 1, AgentPosition.Orientation.FACING_WEST), current);
        }

        [TestMethod]
        public void testAskSafeRooms()
        {
            WumpusKnowledgeBase KB;
            int t = 0;

            KB = new WumpusKnowledgeBase(dpll, 2);
            step(KB, new AgentPercept(false, false, false, false, false), t);
            AreSetEqual(CollectionFactory.CreateSet<Room>(
                new Room(1, 1),
                new Room(1, 2),
                new Room(2, 1)), KB.askSafeRooms(t));

            KB = new WumpusKnowledgeBase(dpll, 2);

            step(KB, new AgentPercept(true, false, false, false, false), t);
            AreSetEqual(CollectionFactory.CreateSet<Room>(new Room(1, 1)), KB.askSafeRooms(t));

            KB = new WumpusKnowledgeBase(dpll, 2);

            step(KB, new AgentPercept(false, true, false, false, false), t);
            AreSetEqual(CollectionFactory.CreateSet<Room>(new Room(1, 1)), KB.askSafeRooms(t));

            KB = new WumpusKnowledgeBase(dpll, 2);

            step(KB, new AgentPercept(true, true, false, false, false), t);
            AreSetEqual(CollectionFactory.CreateSet<Room>(new Room(1, 1)), KB.askSafeRooms(t));
        }

        [TestMethod]
        public void testAskGlitter()
        {
            WumpusKnowledgeBase KB = new WumpusKnowledgeBase(dpll, 2);
            step(KB, new AgentPercept(false, false, false, false, false), 0);
            Assert.IsFalse(KB.askGlitter(0));
            step(KB, new AgentPercept(false, false, false, false, false), 1);
            Assert.IsFalse(KB.askGlitter(1));
            step(KB, new AgentPercept(false, false, true, false, false), 2);
            Assert.IsTrue(KB.askGlitter(2));
            step(KB, new AgentPercept(false, false, false, false, false), 3);
            Assert.IsFalse(KB.askGlitter(3));
        }


        [TestMethod]
        public void testAskUnvistedRooms()
        {
            WumpusKnowledgeBase KB;
            int t = 0;

            KB = new WumpusKnowledgeBase(dpll, 2);
            step(KB, new AgentPercept(false, false, false, false, false), t);
            AreSetEqual(CollectionFactory.CreateSet<Room>(new Room(1, 2),
                new Room(2, 1),
                new Room(2, 2)), KB.askUnvisitedRooms(t));
            KB.makeActionSentence(new Forward(new AgentPosition(1, 1, AgentPosition.Orientation.FACING_EAST)), t); // Move agent to [2,1]		

            t++;

            step(KB, new AgentPercept(true, false, false, false, false), t); // NOTE: Wumpus in [2,2] so have stench
            AreSetEqual(CollectionFactory.CreateSet<Room>(new Room(1, 2),
                new Room(2, 2)), KB.askUnvisitedRooms(t));
        }


        [TestMethod]
        public void testAskPossibleWumpusRooms()
        {
            WumpusKnowledgeBase KB;
            int t = 0;

            KB = new WumpusKnowledgeBase(dpll, 2);
            step(KB, new AgentPercept(false, false, false, false, false), t);
            AreSetEqual(CollectionFactory.CreateSet<Room>(new Room(2, 2)), KB.askPossibleWumpusRooms(t));

            KB = new WumpusKnowledgeBase(dpll, 2);

            step(KB, new AgentPercept(true, false, false, false, false), t);
            AreSetEqual(CollectionFactory.CreateSet<Room>(
                 new Room(1, 2),
                 new Room(2, 1)), KB.askPossibleWumpusRooms(t));

            KB = new WumpusKnowledgeBase(dpll, 3);

            step(KB, new AgentPercept(false, false, false, false, false), t);
            AreSetEqual(CollectionFactory.CreateSet<Room>(
                new Room(1, 3),
                new Room(2, 2),
                new Room(2, 3),
                new Room(3, 1),
                new Room(3, 2),
                new Room(3, 3)), KB.askPossibleWumpusRooms(t));
            KB.makeActionSentence(new Forward(new AgentPosition(1, 1, AgentPosition.Orientation.FACING_EAST)), t); // Move agent to [2,1]		
        }

        void AreSetEqual(ISet<Room> s1, ISet<Room> s2)
        {
            Assert.AreEqual(s1.Size(), s2.Size());
            foreach (Room room in s1)
            {
                Assert.IsTrue(s2.Contains(room));
            }
        }

        [TestMethod]
        public void testAskNotUnsafeRooms()
        {
            WumpusKnowledgeBase KB;
            int t = 0;

            KB = new WumpusKnowledgeBase(dpll, 2);
            step(KB, new AgentPercept(false, false, false, false, false), t);
            AreSetEqual(CollectionFactory.CreateSet<Room>(new Room(1, 1),
                new Room(1, 2),
                new Room(2, 1)), KB.askNotUnsafeRooms(t));

            KB = new WumpusKnowledgeBase(dpll, 2);

            step(KB, new AgentPercept(true, false, false, false, false), t);
            AreSetEqual(CollectionFactory.CreateSet<Room>(new Room(1, 1),
                new Room(1, 2),
                new Room(2, 1),
                new Room(2, 2)), KB.askNotUnsafeRooms(t));
        }

        [TestMethod]
        public void testExampleInSection7_2_described_pg268_AIMA3e()
        {
            // Make smaller in order to reduce the inference time required, this still covers all the relevant rooms for the example
            WumpusKnowledgeBase KB = new WumpusKnowledgeBase(dpll, 3);
            int t = 0;
            // 0
            step(KB, new AgentPercept(false, false, false, false, false), t);
            KB.makeActionSentence(new Forward(new AgentPosition(1, 1, AgentPosition.Orientation.FACING_EAST)), t);

            t++; // 1
            step(KB, new AgentPercept(false, true, false, false, false), t);
            KB.makeActionSentence(new TurnRight(AgentPosition.Orientation.FACING_EAST), t);

            t++; // 2
            step(KB, new AgentPercept(false, true, false, false, false), t);
            KB.makeActionSentence(new TurnRight(AgentPosition.Orientation.FACING_SOUTH), t);

            t++; // 3
            step(KB, new AgentPercept(false, true, false, false, false), t);
            KB.makeActionSentence(new Forward(new AgentPosition(2, 1, AgentPosition.Orientation.FACING_WEST)), t);

            t++; // 4
            step(KB, new AgentPercept(false, false, false, false, false), t);
            KB.makeActionSentence(new TurnRight(AgentPosition.Orientation.FACING_WEST), t);

            t++; // 5
            step(KB, new AgentPercept(false, false, false, false, false), t);
            KB.makeActionSentence(new Forward(new AgentPosition(1, 1, AgentPosition.Orientation.FACING_NORTH)), t);

            t++; // 6
            step(KB, new AgentPercept(true, false, false, false, false), t);

            Assert.IsTrue(KB.ask(KB.newSymbol(WumpusKnowledgeBase.LOCATION, t, 1, 2)));
            Assert.IsTrue(KB.ask(KB.newSymbol(WumpusKnowledgeBase.WUMPUS, 1, 3)));
            Assert.IsTrue(KB.ask(KB.newSymbol(WumpusKnowledgeBase.PIT, 3, 1)));
            Assert.IsTrue(KB.ask(KB.newSymbol(WumpusKnowledgeBase.OK_TO_MOVE_INTO, t, 2, 2)));
        }

    }
}
