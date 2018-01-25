namespace aima.net.search.nondeterministic
{
    /**
     * Represents an if-state-then-plan statement for use with AND-OR search;
     * explanation given on page 135 of AIMA3e.
     * 
     * @author Andrew Brown
     */
    public class IfStateThenPlan
    {

        private object state;
        private Plan plan;

        /**
         * Constructor
         */
        public IfStateThenPlan(object state, Plan plan)
        {
            this.state = state;
            this.plan = plan;
        }

        /**
         * Uses this if-state-then-plan return a result based on the given state
         *
         * @return the plan if the given state matches, null otherwise.
         */
        public Plan ifStateMatches(object state)
        {
            if (this.state.Equals(state))
            {
                return this.plan;
            }
            else
            {
                return null;
            }
        }

        /**
         * Return string representation of this if-then-else
         * 
         * @return a string representation of this if-then-else.
         */
         
        public override string ToString()
        {
            return "if " + state + " then " + plan;
        }
    }
}
