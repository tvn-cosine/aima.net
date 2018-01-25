using aima.net.probability.domain;
using aima.net.probability.domain.api;

namespace aima.net.probability.api
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 486.<br>
     * <br>
     * Variables in probability theory are called random variables and their names
     * begin with an uppercase letter. Every random variable has a domain - the set
     * of possible values it can take on.
     * 
     * @author Ciaran O'Reilly
     */
    public interface IRandomVariable
    {
        /**
         * 
         * @return the name used to uniquely identify this variable.
         */
        string getName();

        /**
         * 
         * @return the Set of possible values the Random Variable can take on.
         */
        IDomain getDomain();
    } 
}
