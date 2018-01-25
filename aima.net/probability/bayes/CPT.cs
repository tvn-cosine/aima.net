using aima.net.collections;
using aima.net.collections.api;
using aima.net.exceptions;
using aima.net.probability.api;
using aima.net.probability.bayes.api;
using aima.net.probability.domain;
using aima.net.probability.domain.api;
using aima.net.probability.proposition;
using aima.net.probability.util;

namespace aima.net.probability.bayes 
{
    /// <summary>
    /// Default implementation of the ConditionalProbabilityTable interface.
    /// </summary>
    public class CPT : IConditionalProbabilityTable
    { 
        private IRandomVariable on = null;
        private ISet<IRandomVariable> parents = CollectionFactory.CreateSet<IRandomVariable>();
        private ProbabilityTable table = null;
        private ICollection<object> onDomain = CollectionFactory.CreateQueue<object>();

        public CPT(IRandomVariable on, double[] values, params IRandomVariable[] conditionedOn)
        {
            this.on = on;
            if (null == conditionedOn)
            {
                conditionedOn = new IRandomVariable[0];
            }
            IRandomVariable[] tableVars = new IRandomVariable[conditionedOn.Length + 1];
            for (int i = 0; i < conditionedOn.Length;++i)
            {
                tableVars[i] = conditionedOn[i];
                parents.Add(conditionedOn[i]);
            }
            tableVars[conditionedOn.Length] = on;
            table = new ProbabilityTable(values, tableVars);
            onDomain.AddAll(((IFiniteDomain)on.getDomain()).GetPossibleValues());

            checkEachRowTotalsOne();
        }

        public virtual double probabilityFor(params object[] values)
        {
            return table.getValue(values);
        }

        public virtual IRandomVariable GetOn()
        {
            return on;
        }


        public virtual ISet<IRandomVariable> GetParents()
        {
            return parents;
        }


        public virtual ISet<IRandomVariable> GetFor()
        {
            return table.getFor();
        }


        public virtual bool Contains(IRandomVariable rv)
        {
            return table.contains(rv);
        }


        public virtual double GetValue(params object[] eventValues)
        {
            return table.getValue(eventValues);
        }


        public virtual double GetValue(params AssignmentProposition[] eventValues)
        {
            return table.getValue(eventValues);
        }


        public virtual object GetSample(double probabilityChoice, params object[] parentValues)
        {
            return ProbUtil.sample(probabilityChoice, on, GetConditioningCase(parentValues).getValues());
        }


        public virtual object GetSample(double probabilityChoice, params AssignmentProposition[] parentValues)
        {
            return ProbUtil.sample(probabilityChoice, on, GetConditioningCase(parentValues).getValues());
        }

        IProbabilityDistribution IConditionalProbabilityDistribution.GetConditioningCase(params object[] parentValues)
        {
            return GetConditioningCase(parentValues);
        }
         
        public virtual ICategoricalDistribution GetConditioningCase(params object[] parentValues)
        {
            if (parentValues.Length != parents.Size())
            {
                throw new IllegalArgumentException(
                        "The number of parent value arguments ["
                                + parentValues.Length
                                + "] is not equal to the number of parents ["
                                + parents.Size() + "] for this CPT.");
            }
            AssignmentProposition[] aps = new AssignmentProposition[parentValues.Length];
            int idx = 0;
            foreach (IRandomVariable parentRV in parents)
            {
                aps[idx] = new AssignmentProposition(parentRV, parentValues[idx]);
                idx++;
            }

            return GetConditioningCase(aps);
        }

        class GetConditionCaseIterator : ProbabilityTable.ProbabilityTableIterator
        {
            private ProbabilityTable cc;
            private int idx = 0;

            public GetConditionCaseIterator(ProbabilityTable cc)
            {
                this.cc = cc;
            }

            public void iterate(IMap<IRandomVariable, object> possibleAssignment, double probability)
            {
                cc.getValues()[idx] = probability;
                idx++;
            }
        }

        IProbabilityDistribution IConditionalProbabilityDistribution.GetConditioningCase(params AssignmentProposition[] parentValues)
        {
            return GetConditioningCase(parentValues);
        }

        public virtual ICategoricalDistribution GetConditioningCase(params AssignmentProposition[] parentValues)
        {
            if (parentValues.Length != parents.Size())
            {
                throw new IllegalArgumentException(
                        "The number of parent value arguments ["
                                + parentValues.Length
                                + "] is not equal to the number of parents ["
                                + parents.Size() + "] for this CPT.");
            }
            ProbabilityTable cc = new ProbabilityTable(GetOn());
            ProbabilityTable.ProbabilityTableIterator pti = new GetConditionCaseIterator(cc);
            table.iterateOverTable(pti, parentValues);

            return cc;
        }

        class getFactorForIterator : ProbabilityTable.ProbabilityTableIterator
        {
            private ProbabilityTable fof;
            private object[] termValues;

            public getFactorForIterator(object[] termValues, ProbabilityTable fof)
            {
                this.termValues = termValues;
                this.fof = fof;
            }

            public void iterate(IMap<IRandomVariable, object> possibleWorld, double probability)
            {
                if (0 == termValues.Length)
                {
                    fof.getValues()[0] += probability;
                }
                else
                {
                    int i = 0;
                    foreach (IRandomVariable rv in fof.getFor())
                    {
                        termValues[i] = possibleWorld.Get(rv);
                       ++i;
                    }
                    fof.getValues()[fof.getIndex(termValues)] += probability;
                }
            }
        }

        public virtual IFactor GetFactorFor(params AssignmentProposition[] evidence)
        {
            ISet<IRandomVariable> fofVars = CollectionFactory.CreateSet<IRandomVariable>(table.getFor());
            foreach (AssignmentProposition ap in evidence)
            {
                fofVars.Remove(ap.getTermVariable());
            }
            ProbabilityTable fof = new ProbabilityTable(fofVars);
            // Otherwise need to iterate through the table for the
            // non evidence variables.
            object[] termValues = new object[fofVars.Size()];
            ProbabilityTable.ProbabilityTableIterator di = new getFactorForIterator(termValues, fof);
            table.iterateOverTable(di, evidence);

            return fof;
        }

        class checkEachRowTotalsOneIterator : ProbabilityTable.ProbabilityTableIterator
        {
            private int rowSize;
            private int iterateCnt = 0;
            private double rowProb = 0;
            private ICollection<object> onDomain;

            public checkEachRowTotalsOneIterator(ICollection<object> onDomain)
            {
                this.onDomain = onDomain;
                rowSize = onDomain.Size();
            }

            public void iterate(IMap<IRandomVariable, object> possibleWorld,
                    double probability)
            {
                iterateCnt++;
                rowProb += probability;
                if (iterateCnt % rowSize == 0)
                {
                    if (System.Math.Abs(1 - rowProb) > ProbabilityModelImpl.DEFAULT_ROUNDING_THRESHOLD)
                    {
                        throw new IllegalArgumentException("Row "
                                + (iterateCnt / rowSize)
                                + " of CPT does not sum to 1.0.");
                    }
                    rowProb = 0;
                }
            }
        }

        private void checkEachRowTotalsOne()
        {
            ProbabilityTable.ProbabilityTableIterator di = new checkEachRowTotalsOneIterator(onDomain);

            table.iterateOverTable(di);
        }
    }
}
