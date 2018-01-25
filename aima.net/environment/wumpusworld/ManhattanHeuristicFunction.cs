using aima.net.agent.api;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.search.framework;
using aima.net.util;
using aima.net.util.api;

namespace aima.net.environment.wumpusworld
{
    /**
     * Heuristic for calculating the Manhattan distance between two rooms within a Wumpus World cave. 
     */
    public class ManhattanHeuristicFunction : IToDoubleFunction<Node<AgentPosition, IAction>>
    {
        ICollection<Room> goals = CollectionFactory.CreateQueue<Room>();

        public ManhattanHeuristicFunction(ISet<Room> goals)
        {
            this.goals.AddAll(goals);
        }

        public double applyAsDouble(Node<AgentPosition, IAction> node)
        {
            AgentPosition pos = node.getState();
            int nearestGoalDist = int.MaxValue;
            foreach (Room g in goals)
            {
                int tmp = evaluateManhattanDistanceOf(pos.getX(), pos.getY(), g.getX(), g.getY());

                if (tmp < nearestGoalDist)
                {
                    nearestGoalDist = tmp;
                }
            }

            return (double)nearestGoalDist;
        }

        private int evaluateManhattanDistanceOf(int x1, int y1, int x2, int y2)
        {
            return System.Math.Abs(x1 - x2) + System.Math.Abs(y1 - y2);
        }
    }
}
