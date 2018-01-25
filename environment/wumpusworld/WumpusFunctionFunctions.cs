using aima.net.agent.api;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.environment.wumpusworld.action;
using aima.net.search.framework.problem;
using aima.net.search.framework.problem.api;

namespace aima.net.environment.wumpusworld
{
    /**
     * Factory class for constructing functions for use in the Wumpus World environment. 
     */
    public class WumpusFunctionFunctions
    {
        public static IActionsFunction<AgentPosition, IAction> createActionsFunction(WumpusCave cave)
        {
            return new WumpusActionsFunction(cave);
        }

        public static IResultFunction<AgentPosition, IAction> createResultFunction()
        {
            return new WumpusResultFunction();
        }

        private class WumpusActionsFunction : IActionsFunction<AgentPosition, IAction>
        {

            private WumpusCave cave;

            public WumpusActionsFunction(WumpusCave cave)
            {
                this.cave = cave;
            }

            public ICollection<IAction> apply(AgentPosition state)
            {
                ICollection<IAction> actions = CollectionFactory.CreateQueue<IAction>();

                ICollection<AgentPosition> linkedPositions = cave.getLocationsLinkedTo(state);
                foreach (AgentPosition linkPos in linkedPositions)
                {
                    if (linkPos.getX() != state.getX()
                        || linkPos.getY() != state.getY())
                    {
                        actions.Add(new Forward(state));
                    }
                }

                actions.Add(new TurnLeft(state.getOrientation()));
                actions.Add(new TurnRight(state.getOrientation()));

                return actions;
            }
        }

        private class WumpusResultFunction : IResultFunction<AgentPosition, IAction>
        {
            public AgentPosition apply(AgentPosition state, IAction action)
            {

                if (action is Forward)
                {
                    Forward fa = (Forward)action;

                    return fa.getToPosition();
                }
                else if (action is TurnLeft)
                {
                    TurnLeft tLeft = (TurnLeft)action;
                    return new AgentPosition(state.getX(), state.getY(), tLeft.getToOrientation());
                }
                else if (action is TurnRight)
                {
                    TurnRight tRight = (TurnRight)action;
                    return new AgentPosition(state.getX(), state.getY(), tRight.getToOrientation());
                }
                // The Action is not understood or is a NoOp
                // the result will be the current state.
                return state;
            }
        }
    }

}
