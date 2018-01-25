﻿using aima.net.agent;

namespace aima.net.environment.wumpusworld.action
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 237.<br>
     * <br>
     * The action Shoot can be used to fire an arrow in a straight line in the
     * direction the agent is facing. The arrow continues until it either hits (and
     * hence kills) the wumpus or hits a wall. The agent has only one arrow, so only
     * the first Shoot action has any effect.
     * 
     * @author Federico Baron
     * @author Alessandro Daniele
     * @author Ciaran O'Reilly
     */
    public class Shoot : DynamicAction
    {
        public const string SHOOT_ACTION_NAME = "Shoot";

        public Shoot()
            : base(SHOOT_ACTION_NAME)
        { }
    }
}
