using aima.net.api;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.exceptions;
using aima.net.text;
using aima.net.text.api;
using aima.net.probability.api;
using aima.net.probability.bayes.api;
using aima.net.probability.domain.api;
using aima.net.probability.proposition;
using aima.net.probability.proposition.api;
using aima.net.util;
using aima.net.util.math;

namespace aima.net.probability.util
{
    public class ProbUtil
    {
        private static readonly IRegularExpression LEGAL_RAND_VAR_NAME_PATTERN = TextFactory.CreateRegularExpression("[A-Za-z0-9-_]+");
        private static readonly IRegularExpression LEGAL_RAND_VAR_NAME_PATTERN_TABS = TextFactory.CreateRegularExpression("^[^\t\n]+$");
        private static readonly IRegularExpression LEGAL_LEADING_CHAR_RAND_VAR_NAME_PATTERN = TextFactory.CreateRegularExpression("^[A-Z].*");

        /**
         * Check if name provided is valid for use as the name of a RandomVariable.
         * 
         * @param name
         *            proposed for the RandomVariable.
         * @throws IllegalArgumentException
         *             if not a valid RandomVariable name.
         */
        public static void checkValidRandomVariableName(string name)
        {
            if (null == name)
            {
                throw new IllegalArgumentException("Name cannot be null.");
            }
            if (!LEGAL_RAND_VAR_NAME_PATTERN_TABS.IsMatch(name)
             || !LEGAL_RAND_VAR_NAME_PATTERN.IsMatch(name))
            {
                throw new IllegalArgumentException("Name of RandomVariable must be specified and contain no leading, trailing or embedded spaces or special characters.");
            }
            if (!LEGAL_LEADING_CHAR_RAND_VAR_NAME_PATTERN.IsMatch(name))
            {
                throw new IllegalArgumentException("Name must start with a leading upper case letter.");
            }
        }

        /**
         * Calculated the expected size of a ProbabilityTable for the provided
         * random variables.
         * 
         * @param vars
         *            null, 0 or more random variables that are to be used to
         *            construct a CategoricalDistribution.
         * @return the size (i.e. getValues().Length) that the
         *         CategoricalDistribution will need to be in order to represent the
         *         specified random variables.
         * 
         * @see CategoricalDistribution#getValues()
         */
        public static int expectedSizeOfProbabilityTable(params IRandomVariable[] vars)
        {
            // initially 1, as this will represent constant assignments
            // e.g. Dice1 = 1.
            int expectedSizeOfDistribution = 1;
            if (null != vars)
            {
                foreach (IRandomVariable rv in vars)
                {
                    // Create ordered domains for each variable
                    if (!(rv.getDomain() is IFiniteDomain))
                    {
                        throw new IllegalArgumentException("Cannot have an infinite domain for a variable in this calculation:"
                                        + rv);
                    }
                    IFiniteDomain d = (IFiniteDomain)rv.getDomain();
                    expectedSizeOfDistribution *= d.Size();
                }
            }

            return expectedSizeOfDistribution;
        }

        /**
         * Calculated the expected size of a CategoricalDistribution for the
         * provided random variables.
         * 
         * @param vars
         *            null, 0 or more random variables that are to be used to
         *            construct a CategoricalDistribution.
         * @return the size (i.e. getValues().Length) that the
         *         CategoricalDistribution will need to be in order to represent the
         *         specified random variables.
         * 
         * @see CategoricalDistribution#getValues()
         */
        public static int expectedSizeOfCategoricalDistribution(params IRandomVariable[] vars)
        {
            // Equivalent calculation
            return expectedSizeOfProbabilityTable(vars);
        }

        /**
         * Convenience method for ensure a conjunction of probabilistic
         * propositions.
         * 
         * @param props
         *            propositions to be combined into a ConjunctiveProposition if
         *            necessary.
         * @return a ConjunctivePropositions if more than 1 proposition in 'props',
         *         otherwise props[0].
         */
        public static IProposition constructConjunction(IProposition[] props)
        {
            return constructConjunction(props, 0);
        }

