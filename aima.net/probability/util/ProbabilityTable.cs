using aima.net.collections;
using aima.net.collections.api;
using aima.net.exceptions;
using aima.net.text;
using aima.net.text.api;
using aima.net.probability.api;
using aima.net.probability.domain.api;
using aima.net.probability.proposition;
using aima.net.util;
using aima.net.util.math;

namespace aima.net.probability.util
{
    /// <summary>
    /// A Utility Class for associating values with a set of finite Random Variables.
    /// This is also the default implementation of the CategoricalDistribution and
    /// Factor interfaces (as they are essentially dependent on the same underlying
    /// data structures).
    /// </summary>
    public class ProbabilityTable : ICategoricalDistribution, IFactor
    {
        private double[] values = null;
        //
        private IMap<IRandomVariable, RVInfo> randomVarInfo = CollectionFactory.CreateInsertionOrderedMap<IRandomVariable, RVInfo>();
        private int[] radices = null;
        private MixedRadixNumber queryMRN = null;
        //
        private string toString = null;
        private double sum = -1;

        /// <summary>
        /// Interface to be implemented by an object/algorithm that wishes to iterate
        /// over the possible assignments for the random variables comprising this
        /// table.
        /// </summary>
        public interface ProbabilityTableIterator : IIterator
        {
            /**
             * Called for each possible assignment for the Random Variables
             * comprising this ProbabilityTable.
             * 
             * @param possibleAssignment
             *            a possible assignment, &omega;, of variable/value pairs.
             * @param probability
             *            the probability associated with &omega;
             */
        }

        public ProbabilityTable(ICollection<IRandomVariable> vars)
            : this(vars.ToArray())
        { }

        public ProbabilityTable(params IRandomVariable[] vars)
            : this(new double[ProbUtil.expectedSizeOfProbabilityTable(vars)], vars)
        { }

        public ProbabilityTable(double[] vals, params IRandomVariable[] vars)
        {
            if (null == vals)
            {
                throw new IllegalArgumentException("Values must be specified");
            }
            if (vals.Length != ProbUtil.expectedSizeOfProbabilityTable(vars))
            {
                throw new IllegalArgumentException("ProbabilityTable of length "
                        + vals.Length + " is not the correct size, should be "
                        + ProbUtil.expectedSizeOfProbabilityTable(vars)
                        + " in order to represent all possible combinations.");
            }
            if (null != vars)
            {
                foreach (IRandomVariable rv in vars)
                {
                    // Track index information relevant to each variable.
                    randomVarInfo.Put(rv, new RVInfo(rv));
                }
            }

            values = new double[vals.Length];
            System.Array.Copy(vals, 0, values, 0, vals.Length);

            radices = createRadixs(randomVarInfo);

            if (radices.Length > 0)
            {
                queryMRN = new MixedRadixNumber(0, radices);
            }
        }

        public int size()
        {
            return values.Length;
        }

        //
        // START-ProbabilityDistribution

        public ISet<IRandomVariable> getFor()
        {
            return CollectionFactory.CreateReadOnlySet<IRandomVariable>(randomVarInfo.GetKeys());
        }


        public bool contains(IRandomVariable rv)
        {
            return randomVarInfo.GetKeys().Contains(rv);
        }


        public double getValue(params object[] assignments)
        {
            return values[getIndex(assignments)];
        }


        public double getValue(params AssignmentProposition[] assignments)
        {
            if (assignments.Length != randomVarInfo.Size())
            {
                throw new IllegalArgumentException(
                        "Assignments passed in is not the same size as variables making up probability table.");
            }
            int[] radixValues = new int[assignments.Length];
            foreach (AssignmentProposition ap in assignments)
            {
                RVInfo rvInfo = randomVarInfo.Get(ap.getTermVariable());
                if (null == rvInfo)
                {
                    throw new IllegalArgumentException(
                            "Assignment passed for a variable that is not part of this probability table:"
                                    + ap.getTermVariable());
                }
                radixValues[rvInfo.getRadixIdx()] = rvInfo.getIdxForDomain(ap
                        .getValue());
            }
            return values[(int)queryMRN.GetCurrentValueFor(radixValues)];
        }

