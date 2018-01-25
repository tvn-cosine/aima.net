using aima.net.collections.api;
using aima.net.probability.api;
using aima.net.probability.proposition;

namespace aima.net.probability.bayes.api
{
    /// <summary>
    /// A conditional probability distribution on a RandomVariable Xi: 
    /// <para />
    /// P(Xi | Parents(Xi)) that quantifies the effect of the
    /// parents on Xi.
    /// </summary>
    public interface IConditionalProbabilityDistribution
    { 
        /// <summary>
        /// Return the Random Variable this conditional probability distribution is on.
        /// </summary>
        /// <returns>the Random Variable this conditional probability distribution is on.</returns>
        IRandomVariable GetOn();

        /// <summary>
        /// Return a consistent ordered Set (e.g. LinkedHashSet) of the parent Random Variables for this conditional distribution.
        /// </summary>
        /// <returns>a consistent ordered Set (e.g. LinkedHashSet) of the parent Random Variables for this conditional distribution.</returns>
        ISet<IRandomVariable> GetParents();

        /// <summary>
        /// A convenience method for merging the elements of getParents() and getOn()
        /// into a consistent ordered set (i.e. getOn() should always be the last
        /// Random Variable returned when iterating over the set).
        /// </summary>
        /// <returns>
        /// a consistent ordered Set (e.g. LinkedHashSet) of the random
        /// variables this conditional probability distribution is for.
        /// </returns>
        ISet<IRandomVariable> GetFor();

        /// <summary>
        /// Return true if the conditional distribution is for the passed in Random Variable, false otherwise.
        /// </summary>
        /// <param name="rv">the Random Variable to be checked.</param>
        /// <returns>true if the conditional distribution is for the passed in Random Variable, false otherwise.</returns>
        bool Contains(IRandomVariable rv);

        /// <summary>
        /// Get the value for the provided set of values for the random variables
        /// comprising the Conditional Distribution (ordering and size of each must
        /// equal getFor() and their domains must match).
        /// </summary>
        /// <param name="eventValues">the values for the random variables comprising the Conditional Distribution</param>
        /// <returns>
        /// the value for the possible worlds associated with the assignments
        /// for the random variables comprising the Conditional Distribution.
        /// </returns>
        double GetValue(params object[] eventValues);

        /// <summary>
        /// Get the value for the provided set of AssignmentPropositions for the
        /// random variables comprising the Conditional Distribution (size of each
        /// must equal and their random variables must match).
        /// </summary>
        /// <param name="eventValues">
        /// the assignment propositions for the random variables
        /// comprising the Conditional Distribution
        /// </param>
        /// <returns>
        /// the value for the possible worlds associated with the assignments
        /// for the random variables comprising the Conditional Distribution.</returns>
        double GetValue(params AssignmentProposition[] eventValues);

        /// <summary>
        /// A conditioning case is just a possible combination of values for the
        /// parent nodes - a miniature possible world, if you like. The returned
        /// distribution must sum to 1, because the entries represent an exhaustive
        /// set of cases for the random variable.
        /// </summary>
        /// <param name="parentValues">
        /// for the conditioning case. The ordering and size of
        /// parentValues must equal getParents() and their domains must
        /// match.</param>
        /// <returns>
        /// the Probability Distribution for the Random Variable the
        /// Conditional Probability Distribution is On.
        /// </returns>
        IProbabilityDistribution GetConditioningCase(params object[] parentValues);

        /// <summary>
        /// A conditioning case is just a possible combination of values for the
        /// parent nodes - a miniature possible world, if you like. The returned
        /// distribution must sum to 1, because the entries represent an exhaustive
        /// set of cases for the random variable.
        /// </summary>
        /// <param name="parentValues">
        /// for the conditioning case. The size of parentValues must equal
        /// getParents() and their Random Variables must match.
        /// </param>
        /// <returns>
        /// the Probability Distribution for the Random Variable the
        /// Conditional Probability Distribution is On.</returns>
        IProbabilityDistribution GetConditioningCase(params AssignmentProposition[] parentValues);

        /// <summary>
        /// Retrieve a specific example for the Random Variable this conditional
        /// distribution is on.
        /// </summary>
        /// <param name="probabilityChoice">
        /// a double value, from the range [0.0d, 1.0d), i.e. 0.0d
        /// (inclusive) to 1.0d (exclusive).
        /// </param>
        /// <param name="parentValues">
        /// for the conditioning case. The ordering and size of
        /// parentValues must equal getParents() and their domains must
        /// match.
        /// </param>
        /// <returns>
        /// a sample value from the domain of the Random Variable this
        /// distribution is on, based on the probability argument passed in.
        /// </returns>
        object GetSample(double probabilityChoice, params object[] parentValues);
         
        /// <summary>
        /// Retrieve a specific example for the Random Variable this conditional
        /// distribution is on.
        /// </summary>
        /// <param name="probabilityChoice">
        /// a double value, from the range [0.0d, 1.0d), i.e. 0.0d
        /// (inclusive) to 1.0d (exclusive).</param>
        /// <param name="parentValues">
        /// for the conditioning case. The size of parentValues must equal
        /// getParents() and their Random Variables must match.
        /// a sample value from the domain of the Random Variable this
        /// distribution is on, based on the probability argument passed in.</param>
        /// <returns></returns>
        object GetSample(double probabilityChoice, params AssignmentProposition[] parentValues);
    } 
}
