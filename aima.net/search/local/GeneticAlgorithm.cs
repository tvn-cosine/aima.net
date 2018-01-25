using aima.net.api;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.exceptions;
using aima.net.search.framework;
using aima.net.search.framework.problem;
using aima.net.search.local.api;
using aima.net.util;
using aima.net;

namespace aima.net.search.local
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): Figure 4.8, page
     * 129.<br>
     * <br>
     * 
     * <pre>
     * function GENETIC-ALGORITHM(population, FITNESS-FN) returns an individual
     *   inputs: population, a set of individuals
     *           FITNESS-FN, a function that measures the fitness of an individual
     *           
     *   repeat
     *     new_population &lt;- empty set
     *     for i = 1 to SIZE(population) do
     *       x &lt;- RANDOM-SELECTION(population, FITNESS-FN)
     *       y &lt;- RANDOM-SELECTION(population, FITNESS-FN)
     *       child &lt;- REPRODUCE(x, y)
     *       if (small random probability) then child &lt;- MUTATE(child)
     *       add child to new_population
     *     population &lt;- new_population
     *   until some individual is fit enough, or enough time has elapsed
     *   return the best individual in population, according to FITNESS-FN
     * --------------------------------------------------------------------------------
     * function REPRODUCE(x, y) returns an individual
     *   inputs: x, y, parent individuals
     *   
     *   n &lt;- LENGTH(x); c &lt;- random number from 1 to n
     *   return APPEND(SUBSTRING(x, 1, c), SUBSTRING(y, c+1, n))
     * </pre>
     * 
     * Figure 4.8 A genetic algorithm. The algorithm is the same as the one
     * diagrammed in Figure 4.6, with one variation: in this more popular version,
     * each mating of two parents produces only one offspring, not two.
     * 
     * @author Ciaran O'Reilly
     * @author Mike Stampone
     * @author Ruediger Lunde
     * 
     * @param <A>
     *            the type of the alphabet used in the representation of the
     *            individuals in the population (this is to provide flexibility in
     *            terms of how a problem can be encoded).
     */
    public class GeneticAlgorithm<A>
    {
        protected const string POPULATION_SIZE = "populationSize";
        protected const string ITERATIONS = "iterations";
        protected const string TIME_IN_MILLISECONDS = "timeInMSec";
        //
        protected Metrics metrics = new Metrics();
        //
        protected int individualLength;
        protected ICollection<A> finiteAlphabet;
        protected double mutationProbability;

        protected IRandom random;
        private ICollection<ProgressTracker> progressTrackers = CollectionFactory.CreateQueue<ProgressTracker>();

        public GeneticAlgorithm(int individualLength, ICollection<A> finiteAlphabet, double mutationProbability)
            : this(individualLength, finiteAlphabet, mutationProbability, CommonFactory.CreateRandom())
        { }

        public GeneticAlgorithm(int individualLength, ICollection<A> finiteAlphabet, double mutationProbability, IRandom random)
        {
            this.individualLength = individualLength;
            this.finiteAlphabet = CollectionFactory.CreateQueue<A>(finiteAlphabet);
            this.mutationProbability = mutationProbability;
            this.random = random;

            if (!(this.mutationProbability >= 0.0 && this.mutationProbability <= 1.0))
            {
                throw new Exception("");
            }
        }

        /** Progress tracers can be used to display progress information. */
        public virtual void addProgressTracer(ProgressTracker pTracer)
        {
            progressTrackers.Add(pTracer);
        }

        /**
         * Starts the genetic algorithm and stops after a specified number of
         * iterations.
         */
        public virtual Individual<A> geneticAlgorithm(ICollection<Individual<A>> initPopulation,
            IFitnessFunction<A> fitnessFn, int maxIterations)
        {
            GoalTest<Individual<A>> goalTest = (state) => getIterations() >= maxIterations;
            return geneticAlgorithm(initPopulation, fitnessFn, goalTest, 0L);
        }

        private bool currIsCancelled;

        public void SetCurrIsCancelled(bool value)
        {
            currIsCancelled = value;
        }

        public bool GetCurrIsCancelled()
        {
            return currIsCancelled;
        }
        /**
         * Template method controlling search. It returns the best individual in the
         * specified population, according to the specified FITNESS-FN and goal
         * test.
         * 
         * @param initPopulation
         *            a set of individuals
         * @param fitnessFn
         *            a function that measures the fitness of an individual
         * @param goalTest
         *            test determines whether a given individual is fit enough to
         *            return. Can be used in subclasses to implement additional
         *            termination criteria, e.g. maximum number of iterations.
         * @param maxTimeMilliseconds
         *            the maximum time in milliseconds that the algorithm is to run
         *            for (approximate). Only used if > 0L.
         * @return the best individual in the specified population, according to the
         *         specified FITNESS-FN and goal test.
         */
        // function GENETIC-ALGORITHM(population, FITNESS-FN) returns an individual
        // inputs: population, a set of individuals
        // FITNESS-FN, a function that measures the fitness of an individual
        public virtual Individual<A> geneticAlgorithm(ICollection<Individual<A>> initPopulation, IFitnessFunction<A> fitnessFn,
                GoalTest<Individual<A>> goalTest, long maxTimeMilliseconds)
        {
            Individual<A> bestIndividual = null;

            // Create a local copy of the population to work with
            ICollection<Individual<A>> population = CollectionFactory.CreateQueue<Individual<A>>(initPopulation);
            // Validate the population and setup the instrumentation
            validatePopulation(population);
            updateMetrics(population, 0, 0L);

            IStopWatch sw = CommonFactory.CreateStopWatch();

            // repeat
            int itCount = 0;
            do
            {
                population = nextGeneration(population, fitnessFn);
                bestIndividual = retrieveBestIndividual(population, fitnessFn);

                updateMetrics(population, ++itCount, sw.GetElapsedMilliseconds());

                // until some individual is fit enough, or enough time has elapsed
                if (maxTimeMilliseconds > 0L && sw.GetElapsedMilliseconds() > maxTimeMilliseconds)
                    break;
                if (currIsCancelled)
                    break;
            } while (!goalTest(bestIndividual));

            notifyProgressTrackers(itCount, population);
            // return the best individual in population, according to FITNESS-FN
            return bestIndividual;
        }

        public virtual Individual<A> retrieveBestIndividual(ICollection<Individual<A>> population, IFitnessFunction<A> fitnessFn)
        {
            Individual<A> bestIndividual = null;
            double bestSoFarFValue = double.NegativeInfinity;

            foreach (Individual<A> individual in population)
            {
                double fValue = fitnessFn.apply(individual);
                if (fValue > bestSoFarFValue)
                {
                    bestIndividual = individual;
                    bestSoFarFValue = fValue;
                }
            }

            return bestIndividual;
        }

        /**
         * Sets the population size and number of iterations to zero.
         */
        public virtual void clearInstrumentation()
        {
            updateMetrics(CollectionFactory.CreateQueue<Individual<A>>(), 0, 0L);
        }

        /**
         * Returns all the metrics of the genetic algorithm.
         * 
         * @return all the metrics of the genetic algorithm.
         */
        public virtual Metrics getMetrics()
        {
            return metrics;
        }

        /**
         * Returns the population size.
         * 
         * @return the population size.
         */
        public virtual int getPopulationSize()
        {
            return metrics.getInt(POPULATION_SIZE);
        }

        /**
         * Returns the number of iterations of the genetic algorithm.
         * 
         * @return the number of iterations of the genetic algorithm.
         */
        public virtual int getIterations()
        {
            return metrics.getInt(ITERATIONS);
        }

        /**
         * 
         * @return the time in milliseconds that the genetic algorithm took.
         */
        public virtual long getTimeInMilliseconds()
        {
            return metrics.getLong(TIME_IN_MILLISECONDS);
        }

        /**
         * Updates statistic data collected during search.
         * 
         * @param itCount
         *            the number of iterations.
         * @param time
         *            the time in milliseconds that the genetic algorithm took.
         */
        protected virtual void updateMetrics(ICollection<Individual<A>> population, int itCount, long time)
        {
            metrics.set(POPULATION_SIZE, population.Size());
            metrics.set(ITERATIONS, itCount);
            metrics.set(TIME_IN_MILLISECONDS, time);
        }

        //
        // PROTECTED METHODS
        //
        // Note: Override these protected methods to create your own desired
        // behavior.
        //
        /**
         * Primitive operation which is responsible for creating the next
         * generation. Override to get progress information!
         */
        protected virtual ICollection<Individual<A>> nextGeneration(ICollection<Individual<A>> population, IFitnessFunction<A> fitnessFn)
        {
            // new_population <- empty set
            ICollection<Individual<A>> newPopulation = CollectionFactory.CreateQueue<Individual<A>>();
            // for i = 1 to SIZE(population) do
            for (int i = 0; i < population.Size(); ++i)
            {
                // x <- RANDOM-SELECTION(population, FITNESS-FN)
                Individual<A> x = randomSelection(population, fitnessFn);
                // y <- RANDOM-SELECTION(population, FITNESS-FN)
                Individual<A> y = randomSelection(population, fitnessFn);
                // child <- REPRODUCE(x, y)
                Individual<A> child = reproduce(x, y);
                // if (small random probability) then child <- MUTATE(child)
                if (random.NextDouble() <= mutationProbability)
                {
                    child = mutate(child);
                }
                // add child to new_population
                newPopulation.Add(child);
            }
            notifyProgressTrackers(getIterations(), population);
            return newPopulation;
        }

        // RANDOM-SELECTION(population, FITNESS-FN)
        protected virtual Individual<A> randomSelection(ICollection<Individual<A>> population, IFitnessFunction<A> fitnessFn)
        {
            // Default result is last individual
            // (just to avoid problems with rounding errors)
            Individual<A> selected = population.Get(population.Size() - 1);

            // Determine all of the fitness values
            double[] fValues = new double[population.Size()];
            for (int i = 0; i < population.Size(); ++i)
            {
                fValues[i] = fitnessFn.apply(population.Get(i));
            }
            // Normalize the fitness values
            fValues = Util.normalize(fValues);
            double prob = random.NextDouble();
            double totalSoFar = 0.0;
            for (int i = 0; i < fValues.Length; ++i)
            {
                // Are at last element so assign by default
                // in case there are rounding issues with the normalized values
                totalSoFar += fValues[i];
                if (prob <= totalSoFar)
                {
                    selected = population.Get(i);
                    break;
                }
            }

            selected.incDescendants();
            return selected;
        }

        // function REPRODUCE(x, y) returns an individual
        // inputs: x, y, parent individuals
        protected virtual Individual<A> reproduce(Individual<A> x, Individual<A> y)
        {
            // n <- LENGTH(x);
            // Note: this is = this.individualLength
            // c <- random number from 1 to n
            int c = randomOffset(individualLength);
            // return APPEND(SUBSTRING(x, 1, c), SUBSTRING(y, c+1, n))
            ICollection<A> childRepresentation = CollectionFactory.CreateQueue<A>();
            childRepresentation.AddAll(x.getRepresentation().subList(0, c));
            childRepresentation.AddAll(y.getRepresentation().subList(c, individualLength));

            return new Individual<A>(childRepresentation);
        }

        protected virtual Individual<A> mutate(Individual<A> child)
        {
            int mutateOffset = randomOffset(individualLength);
            int alphaOffset = randomOffset(finiteAlphabet.Size());

            ICollection<A> mutatedRepresentation = CollectionFactory.CreateQueue<A>(child.getRepresentation());

            mutatedRepresentation.Set(mutateOffset, finiteAlphabet.Get(alphaOffset));

            return new Individual<A>(mutatedRepresentation);
        }

        protected virtual int randomOffset(int length)
        {
            return random.Next(length);
        }

        protected virtual void validatePopulation(ICollection<Individual<A>> population)
        {
            // Require at least 1 individual in population in order
            // for algorithm to work
            if (population.Size() < 1)
            {
                throw new IllegalArgumentException("Must start with at least a population of size 1");
            }
            // string lengths are assumed to be of fixed size,
            // therefore ensure initial populations lengths correspond to this
            foreach (Individual<A> individual in population)
            {
                if (individual.length() != this.individualLength)
                {
                    throw new IllegalArgumentException("Individual [" + individual
                            + "] in population is not the required length of " + this.individualLength);
                }
            }
        }

        private void notifyProgressTrackers(int itCount, ICollection<Individual<A>> generation)
        {
            foreach (ProgressTracker tracer in progressTrackers)
                tracer.trackProgress(getIterations(), generation);
        }

        /**
         * Interface for progress tracers.
         * 
         * @author Ruediger Lunde
         */
        public interface ProgressTracker
        {
            void trackProgress(int itCount, ICollection<Individual<A>> population);
        }
    }
}