        // END-ProbabilityDistribution
        //

        //
        // START-CategoricalDistribution
        public double[] getValues()
        {
            return values;
        }


        public void setValue(int idx, double value)
        {
            values[idx] = value;
            reinitLazyValues();
        }


        public double getSum()
        {
            if (-1 == sum)
            {
                sum = 0;
                for (int i = 0; i < values.Length; ++i)
                {
                    sum += values[i];
                }
            }
            return sum;
        }

        ICategoricalDistribution ICategoricalDistribution.normalize()
        {
            return normalize();
        }

        public ProbabilityTable normalize()
        {
            double s = getSum();
            if (s != 0 && s != 1.0)
            {
                for (int i = 0; i < values.Length; ++i)
                {
                    values[i] = values[i] / s;
                }
                reinitLazyValues();
            }
            return this;
        }


        public int getIndex(params object[] assignments)
        {
            if (assignments.Length != randomVarInfo.Size())
            {
                throw new IllegalArgumentException(
                        "Assignments passed in is not the same size as variables making up the table.");
            }
            int[] radixValues = new int[assignments.Length];
            int i = 0;
            foreach (RVInfo rvInfo in randomVarInfo.GetValues())
            {
                radixValues[rvInfo.getRadixIdx()] = rvInfo
                        .getIdxForDomain(assignments[i]);
                ++i;
            }

            return (int)queryMRN.GetCurrentValueFor(radixValues);
        }

        IFactor IFactor.sumOut(params IRandomVariable[] vars)
        {
            return sumOut(vars);
        }

        public ICategoricalDistribution marginal(params IRandomVariable[] vars)
        {
            return sumOut(vars);
        }


        public ICategoricalDistribution divideBy(ICategoricalDistribution divisor)
        {
            return divideBy((ProbabilityTable)divisor);
        }


        public ICategoricalDistribution multiplyBy(ICategoricalDistribution multiplier)
        {
            return pointwiseProduct((ProbabilityTable)multiplier);
        }


        public ICategoricalDistribution multiplyByPOS(ICategoricalDistribution multiplier, params IRandomVariable[] prodVarOrder)
        {
            return pointwiseProductPOS((ProbabilityTable)multiplier, prodVarOrder);
        }

        void IFactor.iterateOver(FactorIterator fi)
        {
            iterateOverTable(new FactorIteratorAdapter(fi));
        }


        void IFactor.iterateOver(FactorIterator fi, params AssignmentProposition[] fixedValues)
        {
            iterateOverTable(new FactorIteratorAdapter(fi), fixedValues);
        }


        public void iterateOver(FactorIterator fi)
        {
            iterateOverTable(new FactorIteratorAdapter(fi));
        }


        public void iterateOver(FactorIterator fi, params AssignmentProposition[] fixedValues)
        {
            iterateOverTable(new FactorIteratorAdapter(fi), fixedValues);
        }

        public void iterateOver(CategoricalDistributionIterator cdi)
        {
            iterateOverTable(new CategoricalDistributionIteratorAdapter(cdi));
        }


        public void iterateOver(CategoricalDistributionIterator cdi, params AssignmentProposition[] fixedValues)
        {
            iterateOverTable(new CategoricalDistributionIteratorAdapter(cdi), fixedValues);
        }

        // END-CategoricalDistribution
        //

        //
        // START-Factor

        public ISet<IRandomVariable> getArgumentVariables()
        {
            return CollectionFactory.CreateReadOnlySet<IRandomVariable>(randomVarInfo.GetKeys());
        }

        class ProbabilityTableIteratorImpl : ProbabilityTableIterator
        {
            private readonly ProbabilityTable summedOut;
            private readonly object[] termValues;

            public ProbabilityTableIteratorImpl(ProbabilityTable summedOut, object[] termValues)
            {
                this.summedOut = summedOut;
                this.termValues = termValues;
            }

