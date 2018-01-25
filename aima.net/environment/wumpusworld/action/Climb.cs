using aima.net.agent;

namespace aima.net.environment.wumpusworld.action
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 237.<br>
     * <br>
     * The action Climb can be used to climb out of the cave, but only from square
     * [1,1].
     * 
     * @author Ciaran O'Reilly
     */
    public class Climb : DynamicAction
    {
        public const string CLIMB_ACTION_NAME = "Climb";

        public Climb()
            : base(CLIMB_ACTION_NAME)
        { }
    }
}
