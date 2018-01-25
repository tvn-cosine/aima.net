using aima.net.collections;
using aima.net.collections.api;

namespace aima.net.search.nondeterministic
{
    /**
     * Represents the path the agent travels through the AND-OR tree (see figure
     * 4.10, page 135, AIMA3e).
     * 
     * @author Andrew Brown
     */
    public class Path : List<object>
    {
        /**
         * Creating a new path based on this path and the passed in appended states.
         * 
         * @param states
         *            the states to append to a new copy of this path.
         * 
         * @return a new Path that contains this path's states along with the passed
         *         in argument states appended to the end.
         */
        public Path append(params object[] states)
        {
            Path appendedPath = new Path();
            appendedPath.AddAll(this);
            foreach (object obj in states)
                appendedPath.Add(obj);
            return appendedPath;
        }

        /**
         * Create a new path based on the passed in prepended state and this path's
         * current states.
         * 
         * @param state
         *            the state to be prepended.
         * @return a new Path that contains the passed in state along with this
         *         path's current states.
         */
        public Path prepend(object state)
        {
            Path prependedPath = new Path();
            prependedPath.Add(state);
            prependedPath.AddAll(this);

            return prependedPath;
        }
    }

}
