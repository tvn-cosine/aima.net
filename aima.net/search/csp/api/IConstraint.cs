using aima.net.collections.api;

namespace aima.net.search.csp.api
{
    /**
     * A constraint specifies the allowable combinations of values for a set of
     * variables. Each constraint consists of a pair <scope, rel>, where scope is a
     * tuple of variables that participate in the constraint and rel is a relation
     * that defines the values that those variables can take on.<br>
     * <br>
     * <b>Note:</b> Implementations of this interface define the different kinds of
     * relations that constraints can represent.
     * 
     * @author Ruediger Lunde
     */
    public interface IConstraint<VAR, VAL>
        where VAR : Variable
    {
        /** Returns a tuple of variables that participate in the constraint. */
        ICollection<VAR> getScope();

        /** Constrains the values that the variables can take on. */
        bool isSatisfiedWith(Assignment<VAR, VAL> assignment);
    }
}
