using aima.net;
using aima.net.api;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.environment.cellworld;
using aima.net.learning.reinforcement.agent;
using aima.net.learning.reinforcement.example;
using aima.net.probability.example;

namespace aima.net.demo.learning.chapter21
{
    public class PassiveTDAgentDemo : LearningDemoBase
    {
        static void Main(params string[] args)
        {
            System.Console.WriteLine("======================");
            System.Console.WriteLine("DEMO: Passive-TD-Agent");
            System.Console.WriteLine("======================");
            System.Console.WriteLine("Figure 21.5");
            System.Console.WriteLine("-----------");
            passiveTDAgentDemo();
            System.Console.WriteLine("=========================");
        }

        static void passiveTDAgentDemo()
        {
            CellWorld<double> cw = CellWorldFactory.CreateCellWorldForFig17_1();
            CellWorldEnvironment cwe = new CellWorldEnvironment(
                    cw.GetCellAt(1, 1),
                    cw.GetCells(),
                    MDPFactory.createTransitionProbabilityFunctionForFigure17_1(cw),
                    CommonFactory.CreateRandom());

            IMap<Cell<double>, CellWorldAction> fixedPolicy = CollectionFactory.CreateMap<Cell<double>, CellWorldAction>();
            fixedPolicy.Put(cw.GetCellAt(1, 1), CellWorldAction.Up);
            fixedPolicy.Put(cw.GetCellAt(1, 2), CellWorldAction.Up);
            fixedPolicy.Put(cw.GetCellAt(1, 3), CellWorldAction.Right);
            fixedPolicy.Put(cw.GetCellAt(2, 1), CellWorldAction.Left);
            fixedPolicy.Put(cw.GetCellAt(2, 3), CellWorldAction.Right);
            fixedPolicy.Put(cw.GetCellAt(3, 1), CellWorldAction.Left);
            fixedPolicy.Put(cw.GetCellAt(3, 2), CellWorldAction.Up);
            fixedPolicy.Put(cw.GetCellAt(3, 3), CellWorldAction.Right);
            fixedPolicy.Put(cw.GetCellAt(4, 1), CellWorldAction.Left);

            PassiveTDAgent<Cell<double>, CellWorldAction> ptda
                = new PassiveTDAgent<Cell<double>, CellWorldAction>(fixedPolicy, 0.2, 1.0);

            cwe.AddAgent(ptda);

            output_utility_learning_rates(ptda, 20, 500, 100, 1); 
        }
    }
}
