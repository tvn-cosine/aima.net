using aima.net.agent;

namespace aima.net.environment.wumpusworld.action
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 237.<br>
     * <br>
     * The action Grab can be used to pick up the gold if it is in the same square
     * as the agent.
     * 
     * @author Ciaran O'Reilly
     */
    public class Grab : DynamicAction
    {
        public const string GRAB_ACTION_NAME = "Grab";

        public Grab()
            : base(GRAB_ACTION_NAME)
        { }
    } 
}
