using aima.net.collections;
using aima.net.collections.api;
using aima.net.probability;
using aima.net.probability.api;
using aima.net.probability.example;
using aima.net.probability.proposition;
using aima.net.probability.temporal.generic;
using aima.net.probability.util;

namespace aima.net.demo.probability.chapter15
{
    public class ForwardBackWardDemo : ProbabilityDemoBase
    {
        static void Main(params string[] args)
        {
            forwardBackWardDemo();
        }

        static void forwardBackWardDemo()
        {
            System.Console.WriteLine("DEMO: Forward-BackWard");
            System.Console.WriteLine("======================");

            System.Console.WriteLine("Umbrella World");
            System.Console.WriteLine("--------------");
            ForwardBackward uw = new ForwardBackward(
                    GenericTemporalModelFactory.getUmbrellaWorldTransitionModel(),
                    GenericTemporalModelFactory.getUmbrellaWorld_Xt_to_Xtm1_Map(),
                    GenericTemporalModelFactory.getUmbrellaWorldSensorModel());

            ICategoricalDistribution prior = new ProbabilityTable(new double[] {
                0.5, 0.5 }, ExampleRV.RAIN_t_RV);

            // Day 1
            ICollection<ICollection<AssignmentProposition>> evidence = CollectionFactory.CreateQueue<ICollection<AssignmentProposition>>();
            ICollection<AssignmentProposition> e1 = CollectionFactory.CreateQueue<AssignmentProposition>();
            e1.Add(new AssignmentProposition(ExampleRV.UMBREALLA_t_RV, true));
            evidence.Add(e1);

            ICollection<ICategoricalDistribution> smoothed = uw.forwardBackward(evidence, prior);

            System.Console.WriteLine("Day 1 (Umbrealla_t=true) smoothed:\nday 1 = " + smoothed.Get(0));

            // Day 2
            ICollection<AssignmentProposition> e2 = CollectionFactory.CreateQueue<AssignmentProposition>();
            e2.Add(new AssignmentProposition(ExampleRV.UMBREALLA_t_RV, true));
            evidence.Add(e2);

            smoothed = uw.forwardBackward(evidence, prior);

            System.Console.WriteLine("Day 2 (Umbrealla_t=true) smoothed:\nday 1 = "
                    + smoothed.Get(0) + "\nday 2 = " + smoothed.Get(1));

            // Day 3
            ICollection<AssignmentProposition> e3 = CollectionFactory.CreateQueue<AssignmentProposition>();
            e3.Add(new AssignmentProposition(ExampleRV.UMBREALLA_t_RV, false));
            evidence.Add(e3);

            smoothed = uw.forwardBackward(evidence, prior);

            System.Console.WriteLine("Day 3 (Umbrealla_t=false) smoothed:\nday 1 = "
                    + smoothed.Get(0) + "\nday 2 = " + smoothed.Get(1)
                    + "\nday 3 = " + smoothed.Get(2));

            System.Console.WriteLine("======================");
        }
    }
}