        /**
         * 
         * @param probabilityChoice
         *            a probability choice for the sample
         * @param Xi
         *            a Random Variable with a finite domain from which a random
         *            sample is to be chosen based on the probability choice.
         * @param distribution
         *            Xi's distribution.
         * @return a Random Sample from Xi's domain.
         */
        public static object sample(double probabilityChoice, IRandomVariable Xi, double[] distribution)
        {
            IFiniteDomain fd = (IFiniteDomain)Xi.getDomain();
            if (fd.Size() != distribution.Length)
            {
                throw new IllegalArgumentException("Size of domain Xi " + fd.Size()
                        + " is not equal to the size of the distribution "
                        + distribution.Length);
            }
            int i = 0;
            double total = distribution[0];
            while (probabilityChoice > total)
            {
                ++i;
                total += distribution[i];
            }
            return fd.GetValueAt(i);
        }

        /**
         * Get a random sample from <b>P</b>(X<sub>i</sub> | parents(X<sub>i</sub>))
         * 
         * @param Xi
         *            a Node from a Bayesian network for the Random Variable
         *            X<sub>i</sub>.
         * @param event
         *            comprising assignments for parents(X<sub>i</sub>)
         * @param r
         *            a IRandom for generating a probability choice for the
         *            sample.
         * @return a random sample from <b>P</b>(X<sub>i</sub> |
         *         parents(X<sub>i</sub>))
         */
        public static object randomSample(INode Xi, IMap<IRandomVariable, object> even, IRandom r)
        {
            return Xi.GetCPD().GetSample(r.NextDouble(), getEventValuesForParents(Xi, even));
        }

        /**
         * Get a random sample from <b>P</b>(X<sub>i</sub> | mb(X<sub>i</sub>)),
         * where mb(X<sub>i</sub>) is the Markov Blanket of X<sub>i</sub>. The
         * probability of a variable given its Markov blanket is proportional to the
         * probability of the variable given its parents times the probability of
         * each child given its respective parents (see equation 14.12 pg. 538
         * AIMA3e):<br>
         * <br>
         * P(x'<sub>i</sub>|mb(Xi)) =
         * &alpha;P(x'<sub>i</sub>|parents(X<sub>i</sub>)) *
         * &prod;<sub>Y<sub>j</sub> &isin; Children(X<sub>i</sub>)</sub>
         * P(y<sub>j</sub>|parents(Y<sub>j</sub>))
         * 
         * @param Xi
         *            a Node from a Bayesian network for the Random Variable
         *            X<sub>i</sub>.
         * @param event
         *            comprising assignments for the Markov Blanket X<sub>i</sub>.
         * @param r
         *            a IRandom for generating a probability choice for the
         *            sample.
         * @return a random sample from <b>P</b>(X<sub>i</sub> | mb(X<sub>i</sub>))
         */
        public static object mbRandomSample(INode Xi, IMap<IRandomVariable, object> even, IRandom r)
        {
            return sample(r.NextDouble(), Xi.GetRandomVariable(),
                    mbDistribution(Xi, even));
        }