            public void iterate(IMap<IRandomVariable, object> possibleWorld, double probability)
            {

                int i = 0;
                foreach (IRandomVariable rv in summedOut.randomVarInfo.GetKeys())
                {
                    termValues[i] = possibleWorld.Get(rv);
                    ++i;
                }
                summedOut.getValues()[summedOut.getIndex(termValues)] += probability;
            }
        }

        public ProbabilityTable sumOut(params IRandomVariable[] vars)
        {
            ISet<IRandomVariable> soutVars = CollectionFactory.CreateSet<IRandomVariable>(this.randomVarInfo.GetKeys());
            foreach (IRandomVariable rv in vars)
            {
                soutVars.Remove(rv);
            }
            ProbabilityTable summedOut = new ProbabilityTable(soutVars);
            if (1 == summedOut.getValues().Length)
            {
                summedOut.getValues()[0] = getSum();
            }
            else
            {
                // Otherwise need to iterate through this distribution
                // to calculate the summed out distribution.
                object[] termValues = new object[summedOut.randomVarInfo.Size()];
                ProbabilityTableIterator di = new ProbabilityTableIteratorImpl(summedOut, termValues);

                iterateOverTable(di);
            }

            return summedOut;
        }


        public IFactor pointwiseProduct(IFactor multiplier)
        {
            return pointwiseProduct((ProbabilityTable)multiplier);
        }


        public IFactor pointwiseProductPOS(IFactor multiplier,
                params IRandomVariable[] prodVarOrder)
        {
            return pointwiseProductPOS((ProbabilityTable)multiplier, prodVarOrder);
        }


        // END-Factor
        //

        /**
         * Iterate over all the possible value assignments for the Random Variables
         * comprising this ProbabilityTable.
         * 
         * @param pti
         *            the ProbabilityTable Iterator to iterate.
         */
        public void iterateOverTable(IIterator pti)
        {
            IMap<IRandomVariable, object> possibleWorld = CollectionFactory.CreateInsertionOrderedMap<IRandomVariable, object>();
            MixedRadixNumber mrn = new MixedRadixNumber(0, radices);
            do
            {
                foreach (RVInfo rvInfo in randomVarInfo.GetValues())
                {
                    possibleWorld.Put(rvInfo.getVariable(), rvInfo
                            .getDomainValueAt(mrn.GetCurrentNumeralValue(rvInfo
                                    .getRadixIdx())));
                }
                pti.iterate(possibleWorld, values[mrn.IntValue()]);

            } while (mrn.Increment());
        }

