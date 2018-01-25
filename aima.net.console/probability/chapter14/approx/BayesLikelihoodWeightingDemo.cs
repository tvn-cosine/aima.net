using aima.net.probability.bayes.approximate;
using aima.net.probability.bayes.model;
using aima.net.probability.example;

namespace aima.net.demo.probability.chapter14.approx
{
    public class BayesLikelihoodWeightingDemo : ProbabilityDemoBase
    {
        static void Main(params string[] args)
        {
            bayesLikelihoodWeightingDemo();
        }

        static void bayesLikelihoodWeightingDemo()
        {
            System.Console.WriteLine("DEMO: Bayes Likelihood Weighting N = " + NUM_SAMPLES);
            System.Console.WriteLine("================================");
            demoToothacheCavityCatchModel(new FiniteBayesModel(
                    BayesNetExampleFactory.constructToothacheCavityCatchNetwork(),
                    new BayesInferenceApproxAdapter(new LikelihoodWeighting(),
                            NUM_SAMPLES)));
            demoBurglaryAlarmModel(new FiniteBayesModel(
                    BayesNetExampleFactory.constructBurglaryAlarmNetwork(),
                    new BayesInferenceApproxAdapter(new LikelihoodWeighting(),
                            NUM_SAMPLES)));
            System.Console.WriteLine("================================");
        }
    }
}
