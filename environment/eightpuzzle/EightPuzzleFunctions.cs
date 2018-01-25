using aima.net.agent.api;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.datastructures;
using aima.net.search.framework;
using aima.net.search.framework.problem;
using aima.net.search.framework.problem.api;
using aima.net.util;
using aima.net.util.api;

namespace aima.net.environment.eightpuzzle
{
    public class EightPuzzleFunctions
    {
        public static readonly EightPuzzleBoard GOAL_STATE = new EightPuzzleBoard(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 });

        public class ActionFunctionEB : IActionsFunction<EightPuzzleBoard, IAction>
        {
            public ICollection<IAction> apply(EightPuzzleBoard state)
            {
                return getActions(state);
            }
        }

        public class ResultFunctionEB : IResultFunction<EightPuzzleBoard, IAction>
        {
            public EightPuzzleBoard apply(EightPuzzleBoard state, IAction action)
            {
                return getResult(state, action);
            }
        }

        public static ICollection<IAction> getActions(EightPuzzleBoard state)
        {
            ICollection<IAction> actions = CollectionFactory.CreateQueue<IAction>();

            if (state.canMoveGap(EightPuzzleBoard.UP))
                actions.Add(EightPuzzleBoard.UP);
            if (state.canMoveGap(EightPuzzleBoard.DOWN))
                actions.Add(EightPuzzleBoard.DOWN);
            if (state.canMoveGap(EightPuzzleBoard.LEFT))
                actions.Add(EightPuzzleBoard.LEFT);
            if (state.canMoveGap(EightPuzzleBoard.RIGHT))
                actions.Add(EightPuzzleBoard.RIGHT);

            return actions;
        }

        public static EightPuzzleBoard getResult(EightPuzzleBoard state, IAction action)
        {
            EightPuzzleBoard result = new EightPuzzleBoard(state);

            if (EightPuzzleBoard.UP.Equals(action) && state.canMoveGap(EightPuzzleBoard.UP))
                result.moveGapUp();
            else if (EightPuzzleBoard.DOWN.Equals(action) && state.canMoveGap(EightPuzzleBoard.DOWN))
                result.moveGapDown();
            else if (EightPuzzleBoard.LEFT.Equals(action) && state.canMoveGap(EightPuzzleBoard.LEFT))
                result.moveGapLeft();
            else if (EightPuzzleBoard.RIGHT.Equals(action) && state.canMoveGap(EightPuzzleBoard.RIGHT))
                result.moveGapRight();
            return result;
        }

        public static IToDoubleFunction<Node<EightPuzzleBoard, IAction>> createManhattanHeuristicFunction()
        {
            return new ManhattanHeuristicFunction();
        }

        public static IToDoubleFunction<Node<EightPuzzleBoard, IAction>> createMisplacedTileHeuristicFunction()
        {
            return new MisplacedTileHeuristicFunction();
        }
         
        private class ManhattanHeuristicFunction : IToDoubleFunction<Node<EightPuzzleBoard, IAction>>
        {

            public double applyAsDouble(Node<EightPuzzleBoard, IAction> node)
            {
                EightPuzzleBoard board = node.getState();
                int retVal = 0;
                for (int i = 1; i < 9; i++)
                {
                    XYLocation loc = board.getLocationOf(i);
                    retVal += evaluateManhattanDistanceOf(i, loc);
                }
                return retVal;

            }

            private int evaluateManhattanDistanceOf(int i, XYLocation loc)
            {
                int retVal = -1;
                int xpos = loc.GetXCoOrdinate();
                int ypos = loc.GetYCoOrdinate();
                switch (i)
                { 
                    case 1:
                        retVal = System.Math.Abs(xpos - 0) + System.Math.Abs(ypos - 1);
                        break;
                    case 2:
                        retVal = System.Math.Abs(xpos - 0) + System.Math.Abs(ypos - 2);
                        break;
                    case 3:
                        retVal = System.Math.Abs(xpos - 1) + System.Math.Abs(ypos - 0);
                        break;
                    case 4:
                        retVal = System.Math.Abs(xpos - 1) + System.Math.Abs(ypos - 1);
                        break;
                    case 5:
                        retVal = System.Math.Abs(xpos - 1) + System.Math.Abs(ypos - 2);
                        break;
                    case 6:
                        retVal = System.Math.Abs(xpos - 2) + System.Math.Abs(ypos - 0);
                        break;
                    case 7:
                        retVal = System.Math.Abs(xpos - 2) + System.Math.Abs(ypos - 1);
                        break;
                    case 8:
                        retVal = System.Math.Abs(xpos - 2) + System.Math.Abs(ypos - 2);
                        break;

                }
                return retVal;
            }
        }
         
        private class MisplacedTileHeuristicFunction : IToDoubleFunction<Node<EightPuzzleBoard, IAction>>
        {

            public double applyAsDouble(Node<EightPuzzleBoard, IAction> node)
            {
                EightPuzzleBoard board = (EightPuzzleBoard)node.getState();
                return getNumberOfMisplacedTiles(board);
            }

            private int getNumberOfMisplacedTiles(EightPuzzleBoard board)
            {
                int numberOfMisplacedTiles = 0;
                if (!(board.getLocationOf(0).Equals(new XYLocation(0, 0))))
                {
                    numberOfMisplacedTiles++;
                }
                if (!(board.getLocationOf(1).Equals(new XYLocation(0, 1))))
                {
                    numberOfMisplacedTiles++;
                }
                if (!(board.getLocationOf(2).Equals(new XYLocation(0, 2))))
                {
                    numberOfMisplacedTiles++;
                }
                if (!(board.getLocationOf(3).Equals(new XYLocation(1, 0))))
                {
                    numberOfMisplacedTiles++;
                }
                if (!(board.getLocationOf(4).Equals(new XYLocation(1, 1))))
                {
                    numberOfMisplacedTiles++;
                }
                if (!(board.getLocationOf(5).Equals(new XYLocation(1, 2))))
                {
                    numberOfMisplacedTiles++;
                }
                if (!(board.getLocationOf(6).Equals(new XYLocation(2, 0))))
                {
                    numberOfMisplacedTiles++;
                }
                if (!(board.getLocationOf(7).Equals(new XYLocation(2, 1))))
                {
                    numberOfMisplacedTiles++;
                }
                if (!(board.getLocationOf(8).Equals(new XYLocation(2, 2))))
                {
                    numberOfMisplacedTiles++;
                }
                // Subtract the gap position from the # of misplaced tiles
                // as its not actually a tile (see issue 73).
                if (numberOfMisplacedTiles > 0)
                {
                    numberOfMisplacedTiles--;
                }
                return numberOfMisplacedTiles;
            }
        }
    }
}
