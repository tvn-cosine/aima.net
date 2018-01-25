using aima.net.probability.bayes.approximate;
using aima.net.probability.bayes.model;
using aima.net.probability.example;

namespace aima.net.demo.probability.chapter14.approx
{
    class BayesGibbsAskDemo : ProbabilityDemoBase
    {
        static void Main(params string[] args)
        {
            bayesGibbsAskDemo();
        }

        static void bayesGibbsAskDemo()
        {
            System.Console.WriteLine("DEMO: Bayes Gibbs Ask N = " + NUM_SAMPLES);
            System.Console.WriteLine("=====================");
            demoToothacheCavityCatchModel(new FiniteBayesModel(
                    BayesNetExampleFactory.constructToothacheCavityCatchNetwork(),
                    new BayesInferenceApproxAdapter(new GibbsAsk(), NUM_SAMPLES)));
            demoBurglaryAlarmModel(new FiniteBayesModel(
                    BayesNetExampleFactory.constructBurglaryAlarmNetwork(),
                    new BayesInferenceApproxAdapter(new GibbsAsk(), NUM_SAMPLES)));
            System.Console.WriteLine("=====================");
        } 
    }
}
