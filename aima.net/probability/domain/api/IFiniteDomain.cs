using aima.net.collections.api;

namespace aima.net.probability.domain.api
{ 
    /// <summary>
    /// A Domain over a countable/discrete and finite set of objects.
    /// </summary>
    public interface IFiniteDomain : IDiscreteDomain
    {
        /// <summary>
        /// Return a consistent ordered Set (e.g. LinkedHashSet) of the possible values this domain can take on.
        /// </summary>
        /// <returns>a consistent ordered Set (e.g. LinkedHashSet) of the possible values this domain can take on.</returns>
        ISet<object> GetPossibleValues();

        /// <summary>
        /// The possible values for a finite domain are to have a consistent ordering
        /// (whether they are actually ordered by value or not). This will return an
        /// offset into that ordering.
        /// </summary>
        /// <param name="value">a value from the domain.</param>
        /// <returns>an offset (starting from 0) into the consistent order of the set of possible values.</returns>
        /// <exception cref="common.exceptions.IllegalArgumentException" />
        int GetOffset(object value);

        /// <summary>
        /// Return the object at the specified offset in this domains consistent
        /// ordered set of values. null if the offset does not index the
        /// domain correctly.
        /// </summary>
        /// <param name="offset">an offset into the consistent ordering for this domain.</param>
        /// <returns>
        /// the object at the specified offset in this domains consistent
        /// ordered set of values. null if the offset does not index the
        /// domain correctly.
        /// </returns>
        object GetValueAt(int offset);
    }
}
