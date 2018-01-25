using aima.net.collections;
using aima.net.collections.api;

namespace aima.net.environment.wumpusworld
{
    /**
     * Representation of an Agent's [x,y] position and orientation [up, down, right, or left] within a Wumpus World cave. 
     */
    public class AgentPosition
    {
        public class Orientation
        {
            private static ICollection<Orientation> _values = CollectionFactory.CreateQueue<Orientation>();

            public static Orientation FACING_NORTH = new Orientation("FacingNorth");
            public static Orientation FACING_SOUTH = new Orientation("FacingSouth");
            public static Orientation FACING_EAST = new Orientation("FacingEast");
            public static Orientation FACING_WEST = new Orientation("FacingWest");

            public override string ToString()
            {
                return name;
            }

            public static ICollection<Orientation> values()
            {
                return _values;
            }

            private readonly string name;

            Orientation(string name)
            {
                this.name = name;
                _values.Add(this);
            }
        }

        private Room room;
        private Orientation orientation;

        public AgentPosition(int x, int y, Orientation orientation)
            : this(new Room(x, y), orientation)
        { }

        public AgentPosition(Room room, Orientation orientation)
        {
            this.room = room;
            this.orientation = orientation;
        }

        public Room getRoom()
        {
            return room;
        }

        public int getX()
        {
            return room.getX();
        }

        public int getY()
        {
            return room.getY();
        }

        public Orientation getOrientation()
        {
            return orientation;
        }

        public override string ToString()
        {
            return room.ToString() + "->" + orientation;
        }

        public override bool Equals(object obj)
        {
            if (obj != null && GetType() == obj.GetType())
            {
                AgentPosition other = (AgentPosition)obj;
                return (getX() == other.getX()) && (getY() == other.getY())
                        && (orientation == other.getOrientation());
            }
            return false;
        }

        public override int GetHashCode()
        {
            int result = 17;
            result = 37 * result + room.GetHashCode();
            result = 43 * result + orientation.GetHashCode();
            return result;
        }
    }
}
