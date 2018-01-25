using aima.net.environment.cellworld;
using aima.net.probability.example;
using aima.net.probability.mdp;
using aima.net.probability.mdp.api;
using aima.net.probability.mdp.search;

namespace aima.net.demo.probability.chapter17
{
    public class PolicyIterationDemo : ProbabilityDemoBase
    {
        static void Main(params string[] args)
        {
            policyIterationDemo();
        }

        static void policyIterationDemo()
        {
            System.Console.WriteLine("DEMO: Policy Iteration");
            System.Console.WriteLine("======================");
            System.Console.WriteLine("Figure 17.3");
            System.Console.WriteLine("-----------");

            CellWorld<double> cw = CellWorldFactory.CreateCellWorldForFig17_1();
            IMarkovDecisionProcess<Cell<double>, CellWorldAction> mdp = MDPFactory.createMDPForFigure17_3(cw);
            PolicyIteration<Cell<double>, CellWorldAction> 
                pi = new PolicyIteration<Cell<double>, CellWorldAction>(
                    new ModifiedPolicyEvaluation<Cell<double>, CellWorldAction>(50, 1.0));

            IPolicy<Cell<double>, CellWorldAction> policy = pi.policyIteration(mdp);

            System.Console.WriteLine("(1,1) = " + policy.action(cw.GetCellAt(1, 1)));
            System.Console.WriteLine("(1,2) = " + policy.action(cw.GetCellAt(1, 2)));
            System.Console.WriteLine("(1,3) = " + policy.action(cw.GetCellAt(1, 3)));

            System.Console.WriteLine("(2,1) = " + policy.action(cw.GetCellAt(2, 1)));
            System.Console.WriteLine("(2,3) = " + policy.action(cw.GetCellAt(2, 3)));

            System.Console.WriteLine("(3,1) = " + policy.action(cw.GetCellAt(3, 1)));
            System.Console.WriteLine("(3,2) = " + policy.action(cw.GetCellAt(3, 2)));
            System.Console.WriteLine("(3,3) = " + policy.action(cw.GetCellAt(3, 3)));

            System.Console.WriteLine("(4,1) = " + policy.action(cw.GetCellAt(4, 1)));
            System.Console.WriteLine("(4,2) = " + policy.action(cw.GetCellAt(4, 2)));
            System.Console.WriteLine("(4,3) = " + policy.action(cw.GetCellAt(4, 3)));

            System.Console.WriteLine("=========================");
        }
    }
}
