using aima.net;
using aima.net.api;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.exceptions;
using aima.net.text;
using aima.net.text.api;
using aima.net.environment.cellworld;
using aima.net.learning.reinforcement.agent;
using aima.net.learning.reinforcement.example;
using aima.net.probability.example;

namespace aima.net.demo.learning.chapter21
{
    public abstract class LearningDemoBase
    {
        protected static void output_utility_learning_rates(
              ReinforcementAgent<Cell<double>, CellWorldAction> reinforcementAgent,
              int numRuns, int numTrialsPerRun, int rmseTrialsToReport,
              int reportEveryN)
        {

            if (rmseTrialsToReport > (numTrialsPerRun / reportEveryN))
            {
                throw new IllegalArgumentException("Requesting to report too many RMSE trials, max allowed for args is "
                                + (numTrialsPerRun / reportEveryN));
            }

            CellWorld<double> cw = CellWorldFactory.CreateCellWorldForFig17_1();
            CellWorldEnvironment cwe = new CellWorldEnvironment(
                    cw.GetCellAt(1, 1),
                    cw.GetCells(),
                    MDPFactory.createTransitionProbabilityFunctionForFigure17_1(cw),
                    CommonFactory.CreateRandom());

            cwe.AddAgent(reinforcementAgent);

            IMap<int, ICollection<IMap<Cell<double>, double>>> runs = CollectionFactory.CreateInsertionOrderedMap<int, ICollection<IMap<Cell<double>, double>>>();
            for (int r = 0; r < numRuns; r++)
            {
                reinforcementAgent.reset();
                ICollection<IMap<Cell<double>, double>> trials = CollectionFactory.CreateQueue<IMap<Cell<double>, double>>();
                for (int t = 0; t < numTrialsPerRun; t++)
                {
                    cwe.executeTrial();
                    if (0 == t % reportEveryN)
                    {
                        IMap<Cell<double>, double> u = reinforcementAgent
                                .getUtility();
                        //if (null == u.Get(cw.getCellAt(1, 1)))
                        //{
                        //    throw new IllegalStateException(
                        //            "Bad Utility State Encountered: r=" + r
                        //                    + ", t=" + t + ", u=" + u);
                        //}
                        trials.Add(u);
                    }
                }
                runs.Put(r, trials);
            }

            IStringBuilder v4_3 = TextFactory.CreateStringBuilder();
            IStringBuilder v3_3 = TextFactory.CreateStringBuilder();
            IStringBuilder v1_3 = TextFactory.CreateStringBuilder();
            IStringBuilder v1_1 = TextFactory.CreateStringBuilder();
            IStringBuilder v3_2 = TextFactory.CreateStringBuilder();
            IStringBuilder v2_1 = TextFactory.CreateStringBuilder();
            for (int t = 0; t < (numTrialsPerRun / reportEveryN); t++)
            {
                // Use the last run
                IMap<Cell<double>, double> u = runs.Get(numRuns - 1).Get(t);
                v4_3.Append((u.ContainsKey(cw.GetCellAt(4, 3)) ? u.Get(cw
                        .GetCellAt(4, 3)) : 0.0) + "\t");
                v3_3.Append((u.ContainsKey(cw.GetCellAt(3, 3)) ? u.Get(cw
                        .GetCellAt(3, 3)) : 0.0) + "\t");
                v1_3.Append((u.ContainsKey(cw.GetCellAt(1, 3)) ? u.Get(cw
                        .GetCellAt(1, 3)) : 0.0) + "\t");
                v1_1.Append((u.ContainsKey(cw.GetCellAt(1, 1)) ? u.Get(cw
                        .GetCellAt(1, 1)) : 0.0) + "\t");
                v3_2.Append((u.ContainsKey(cw.GetCellAt(3, 2)) ? u.Get(cw
                        .GetCellAt(3, 2)) : 0.0) + "\t");
                v2_1.Append((u.ContainsKey(cw.GetCellAt(2, 1)) ? u.Get(cw
                        .GetCellAt(2, 1)) : 0.0) + "\t");
            }

            IStringBuilder rmseValues = TextFactory.CreateStringBuilder();
            for (int t = 0; t < rmseTrialsToReport; t++)
            {
                // Calculate the Root Mean Square Error for utility of 1,1
                // for this trial# across all runs
                double xSsquared = 0;
                for (int r = 0; r < numRuns; r++)
                {
                    IMap<Cell<double>, double> u = runs.Get(r).Get(t);
                    double val1_1 = u.Get(cw.GetCellAt(1, 1));
                    //if (null == val1_1)
                    //{
                    //    throw new IllegalStateException(
                    //            "U(1,1,) is not present: r=" + r + ", t=" + t
                    //                    + ", runs.size=" + runs.Size()
                    //                    + ", runs(r).Size()=" + runs.Get(r).Size()
                    //                    + ", u=" + u);
                    //}
                    xSsquared += System.Math.Pow(0.705 - val1_1, 2);
                }
                double rmse = System.Math.Sqrt(xSsquared / runs.Size());
                rmseValues.Append(rmse);
                rmseValues.Append("\t");
            }

            System.Console
                .WriteLine("Note: You may copy and paste the following lines into a spreadsheet to generate graphs of learning rate and RMS error in utility:");
            System.Console.WriteLine("(4,3)" + "\t" + v4_3);
            System.Console.WriteLine("(3,3)" + "\t" + v3_3);
            System.Console.WriteLine("(1,3)" + "\t" + v1_3);
            System.Console.WriteLine("(1,1)" + "\t" + v1_1);
            System.Console.WriteLine("(3,2)" + "\t" + v3_2);
            System.Console.WriteLine("(2,1)" + "\t" + v2_1);
            System.Console.WriteLine("RMSeiu" + "\t" + rmseValues);
        }
    }
}
