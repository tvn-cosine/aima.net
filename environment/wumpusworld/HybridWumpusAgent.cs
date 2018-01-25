using aima.net.agent.api;
using aima.net.agent;
using aima.net.collections.api;
using aima.net.exceptions;
using aima.net.environment.wumpusworld.action;
using aima.net.search.framework;
using aima.net.search.framework.agent;
using aima.net.search.framework.problem;
using aima.net.search.framework.qsearch;
using aima.net.search.informed;
using aima.net.util;
using aima.net.collections;
using aima.net.search.framework.api;
using aima.net.search.framework.problem.api;
using aima.net.util.api;

namespace aima.net.environment.wumpusworld
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 270.<br>
     * <br>
     * 
     * <pre>
     * <code>
     * function HYBRID-WUMPUS-AGENT(percept) returns an action
     *   inputs: percept, a list, [stench, breeze, glitter, bump, scream]
     *   persistent: KB, a knowledge base, initially the atemporal "wumpus physics"
     *               t, a counter, initially 0, indicating time
     *               plan, an action sequence, initially empty
     * 
     *   TELL(KB, MAKE-PERCEPT-SENTENCE(percept, t))
     *   TELL the KB the temporal "physics" sentences for time t
     *   safe <- {[x, y] : ASK(KB, OK<sup>t</sup><sub>x,y</sub>) = true}
     *   if ASK(KB, Glitter<sup>t</sup>) = true then
     *      plan <- [Grab] + PLAN-ROUTE(current, {[1,1]}, safe) + [Climb]
     *   if plan is empty then
     *      unvisited <- {[x, y] : ASK(KB, L<sup>t'</sup><sub>x,y</sub>) = false for all t' &le; t}
     *      plan <- PLAN-ROUTE(current, unvisited &cap; safe, safe)
     *   if plan is empty and ASK(KB, HaveArrow<sup>t</sup>) = true then
     *      possible_wumpus <- {[x, y] : ASK(KB, ~W<sub>x,y</sub>) = false}
     *      plan <- PLAN-SHOT(current, possible_wumpus, safe)
     *   if plan is empty then //no choice but to take a risk
     *      not_unsafe <- {[x, y] : ASK(KB, ~OK<sup>t</sup><sub>x,y</sub>) = false}
     *      plan <- PLAN-ROUTE(current, unvisited &cap; not_unsafe, safe)
     *   if plan is empty then
     *      plan <- PLAN-ROUTE(current, {[1,1]}, safe) + [Climb]
     *   action <- POP(plan)
     *   TELL(KB, MAKE-ACTION-SENTENCE(action, t))
     *   t <- t+1
     *   return action
     * 
     * --------------------------------------------------------------------------------
     * 
     * function PLAN-ROUTE(current, goals, allowed) returns an action sequence
     *   inputs: current, the agent's current position
     *           goals, a set of squares; try to plan a route to one of them
     *           allowed, a set of squares that can form part of the route 
     *    
     *   problem <- ROUTE-PROBLEM(current, goals, allowed)
     *   return A*-GRAPH-SEARCH(problem)
     * </code>
     * </pre>
     * 
     * Figure 7.20 A hybrid agent program for the wumpus world. It uses a
     * propositional knowledge base to infer the state of the world, and a
     * combination of problem-solving search and domain-specific code to decide what
     * actions to take
     * 
     * @author Federico Baron
     * @author Alessandro Daniele
     * @author Ciaran O'Reilly
     * @author Ruediger Lunde
     */
    public class HybridWumpusAgent : DynamicAgent
    {
        // persistent: KB, a knowledge base, initially the atemporal
        // "wumpus physics"
        private WumpusKnowledgeBase kb = null;
        // t, a counter, initially 0, indicating time
        private int t = 0;
        // plan, an action sequence, initially empty
        private ICollection<IAction> plan = CollectionFactory.CreateFifoQueue<IAction>(); // FIFOQueue

        /**
         * function HYBRID-WUMPUS-AGENT(percept) returns an action<br>
         * 
         * @param percept
         *            a list, [stench, breeze, glitter, bump, scream]
         * 
         * @return an action the agent should take.
         */
        public override IAction Execute(IPercept percept)
        {
            // TELL(KB, MAKE-PERCEPT-SENTENCE(percept, t))
            kb.makePerceptSentence((AgentPercept)percept, t);
            // TELL the KB the temporal "physics" sentences for time t
            kb.tellTemporalPhysicsSentences(t);

            AgentPosition current = kb.askCurrentPosition(t);

            // safe <- {[x, y] : ASK(KB, OK<sup>t</sup><sub>x,y</sub>) = true}
            ISet<Room> safe = kb.askSafeRooms(t);

            // if ASK(KB, Glitter<sup>t</sup>) = true then
            if (kb.askGlitter(t))
            {
                // plan <- [Grab] + PLAN-ROUTE(current, {[1,1]}, safe) + [Climb]
                ISet<Room> goals = CollectionFactory.CreateSet<Room>();
                goals.Add(new Room(1, 1));

                plan.Add(new Grab());
                plan.AddAll(planRoute(current, goals, safe));
                plan.Add(new Climb());
            }

            // if plan is empty then
            // unvisited <- {[x, y] : ASK(KB, L<sup>t'</sup><sub>x,y</sub>) = false
            // for all t' &le; t}
            ISet<Room> unvisited = kb.askUnvisitedRooms(t);
            if (plan.IsEmpty())
            {
                // plan <- PLAN-ROUTE(current, unvisited &cap; safe, safe)
                plan.AddAll(planRoute(current, SetOps.intersection(unvisited, safe), safe));
            }

            // if plan is empty and ASK(KB, HaveArrow<sup>t</sup>) = true then
            if (plan.IsEmpty() && kb.askHaveArrow(t))
            {
                // possible_wumpus <- {[x, y] : ASK(KB, ~W<sub>x,y</sub>) = false}
                ISet<Room> possibleWumpus = kb.askPossibleWumpusRooms(t);
                // plan <- PLAN-SHOT(current, possible_wumpus, safe)
                plan.AddAll(planShot(current, possibleWumpus, safe));
            }

            // if plan is empty then //no choice but to take a risk
            if (plan.IsEmpty())
            {
                // not_unsafe <- {[x, y] : ASK(KB, ~OK<sup>t</sup><sub>x,y</sub>) =
                // false}
                ISet<Room> notUnsafe = kb.askNotUnsafeRooms(t);
                // plan <- PLAN-ROUTE(current, unvisited &cap; not_unsafe, safe)
                plan.AddAll(planRoute(current, SetOps.intersection(unvisited, notUnsafe), safe));
            }

            // if plan is empty then
            if (plan.IsEmpty())
            {
                // plan PLAN-ROUTE(current, {[1,1]}, safe) + [Climb]
                ISet<Room> start = CollectionFactory.CreateSet<Room>();
                start.Add(new Room(1, 1));
                plan.AddAll(planRoute(current, start, safe));
                plan.Add(new Climb());
            }
            // action <- POP(plan)
            IAction action = plan.Pop();
            // TELL(KB, MAKE-ACTION-SENTENCE(action, t))
            kb.makeActionSentence(action, t);
            // t <- t+1
            t = t + 1;
            // return action
            return action;
        }

        /**
         * Returns a sequence of actions using A* Search.
         * 
         * @param current
         *            the agent's current position
         * @param goals
         *            a set of squares; try to plan a route to one of them
         * @param allowed
         *            a set of squares that can form part of the route
         * 
         * @return the best sequence of actions that the agent have to do to reach a
         *         goal from the current position.
         */
        public ICollection<IAction> planRoute(AgentPosition current, ISet<Room> goals, ISet<Room> allowed)
        {

            // Every square represent 4 possible positions for the agent, it could
            // be in different orientations. For every square in allowed and goals
            // sets we add 4 squares.
            ISet<AgentPosition> allowedPositions = CollectionFactory.CreateSet<AgentPosition>();
            foreach (Room allowedRoom in allowed)
            {
                int x = allowedRoom.getX();
                int y = allowedRoom.getY();

                allowedPositions.Add(new AgentPosition(x, y, AgentPosition.Orientation.FACING_WEST));
                allowedPositions.Add(new AgentPosition(x, y, AgentPosition.Orientation.FACING_EAST));
                allowedPositions.Add(new AgentPosition(x, y, AgentPosition.Orientation.FACING_NORTH));
                allowedPositions.Add(new AgentPosition(x, y, AgentPosition.Orientation.FACING_SOUTH));
            }
            ISet<AgentPosition> goalPositions = CollectionFactory.CreateSet<AgentPosition>();
            foreach (Room goalRoom in goals)
            {
                int x = goalRoom.getX();
                int y = goalRoom.getY();

                goalPositions.Add(new AgentPosition(x, y, AgentPosition.Orientation.FACING_WEST));
                goalPositions.Add(new AgentPosition(x, y, AgentPosition.Orientation.FACING_EAST));
                goalPositions.Add(new AgentPosition(x, y, AgentPosition.Orientation.FACING_NORTH));
                goalPositions.Add(new AgentPosition(x, y, AgentPosition.Orientation.FACING_SOUTH));
            }

            WumpusCave cave = new WumpusCave(kb.getCaveXDimension(), kb.getCaveYDimension(), allowedPositions);

            GoalTest<AgentPosition> goalTest = goalPositions.Contains;

            IProblem<AgentPosition, IAction> problem = new GeneralProblem<AgentPosition, IAction>(current,
                    WumpusFunctionFunctions.createActionsFunction(cave),
                    WumpusFunctionFunctions.createResultFunction(), goalTest);

            IToDoubleFunction<Node<AgentPosition, IAction>> h = new ManhattanHeuristicFunction(goals);

            ISearchForActions<AgentPosition, IAction> search = new AStarSearch<AgentPosition, IAction>(
                new GraphSearch<AgentPosition, IAction>(), h);
            SearchAgent<AgentPosition, IAction> agent;
            ICollection<IAction> actions = null;
            try
            {
                agent = new SearchAgent<AgentPosition, IAction>(problem, search);
                actions = agent.getActions();
                // Search agent can return a NoOp if already at goal,
                // in the context of this agent we will just return
                // no actions.
                if (actions.Size() == 1 && actions.Get(0).IsNoOp())
                {
                    actions = CollectionFactory.CreateQueue<IAction>();
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return actions;
        }

        /**
         * 
         * @param current
         *            the agent's current position
         * @param possibleWumpus
         *            a set of squares where we don't know that there isn't the
         *            wumpus.
         * @param allowed
         *            a set of squares that can form part of the route
         * 
         * @return the sequence of actions to reach the nearest square that is in
         *         line with a possible wumpus position. The last action is a shot.
         */
        public ICollection<IAction> planShot(AgentPosition current, ISet<Room> possibleWumpus, ISet<Room> allowed)
        {
            ISet<AgentPosition> shootingPositions = CollectionFactory.CreateSet<AgentPosition>();

            foreach (Room p in possibleWumpus)
            {
                int x = p.getX();
                int y = p.getY();

                for (int i = 1; i <= kb.getCaveXDimension(); i++)
                {
                    if (i < x)
                    {
                        shootingPositions.Add(new AgentPosition(i, y, AgentPosition.Orientation.FACING_EAST));
                    }
                    if (i > x)
                    {
                        shootingPositions.Add(new AgentPosition(i, y, AgentPosition.Orientation.FACING_WEST));
                    }
                    if (i < y)
                    {
                        shootingPositions.Add(new AgentPosition(x, i, AgentPosition.Orientation.FACING_NORTH));
                    }
                    if (i > y)
                    {
                        shootingPositions.Add(new AgentPosition(x, i, AgentPosition.Orientation.FACING_SOUTH));
                    }
                }
            }

            // Can't have a shooting position from any of the rooms the wumpus could
            // reside
            foreach (Room p in possibleWumpus)
            {
                foreach (AgentPosition.Orientation orientation in AgentPosition.Orientation.values())
                {
                    shootingPositions.Remove(new AgentPosition(p.getX(), p.getY(), orientation));
                }
            }

            ISet<Room> shootingPositionsArray = CollectionFactory.CreateSet<Room>();
            foreach (AgentPosition tmp in shootingPositions)
            {
                shootingPositionsArray.Add(new Room(tmp.getX(), tmp.getY()));
            }

            ICollection<IAction> actions = planRoute(current, shootingPositionsArray, allowed);

            AgentPosition newPos = current;
            if (actions.Size() > 0)
            {
                newPos = ((Forward)actions.Get(actions.Size() - 1)).getToPosition();
            }

            while (!shootingPositions.Contains(newPos))
            {
                TurnLeft tLeft = new TurnLeft(newPos.getOrientation());
                newPos = new AgentPosition(newPos.getX(), newPos.getY(), tLeft.getToOrientation());
                actions.Add(tLeft);
            }

            actions.Add(new Shoot());
            return actions;
        }

        //
        // SUPPORTING CODE
        //
        public HybridWumpusAgent()
            : this(4)
        {
            // i.e. default is a 4x4 world as depicted in figure 7.2
        }

        public HybridWumpusAgent(int caveXandYDimensions)
        {
            kb = new WumpusKnowledgeBase(caveXandYDimensions);
        }
    }
}
