using aima.net.probability.bayes.approximate;
using aima.net.probability.bayes.model;
using aima.net.probability.example;

namespace aima.net.demo.probability.chapter14.approx
{
    public class BayesRejectionSamplingDemo : ProbabilityDemoBase
    {
        static void Main(params string[] args)
        {
            bayesRejectionSamplingDemo();
        }

        static void bayesRejectionSamplingDemo()
        {
            System.Console.WriteLine("DEMO: Bayes Rejection Sampling N = " + NUM_SAMPLES);
            System.Console.WriteLine("==============================");
            demoToothacheCavityCatchModel(new FiniteBayesModel(
                    BayesNetExampleFactory.constructToothacheCavityCatchNetwork(),
                    new BayesInferenceApproxAdapter(new RejectionSampling(),
                            NUM_SAMPLES)));
            demoBurglaryAlarmModel(new FiniteBayesModel(
                    BayesNetExampleFactory.constructBurglaryAlarmNetwork(),
                    new BayesInferenceApproxAdapter(new RejectionSampling(),
                            NUM_SAMPLES)));
            System.Console.WriteLine("==============================");
        }
    }
}
