using aima.net.search.framework.problem.api;

namespace aima.net.search.framework.problem
{  
    public static class ProblemHelper
    {
        /**
         * Tests whether a node represents an acceptable solution. The default implementation
         * delegates the check to the goal test. Other implementations could make use of the additional
         * information given by the node (e.g. the sequence of actions leading to the node). A
         * solution tester implementation could for example always return false and internally collect
         * the paths of all nodes whose state passes the goal test. Search implementations should always
         * access the goal test via this method to support solution acceptance testing.
         */
        public static bool testSolution<S, A>(this IProblem<S, A> problem, Node<S, A> node)
        { 
            return problem.testGoal(node.getState());
        }
    }
}