        /**
         * Calculate the probability distribution for <b>P</b>(X<sub>i</sub> |
         * mb(X<sub>i</sub>)), where mb(X<sub>i</sub>) is the Markov Blanket of
         * X<sub>i</sub>. The probability of a variable given its Markov blanket is
         * proportional to the probability of the variable given its parents times
         * the probability of each child given its respective parents (see equation
         * 14.12 pg. 538 AIMA3e):<br>
         * <br>
         * P(x'<sub>i</sub>|mb(Xi)) =
         * &alpha;P(x'<sub>i</sub>|parents(X<sub>i</sub>)) *
         * &prod;<sub>Y<sub>j</sub> &isin; Children(X<sub>i</sub>)</sub>
         * P(y<sub>j</sub>|parents(Y<sub>j</sub>))
         * 
         * @param Xi
         *            a Node from a Bayesian network for the Random Variable
         *            X<sub>i</sub>.
         * @param event
         *            comprising assignments for the Markov Blanket X<sub>i</sub>.
         * @return a random sample from <b>P</b>(X<sub>i</sub> | mb(X<sub>i</sub>))
         */
        public static double[] mbDistribution(INode Xi, IMap<IRandomVariable, object> even)
        {
            IFiniteDomain fd = (IFiniteDomain)Xi.GetRandomVariable().getDomain();
            double[] X = new double[fd.Size()];

            /**
             * As we iterate over the domain of a ramdom variable corresponding to Xi
             * it is necessary to make the modified values of the variable visible
             * to the child nodes of Xi in the computation of the markov blanket
             * probabilities. 
             */
            //Copy contents of event to generatedEvent so as to leave event untouched
            IMap<IRandomVariable, object> generatedEvent = CollectionFactory.CreateInsertionOrderedMap<IRandomVariable, object>();
            foreach (var entry in even)
            {
                generatedEvent.Put(entry.GetKey(), entry.GetValue());
            }

            for (int i = 0; i < fd.Size(); ++i)
            {
                /** P(x'<sub>i</sub>|mb(Xi)) =
                 * &alpha;P(x'<sub>i</sub>|parents(X<sub>i</sub>)) *
                 * &prod;<sub>Y<sub>j</sub> &isin; Children(X<sub>i</sub>)</sub>
                 * P(y<sub>j</sub>|parents(Y<sub>j</sub>))
                 */
                generatedEvent.Put(Xi.GetRandomVariable(), fd.GetValueAt(i));
                double cprob = 1.0;
                foreach (INode Yj in Xi.GetChildren())
                {
                    cprob *= Yj.GetCPD().GetValue(
                            getEventValuesForXiGivenParents(Yj, generatedEvent));
                }
                X[i] = Xi.GetCPD()
                                .GetValue(
                                        getEventValuesForXiGivenParents(Xi,
                                                fd.GetValueAt(i), even))
                    * cprob;
            }

            return Util.normalize(X);
        }

        /**
         * Get the parent values for the Random Variable Xi from the provided event.
         * 
         * @param Xi
         *            a Node for the Random Variable Xi whose parent values are to
         *            be extracted from the provided event in the correct order.
         * @param event
         *            an event containing assignments for Xi's parents.
         * @return an ordered set of values for the parents of Xi from the provided
         *         event.
         */
        public static object[] getEventValuesForParents(INode Xi, IMap<IRandomVariable, object> even)
        {
            object[] parentValues = new object[Xi.GetParents().Size()];
            int i = 0;
            foreach (INode pn in Xi.GetParents())
            {
                parentValues[i] = even.Get(pn.GetRandomVariable());
                ++i;
            }
            return parentValues;
        }

        /**
         * Get the values for the Random Variable Xi's parents and its own value
         * from the provided event.
         * 
         * @param Xi
         *            a Node for the Random Variable Xi whose parent values and
         *            value are to be extracted from the provided event in the
         *            correct order.
         * @param event
         *            an event containing assignments for Xi's parents and its own
         *            value.
         * @return an ordered set of values for the parents of Xi and its value from
         *         the provided event.
         */
        public static object[] getEventValuesForXiGivenParents(INode Xi, IMap<IRandomVariable, object> even)
        {
            return getEventValuesForXiGivenParents(Xi,
                        even.Get(Xi.GetRandomVariable()), even);
        }

        /**
         * Get the values for the Random Variable Xi's parents and its own value
         * from the provided event.
         * 
         * @param Xi
         *            a Node for the Random Variable Xi whose parent values are to
         *            be extracted from the provided event in the correct order.
         * @param xDelta
         *            the value for the Random Variable Xi to be assigned to the
         *            values returned.
         * @param event
         *            an event containing assignments for Xi's parents and its own
         *            value.
         * @return an ordered set of values for the parents of Xi and its value from
         *         the provided event.
         */
        public static object[] getEventValuesForXiGivenParents(INode Xi, object xDelta, IMap<IRandomVariable, object> even)
        {
            object[] values = new object[Xi.GetParents().Size() + 1];

            int idx = 0;
            foreach (INode pn in Xi.GetParents())
            {
                values[idx] = even.Get(pn.GetRandomVariable());
                idx++;
            }
            values[idx] = xDelta;
            return values;
        }

