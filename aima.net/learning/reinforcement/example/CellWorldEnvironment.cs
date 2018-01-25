using aima.net.agent.api;
using aima.net.agent;
using aima.net.api;
using aima.net.collections.api;
using aima.net.exceptions;
using aima.net.environment.cellworld;
using aima.net.probability.mdp;
using aima.net.collections;
using aima.net.probability.mdp.api;

namespace aima.net.learning.reinforcement.example
{
    /// <summary> 
    /// Implementation of the Cell World Environment, supporting the execution of
    /// trials for reinforcement learning agents. 
    /// </summary>
    public class CellWorldEnvironment : EnvironmentBase
    {
        private Cell<double> startingCell = null;
        private ISet<Cell<double>> allStates = CollectionFactory.CreateSet<Cell<double>>();
        private ITransitionProbabilityFunction<Cell<double>, CellWorldAction> tpf;
        private IRandom r = null;
        private CellWorldEnvironmentState currentState = new CellWorldEnvironmentState();
        
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="startingCell">
        /// the cell that agent(s) are to start from at the beginning of
        /// each trial within the environment.
        /// </param>
        /// <param name="allStates">all the possible states in this environment.</param>
        /// <param name="tpf">
        /// the transition probability function that simulates how the
        /// environment is meant to behave in response to an agent action.
        /// </param>
        /// <param name="r">
        /// a IRandom used to sample actions that are actually to be
        /// executed based on the transition probabilities for actions.
        /// </param>
        public CellWorldEnvironment(Cell<double> startingCell,
                ISet<Cell<double>> allStates,
                ITransitionProbabilityFunction<Cell<double>, CellWorldAction> tpf,
                IRandom r)
        {
            this.startingCell = startingCell;
            this.allStates.AddAll(allStates);
            this.tpf = tpf;
            this.r = r;
        }

        /// <summary>
        /// Execute N trials.
        /// </summary>
        /// <param name="n">the number of trials to execute.</param>
        public void executeTrials(int n)
        {
            for (int i = 0; i < n; ++i)
            {
                executeTrial();
            }
        }

        /// <summary>
        /// Execute a single trial.
        /// </summary>
        public void executeTrial()
        {
            currentState.reset();
            foreach (IAgent a in agents)
            {
                a.SetAlive(true);
                currentState.setAgentLocation(a, startingCell);
            }
            StepUntilDone();
        }
         
        public override void executeAction(IAgent agent, IAction action)
        {
            if (!action.IsNoOp())
            {
                Cell<double> s = currentState.getAgentLocation(agent);
                double probabilityChoice = r.NextDouble();
                double total = 0;
                bool set = false;
                foreach (Cell<double> sDelta in allStates)
                {
                    total += tpf.probability(sDelta, s, (CellWorldAction)action);
                    if (total > 1.0)
                    {
                        throw new IllegalStateException("Bad probability calculation.");
                    }
                    if (total > probabilityChoice)
                    {
                        currentState.setAgentLocation(agent, sDelta);
                        set = true;
                        break;
                    }
                }
                if (!set)
                {
                    throw new IllegalStateException("Failed to simulate the action=" + action + " correctly from s=" + s);
                }
            }
        }
         
        public override IPercept getPerceptSeenBy(IAgent anAgent)
        {
            return currentState.getPerceptFor(anAgent);
        }
    }
}
