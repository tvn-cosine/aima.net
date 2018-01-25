using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.agent.api;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.environment.wumpusworld;
using aima.net.environment.wumpusworld.action;

namespace aima.net.test.unit.environment.wumpusworld
{
    [TestClass]
    public class HybridWumpusAgentTest
    {
        private static ISet<Room> allRooms(int caveXandYDimensions)
        {
            ISet<Room> allRooms = CollectionFactory.CreateSet<Room>();
            for (int x = 1; x <= caveXandYDimensions; x++)
            {
                for (int y = 1; y <= caveXandYDimensions; y++)
                {
                    allRooms.Add(new Room(x, y));
                }
            }

            return allRooms;
        }

        [TestMethod]
        public void testGrabAndClimb()
        {
            HybridWumpusAgent hwa = new HybridWumpusAgent(2);
            // The gold is in the first square
            IAction a = hwa.Execute(new AgentPercept(true, true, true, false, false));
            Assert.IsTrue(a is Grab);
            a = hwa.Execute(new AgentPercept(true, true, true, false, false));
            Assert.IsTrue(a is Climb);
        }

        [TestMethod]
        public void testPlanShot()
        {
            HybridWumpusAgent hwa = new HybridWumpusAgent(4);
            // Should be just shoot as are facing the Wumpus
            Assert.AreEqual(CollectionFactory.CreateQueue<IAction>(new Shoot()),
                hwa.planShot(new AgentPosition(1, 1, AgentPosition.Orientation.FACING_EAST),
                    CollectionFactory.CreateSet<Room>(new Room(3, 1)),
                    allRooms(4)
            ));
            Assert.AreEqual(CollectionFactory.CreateQueue<IAction>(
                    new TurnLeft(AgentPosition.Orientation.FACING_EAST),
                    new Shoot()
                ),
                hwa.planShot(new AgentPosition(1, 1, AgentPosition.Orientation.FACING_EAST),
                    CollectionFactory.CreateSet<Room>(new Room(1, 2)),
                    allRooms(4)
            ));
            Assert.AreEqual(CollectionFactory.CreateQueue<IAction>(
                new Forward(new AgentPosition(1, 1, AgentPosition.Orientation.FACING_EAST)),
                new TurnLeft(AgentPosition.Orientation.FACING_EAST),
                new Shoot()
            ),
            hwa.planShot(new AgentPosition(1, 1, AgentPosition.Orientation.FACING_EAST),
                CollectionFactory.CreateSet<Room>(new Room(2, 2)),
                 allRooms(4)
            ));
        }

        [TestMethod]
        public void testPlanRoute()
        {
            HybridWumpusAgent hwa = new HybridWumpusAgent(4);
            // Should be a NoOp plan as we are already at the goal.
            Assert.AreEqual(CollectionFactory.CreateQueue<IAction>(),
                hwa.planRoute(new AgentPosition(1, 1, AgentPosition.Orientation.FACING_EAST),
                    CollectionFactory.CreateSet<Room>(new Room(1, 1)),

                allRooms(4)
            ));
            Assert.AreEqual(CollectionFactory.CreateQueue<IAction>(
                    new TurnLeft(AgentPosition.Orientation.FACING_EAST),
                    new TurnLeft(AgentPosition.Orientation.FACING_NORTH),
                    new Forward(new AgentPosition(2, 1, AgentPosition.Orientation.FACING_WEST))
                ),
                hwa.planRoute(new AgentPosition(2, 1, AgentPosition.Orientation.FACING_EAST),
                    CollectionFactory.CreateSet<Room>(new Room(1, 1)),

                allRooms(4)
            ));
            ISet<Room> impl = CollectionFactory.CreateSet<Room>(allRooms(4));
            impl.Remove(new Room(2, 1));
            impl.Remove(new Room(2, 2));

            Assert.AreEqual(CollectionFactory.CreateQueue<IAction>(
                    new TurnLeft(AgentPosition.Orientation.FACING_EAST),
                    new Forward(new AgentPosition(3, 1, AgentPosition.Orientation.FACING_NORTH)),
                    new Forward(new AgentPosition(3, 2, AgentPosition.Orientation.FACING_NORTH)),
                    new TurnLeft(AgentPosition.Orientation.FACING_NORTH),
                    new Forward(new AgentPosition(3, 3, AgentPosition.Orientation.FACING_WEST)),
                    new Forward(new AgentPosition(2, 3, AgentPosition.Orientation.FACING_WEST)),
                    new TurnLeft(AgentPosition.Orientation.FACING_WEST),
                    new Forward(new AgentPosition(1, 3, AgentPosition.Orientation.FACING_SOUTH)),
                    new Forward(new AgentPosition(1, 2, AgentPosition.Orientation.FACING_SOUTH))
                ),
                hwa.planRoute(new AgentPosition(3, 1, AgentPosition.Orientation.FACING_EAST),
                    CollectionFactory.CreateSet<Room>(new Room(1, 1)),
                    impl));
        }
    }
}