        /**
         * Iterate over all possible values assignments for the Random Variables
         * comprising this ProbabilityTable that are not in the fixed set of values.
         * This allows you to iterate over a subset of possible combinations.
         * 
         * @param pti
         *            the ProbabilityTable Iterator to iterate
         * @param fixedValues
         *            Fixed values for a subset of the Random Variables comprising
         *            this Probability Table.
         */
        public void iterateOverTable(IIterator pti, params AssignmentProposition[] fixedValues)
        {
            IMap<IRandomVariable, object> possibleWorld = CollectionFactory.CreateInsertionOrderedMap<IRandomVariable, object>();
            MixedRadixNumber tableMRN = new MixedRadixNumber(0, radices);
            int[] tableRadixValues = new int[radices.Length];

            // Assert that the Random Variables for the fixed values
            // are part of this probability table and assign
            // all the fixed values to the possible world.
            foreach (AssignmentProposition ap in fixedValues)
            {
                if (!randomVarInfo.ContainsKey(ap.getTermVariable()))
                {
                    throw new IllegalArgumentException("Assignment proposition ["
                            + ap + "] does not belong to this probability table.");
                }
                possibleWorld.Put(ap.getTermVariable(), ap.getValue());
                RVInfo fixedRVI = randomVarInfo.Get(ap.getTermVariable());
                tableRadixValues[fixedRVI.getRadixIdx()] = fixedRVI
                        .getIdxForDomain(ap.getValue());
            }
            // If have assignments for all the random variables
            // in this probability table
            if (fixedValues.Length == randomVarInfo.Size())
            {
                // Then only 1 iteration call is required.
                pti.iterate(possibleWorld, getValue(fixedValues));
            }
            else
            {
                // Else iterate over the non-fixed values
                ISet<IRandomVariable> freeVariables = SetOps.difference(
                    CollectionFactory.CreateSet<IRandomVariable>(this.randomVarInfo.GetKeys()),
                    CollectionFactory.CreateSet<IRandomVariable>(possibleWorld.GetKeys()));
                IMap<IRandomVariable, RVInfo> freeVarInfo = CollectionFactory.CreateInsertionOrderedMap<IRandomVariable, RVInfo>();
                // Remove the fixed Variables
                foreach (IRandomVariable fv in freeVariables)
                {
                    freeVarInfo.Put(fv, new RVInfo(fv));
                }
                int[] freeRadixValues = createRadixs(freeVarInfo);
                MixedRadixNumber freeMRN = new MixedRadixNumber(0, freeRadixValues);
                object fval = null;
                // Iterate through all combinations of the free variables
                do
                {
                    // Put the current assignments for the free variables
                    // into the possible world and update
                    // the current index in the table MRN
                    foreach (RVInfo freeRVI in freeVarInfo.GetValues())
                    {
                        fval = freeRVI.getDomainValueAt(freeMRN
                                .GetCurrentNumeralValue(freeRVI.getRadixIdx()));
                        possibleWorld.Put(freeRVI.getVariable(), fval);

                        tableRadixValues[randomVarInfo.Get(freeRVI.getVariable())
                                .getRadixIdx()] = freeRVI.getIdxForDomain(fval);
                    }
                    pti.iterate(possibleWorld, values[(int)tableMRN
                            .GetCurrentValueFor(tableRadixValues)]);

                } while (freeMRN.Increment());
            }
        }

        class ProbabilityTableIteratorImp2 : ProbabilityTableIterator
        {
            private IMap<IRandomVariable, RVInfo> diff;
            private MixedRadixNumber dMRN;
            private ProbabilityTable probabilityTable;
            private MixedRadixNumber qMRN;
            private int[] qRVs;
            private ProbabilityTable quotient;

            public ProbabilityTableIteratorImp2(ProbabilityTable quotient, int[] qRVs, MixedRadixNumber qMRN, MixedRadixNumber dMRN, IMap<IRandomVariable, RVInfo> diff, ProbabilityTable probabilityTable)
            {
                this.quotient = quotient;
                this.qRVs = qRVs;
                this.qMRN = qMRN;
                this.dMRN = dMRN;
                this.diff = diff;
                this.probabilityTable = probabilityTable;
            }

            public void iterate(IMap<IRandomVariable, object> possibleWorld, double probability)
            {
                foreach (IRandomVariable rv in possibleWorld.GetKeys())
                {
                    RVInfo rvInfo = quotient.randomVarInfo.Get(rv);
                    qRVs[rvInfo.getRadixIdx()] = rvInfo
                            .getIdxForDomain(possibleWorld.Get(rv));
                }
                if (null != diff)
                {
                    // Start from 0 off the diff
                    dMRN.SetCurrentValueFor(new int[diff.Size()]);
                    do
                    {
                        foreach (IRandomVariable rv in diff.GetKeys())
                        {
                            RVInfo drvInfo = diff.Get(rv);
                            RVInfo qrvInfo = quotient.randomVarInfo.Get(rv);
                            qRVs[qrvInfo.getRadixIdx()] = dMRN
                                    .GetCurrentNumeralValue(drvInfo
                                            .getRadixIdx());
                        }
                        updateQuotient(probability);
                    } while (dMRN.Increment());
                }
                else
                {
                    updateQuotient(probability);
                }
            }

            //
            //
            private void updateQuotient(double probability)
            {
                int offset = (int)qMRN.GetCurrentValueFor(qRVs);
                if (0 == probability)
                {
                    quotient.getValues()[offset] = 0;
                }
                else
                {
                    quotient.getValues()[offset] += probabilityTable.getValues()[offset] / probability;
                }
            }
        }

