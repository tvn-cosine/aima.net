using aima.net.agent.api;
using aima.net.collections;
using aima.net.collections.api;

namespace aima.net.environment.cellworld
{
    /// <summary> 
    /// Artificial Intelligence A Modern Approach (3rd Edition): page 645. 
    /// <para />
    /// 
    /// The actions in every state are Up, Down, Left, and Right. 
    /// <para />
    /// Note: Moving 'North' causes y to increase by 1, 'Down' y to decrease by
    /// 1, 'Left' x to decrease by 1, and 'Right' x to increase by 1 within a Cell
    /// World.
    /// </summary>
    public class CellWorldAction : IAction
    {
        public static readonly CellWorldAction Up = new CellWorldAction();
        public static readonly CellWorldAction Down = new CellWorldAction();
        public static readonly CellWorldAction Left = new CellWorldAction();
        public static readonly CellWorldAction Right = new CellWorldAction();
        public static readonly CellWorldAction None = new CellWorldAction();

        private static readonly ISet<CellWorldAction> _actions;

        static CellWorldAction()
        {
            _actions = CollectionFactory.CreateSet<CellWorldAction>();
            _actions.Add(Up);
            _actions.Add(Down);
            _actions.Add(Left);
            _actions.Add(Right);
            _actions.Add(None);
        }

        /// <summary>
        /// Get a set of the actual actions.
        /// </summary>
        /// <returns>a set of the actual actions.</returns>
        public static ISet<CellWorldAction> Actions()
        {
            return CollectionFactory.CreateReadOnlySet<CellWorldAction>(_actions);
        }

        public bool IsNoOp()
        {
            if (None == this)
            {
                return true;
            }
            return false;
        }
        
        /// <summary>
        /// Get the result on the x position of applying this action.
        /// </summary>
        /// <param name="curX">the current x position.</param>
        /// <returns>the result on the x position of applying this action.</returns>
        public int GetXResult(int curX)
        {
            int newX = curX;

            if (Left == this)
            {
                newX--;
            }
            else if (Right == this)
            {
                newX++;
            }

            return newX;
        }
         
        /// <summary>
        /// Get the result on the y position of applying this action.
        /// </summary>
        /// <param name="curY">the current y position.</param>
        /// <returns>the result on the y position of applying this action.</returns>
        public int GetYResult(int curY)
        {
            int newY = curY;

            if (Up == this)
            {
                newY++;
            }
            else if (Down == this)
            {
                newY--;
            }

            return newY;
        }

        /// <summary>
        /// Get the first right angled action related to this action.
        /// </summary>
        /// <returns>the first right angled action related to this action.</returns>
        public CellWorldAction GetFirstRightAngledAction()
        {
            CellWorldAction a = null;

            if (Up == this
             || Down == this)
            {
                a = Left;
            }
            else if (Left == this
                 || Right == this)
            {
                a = Down;
            }
            else if (None == this)
            {
                a = None;
            }

            return a;
        }

        /// <summary>
        /// Get the second right angled action related to this action.
        /// </summary>
        /// <returns>the second right angled action related to this action.</returns>
        public CellWorldAction GetSecondRightAngledAction()
        {
            CellWorldAction a = null;

            if (Up == this
             || Down == this)
            {
                a = Right;
            }
            else if (Left == this
                 || Right == this)
            {
                a = Up;
            }
            else if (None == this)
            {
                a = None;
            }

            return a;
        }
    }
}
