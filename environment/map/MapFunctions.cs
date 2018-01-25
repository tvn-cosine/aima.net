using aima.net.agent.api;
using aima.net.agent;
using aima.net.collections.api;
using aima.net.search.framework;
using aima.net.search.framework.problem;
using aima.net.util;
using aima.net.util.math.geom.shapes;
using aima.net.collections;
using aima.net.search.framework.problem.api;
using aima.net.util.api;

namespace aima.net.environment.map
{
    public class MapFunctions
    { 
        public static IActionsFunction<string, MoveToAction> createActionsFunction(Map map)
        {
            return new MapActionsFunction(map, false);
        }

        public static IActionsFunction<string, MoveToAction> createReverseActionsFunction(Map map)
        {
            return new MapActionsFunction(map, true);
        }

        public static IResultFunction<string, MoveToAction> createResultFunction()
        {
            return new MapResultFunction();
        }

        public static IStepCostFunction<string, MoveToAction> createDistanceStepCostFunction(Map map)
        {
            return new DistanceStepCostFunction(map);
        }

        public static Function<IPercept, string> createPerceptToStateFunction()
        {
            return p => (string)((DynamicPercept)p).GetAttribute(DynAttributeNames.PERCEPT_IN);
        }

        /** Returns a heuristic function based on straight line distance computation. */
        public static IToDoubleFunction<Node<string, MoveToAction>> createSLDHeuristicFunction(string goal, Map map)
        {
            return new SLDHeuristicFunction(goal, map);
        }

        public static double getSLD(string loc1, string loc2, Map map)
        {
            double result = 0.0;
            Point2D pt1 = map.getPosition(loc1);
            Point2D pt2 = map.getPosition(loc2);
            if (pt1 != null && pt2 != null)
                result = pt1.distance(pt2);
            return result;
        }

        public class SLDHeuristicFunction : IToDoubleFunction<Node<string, MoveToAction>>
        {
            private string goal;
            private Map map;

            public SLDHeuristicFunction(string goal, Map map)
            {
                this.goal = goal;
                this.map = map;
            }

            public double applyAsDouble(Node<string, MoveToAction> node)
            {
                return getSLD(node.getState(), goal, map);
            }
        }

        public class MapActionsFunction : IActionsFunction<string, MoveToAction>
        {
            private Map map = null;
            private bool reverseMode;

            public MapActionsFunction(Map map, bool reverseMode)
            {
                this.map = map;
                this.reverseMode = reverseMode;
            }

            public ICollection<MoveToAction> apply(string state)
            {
                ICollection<MoveToAction> actions = CollectionFactory.CreateQueue<MoveToAction>();

                ICollection<string> linkedLocations = reverseMode ? map.getPossiblePrevLocations(state) : map.getPossibleNextLocations(state);

                foreach (string location in linkedLocations)
                {
                    actions.Add(new MoveToAction(location));

                }
                return actions;
            }
        }

        public class MapResultFunction : IResultFunction<string, MoveToAction>
        {

            public string apply(string s, MoveToAction a)
            {
                if (a != null)
                    return a.getToLocation();
                // If the action is NoOp the result will be the current state.
                return s;
            }
        }

        /**
         * Implementation of StepCostFunction interface that uses the distance between
         * locations to calculate the cost in addition to a constant cost, so that it
         * may be used in conjunction with a Uniform-cost search.
         */
        public class DistanceStepCostFunction : IStepCostFunction<string, MoveToAction>
        {
            private Map map = null;

            // Used by Uniform-cost search to ensure every step is greater than or equal
            // to some small positive constant
            private static double constantCost = 1.0;

            public DistanceStepCostFunction(Map map)
            {
                this.map = map;
            }

            public double applyAsDouble(string state, MoveToAction action, string statePrimed)
            {
                double? distance = map.getDistance(state, statePrimed);
                if (null == distance || distance <= 0)
                    return constantCost;
                return distance.Value;
            }
        }
    } 
}
