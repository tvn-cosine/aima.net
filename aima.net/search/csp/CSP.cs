using aima.net.collections;
using aima.net.collections.api;
using aima.net.exceptions;
using aima.net.search.csp.api;

namespace aima.net.search.csp
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Ed.): Section 6.1, Page 202.<br>
     * <br>
     * A constraint satisfaction problem or CSP consists of three components, X, D,
     * and C:
     * <ul>
     * <li>X is a set of variables, {X1, ... ,Xn}.</li>
     * <li>D is a set of domains, {D1, ... ,Dn}, one for each variable.</li>
     * <li>C is a set of constraints that specify allowable combinations of values.</li>
     * </ul>
     *
     * @param <VAR> Type which is used to represent variables
     * @param <VAL> Type which is used to represent the values in the domains
     *
     * @author Ruediger Lunde
     */
    public class CSP<VAR, VAL>
       where VAR : Variable
    {
        private ICollection<VAR> variables;
        private ICollection<Domain<VAL>> domains;
        private ICollection<IConstraint<VAR, VAL>> constraints;

        /**
         * Lookup, which maps a variable to its index in the list of variables.
         */
        private IMap<Variable, int> varIndexHash;
        /**
         * Constraint network. Maps variables to those constraints in which they
         * participate.
         */
        private IMap<Variable, ICollection<IConstraint<VAR, VAL>>> cnet;

        /**
         * Creates a new CSP.
         */
        public CSP()
        {
            variables = CollectionFactory.CreateQueue<VAR>();
            domains = CollectionFactory.CreateQueue<Domain<VAL>>();
            constraints = CollectionFactory.CreateQueue<IConstraint<VAR, VAL>>();
            varIndexHash = CollectionFactory.CreateInsertionOrderedMap<Variable, int>();
            cnet = CollectionFactory.CreateInsertionOrderedMap<Variable, ICollection<IConstraint<VAR, VAL>>>();
        }

        /**
         * Creates a new CSP.
         */
        public CSP(ICollection<VAR> vars)
            : this()
        {
            foreach (var v in vars)
            {
                addVariable(v);
            }
        }

        /**
         * Adds a new variable only if its name is new.
         */
        protected void addVariable(VAR var)
        {
            if (!varIndexHash.ContainsKey(var))
            {
                Domain<VAL> emptyDomain = new Domain<VAL>(CollectionFactory.CreateQueue<VAL>());
                variables.Add(var);
                domains.Add(emptyDomain);
                varIndexHash.Put(var, variables.Size() - 1);
                cnet.Put(var, CollectionFactory.CreateQueue<IConstraint<VAR, VAL>>());
            }
            else
            {
                throw new IllegalArgumentException("Variable with same name already exists.");
            }
        }

        public ICollection<VAR> getVariables()
        {
            return CollectionFactory.CreateReadOnlyQueue<VAR>(variables);
        }

        public int indexOf(Variable var)
        {
            return varIndexHash.Get(var);
        }

        public void setDomain(VAR var, Domain<VAL> domain)
        {
            domains.Set(indexOf(var), domain);
        }

        public Domain<VAL> getDomain(Variable var)
        {
            return domains.Get(varIndexHash.Get(var));
        }

        /**
         * Replaces the domain of the specified variable by new domain, which
         * contains all values of the old domain except the specified value.
         */
        public bool removeValueFromDomain(VAR var, VAL value)
        {
            Domain<VAL> currDomain = getDomain(var);
            ICollection<VAL> values = CollectionFactory.CreateQueue<VAL>();
            foreach (VAL v in currDomain)
                if (!v.Equals(value))
                    values.Add(v);
            if (values.Size() < currDomain.size())
            {
                setDomain(var, new Domain<VAL>(values));
                return true;
            }
            return false;
        }

        public void addConstraint(IConstraint<VAR, VAL> constraint)
        {
            constraints.Add(constraint);
            foreach (VAR var in constraint.getScope())
                cnet.Get(var).Add(constraint);
        }

        public bool removeConstraint(IConstraint<VAR, VAL> constraint)
        {
            bool result = constraints.Remove(constraint);
            if (result)
                foreach (VAR var in constraint.getScope())
                    cnet.Get(var).Remove(constraint);
            return result;
        }

        public ICollection<IConstraint<VAR, VAL>> getConstraints()
        {
            return constraints;
        }

        /**
         * Returns all constraints in which the specified variable participates.
         */
        public ICollection<IConstraint<VAR, VAL>> getConstraints(Variable var)
        {
            return cnet.Get(var);
        }

        /**
         * Returns for binary constraints the other variable from the scope.
         *
         * @return a variable or null for non-binary constraints.
         */
        public VAR getNeighbor(VAR var, IConstraint<VAR, VAL> constraint)
        {
            ICollection<VAR> scope = constraint.getScope();
            if (scope.Size() == 2)
            {
                if (var.Equals(scope.Get(0)))
                    return scope.Get(1);
                else if (var.Equals(scope.Get(1)))
                    return scope.Get(0);
            }
            return null;
        }

        /**
         * Returns a copy which contains a copy of the domains list and is in all
         * other aspects a flat copy of this.
         */
        public CSP<VAR, VAL> copyDomains()
        {
            CSP<VAR, VAL> result;
            result = new CSP<VAR, VAL>();
            result.domains = CollectionFactory.CreateQueue<Domain<VAL>>();
            result.domains.AddAll(domains);
            result.variables.AddAll(variables);
            result.constraints.AddAll(constraints);
            result.varIndexHash.AddAll(varIndexHash);
            result.cnet.AddAll(cnet);
            return result; 
             
        }
    }
}
