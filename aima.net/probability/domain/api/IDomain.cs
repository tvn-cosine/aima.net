namespace aima.net.probability.domain.api
{
    /// <summary>
    /// Artificial Intelligence A Modern Approach (3rd Edition): page 486. 
    /// <para />
    /// Every random variable has a <b>domain</b> - the set of possible values it can
    /// take on. The domain of <i>Total</i> for two dice is the set {2,...,12} and
    /// the domain of Die<sub>1</sub> is {1,...,6}. A Boolean random variable has the
    /// domain {true, false}.
    /// </summary>
    public interface IDomain
    {
        /// <summary>
        /// Return true if the Domain is finite, false otherwise (i.e. discrete (like the integers) or continuous (like the reals)).
        /// </summary>
        /// <returns>true if the Domain is finite, false otherwise (i.e. discrete (like the integers) or continuous (like the reals)).</returns>
        bool IsFinite();

        /// <summary>
        /// !isFinite().
        /// </summary>
        /// <returns>!isFinite().</returns>
        bool IsInfinite();

        /// <summary>
        /// Return the size of the Domain, only applicable if isFinite() == true.
        /// </summary>
        /// <returns>the size of the Domain, only applicable if isFinite() == true.</returns>
        int Size();

        /// <summary>
        /// Return true if the domain is ordered, false otherwise. i.e. you can
        /// specify 1 object from the domain is &lt; or = another object in the
        /// domain.
        /// </summary>
        /// <returns>
        /// true if the domain is ordered, false otherwise. i.e. you can
        /// specify 1 object from the domain is &lt; or = another object in the
        /// domain.
        /// </returns>
        bool IsOrdered();
    } 
}
