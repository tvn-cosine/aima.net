using aima.net.collections;
using aima.net.collections.api;
using aima.net.exceptions;
using aima.net.environment.nqueens;
using aima.net.search.framework.problem;
using aima.net.search.local;
using aima.net.search.local.api;

namespace aima.net.demo.search.nqueens
{
    public class NQueensGeneticAlgorithmSearch : NQueensDemoBase
    {
        static void Main(params string[] args)
        {
            nQueensGeneticAlgorithmSearch();
        }

        static void nQueensGeneticAlgorithmSearch()
        {
            System.Console.WriteLine("\nNQueensDemo GeneticAlgorithm  -->");
            try
            {
                IFitnessFunction<int> fitnessFunction = NQueensGenAlgoUtil.getFitnessFunction();
                GoalTest<Individual<int>> goalTest = NQueensGenAlgoUtil.getGoalTest();
                // Generate an initial population
                ISet<Individual<int>> population = CollectionFactory.CreateSet<Individual<int>>();
                for (int i = 0; i < 50; ++i)
                {
                    population.Add(NQueensGenAlgoUtil.generateRandomIndividual(boardSize));
                }

                GeneticAlgorithm<int> ga = new GeneticAlgorithm<int>(boardSize,
                        NQueensGenAlgoUtil.getFiniteAlphabetForBoardOfSize(boardSize), 0.15);

                // Run for a set amount of time
                Individual<int> bestIndividual = ga.geneticAlgorithm(population, fitnessFunction, goalTest, 1000L);

                System.Console.WriteLine("Max Time (1 second) Best Individual=\n"
                        + NQueensGenAlgoUtil.getBoardForIndividual(bestIndividual));
                System.Console.WriteLine("Board Size      = " + boardSize);
                //System.Console.WriteLine("# Board Layouts = " + (new BigDecimal(boardSize)).pow(boardSize)/*);*/
                System.Console.WriteLine("Fitness         = " + fitnessFunction.apply(bestIndividual));
                System.Console.WriteLine("Is Goal         = " + goalTest(bestIndividual));
                System.Console.WriteLine("Population Size = " + ga.getPopulationSize());
                System.Console.WriteLine("Iterations      = " + ga.getIterations());
                System.Console.WriteLine("Took            = " + ga.getTimeInMilliseconds() + "ms.");

                // Run till goal is achieved
                bestIndividual = ga.geneticAlgorithm(population, fitnessFunction, goalTest, 0L);

                System.Console.WriteLine("");
                System.Console
                    .WriteLine("Goal Test Best Individual=\n" + NQueensGenAlgoUtil.getBoardForIndividual(bestIndividual));
                System.Console.WriteLine("Board Size      = " + boardSize);
                //System.Console.WriteLine("# Board Layouts = " + (new BigDecimal(boardSize)).pow(boardSize)/*);*/
                System.Console.WriteLine("Fitness         = " + fitnessFunction.apply(bestIndividual));
                System.Console.WriteLine("Is Goal         = " + goalTest(bestIndividual));
                System.Console.WriteLine("Population Size = " + ga.getPopulationSize());
                System.Console.WriteLine("Itertions       = " + ga.getIterations());
                System.Console.WriteLine("Took            = " + ga.getTimeInMilliseconds() + "ms.");

            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
