using aima.net.collections;
using aima.net.collections.api;
using aima.net.exceptions;
using aima.net.probability.api;
using aima.net.probability.domain.api;
using aima.net.probability.hmm.api;
using aima.net.probability.proposition;
using aima.net.probability.util;
using aima.net.util;
using aima.net.util.math;

namespace aima.net.probability.hmm
{
    /// <summary>
    /// Default implementation of the HiddenMarkovModel interface.
    /// </summary>
    public class HiddenMarkovModel : IHiddenMarkovModel
    {
        private IRandomVariable stateVariable = null;
        private IFiniteDomain stateVariableDomain = null;
        private Matrix transitionModel = null;
        private IMap<object, Matrix> sensorModel = null;
        private Matrix prior = null;

        /// <summary>
        /// Instantiate a Hidden Markov Model.
        /// </summary>
        /// <param name="stateVariable">
        /// the single discrete random variable used to describe the process states 1,...,S.
        /// </param>
        /// <param name="transitionModel">
        /// the transition model:<para />
        /// P(Xt | Xt-1)<para />
        /// is represented by an S * S matrix T where
        /// Tij= P(Xt = j | Xt-1 = i).
        /// </param>
        /// <param name="sensorModel">
        /// the sensor model in matrix form:<para />
        /// P(et | Xt = i) for each state i. For
        /// mathematical convenience we place each of these values into an
        /// S * S diagonal matrix.
        /// </param>
        /// <param name="prior">the prior distribution represented as a column vector in Matrix form.</param>
        public HiddenMarkovModel(IRandomVariable stateVariable, Matrix transitionModel, IMap<object, Matrix> sensorModel, Matrix prior)
        {
            if (!stateVariable.getDomain().IsFinite())
            {
                throw new IllegalArgumentException("State Variable for HHM must be finite.");
            }
            this.stateVariable = stateVariable;
            stateVariableDomain = (IFiniteDomain)stateVariable.getDomain();
            if (transitionModel.GetRowDimension() != transitionModel
                    .GetColumnDimension())
            {
                throw new IllegalArgumentException("Transition Model row and column dimensions must match.");
            }
            if (stateVariableDomain.Size() != transitionModel.GetRowDimension())
            {
                throw new IllegalArgumentException("Transition Model Matrix does not map correctly to the HMM's State Variable.");
            }
            this.transitionModel = transitionModel;
            foreach (Matrix smVal in sensorModel.GetValues())
            {
                if (smVal.GetRowDimension() != smVal.GetColumnDimension())
                {
                    throw new IllegalArgumentException("Sensor Model row and column dimensions must match.");
                }
                if (stateVariableDomain.Size() != smVal.GetRowDimension())
                {
                    throw new IllegalArgumentException("Sensor Model Matrix does not map correctly to the HMM's State Variable.");
                }
            }
            this.sensorModel = sensorModel;
            if (transitionModel.GetRowDimension() != prior.GetRowDimension()
                    && prior.GetColumnDimension() != 1)
            {
                throw new IllegalArgumentException("Prior is not of the correct dimensions.");
            }
            this.prior = prior;
        }

        public virtual IRandomVariable getStateVariable()
        {
            return stateVariable;
        }

        public virtual Matrix getTransitionModel()
        {
            return transitionModel;
        }

        public virtual IMap<object, Matrix> getSensorModel()
        {
            return sensorModel;
        }

        public virtual Matrix getPrior()
        {
            return prior;
        }

        public virtual Matrix getEvidence(ICollection<AssignmentProposition> evidence)
        {
            if (evidence.Size() != 1)
            {
                throw new IllegalArgumentException("Only a single evidence observation value should be provided.");
            }
            Matrix e = sensorModel.Get(evidence.Get(0).getValue());
            if (null == e)
            {
                throw new IllegalArgumentException("Evidence does not map to sensor model.");
            }
            return e;
        }


        public virtual Matrix createUnitMessage()
        {
            double[] values = new double[stateVariableDomain.Size()];
            for (int i = 0; i < values.Length; ++i)
            {
                values[i] = 1D;
            }

            return new Matrix(values, values.Length);
        }


        public virtual Matrix convert(ICategoricalDistribution fromCD)
        {
            double[] values = fromCD.getValues();
            return new Matrix(values, values.Length);
        }


        public virtual ICategoricalDistribution convert(Matrix fromMessage)
        {
            return new ProbabilityTable(fromMessage.GetRowPackedCopy(), stateVariable);
        }

        public virtual ICollection<ICategoricalDistribution> convert(ICollection<Matrix> matrixs)
        {
            ICollection<ICategoricalDistribution> cds = CollectionFactory.CreateQueue<ICategoricalDistribution>();
            foreach (Matrix m in matrixs)
            {
                cds.Add(convert(m));
            }
            return cds;
        }

        public virtual Matrix normalize(Matrix m)
        {
            double[] values = m.GetRowPackedCopy();
            return new Matrix(Util.normalize(values), values.Length);
        }
    }
}
