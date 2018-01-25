using aima.net.collections;
using aima.net.collections.api;
using aima.net.datastructures;
using aima.net.search.framework;
using aima.net.search.framework.problem;
using aima.net.search.framework.problem.api;
using aima.net.util.api;

namespace aima.net.environment.nqueens
{
    /**
     * Provides useful functions for two versions of the n-queens problem. The
     * incremental formulation and the complete-state formulation share the same
     * RESULT function but use different ACTIONS functions.
     *
     * @author Ruediger Lunde
     * @author Ciaran O'Reilly
     */
    public class NQueensFunctions
    {
        public static IProblem<NQueensBoard, QueenAction> createIncrementalFormulationProblem(int boardSize)
        {
            return new GeneralProblem<NQueensBoard, QueenAction>(new NQueensBoard(boardSize),
                NQueensFunctions.getIFActionsFunction(),
                NQueensFunctions.getResultFunction(),
                NQueensFunctions.testGoal);
        }

        public static IProblem<NQueensBoard, QueenAction> createCompleteStateFormulationProblem
                (int boardSize, NQueensBoard.Config config)
        {
            return new GeneralProblem<NQueensBoard, QueenAction>(new NQueensBoard(boardSize, config),
                NQueensFunctions.getCSFActionsFunction(),
                NQueensFunctions.getResultFunction(),
                NQueensFunctions.testGoal);
        }

        public class IfActionFunction : IActionsFunction<NQueensBoard, QueenAction>
        {
            public ICollection<QueenAction> apply(NQueensBoard state)
            {
                ICollection<QueenAction> actions = CollectionFactory.CreateQueue<QueenAction>();

                int numQueens = state.getNumberOfQueensOnBoard();
                int boardSize = state.getSize();
                for (int i = 0; i < boardSize; ++i)
                {
                    XYLocation newLocation = new XYLocation(numQueens, i);
                    if (!(state.isSquareUnderAttack(newLocation)))
                    {
                        actions.Add(new QueenAction(QueenAction.PLACE_QUEEN, newLocation));
                    }
                }
                return actions;
            }
        }

        /**
         * Implements an ACTIONS function for the incremental formulation of the
         * n-queens problem.
         * <p>
         * Assumes that queens are placed column by column, starting with an empty
         * board, and provides queen placing actions for all non-attacked positions
         * of the first free column.
         */
        public static IActionsFunction<NQueensBoard, QueenAction> getIFActionsFunction()
        {
            return new IfActionFunction();
        }
         
        public class CSFActionFunction : IActionsFunction<NQueensBoard, QueenAction>
        {
            public ICollection<QueenAction> apply(NQueensBoard state)
            {
                ICollection<QueenAction> actions = CollectionFactory.CreateQueue<QueenAction>();
                for (int i = 0; i < state.getSize(); ++i)
                    for (int j = 0; j < state.getSize(); j++)
                    {
                        XYLocation loc = new XYLocation(i, j);
                        if (!state.queenExistsAt(loc))
                            actions.Add(new QueenAction(QueenAction.MOVE_QUEEN, loc));
                    }
                return actions;
            }
        }
        /**
         * Implements an ACTIONS function for the complete-state formulation of the
         * n-queens problem.
         * <p>
         * Assumes exactly one queen in each column and provides all possible queen
         * movements in vertical direction as actions.
         */
        public static IActionsFunction<NQueensBoard, QueenAction> getCSFActionsFunction()
        {
            return new CSFActionFunction();
        }

        public class ResultFunction : IResultFunction<NQueensBoard, QueenAction>
        {
            public NQueensBoard apply(NQueensBoard state, QueenAction action)
            {
                NQueensBoard result = new NQueensBoard(state.getSize());
                result.setQueensAt(state.getQueenPositions());
                if (action.GetName().Equals(QueenAction.PLACE_QUEEN))
                    result.addQueenAt(action.getLocation());
                else if (action.GetName().Equals(QueenAction.REMOVE_QUEEN))
                    result.removeQueenFrom(action.getLocation());
                else if (action.GetName().Equals(QueenAction.MOVE_QUEEN))
                    result.moveQueenTo(action.getLocation());
                // if action is not understood or is a NoOp
                // the result will be the current state.
                return result;
            }
        }

        /**
         * Implements a RESULT function for the n-queens problem.
         * Supports queen placing, queen removal, and queen movement actions.
         */
        public static IResultFunction<NQueensBoard, QueenAction> getResultFunction()
        {
            return new ResultFunction();
        }

        /**
         * Implements a GOAL-TEST for the n-queens problem.
         */
        public static bool testGoal(NQueensBoard state)
        {
            return state.getNumberOfQueensOnBoard() == state.getSize() && state.getNumberOfAttackingPairs() == 0;
        }

        class AttackingPairsHeuristicFunction : IToDoubleFunction<Node<NQueensBoard, QueenAction>>
        {
            public double applyAsDouble(Node<NQueensBoard, QueenAction> node)
            {
                return node.getState().getNumberOfAttackingPairs();
            }
        }

        /**
         * Estimates the distance to goal by the number of attacking pairs of queens on
         * the board.
         */
        public static IToDoubleFunction<Node<NQueensBoard, QueenAction>> createAttackingPairsHeuristicFunction()
        {
            return new AttackingPairsHeuristicFunction();
        }
    }
}
