using aima.net.collections;
using aima.net.collections.api;
using aima.net.search.csp.api;

namespace aima.net.search.csp.examples
{
    /// <summary>
    /// Represents a binary constraint which forbids equal values.
    /// </summary>
    /// <typeparam name="VAR"></typeparam>
    /// <typeparam name="VAL"></typeparam>
    public class NotEqualConstraint<VAR, VAL> : IConstraint<VAR, VAL>
        where VAR : Variable
    {
        private VAR var1;
        private VAR var2;
        private ICollection<VAR> scope;

        public NotEqualConstraint(VAR var1, VAR var2)
        {
            this.var1 = var1;
            this.var2 = var2;
            scope = CollectionFactory.CreateQueue<VAR>();
            scope.Add(var1);
            scope.Add(var2);
        }
         
        public ICollection<VAR> getScope()
        {
            return scope;
        }
         
        public bool isSatisfiedWith(Assignment<VAR, VAL> assignment)
        {
            object value1 = assignment.getValue(var1);
            return value1 == null || !value1.Equals(assignment.getValue(var2));
        }
    } 
}
