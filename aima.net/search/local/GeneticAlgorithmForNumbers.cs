using aima.net.collections;
using aima.net.collections.api;

namespace aima.net.search.local
{
    /**
     * Variant of the genetic algorithm which uses double numbers from a fixed
     * interval instead of symbols from a finite alphabet in the representations of
     * individuals. Reproduction uses values somewhere between the values of the
     * parents. Mutation adds some random offset. Progress tracer implementations
     * can be used to get informed about the running iterations. <br>
     * A typical use case for this genetic algorithm version is finding maximums in
     * a given mathematical (fitness) function.
     * 
     * @author Ruediger Lunde
     *
     */
    public class GeneticAlgorithmForNumbers : GeneticAlgorithm<double>
    {
        private double minimum;
        private double maximum;

        /**
         * Constructor.
         * 
         * @param individualLength
         *            vector length used for the representations of individuals. Use
         *            1 for analysis of functions f(x).
         * @param min
         *            minimal value to be used in vector elements.
         * @param max
         *            maximal value to be used in vector elements.
         * @param mutationProbability
         *            probability of mutations.
         */
        public GeneticAlgorithmForNumbers(int individualLength, double min, double max, double mutationProbability)
                : base(individualLength, CollectionFactory.CreateQueue<double>(), mutationProbability)
        {
            minimum = min;
            maximum = max;
        }

        /** Convenience method. */
        public Individual<double> createRandomIndividual()
        {
            ICollection<double> representation = CollectionFactory.CreateQueue<double>();
            for (int i = 0; i < individualLength;++i)
                representation.Add(minimum + random.NextDouble() * (maximum - minimum));
            return new Individual<double>(representation);
        }

        /**
         * Produces for each number in the descendant's representation a random
         * value between the corresponding values of its parents.
         */

        protected override Individual<double> reproduce(Individual<double> x, Individual<double> y)
        {
            ICollection<double> newRep = CollectionFactory.CreateQueue<double>();
            double r = random.NextDouble();
            for (int i = 0; i < x.length();++i)
                newRep.Add(x.getRepresentation().Get(i) * r + y.getRepresentation().Get(i) * (1 - r));
            return new Individual<double>(newRep);
        }

        /**
         * Changes each component in the representation by random. The maximum
         * change is +- (maximum - minimum) / 4, but smaller changes have a higher
         * likelihood.
         */

        protected override Individual<double> mutate(Individual<double> child)
        {
            ICollection<double> rep = child.getRepresentation();
            ICollection<double> newRep = CollectionFactory.CreateQueue<double>();
            foreach (double numIter in rep)
            {
                double num = numIter;
                double r = random.NextDouble() - 0.5;
                num += r * r * r * (maximum - minimum) / 2;
                if (num < minimum)
                    num = minimum;
                else if (num > maximum)
                    num = maximum;
                newRep.Add(num);
            }
            return new Individual<double>(newRep);
        }
    }

}
