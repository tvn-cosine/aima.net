using aima.net.probability;
using aima.net.probability.api;
using aima.net.probability.example;
using aima.net.probability.proposition;

namespace aima.net.demo.probability
{
    public class ProbabilityDemoBase
    {
        // Note: You should increase this to 1000000+
        // in order to get answers from the approximate
        // algorithms (i.e. Rejection, Likelihood and Gibbs)
        // that look close to their exact inference
        // counterparts.
        public const int NUM_SAMPLES = 1000;

        protected static void demoToothacheCavityCatchModel(IFiniteProbabilityModel model)
        {
            System.Console.WriteLine("Toothache, Cavity, and Catch Model");
            System.Console.WriteLine("----------------------------------");
            AssignmentProposition atoothache = new AssignmentProposition(
                    ExampleRV.TOOTHACHE_RV, true);
            AssignmentProposition acavity = new AssignmentProposition(
                    ExampleRV.CAVITY_RV, true);
            AssignmentProposition anotcavity = new AssignmentProposition(
                    ExampleRV.CAVITY_RV, false);
            AssignmentProposition acatch = new AssignmentProposition(
                    ExampleRV.CATCH_RV, true);

            // AIMA3e pg. 485
            System.Console.WriteLine("P(cavity) = " + model.prior(acavity));
            System.Console.WriteLine("P(cavity | toothache) = "
                    + model.posterior(acavity, atoothache));

            // AIMA3e pg. 492
            DisjunctiveProposition cavityOrToothache = new DisjunctiveProposition(
                    acavity, atoothache);
            System.Console.WriteLine("P(cavity OR toothache) = "
                    + model.prior(cavityOrToothache));

            // AIMA3e pg. 493
            System.Console.WriteLine("P(~cavity | toothache) = "
                    + model.posterior(anotcavity, atoothache));

            // AIMA3e pg. 493
            // P<>(Cavity | toothache) = <0.6, 0.4>
            System.Console.WriteLine("P<>(Cavity | toothache) = "
                    + model.posteriorDistribution(ExampleRV.CAVITY_RV, atoothache));

            // AIMA3e pg. 497
            // P<>(Cavity | toothache AND catch) = <0.871, 0.129>
            System.Console.WriteLine("P<>(Cavity | toothache AND catch) = "
                    + model.posteriorDistribution(ExampleRV.CAVITY_RV, atoothache,
                            acatch));
        }

        protected static void demoBurglaryAlarmModel(IFiniteProbabilityModel model)
        {
            System.Console.WriteLine("--------------------");
            System.Console.WriteLine("Burglary Alarm Model");
            System.Console.WriteLine("--------------------");

            AssignmentProposition aburglary = new AssignmentProposition(
                    ExampleRV.BURGLARY_RV, true);
            AssignmentProposition anotburglary = new AssignmentProposition(
                    ExampleRV.BURGLARY_RV, false);
            AssignmentProposition anotearthquake = new AssignmentProposition(
                    ExampleRV.EARTHQUAKE_RV, false);
            AssignmentProposition aalarm = new AssignmentProposition(
                    ExampleRV.ALARM_RV, true);
            AssignmentProposition anotalarm = new AssignmentProposition(
                    ExampleRV.ALARM_RV, false);
            AssignmentProposition ajohnCalls = new AssignmentProposition(
                    ExampleRV.JOHN_CALLS_RV, true);
            AssignmentProposition amaryCalls = new AssignmentProposition(
                    ExampleRV.MARY_CALLS_RV, true);

            // AIMA3e pg. 514
            System.Console.WriteLine("P(j,m,a,~b,~e) = "
                    + model.prior(ajohnCalls, amaryCalls, aalarm, anotburglary,
                            anotearthquake));
            System.Console.WriteLine("P(j,m,~a,~b,~e) = "
                    + model.prior(ajohnCalls, amaryCalls, anotalarm, anotburglary,
                            anotearthquake));

            // AIMA3e. pg. 514
            // P<>(Alarm | JohnCalls = true, MaryCalls = true, Burglary = false,
            // Earthquake = false)
            // = <0.558, 0.442>
            System.Console
                .WriteLine("P<>(Alarm | JohnCalls = true, MaryCalls = true, Burglary = false, Earthquake = false) = "
                        + model.posteriorDistribution(ExampleRV.ALARM_RV,
                                ajohnCalls, amaryCalls, anotburglary,
                                anotearthquake));

            // AIMA3e pg. 523
            // P<>(Burglary | JohnCalls = true, MaryCalls = true) = <0.284, 0.716>
            System.Console
                .WriteLine("P<>(Burglary | JohnCalls = true, MaryCalls = true) = "
                        + model.posteriorDistribution(ExampleRV.BURGLARY_RV,
                                ajohnCalls, amaryCalls));

            // AIMA3e pg. 528
            // P<>(JohnCalls | Burglary = true)
            System.Console.WriteLine("P<>(JohnCalls | Burglary = true) = "
                    + model.posteriorDistribution(ExampleRV.JOHN_CALLS_RV,
                            aburglary));
        }
    }
}
