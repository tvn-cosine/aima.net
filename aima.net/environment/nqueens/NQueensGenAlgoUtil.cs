using aima.net;
using aima.net.api;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.datastructures;
using aima.net.search.framework.problem;
using aima.net.search.local;
using aima.net.search.local.api;

namespace aima.net.environment.nqueens
{
    /**
     * A class whose purpose is to provide static utility methods for solving the
     * n-queens problem with genetic algorithms. This includes fitness function,
     * goal test, random creation of individuals and convenience methods for
     * translating between between an NQueensBoard representation and the int list
     * representation used by the GeneticAlgorithm.
     * 
     * @author Ciaran O'Reilly
     * @author Ruediger Lunde
     * 
     */
    public class NQueensGenAlgoUtil
    {

        public static IFitnessFunction<int> getFitnessFunction()
        {
            return new NQueensFitnessFunction();
        }

        public static GoalTest<Individual<int>> getGoalTest()
        {
            return new NQueensGenAlgoGoalTest().test;
        }


        public static Individual<int> generateRandomIndividual(int boardSize)
        {
            ICollection<int> individualRepresentation = CollectionFactory.CreateQueue<int>();
            for (int i = 0; i < boardSize;++i)
            {
                individualRepresentation.Add(CommonFactory.CreateRandom().Next(boardSize));
            }
            return new Individual<int>(individualRepresentation);
        }

        public static ICollection<int> getFiniteAlphabetForBoardOfSize(int size)
        {
            ICollection<int> fab = CollectionFactory.CreateQueue<int>();

            for (int i = 0; i < size;++i)
            {
                fab.Add(i);
            }

            return fab;
        }

        public class NQueensFitnessFunction : IFitnessFunction<int>
        {
            public double apply(Individual<int> individual)
            {
                double fitness = 0;

                NQueensBoard board = getBoardForIndividual(individual);
                int boardSize = board.getSize();

                // Calculate the number of non-attacking pairs of queens (refer to
                // AIMA
                // page 117).
                ICollection<XYLocation> qPositions = board.getQueenPositions();
                for (int fromX = 0; fromX < (boardSize - 1); fromX++)
                {
                    for (int toX = fromX + 1; toX < boardSize; toX++)
                    {
                        int fromY = qPositions.Get(fromX).GetYCoOrdinate();
                        bool nonAttackingPair = true;
                        // Check right beside
                        int toY = fromY;
                        if (board.queenExistsAt(new XYLocation(toX, toY)))
                        {
                            nonAttackingPair = false;
                        }
                        // Check right and above
                        toY = fromY - (toX - fromX);
                        if (toY >= 0)
                        {
                            if (board.queenExistsAt(new XYLocation(toX, toY)))
                            {
                                nonAttackingPair = false;
                            }
                        }
                        // Check right and below
                        toY = fromY + (toX - fromX);
                        if (toY < boardSize)
                        {
                            if (board.queenExistsAt(new XYLocation(toX, toY)))
                            {
                                nonAttackingPair = false;
                            }
                        }

                        if (nonAttackingPair)
                        {
                            fitness += 1.0;
                        }
                    }
                }

                return fitness;
            }
        }

        public class NQueensGenAlgoGoalTest  
        {
            private readonly GoalTest<NQueensBoard> goalTest = NQueensFunctions.testGoal;

            public bool test(Individual<int> state)
            {
                return goalTest(getBoardForIndividual(state));
            }
        }

        public static NQueensBoard getBoardForIndividual(Individual<int> individual)
        {
            int boardSize = individual.length();
            NQueensBoard board = new NQueensBoard(boardSize);
            for (int i = 0; i < boardSize;++i)
            {
                int pos = individual.getRepresentation().Get(i);
                board.addQueenAt(new XYLocation(i, pos));
            }
            return board;
        }
    }
}
