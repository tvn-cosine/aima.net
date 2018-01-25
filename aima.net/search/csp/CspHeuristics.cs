using aima.net.api;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.datastructures;
using aima.net.search.csp.api;

namespace aima.net.search.csp
{
    /**
     * Defines variable and value selection heuristics for CSP backtracking strategies.
     * @author Ruediger Lunde
     */
    public class CspHeuristics
    {
        public interface VariableSelection<VAR, VAL>
            where VAR : Variable
        {
            ICollection<VAR> apply(CSP<VAR, VAL> csp, ICollection<VAR> vars);
        }

        public interface ValueSelection<VAR, VAL>
            where VAR : Variable
        {
            ICollection<VAL> apply(CSP<VAR, VAL> csp, Assignment<VAR, VAL> assignment, VAR var);
        }

        public static VariableSelection<VAR, VAL> mrv<VAR, VAL>() where VAR : Variable
        {
            return new MrvHeuristic<VAR, VAL>();
        }

        public static VariableSelection<VAR, VAL> deg<VAR, VAL>() where VAR : Variable
        {
            return new DegHeuristic<VAR, VAL>();
        }

        public static VariableSelection<VAR, VAL> mrvDeg<VAR, VAL>()
            where VAR : Variable
        {
            return new MrvDegHeuristic<VAR, VAL>(); ;
        }

        public static ValueSelection<VAR, VAL> lcv<VAR, VAL>()
            where VAR : Variable
        {
            return new LcvHeuristic<VAR, VAL>();
        }

        public class MrvDegHeuristic<VAR, VAL> : VariableSelection<VAR, VAL>
            where VAR : Variable
        {
            public ICollection<VAR> apply(CSP<VAR, VAL> csp, ICollection<VAR> vars)
            {
                return new DegHeuristic<VAR, VAL>().apply(csp, new MrvHeuristic<VAR, VAL>().apply(csp, vars));
            }
        }

        /**
         * Implements the minimum-remaining-values heuristic.
         */
        public class MrvHeuristic<VAR, VAL> : VariableSelection<VAR, VAL>
            where VAR : Variable
        {

            /** Returns variables from <code>vars</code> which are the best with respect to MRV. */
            public ICollection<VAR> apply(CSP<VAR, VAL> csp, ICollection<VAR> vars)
            {
                ICollection<VAR> result = CollectionFactory.CreateQueue<VAR>();
                int mrv = int.MaxValue;
                foreach (VAR var in vars)
                {
                    int rv = csp.getDomain(var).size();
                    if (rv <= mrv)
                    {
                        if (rv < mrv)
                        {
                            result.Clear();
                            mrv = rv;
                        }
                        result.Add(var);
                    }
                }
                return result;
            }
        }

        /**
         * Implements the degree heuristic. Constraints with arbitrary scope size are supported.
         */
        public class DegHeuristic<VAR, VAL> : VariableSelection<VAR, VAL>
            where VAR : Variable
        {

            /** Returns variables from <code>vars</code> which are the best with respect to DEG. */
            public ICollection<VAR> apply(CSP<VAR, VAL> csp, ICollection<VAR> vars)
            {
                ICollection<VAR> result = CollectionFactory.CreateQueue<VAR>();
                int maxDegree = -1;
                foreach (VAR var in vars)
                {
                    int degree = csp.getConstraints(var).Size();
                    if (degree >= maxDegree)
                    {
                        if (degree > maxDegree)
                        {
                            result.Clear();
                            maxDegree = degree;
                        }
                        result.Add(var);
                    }
                }
                return result;
            }
        }

        class PairComparer<VAL> : IComparer<Pair<VAL, int>>
        {
            private readonly System.Collections.Generic.Comparer<int> comparer = System.Collections.Generic.Comparer<int>.Default;

            public int Compare(Pair<VAL, int> x, Pair<VAL, int> y)
            {
                return comparer.Compare(x.getSecond(), y.getSecond());
            }
        }

        /**
         * Implements the least constraining value heuristic.
         */
        public class LcvHeuristic<VAR, VAL> : ValueSelection<VAR, VAL>
            where VAR : Variable
        {

            /** Returns the values of Dom(var) in a special order. The least constraining value comes first. */
            public ICollection<VAL> apply(CSP<VAR, VAL> csp, Assignment<VAR, VAL> assignment, VAR var)
            {
                ICollection<Pair<VAL, int>> pairs = CollectionFactory.CreateQueue<Pair<VAL, int>>();
                foreach (VAL value in csp.getDomain(var))
                {
                    int num = countLostValues(csp, assignment, var, value);
                    pairs.Add(new Pair<VAL, int>(value, num));
                }

                pairs.Sort(new PairComparer<VAL>());
                ICollection<VAL> obj = CollectionFactory.CreateQueue<VAL>();

                foreach (Pair<VAL, int> val in pairs)
                {
                    obj.Add(val.GetFirst());
                }
                return obj;
            }

            /**
             * Ignores constraints which are not binary.
             */
            private int countLostValues(CSP<VAR, VAL> csp, Assignment<VAR, VAL> assignment, VAR var, VAL value)
            {
                int result = 0;
                Assignment<VAR, VAL> assign = new Assignment<VAR, VAL>();
                assign.add(var, value);
                foreach (IConstraint<VAR, VAL> constraint in csp.getConstraints(var))
                {
                    if (constraint.getScope().Size() == 2)
                    {
                        VAR neighbor = csp.getNeighbor(var, constraint);
                        if (!assignment.contains(neighbor))
                            foreach (VAL nValue in csp.getDomain(neighbor))
                            {
                                assign.add(neighbor, nValue);
                                if (!constraint.isSatisfiedWith(assign))
                                {
                                    ++result;
                                }
                            }
                    }
                }
                return result;
            }
        }
    }
}
