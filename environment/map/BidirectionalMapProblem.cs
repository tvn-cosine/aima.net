using aima.net.collections.api;
using aima.net.search.framework.problem;
using aima.net.search.framework.problem.api;

namespace aima.net.environment.map
{
    public class BidirectionalMapProblem : GeneralProblem<string, MoveToAction>, IBidirectionalProblem<string, MoveToAction>
    {
        private IProblem<string, MoveToAction> reverseProblem;

        public BidirectionalMapProblem(Map map, string initialState, string goalState)
            : this(map, initialState, goalState, goalState.Equals)
        {

        }

        public BidirectionalMapProblem(Map map, string initialState, string goalState, GoalTest<string> goalTest)
            : base(initialState,
                  MapFunctions.createActionsFunction(map),
                  MapFunctions.createResultFunction(),
                  goalTest,
                  MapFunctions.createDistanceStepCostFunction(map))
        {  
            reverseProblem = new GeneralProblem<string, MoveToAction>(
                    goalState, 
                    MapFunctions.createReverseActionsFunction(map),
                    MapFunctions.createResultFunction(),
                    initialState.Equals,
                    MapFunctions.createDistanceStepCostFunction(map));
        }

        public IProblem<string, MoveToAction> getOriginalProblem()
        {
            return this;
        }

        public IProblem<string, MoveToAction> getReverseProblem()
        {
            return reverseProblem;
        }
    }
}
