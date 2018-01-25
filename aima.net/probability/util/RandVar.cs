using aima.net.collections;
using aima.net.collections.api;
using aima.net.exceptions;
using aima.net.probability.api;
using aima.net.probability.domain.api;
using aima.net.probability.proposition.api;

namespace aima.net.probability.util
{
    /// <summary>
    /// Default implementation of the RandomVariable interface.
    /// <para />
    /// Note: Also : the TermProposition interface so its easy to use
    /// RandomVariables in conjunction with propositions about them in the
    /// Probability Model APIs.
    /// </summary>
    public class RandVar : IRandomVariable, ITermProposition
    {
        private string name = null;
        private IDomain domain = null;
        private ISet<IRandomVariable> scope = CollectionFactory.CreateSet<IRandomVariable>();

        public RandVar(string name, IDomain domain)
        {
            ProbUtil.checkValidRandomVariableName(name);
            if (null == domain)
            {
                throw new IllegalArgumentException("Domain of RandomVariable must be specified.");
            }

            this.name = name;
            this.domain = domain;
            this.scope.Add(this);
        }
         
        public string getName()
        {
            return name;
        }


        public IDomain getDomain()
        {
            return domain;
        }
         
        public IRandomVariable getTermVariable()
        {
            return this;
        }


        public ISet<IRandomVariable> getScope()
        {
            return scope;
        }


        public ISet<IRandomVariable> getUnboundScope()
        {
            return scope;
        }


        public bool holds(IMap<IRandomVariable, object> possibleWorld)
        {
            return possibleWorld.ContainsKey(getTermVariable());
        }
         
        public override bool Equals(object o)
        {

            if (this == o)
            {
                return true;
            }
            if (!(o is IRandomVariable))
            {
                return false;
            }

            // The name (not the name:domain combination) uniquely identifies a Random Variable
            IRandomVariable other = (IRandomVariable)o;

            return this.name.Equals(other.getName());
        }


        public override int GetHashCode()
        {
            return name.GetHashCode();
        }


        public override string ToString()
        {
            return getName();
        }
    }
}