        public ProbabilityTable divideBy(ProbabilityTable divisor)
        {
            if (!randomVarInfo.GetKeys().ContainsAll(divisor.randomVarInfo.GetKeys()))
            {
                throw new IllegalArgumentException("Divisor must be a subset of the dividend.");
            }

            ProbabilityTable quotient = new ProbabilityTable(randomVarInfo.GetKeys());

            if (1 == divisor.getValues().Length)
            {
                double d = divisor.getValues()[0];
                for (int i = 0; i < quotient.getValues().Length; ++i)
                {
                    if (0 == d)
                    {
                        quotient.getValues()[i] = 0;
                    }
                    else
                    {
                        quotient.getValues()[i] = getValues()[i] / d;
                    }
                }
            }
            else
            {
                ISet<IRandomVariable> dividendDivisorDiff = SetOps
                        .difference(
                    CollectionFactory.CreateSet<IRandomVariable>(this.randomVarInfo.GetKeys()),
                    CollectionFactory.CreateSet<IRandomVariable>(divisor.randomVarInfo.GetKeys()));
                IMap<IRandomVariable, RVInfo> tdiff = null;
                MixedRadixNumber tdMRN = null;
                if (dividendDivisorDiff.Size() > 0)
                {
                    tdiff = CollectionFactory.CreateInsertionOrderedMap<IRandomVariable, RVInfo>();
                    foreach (IRandomVariable rv in dividendDivisorDiff)
                    {
                        tdiff.Put(rv, new RVInfo(rv));
                    }
                    tdMRN = new MixedRadixNumber(0, createRadixs(tdiff));
                }
                IMap<IRandomVariable, RVInfo> diff = tdiff;
                MixedRadixNumber dMRN = tdMRN;
                int[] qRVs = new int[quotient.radices.Length];
                MixedRadixNumber qMRN = new MixedRadixNumber(0, quotient.radices);
                ProbabilityTableIterator divisorIterator = new ProbabilityTableIteratorImp2(quotient, qRVs, qMRN, dMRN, diff, this);

                divisor.iterateOverTable(divisorIterator);
            }

            return quotient;
        }


        class ProbabilityTablecIteratorImpl3 : ProbabilityTableIterator
        {
            private int idx = 0;
            private ProbabilityTable multiplier;
            private ProbabilityTable probabilityTable;
            private ProbabilityTable product;
            private object[] term1Values;
            private object[] term2Values;

            public ProbabilityTablecIteratorImpl3(object[] term1Values, object[] term2Values, ProbabilityTable product, ProbabilityTable multiplier, ProbabilityTable probabilityTable)
            {
                this.term1Values = term1Values;
                this.term2Values = term2Values;
                this.product = product;
                this.multiplier = multiplier;
                this.probabilityTable = probabilityTable;
            }

            public void iterate(IMap<IRandomVariable, object> possibleWorld, double probability)
            {
                int term1Idx = termIdx(term1Values, probabilityTable, possibleWorld);
                int term2Idx = termIdx(term2Values, multiplier, possibleWorld);

                product.getValues()[idx] = probabilityTable.getValues()[term1Idx] * multiplier.getValues()[term2Idx];

                idx++;
            }

            private int termIdx(object[] termValues, ProbabilityTable d, IMap<IRandomVariable, object> possibleWorld)
            {
                if (0 == termValues.Length)
                {
                    // The term has no variables so always position 0.
                    return 0;
                }

                int i = 0;
                foreach (IRandomVariable rv in d.randomVarInfo.GetKeys())
                {
                    termValues[i] = possibleWorld.Get(rv);
                    ++i;
                }

                return d.getIndex(termValues);
            }
        }

        public ProbabilityTable pointwiseProduct(ProbabilityTable multiplier)
        {
            ISet<IRandomVariable> prodVars = SetOps.union(CollectionFactory.CreateSet<IRandomVariable>(randomVarInfo.GetKeys()),
                     CollectionFactory.CreateSet<IRandomVariable>(multiplier.randomVarInfo.GetKeys()));
            return pointwiseProductPOS(multiplier, prodVars.ToArray());
        }

