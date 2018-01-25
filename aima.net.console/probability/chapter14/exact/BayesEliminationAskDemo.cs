using aima.net.probability.bayes.exact;
using aima.net.probability.bayes.model;
using aima.net.probability.example;

namespace aima.net.demo.probability.chapter14.exact
{
    public class BayesEliminationAskDemo : ProbabilityDemoBase
    {
        public static void Main(params string[] args)
        {
            bayesEliminationAskDemo();
        }

        static void bayesEliminationAskDemo()
        {
            System.Console.WriteLine("DEMO: Bayes Elimination Ask");
            System.Console.WriteLine("===========================");
            demoToothacheCavityCatchModel(new FiniteBayesModel(
                    BayesNetExampleFactory.constructToothacheCavityCatchNetwork(),
                    new EliminationAsk()));
            demoBurglaryAlarmModel(new FiniteBayesModel(
                    BayesNetExampleFactory.constructBurglaryAlarmNetwork(),
                    new EliminationAsk()));
            System.Console.WriteLine("===========================");
        }
    }
}
