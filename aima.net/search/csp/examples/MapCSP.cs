using aima.net.collections;
using aima.net.collections.api;

namespace aima.net.search.csp.examples
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Ed.): Figure 6.1, Page 204.<br>
     * <br>
     * The principal states and territories of Australia. Coloring this map can be
     * viewed as a constraint satisfaction problem (CSP). The goal is to assign
     * colors to each region so that no neighboring regions have the same color.
     * 
     * @author Ruediger Lunde
     * @author Mike Stampone
     */
    public class MapCSP : CSP<Variable, string>
    {

        public static readonly Variable NSW = new Variable("NSW");
        public static readonly Variable NT = new Variable("NT");
        public static readonly Variable Q = new Variable("Q");
        public static readonly Variable SA = new Variable("SA");
        public static readonly Variable T = new Variable("T");
        public static readonly Variable V = new Variable("V");
        public static readonly Variable WA = new Variable("WA");

        public static readonly string RED = "RED";
        public static readonly string GREEN = "GREEN";
        public static readonly string BLUE = "BLUE";

        /**
         * Constructs a map CSP for the principal states and territories of
         * Australia, with the colors Red, Green, and Blue.
         */
        public MapCSP()
                : base(CollectionFactory.CreateQueue<Variable>(new[] { NSW, WA, NT, Q, SA, V, T }))
        {
            Domain<string> colors = new Domain<string>(RED, GREEN, BLUE);
            foreach (Variable var in getVariables())
                setDomain(var, colors);

            addConstraint(new NotEqualConstraint<Variable, string>(WA, NT));
            addConstraint(new NotEqualConstraint<Variable, string>(WA, SA));
            addConstraint(new NotEqualConstraint<Variable, string>(NT, SA));
            addConstraint(new NotEqualConstraint<Variable, string>(NT, Q));
            addConstraint(new NotEqualConstraint<Variable, string>(SA, Q));
            addConstraint(new NotEqualConstraint<Variable, string>(SA, NSW));
            addConstraint(new NotEqualConstraint<Variable, string>(SA, V));
            addConstraint(new NotEqualConstraint<Variable, string>(Q, NSW));
            addConstraint(new NotEqualConstraint<Variable, string>(NSW, V));
        }
    }
}
