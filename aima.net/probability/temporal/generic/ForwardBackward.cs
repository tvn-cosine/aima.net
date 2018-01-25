using aima.net.collections;
using aima.net.collections.api;
using aima.net.probability.api;
using aima.net.probability.proposition;
using aima.net.probability.proposition.api;
using aima.net.probability.temporal.api;
using aima.net.probability.util;

namespace aima.net.probability.temporal.generic
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 576.<br>
     * <br>
     * 
     * <pre>
     * function FORWARD-BACKWARD(ev, prior) returns a vector of probability distributions
     *   inputs: ev, a vector of evidence values for steps 1,...,t
     *           prior, the prior distribution on the initial state, <b>P</b>(X<sub>0</sub>)
     *   local variables: fv, a vector of forward messages for steps 0,...,t
     *                    b, a representation of the backward message, initially all 1s
     *                    sv, a vector of smoothed estimates for steps 1,...,t
     *                    
     *   fv[0] <- prior
     *   for i = 1 to t do
     *       fv[i] <- FORWARD(fv[i-1], ev[i])
     *   for i = t downto 1 do
     *       sv[i] <- NORMALIZE(fv[i] * b)
     *       b <- BACKWARD(b, ev[i])
     *   return sv
     * </pre>
     * 
     * Figure 15.4 The forward-backward algorithm for smoothing: computing posterior
     * probabilities of a sequence of states given a sequence of observations. The
     * FORWARD and BACKWARD operators are defined by Equations (15.5) and (15.9),
     * respectively.<br>
     * <br>
     * <b>Note:</b> An implementation of the FORWARD-BACKWARD algorithm using the
     * general purpose probability APIs, i.e. the underlying model implementation is
     * abstracted away.
     * 
     * @author Ciaran O'Reilly
     */
    public class ForwardBackward : IForwardBackwardInference
    {
        private IFiniteProbabilityModel transitionModel = null;
        private IMap<IRandomVariable, IRandomVariable> tToTm1StateVarMap = CollectionFactory.CreateInsertionOrderedMap<IRandomVariable, IRandomVariable>();
        private IFiniteProbabilityModel sensorModel = null;

        /**
         * Instantiate an instance of the Forward Backward algorithm.
         * 
         * @param transitionModel
         *            the transition model.
         * @param tToTm1StateVarMap
         *            a map from the X<sub>t<sub> random variables in the transition
         *            model the to X<sub>t-1</sub> random variables.
         * @param sensorModel
         *            the sensor model.
         */
        public ForwardBackward(IFiniteProbabilityModel transitionModel,
                IMap<IRandomVariable, IRandomVariable> tToTm1StateVarMap,
                IFiniteProbabilityModel sensorModel)
        {
            this.transitionModel = transitionModel;
            this.tToTm1StateVarMap.AddAll(tToTm1StateVarMap);
            this.sensorModel = sensorModel;
        }
         
        // function FORWARD-BACKWARD(ev, prior) returns a vector of probability distributions 
        public ICollection<ICategoricalDistribution> forwardBackward(
               ICollection<ICollection<AssignmentProposition>> ev, ICategoricalDistribution prior)
        {
            // local variables: fv, a vector of forward messages for steps 0,...,t
            ICollection<ICategoricalDistribution> fv = CollectionFactory.CreateQueue<ICategoricalDistribution>();
            // b, a representation of the backward message, initially all 1s
            ICategoricalDistribution b = initBackwardMessage();
            // sv, a vector of smoothed estimates for steps 1,...,t
            ICollection<ICategoricalDistribution> sv = CollectionFactory.CreateQueue<ICategoricalDistribution>();

            // fv[0] <- prior
            fv.Add(prior);
            // for i = 1 to t do
            for (int i = 0; i < ev.Size();++i)
            {
                // fv[i] <- FORWARD(fv[i-1], ev[i])
                fv.Add(forward(fv.Get(i), ev.Get(i)));
            }
            // for i = t downto 1 do
            for (int i = ev.Size() - 1; i >= 0; i--)
            {
                // sv[i] <- NORMALIZE(fv[i] * b)
                sv.Insert(0, fv.Get(i + 1).multiplyBy(b).normalize());
                // b <- BACKWARD(b, ev[i])
                b = backward(b, ev.Get(i));
            }

            // return sv
            return sv;
        }

        class CategoricalDistributionIteratorImpl : CategoricalDistributionIterator
        {
            private ICategoricalDistribution s1;
            private IFiniteProbabilityModel transitionModel;
            private AssignmentProposition[] xt;
            private IProposition xtp1;
            private IMap<IRandomVariable, AssignmentProposition> xtVarAssignMap;

            public CategoricalDistributionIteratorImpl(IFiniteProbabilityModel transitionModel, IMap<IRandomVariable, AssignmentProposition> xtVarAssignMap, ICategoricalDistribution s1, IProposition xtp1, AssignmentProposition[] xt)
            {
                this.transitionModel = transitionModel;
                this.xtVarAssignMap = xtVarAssignMap;
                this.s1 = s1;
                this.xtp1 = xtp1;
                this.xt = xt;
            }

            public void iterate(IMap<IRandomVariable, object> possibleWorld, double probability)
            {
                // <b>P</b>(X<sub>t+1</sub> | x<sub>t</sub>)*
                // P(x<sub>t</sub> | e<sub>1:t</sub>)
                foreach (var av in possibleWorld)
                {
                    xtVarAssignMap.Get(av.GetKey()).setValue(av.GetValue());
                }
                int i = 0;
                foreach (double tp in transitionModel.posteriorDistribution(xtp1, xt).getValues())
                {
                    s1.setValue(i, s1.getValues()[i] + (tp * probability));
                   ++i;
                }
            }
        }

        public ICategoricalDistribution forward(ICategoricalDistribution f1_t, ICollection<AssignmentProposition> e_tp1)
        {
            ICategoricalDistribution s1 = new ProbabilityTable(f1_t.getFor());
            // Set up required working variables
            IProposition[] props = new IProposition[s1.getFor().Size()];
            int i = 0;
            foreach (IRandomVariable rv in s1.getFor())
            {
                props[i] = new RandVar(rv.getName(), rv.getDomain());
               ++i;
            }
            IProposition Xtp1 = ProbUtil.constructConjunction(props);
            AssignmentProposition[] xt = new AssignmentProposition[tToTm1StateVarMap.Size()];
            IMap<IRandomVariable, AssignmentProposition> xtVarAssignMap = CollectionFactory.CreateInsertionOrderedMap<IRandomVariable, AssignmentProposition>();
            i = 0;
            foreach (IRandomVariable rv in tToTm1StateVarMap.GetKeys())
            {
                xt[i] = new AssignmentProposition(tToTm1StateVarMap.Get(rv), "<Dummy Value>");
                xtVarAssignMap.Put(rv, xt[i]);
               ++i;
            }

            // Step 1: Calculate the 1 time step prediction
            // &sum;<sub>x<sub>t</sub></sub>
            CategoricalDistributionIterator if1_t = new CategoricalDistributionIteratorImpl(transitionModel,
                xtVarAssignMap, s1, Xtp1, xt);
            f1_t.iterateOver(if1_t);

            // Step 2: multiply by the probability of the evidence
            // and normalize
            // <b>P</b>(e<sub>t+1</sub> | X<sub>t+1</sub>)
            ICategoricalDistribution s2 = sensorModel.posteriorDistribution(ProbUtil
                    .constructConjunction(e_tp1.ToArray()), Xtp1);

            return s2.multiplyBy(s1).normalize();
        }

        class CategoricalDistributionIteratorImpl2 : CategoricalDistributionIterator
        {
            private ICategoricalDistribution b_kp1t;
            private IProposition pe_kp1;
            private IFiniteProbabilityModel sensorModel;
            private IFiniteProbabilityModel transitionModel;
            private IProposition xk;
            private IProposition x_kp1;
            private IMap<IRandomVariable, AssignmentProposition> x_kp1VarAssignMap;

            public CategoricalDistributionIteratorImpl2(IMap<IRandomVariable, AssignmentProposition> x_kp1VarAssignMap, IFiniteProbabilityModel sensorModel, IFiniteProbabilityModel transitionModel, ICategoricalDistribution b_kp1t, IProposition pe_kp1, IProposition xk, IProposition x_kp1)
            {
                this.x_kp1VarAssignMap = x_kp1VarAssignMap;
                this.sensorModel = sensorModel;
                this.transitionModel = transitionModel;
                this.b_kp1t = b_kp1t;
                this.pe_kp1 = pe_kp1;
                this.xk = xk;
                this.x_kp1 = x_kp1;
            }

            public void iterate(IMap<IRandomVariable, object> possibleWorld, double probability)
            {
                // Assign current values for x<sub>k+1</sub>
                foreach (var av in possibleWorld)
                {
                    x_kp1VarAssignMap.Get(av.GetKey()).setValue(av.GetValue());
                }

                // P(e<sub>k+1</sub> | x<sub>k+1</sub>)
                // P(e<sub>k+2:t</sub> | x<sub>k+1</sub>)
                double p = sensorModel.posterior(pe_kp1, x_kp1) * probability;

                // <b>P</b>(x<sub>k+1</sub> | X<sub>k</sub>)
                int i = 0;
                foreach (double tp in transitionModel.posteriorDistribution(x_kp1, xk).getValues())
                {
                    b_kp1t.setValue(i, b_kp1t.getValues()[i] + (tp * p));
                   ++i;
                }
            }
        }

        public ICategoricalDistribution backward(ICategoricalDistribution b_kp2t, ICollection<AssignmentProposition> e_kp1)
        {
            ICategoricalDistribution b_kp1t = new ProbabilityTable(b_kp2t.getFor());
            // Set up required working variables
            IProposition[] props = new IProposition[b_kp1t.getFor().Size()];
            int i = 0;
            foreach (IRandomVariable rv in b_kp1t.getFor())
            {
                IRandomVariable prv = tToTm1StateVarMap.Get(rv);
                props[i] = new RandVar(prv.getName(), prv.getDomain());
               ++i;
            }
            IProposition Xk = ProbUtil.constructConjunction(props);
            AssignmentProposition[] ax_kp1 = new AssignmentProposition[tToTm1StateVarMap.Size()];
            IMap<IRandomVariable, AssignmentProposition> x_kp1VarAssignMap = CollectionFactory.CreateInsertionOrderedMap<IRandomVariable, AssignmentProposition>();
            i = 0;
            foreach (IRandomVariable rv in b_kp1t.getFor())
            {
                ax_kp1[i] = new AssignmentProposition(rv, "<Dummy Value>");
                x_kp1VarAssignMap.Put(rv, ax_kp1[i]);
               ++i;
            }
            IProposition x_kp1 = ProbUtil.constructConjunction(ax_kp1);
            props = e_kp1.ToArray();
            IProposition pe_kp1 = ProbUtil.constructConjunction(props);

            // &sum;<sub>x<sub>k+1</sub></sub>
            CategoricalDistributionIterator ib_kp2t = new CategoricalDistributionIteratorImpl2(x_kp1VarAssignMap, 
                sensorModel, transitionModel, b_kp1t, pe_kp1, Xk, x_kp1);
            b_kp2t.iterateOver(ib_kp2t);

            return b_kp1t;
        }
         
        private ICategoricalDistribution initBackwardMessage()
        {
            ProbabilityTable b = new ProbabilityTable(tToTm1StateVarMap.GetKeys());

            for (int i = 0; i < b.size();++i)
            {
                b.setValue(i, 1.0);
            }

            return b;
        }
    }
}