        public ProbabilityTable pointwiseProductPOS(ProbabilityTable multiplier, params IRandomVariable[] prodVarOrder)
        {
            ProbabilityTable product = new ProbabilityTable(prodVarOrder);
            if (!product.randomVarInfo.GetKeys()
                .SequenceEqual(
                    SetOps.union(CollectionFactory.CreateSet<IRandomVariable>(randomVarInfo.GetKeys()), CollectionFactory.CreateSet<IRandomVariable>(multiplier.randomVarInfo.GetKeys()))))
            {
                throw new IllegalArgumentException("Specified list deatailing order of mulitplier is inconsistent.");
            }

            // If no variables in the product
            if (1 == product.getValues().Length)
            {
                product.getValues()[0] = getValues()[0] * multiplier.getValues()[0];
            }
            else
            {
                // Otherwise need to iterate through the product
                // to calculate its values based on the terms.
                object[] term1Values = new object[randomVarInfo.Size()];
                object[] term2Values = new object[multiplier.randomVarInfo.Size()];
                ProbabilityTableIterator di = new ProbabilityTablecIteratorImpl3(term1Values,
                    term2Values, product, multiplier, this);
                product.iterateOverTable(di);
            }

            return product;
        }


        public override string ToString()
        {
            if (null == toString)
            {
                IStringBuilder sb = TextFactory.CreateStringBuilder();
                sb.Append("<");
                for (int i = 0; i < values.Length; ++i)
                {
                    if (i > 0)
                    {
                        sb.Append(", ");
                    }
                    sb.Append(values[i]);
                }
                sb.Append(">");

                toString = sb.ToString();
            }
            return toString;
        }
         
        private void reinitLazyValues()
        {
            sum = -1;
            toString = null;
        }

        private int[] createRadixs(IMap<IRandomVariable, RVInfo> mapRtoInfo)
        {
            int[] r = new int[mapRtoInfo.Size()];
            // Read in reverse order so that the enumeration
            // through the distributions is of the following
            // order using a MixedRadixNumber, e.g. for two Booleans:
            // X Y
            // true true
            // true false
            // false true
            // false false
            // which corresponds with how displayed in book.
            int x = mapRtoInfo.Size() - 1;
            foreach (RVInfo rvInfo in mapRtoInfo.GetValues())
            {
                r[x] = rvInfo.getDomainSize();
                rvInfo.setRadixIdx(x);
                x--;
            }
            return r;
        }

        private class RVInfo
        {
            private IRandomVariable variable;
            private IFiniteDomain varDomain;
            private int radixIdx = 0;

            public RVInfo(IRandomVariable rv)
            {
                variable = rv;
                varDomain = (IFiniteDomain)variable.getDomain();
            }

            public IRandomVariable getVariable()
            {
                return variable;
            }

            public int getDomainSize()
            {
                return varDomain.Size();
            }

            public int getIdxForDomain(object value)
            {
                return varDomain.GetOffset(value);
            }

            public object getDomainValueAt(int idx)
            {
                return varDomain.GetValueAt(idx);
            }

            public void setRadixIdx(int idx)
            {
                radixIdx = idx;
            }

            public int getRadixIdx()
            {
                return radixIdx;
            }
        }

        private class CategoricalDistributionIteratorAdapter : IIterator
        {
            private CategoricalDistributionIterator cdi = null;

            public CategoricalDistributionIteratorAdapter(CategoricalDistributionIterator cdi)
            {
                this.cdi = cdi;
            }

            public void iterate(IMap<IRandomVariable, object> possibleAssignment, double probability)
            {
                cdi.iterate(possibleAssignment, probability);
            }
        }

        private class FactorIteratorAdapter : IIterator
        {

            private FactorIterator fi = null;

            public FactorIteratorAdapter(FactorIterator fi)
            {
                this.fi = fi;
            }

            public void iterate(IMap<IRandomVariable, object> possibleAssignment, double probability)
            {
                fi.iterate(possibleAssignment, probability);
            }
        }
    }
}
