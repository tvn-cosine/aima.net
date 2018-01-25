using aima.net.probability.example;

namespace aima.net.demo.probability.chapter13
{
    public class FullJointDistributionModelDemo : ProbabilityDemoBase
    {
        public static void Main(params string[] args)
        {
            fullJointDistributionModelDemo();
        }

        static void fullJointDistributionModelDemo()
        {
            System.Console.WriteLine("DEMO: Full Joint Distribution Model");
            System.Console.WriteLine("===================================");
            demoToothacheCavityCatchModel(new FullJointDistributionToothacheCavityCatchModel());
            demoBurglaryAlarmModel(new FullJointDistributionBurglaryAlarmModel());
            System.Console.WriteLine("===================================");
        }
    }
}