        /**
         * Calculate the index into a vector representing the enumeration of the
         * value assignments for the variables X and their corresponding assignment
         * in x. For example the Random Variables:<br>
         * Q::{true, false}, R::{'A', 'B','C'}, and T::{true, false}, would be
         * enumerated in a Vector as follows:
         * 
         * <pre>
         * Index  Q      R  T
         * -----  -      -  -
         * 00:    true,  A, true
         * 01:    true,  A, false
         * 02:    true,  B, true
         * 03:    true,  B, false
         * 04:    true,  C, true
         * 05:    true,  C, false
         * 06:    false, A, true
         * 07:    false, A, false
         * 08:    false, B, true
         * 09:    false, B, false
         * 10:    false, C, true
         * 11:    false, C, false
         * </pre>
         * 
         * if x = {Q=true, R='C', T=false} the index returned would be 5.
         * 
         * @param X
         *            a list of the Random Variables that would comprise the vector.
         * @param x
         *            an assignment for the Random Variables in X.
         * @return an index into a vector that would represent the enumeration of
         *         the values for X.
         */
        public static int indexOf(IRandomVariable[] X, IMap<IRandomVariable, object> x)
        {
            if (0 == X.Length)
            {
                return ((IFiniteDomain)X[0].getDomain()).GetOffset(x.Get(X[0]));
            }
            // X.Length > 1 then calculate using a mixed radix number
            //
            // Note: Create radices in reverse order so that the enumeration
            // through the distributions is of the following
            // order using a MixedRadixNumber, e.g. for two Booleans:
            // X Y
            // true true
            // true false
            // false true
            // false false
            // which corresponds with how displayed in book.
            int[] radixValues = new int[X.Length];
            int[] radices = new int[X.Length];
            int j = X.Length - 1;
            for (int i = 0; i < X.Length; ++i)
            {
                IFiniteDomain fd = (IFiniteDomain)X[i].getDomain();
                radixValues[j] = fd.GetOffset(x.Get(X[i]));
                radices[j] = fd.Size();
                j--;
            }

            return new MixedRadixNumber(radixValues, radices).IntValue();
        }

        /**
         * Calculate the indexes for X[i] into a vector representing the enumeration
         * of the value assignments for the variables X and their corresponding
         * assignment in x. For example the Random Variables:<br>
         * Q::{true, false}, R::{'A', 'B','C'}, and T::{true, false}, would be
         * enumerated in a Vector as follows:
         * 
         * <pre>
         * Index  Q      R  T
         * -----  -      -  -
         * 00:    true,  A, true
         * 01:    true,  A, false
         * 02:    true,  B, true
         * 03:    true,  B, false
         * 04:    true,  C, true
         * 05:    true,  C, false
         * 06:    false, A, true
         * 07:    false, A, false
         * 08:    false, B, true
         * 09:    false, B, false
         * 10:    false, C, true
         * 11:    false, C, false
         * </pre>
         * 
         * if X[i] = R and x = {..., R='C', ...} then the indexes returned would be
         * [4, 5, 10, 11].
         * 
         * @param X
         *            a list of the Random Variables that would comprise the vector.
         * @param idx
         *            the index into X for the Random Variable whose assignment we
         *            wish to retrieve its indexes for.
         * @param x
         *            an assignment for the Random Variables in X.
         * @return the indexes into a vector that would represent the enumeration of
         *         the values for X[i] in x.
         */
        public static int[] indexesOfValue(IRandomVariable[] X, int idx, IMap<IRandomVariable, object> x)
        {
            int csize = ProbUtil.expectedSizeOfCategoricalDistribution(X);

            IFiniteDomain fd = (IFiniteDomain)X[idx].getDomain();
            int vdoffset = fd.GetOffset(x.Get(X[idx]));
            int vdosize = fd.Size();
            int[] indexes = new int[csize / vdosize];

            int blocksize = csize;
            for (int i = 0; i < X.Length; ++i)
            {
                blocksize = blocksize / X[i].getDomain().Size();
                if (i == idx)
                {
                    break;
                }
            }

            for (int i = 0; i < indexes.Length; i += blocksize)
            {
                int offset = ((i / blocksize) * vdosize * blocksize)
                        + (blocksize * vdoffset);
                for (int b = 0; b < blocksize; b++)
                {
                    indexes[i + b] = offset + b;
                }
            }

            return indexes;
        }
         
        private static IProposition constructConjunction(IProposition[] props, int idx)
        {
            if ((idx + 1) == props.Length)
            {
                return props[idx];
            }

            return new ConjunctiveProposition(props[idx], constructConjunction(props, idx + 1));
        }
    } 
}
