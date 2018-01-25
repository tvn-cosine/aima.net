using aima.net.collections.api;
using aima.net.environment.cellworld;
using aima.net.probability.example;
using aima.net.probability.mdp;
using aima.net.probability.mdp.api;
using aima.net.probability.mdp.search;

namespace aima.net.demo.probability.chapter17
{
    public class ValueIterationDemo : ProbabilityDemoBase
    {
        static void Main(params string[] args)
        {
            valueIterationDemo();
        }

        static void valueIterationDemo()
        { 
            System.Console.WriteLine("DEMO: Value Iteration");
            System.Console.WriteLine("=====================");
            System.Console.WriteLine("Figure 17.3");
            System.Console.WriteLine("-----------");

            CellWorld<double> cw = CellWorldFactory.CreateCellWorldForFig17_1();
            IMarkovDecisionProcess<Cell<double>, CellWorldAction> mdp = MDPFactory.createMDPForFigure17_3(cw);
            ValueIteration<Cell<double>, CellWorldAction>
                vi = new ValueIteration<Cell<double>, CellWorldAction>(1.0);

            IMap<Cell<double>, double> U = vi.valueIteration(mdp, 0.0001);

            System.Console.WriteLine("(1,1) = " + U.Get(cw.GetCellAt(1, 1)));
            System.Console.WriteLine("(1,2) = " + U.Get(cw.GetCellAt(1, 2)));
            System.Console.WriteLine("(1,3) = " + U.Get(cw.GetCellAt(1, 3)));

            System.Console.WriteLine("(2,1) = " + U.Get(cw.GetCellAt(2, 1)));
            System.Console.WriteLine("(2,3) = " + U.Get(cw.GetCellAt(2, 3)));

            System.Console.WriteLine("(3,1) = " + U.Get(cw.GetCellAt(3, 1)));
            System.Console.WriteLine("(3,2) = " + U.Get(cw.GetCellAt(3, 2)));
            System.Console.WriteLine("(3,3) = " + U.Get(cw.GetCellAt(3, 3)));

            System.Console.WriteLine("(4,1) = " + U.Get(cw.GetCellAt(4, 1)));
            System.Console.WriteLine("(4,2) = " + U.Get(cw.GetCellAt(4, 2)));
            System.Console.WriteLine("(4,3) = " + U.Get(cw.GetCellAt(4, 3)));

            System.Console.WriteLine("=========================");
        } 
    }
}
