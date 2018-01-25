using aima.net.agent.api;
using aima.net.agent;
using aima.net.api;
using aima.net.collections.api;
using aima.net.datastructures;
using aima.net.collections;

namespace aima.net.environment.eightpuzzle
{
    public class EightPuzzleBoard
    {

        public static IAction LEFT = new DynamicAction("Left");

        public static IAction RIGHT = new DynamicAction("Right");

        public static IAction UP = new DynamicAction("Up");

        public static IAction DOWN = new DynamicAction("Down");

        private int[] state;

        //
        // PUBLIC METHODS
        //

        public EightPuzzleBoard()
        {
            state = new int[] { 5, 4, 0, 6, 1, 8, 7, 3, 2 };
        }

        public EightPuzzleBoard(int[] state)
        {
            this.state = new int[state.Length];
            System.Array.Copy(state, 0, this.state, 0, state.Length);
        }

        public EightPuzzleBoard(EightPuzzleBoard copyBoard)
            : this(copyBoard.getState())
        {

        }

        public int[] getState()
        {
            return state;
        }

        public int getValueAt(XYLocation loc)
        {
            return getValueAt(loc.GetXCoOrdinate(), loc.GetYCoOrdinate());
        }

        public XYLocation getLocationOf(int val)
        {
            int absPos = getPositionOf(val);
            return new XYLocation(getXCoord(absPos), getYCoord(absPos));
        }

        public void moveGapRight()
        {
            int gapPos = getGapPosition();
            int x = getXCoord(gapPos);
            int ypos = getYCoord(gapPos);
            if (!(ypos == 2))
            {
                int valueOnRight = getValueAt(x, ypos + 1);
                setValue(x, ypos, valueOnRight);
                setValue(x, ypos + 1, 0);
            }

        }

        public void moveGapLeft()
        {
            int gapPos = getGapPosition();
            int x = getXCoord(gapPos);
            int ypos = getYCoord(gapPos);
            if (!(ypos == 0))
            {
                int valueOnLeft = getValueAt(x, ypos - 1);
                setValue(x, ypos, valueOnLeft);
                setValue(x, ypos - 1, 0);
            }

        }

        public void moveGapDown()
        {
            int gapPos = getGapPosition();
            int x = getXCoord(gapPos);
            int y = getYCoord(gapPos);
            if (!(x == 2))
            {
                int valueOnBottom = getValueAt(x + 1, y);
                setValue(x, y, valueOnBottom);
                setValue(x + 1, y, 0);
            }

        }

        public void moveGapUp()
        {
            int gapPos = getGapPosition();
            int x = getXCoord(gapPos);
            int y = getYCoord(gapPos);
            if (!(x == 0))
            {
                int valueOnTop = getValueAt(x - 1, y);
                setValue(x, y, valueOnTop);
                setValue(x - 1, y, 0);
            }
        }

        public ICollection<XYLocation> getPositions()
        {
            ICollection<XYLocation> retVal = CollectionFactory.CreateQueue<XYLocation>();
            for (int i = 0; i < 9; i++)
            {
                int absPos = getPositionOf(i);
                XYLocation loc = new XYLocation(getXCoord(absPos),
                        getYCoord(absPos));
                retVal.Add(loc);

            }
            return retVal;
        }

        public void setBoard(ICollection<XYLocation> locs)
        {
            int count = 0;
            for (int i = 0; i < locs.Size(); i++)
            {
                XYLocation loc = locs.Get(i);
                this.setValue(loc.GetXCoOrdinate(), loc.GetYCoOrdinate(), count);
                count = count + 1;
            }
        }

        public bool canMoveGap(IAction where)
        {
            bool retVal = true;
            int absPos = getPositionOf(0);
            if (where.Equals(LEFT))
                retVal = (getYCoord(absPos) != 0);
            else if (where.Equals(RIGHT))
                retVal = (getYCoord(absPos) != 2);
            else if (where.Equals(UP))
                retVal = (getXCoord(absPos) != 0);
            else if (where.Equals(DOWN))
                retVal = (getXCoord(absPos) != 2);
            return retVal;
        }

        public override bool Equals(object o)
        {
            if (this == o)
                return true;
            if (o != null && GetType() == o.GetType())
            {
                EightPuzzleBoard aBoard = (EightPuzzleBoard)o;
                for (int i = 0; i < 8; i++)
                {
                    if (this.getPositionOf(i) != aBoard.getPositionOf(i))
                        return false;
                }
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            int result = 17;
            for (int i = 0; i < 8; i++)
            {
                int position = this.getPositionOf(i);
                result = 37 * result + position;
            }
            return result;
            //    return ToString().GetHashCode();
        }

        public override string ToString()
        {
            return state[0] + " " + state[1] + " " + state[2] + "\n"
                    + state[3] + " " + state[4] + " " + state[5] + " " + "\n"
                    + state[6] + " " + state[7] + " " + state[8];
        }

        //
        // PRIVATE METHODS
        //

        /**
         * Note: The graphic representation maps x values on row numbers (x-axis in
         * vertical direction).
         */
        private int getXCoord(int absPos)
        {
            return absPos / 3;
        }

        /**
         * Note: The graphic representation maps y values on column numbers (y-axis
         * in horizontal direction).
         */
        private int getYCoord(int absPos)
        {
            return absPos % 3;
        }

        private int getAbsPosition(int x, int y)
        {
            return x * 3 + y;
        }

        private int getValueAt(int x, int y)
        {
            // refactor this use either case or a div/mod soln
            return state[getAbsPosition(x, y)];
        }

        private int getGapPosition()
        {
            return getPositionOf(0);
        }

        private int getPositionOf(int val)
        {
            for (int i = 0; i < 9; i++)
            {
                if (state[i] == val)
                {
                    return i;
                }
            }
            return -1;
        }

        private void setValue(int x, int y, int val)
        {
            int absPos = getAbsPosition(x, y);
            state[absPos] = val;

        }
    }

}
