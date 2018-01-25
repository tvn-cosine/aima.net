using aima.net.collections.api;

namespace aima.net.probability.api
{
    /**
           * Interface to be implemented by an object/algorithm that wishes to iterate
           * over the possible assignments for the random variables comprising this
           * categorical distribution.
           * 
           * @see CategoricalDistribution#iterateOver(Iterator)
           * @see CategoricalDistribution#iterateOver(Iterator,
           *      params AssignmentProposition[] )
           */
    public interface IIterator
    {
        /**
         * Called for each possible assignment for the Random Variables
         * comprising this CategoricalDistribution.
         * 
         * @param possibleAssignment
         *            a possible assignment, &omega;, of variable/value pairs.
         * @param probability
         *            the probability associated with &omega;
         */
        void iterate(IMap<IRandomVariable, object> possibleAssignment, double probability);
    }
}
