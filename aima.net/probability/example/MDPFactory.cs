using aima.net.collections;
using aima.net.collections.api;
using aima.net.environment.cellworld;
using aima.net.probability.mdp;
using aima.net.probability.mdp.api;

namespace aima.net.probability.example
{
    public class MDPFactory
    {
        /// <summary>
        /// Constructs an MDP that can be used to generate the utility values detailed in Fig 17.3.
        /// </summary>
        /// <param name="cw">the cell world from figure 17.1.</param>
        /// <returns>an MDP that can be used to generate the utility values detailed in Fig 17.3.</returns>
        public static IMarkovDecisionProcess<Cell<double>, CellWorldAction> createMDPForFigure17_3(CellWorld<double> cw)
        {
            return new MDP<Cell<double>, CellWorldAction>(cw.GetCells(),
                    cw.GetCellAt(1, 1), createActionsFunctionForFigure17_1(cw),
                    createTransitionProbabilityFunctionForFigure17_1(cw),
                    createRewardFunctionForFigure17_1());
        }

        class createActionsFunctionForFigure17_1ActionsFunction : IActionsFunction<Cell<double>, CellWorldAction>
        {
            private ISet<Cell<double>> terminals;

            public createActionsFunctionForFigure17_1ActionsFunction(ISet<Cell<double>> terminals)
            {
                this.terminals = terminals;
            }

            public ISet<CellWorldAction> actions(Cell<double> s)
            {
                // All actions can be performed in each cell
                // (except terminal states)
                if (terminals.Contains(s))
                {
                    return CollectionFactory.CreateSet<CellWorldAction>();
                }
                return CellWorldAction.Actions();
            }
        }
        /**
         * Returns the allowed actions from a specified cell within the cell world
         * described in Fig 17.1.
         * 
         * @param cw
         *            the cell world from figure 17.1.
         * @return the set of actions allowed at a particular cell. This set will be
         *         empty if at a terminal state.
         */
        public static IActionsFunction<Cell<double>, CellWorldAction> createActionsFunctionForFigure17_1(CellWorld<double> cw)
        {
            ISet<Cell<double>> terminals = CollectionFactory.CreateSet<Cell<double>>();
            terminals.Add(cw.GetCellAt(4, 3));
            terminals.Add(cw.GetCellAt(4, 2));

            IActionsFunction<Cell<double>, CellWorldAction> af = new createActionsFunctionForFigure17_1ActionsFunction(terminals);
            return af;
        }

        class createTransitionProbabilityFunctionForFigure17_1TransitionProbabilityFunction : ITransitionProbabilityFunction<Cell<double>, CellWorldAction>
        {
            private CellWorld<double> cw;
            private double[] distribution = new double[] { 0.8, 0.1, 0.1 };

            public createTransitionProbabilityFunctionForFigure17_1TransitionProbabilityFunction(CellWorld<double> cw)
            {
                this.cw = cw;
            }

            public double probability(Cell<double> sDelta, Cell<double> s, CellWorldAction a)
            {
                double prob = 0;

                ICollection<Cell<double>> outcomes = possibleOutcomes(s, a);
                for (int i = 0; i < outcomes.Size(); ++i)
                {
                    if (sDelta.Equals(outcomes.Get(i)))
                    {
                        // Note: You have to sum the matches to
                        // sDelta as the different actions
                        // could have the same effect (i.e.
                        // staying in place due to there being
                        // no adjacent cells), which increases
                        // the probability of the transition for
                        // that state.
                        prob += distribution[i];
                    }
                }

                return prob;
            }

            private ICollection<Cell<double>> possibleOutcomes(Cell<double> c, CellWorldAction a)
            {
                // There can be three possible outcomes for the planned action
                ICollection<Cell<double>> outcomes = CollectionFactory.CreateQueue<Cell<double>>();

                outcomes.Add(cw.Result(c, a));
                outcomes.Add(cw.Result(c, a.GetFirstRightAngledAction()));
                outcomes.Add(cw.Result(c, a.GetSecondRightAngledAction()));

                return outcomes;
            }
        }

        /**
         * Figure 17.1 (b) Illustration of the transition model of the environment:
         * the 'intended' outcome occurs with probability 0.8, but with probability
         * 0.2 the agent moves at right angles to the intended direction. A
         * collision with a wall results in no movement.
         * 
         * @param cw
         *            the cell world from figure 17.1.
         * @return the transition probability function as described in figure 17.1.
         */
        public static ITransitionProbabilityFunction<Cell<double>, CellWorldAction> createTransitionProbabilityFunctionForFigure17_1(CellWorld<double> cw)
        {
            ITransitionProbabilityFunction<Cell<double>, CellWorldAction> tf = new createTransitionProbabilityFunctionForFigure17_1TransitionProbabilityFunction(cw);

            return tf;
        }


        class createRewardFunctionForFigure17_1RewardFunction : IRewardFunction<Cell<double>>
        {
            public double reward(Cell<double> s)
            {
                return s.getContent();
            }
        }
        /**
         * 
         * @return the reward function which takes the content of the cell as being
         *         the reward value.
         */
        public static IRewardFunction<Cell<double>> createRewardFunctionForFigure17_1()
        {
            IRewardFunction<Cell<double>> rf = new createRewardFunctionForFigure17_1RewardFunction();
            return rf;
        }
    }
}
