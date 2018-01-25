using aima.net.probability.bayes.approximate;
using aima.net.probability.example;
using aima.net.probability.proposition;
using aima.net.util;

namespace aima.net.demo.probability.chapter15
{
    public class ParticleFilterinfDemo : ProbabilityDemoBase
    {
        static void Main(params string[] args)
        {
            particleFilterinfDemo();
        }

        static void particleFilterinfDemo()
        {
            System.Console.WriteLine("DEMO: Particle-Filtering");
            System.Console.WriteLine("========================");
            System.Console.WriteLine("Figure 15.18");
            System.Console.WriteLine("------------");

            MockRandomizer mr = new MockRandomizer(new double[] {
				// Prior Sample:
				// 8 with Rain_t-1=true from prior distribution
				0.5, 0.5, 0.5, 0.5, 0.5, 0.5, 0.5, 0.5,
				// 2 with Rain_t-1=false from prior distribution
				0.6, 0.6,
				// (a) Propagate 6 samples Rain_t=true
				0.7, 0.7, 0.7, 0.7, 0.7, 0.7,
				// 4 samples Rain_t=false
				0.71, 0.71, 0.31, 0.31,
				// (b) Weight should be for first 6 samples:
				// Rain_t-1=true, Rain_t=true, Umbrella_t=false = 0.1
				// Next 2 samples:
				// Rain_t-1=true, Rain_t=false, Umbrealla_t=false= 0.8
				// Final 2 samples:
				// Rain_t-1=false, Rain_t=false, Umbrella_t=false = 0.8
				// gives W[] =
				// [0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.8, 0.8, 0.8, 0.8]
				// normalized =
				// [0.026, ...., 0.211, ....] is approx. 0.156 = true
				// the remainder is false
				// (c) Resample 2 Rain_t=true, 8 Rain_t=false
				0.15, 0.15, 0.2, 0.2, 0.2, 0.2, 0.2, 0.2, 0.2, 0.2,
				//
				// Next Sample:
				// (a) Propagate 1 samples Rain_t=true
				0.7,
				// 9 samples Rain_t=false
				0.71, 0.31, 0.31, 0.31, 0.31, 0.31, 0.31, 0.31, 0.31,
				// (c) resample 1 Rain_t=true, 9 Rain_t=false
				0.0001, 0.2, 0.2, 0.2, 0.2, 0.2, 0.2, 0.2, 0.2, 0.2 });

            int N = 10;
            ParticleFiltering pf = new ParticleFiltering(N, DynamicBayesNetExampleFactory.getUmbrellaWorldNetwork(), mr);

            AssignmentProposition[] e = new AssignmentProposition[] { new AssignmentProposition(ExampleRV.UMBREALLA_t_RV, false) };

            System.Console.WriteLine("First Sample Set:");
            AssignmentProposition[][] S = pf.particleFiltering(e);
            for (int i = 0; i < N; ++i)
            {
                System.Console.WriteLine("Sample " + (i + 1) + " = " + S[i][0]);
            }
            System.Console.WriteLine("Second Sample Set:");
            S = pf.particleFiltering(e);
            for (int i = 0; i < N; ++i)
            {
                System.Console.WriteLine("Sample " + (i + 1) + " = " + S[i][0]);
            }

            System.Console.WriteLine("========================");
        }
    }
}
