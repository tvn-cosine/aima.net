﻿namespace aima.net.search.local.api
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 127.<br>
     * <br>
     * Each state is rated by the objective function, or (in Genetic Algorithm
     * terminology) the fitness function. A fitness function should return higher
     * values for better states.
     * <br>
     * Here, we assume that all values are greater or equal to zero.
     * 
     * @author Ciaran O'Reilly
     * 
     * @param <A>
     *            the type of the alphabet used in the representation of the
     *            individuals in the population (this is to provide flexibility in
     *            terms of how a problem can be encoded).
     */
    public interface IFitnessFunction<A>
    {

        /**
         * 
         * @param individual
         *            the individual whose fitness is to be accessed.
         * @return the individual's fitness value (the higher the better).
         */
        double apply(Individual<A> individual);
    }

}
