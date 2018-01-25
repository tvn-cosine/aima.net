using aima.net.agent;

namespace aima.net.environment.wumpusworld.action
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 237.<br>
     * <br>
     * The agent can TurnRight by 90 degrees.
     * 
     * @author Federico Baron
     * @author Alessandro Daniele
     * @author Ciaran O'Reilly
     */
    public class TurnRight : DynamicAction
    {
        public const string TURN_RIGHT_ACTION_NAME = "TurnRight";
        public const string ATTRIBUTE_TO_ORIENTATION = "toOrientation";
        //
        private AgentPosition.Orientation toOrientation;

        /**
         * Constructor.
         * 
         * @param currentOrientation
         */
        public TurnRight(AgentPosition.Orientation currentOrientation)
            : base(TURN_RIGHT_ACTION_NAME)
        {
            if (currentOrientation.Equals(AgentPosition.Orientation.FACING_NORTH))
            {
                toOrientation = AgentPosition.Orientation.FACING_EAST;
            }
            else if (currentOrientation.Equals(AgentPosition.Orientation.FACING_SOUTH))
            {
                toOrientation = AgentPosition.Orientation.FACING_WEST;
            }
            else if (currentOrientation.Equals(AgentPosition.Orientation.FACING_EAST))
            {
                toOrientation = AgentPosition.Orientation.FACING_SOUTH;
            }
            else if (currentOrientation.Equals(AgentPosition.Orientation.FACING_WEST))
            {
                toOrientation = AgentPosition.Orientation.FACING_NORTH; 
            }
            SetAttribute(ATTRIBUTE_TO_ORIENTATION, toOrientation);
        }

        /**
         * 
         * @return the orientation the agent should be after the action occurred.
         *         <b>Note:<b> this may not be a legal orientation within the
         *         environment in which the action was performed and this should be
         *         checked for.
         */
        public AgentPosition.Orientation getToOrientation()
        {
            return toOrientation;
        }
    }
}
