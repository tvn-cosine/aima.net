using aima.net.collections;
using aima.net.collections.api;
using aima.net.probability;
using aima.net.probability.api;
using aima.net.probability.example;
using aima.net.probability.hmm.exact;
using aima.net.probability.proposition;

namespace aima.net.demo.probability.chapter15
{
    public class FixedLagSmoothingDemo : ProbabilityDemoBase
    {
        static void Main(params string[] args)
        {
            fixedLagSmoothingDemo();
        }

        static void fixedLagSmoothingDemo()
        {
            System.Console.WriteLine("DEMO: Fixed-Lag-Smoothing");
            System.Console.WriteLine("=========================");
            System.Console.WriteLine("Lag = 1");
            System.Console.WriteLine("-------");
            FixedLagSmoothing uw = new FixedLagSmoothing(HMMExampleFactory.getUmbrellaWorldModel(), 1);

            // Day 1 - Lag 1
            ICollection<AssignmentProposition> e1 = CollectionFactory.CreateQueue<AssignmentProposition>();
            e1.Add(new AssignmentProposition(ExampleRV.UMBREALLA_t_RV, true));

            ICategoricalDistribution smoothed = uw.fixedLagSmoothing(e1);

            System.Console.WriteLine("Day 1 (Umbrella_t=true) smoothed:\nday 1=" + smoothed);

            // Day 2 - Lag 1
            ICollection<AssignmentProposition> e2 = CollectionFactory.CreateQueue<AssignmentProposition>();
            e2.Add(new AssignmentProposition(ExampleRV.UMBREALLA_t_RV, true));

            smoothed = uw.fixedLagSmoothing(e2);

            System.Console.WriteLine("Day 2 (Umbrella_t=true) smoothed:\nday 1=" + smoothed);

            // Day 3 - Lag 1
            ICollection<AssignmentProposition> e3 = CollectionFactory.CreateQueue<AssignmentProposition>();
            e3.Add(new AssignmentProposition(ExampleRV.UMBREALLA_t_RV,                    false));

            smoothed = uw.fixedLagSmoothing(e3);

            System.Console.WriteLine("Day 3 (Umbrella_t=false) smoothed:\nday 2=" + smoothed);

            System.Console.WriteLine("-------");
            System.Console.WriteLine("Lag = 2");
            System.Console.WriteLine("-------");

            uw = new FixedLagSmoothing(HMMExampleFactory.getUmbrellaWorldModel(), 2);

            // Day 1 - Lag 2
            e1 = CollectionFactory.CreateQueue<AssignmentProposition>();
            e1.Add(new AssignmentProposition(ExampleRV.UMBREALLA_t_RV, true));

            smoothed = uw.fixedLagSmoothing(e1);

            System.Console.WriteLine("Day 1 (Umbrella_t=true) smoothed:\nday 1=" + smoothed);

            // Day 2 - Lag 2
            e2 = CollectionFactory.CreateQueue<AssignmentProposition>();
            e2.Add(new AssignmentProposition(ExampleRV.UMBREALLA_t_RV, true));

            smoothed = uw.fixedLagSmoothing(e2);

            System.Console.WriteLine("Day 2 (Umbrella_t=true) smoothed:\nday 1=" + smoothed);

            // Day 3 - Lag 2
            e3 = CollectionFactory.CreateQueue<AssignmentProposition>();
            e3.Add(new AssignmentProposition(ExampleRV.UMBREALLA_t_RV, false));

            smoothed = uw.fixedLagSmoothing(e3);

            System.Console.WriteLine("Day 3 (Umbrella_t=false) smoothed:\nday 1=" + smoothed);

            System.Console.WriteLine("=========================");
        }
    }
}
