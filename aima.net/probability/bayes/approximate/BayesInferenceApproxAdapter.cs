using aima.net.probability.api;
using aima.net.probability.bayes.api;
using aima.net.probability.bayes.approximate.api;
using aima.net.probability.proposition;

namespace aima.net.probability.bayes.approximate
{
    /// <summary> 
    /// An Adapter class to let BayesSampleInference implementations to be used in
    /// places where calls are being made to the BayesInference API.
    /// </summary>
    public class BayesInferenceApproxAdapter : IBayesInference
    {
        private int N = 1000;
        private IBayesSampleInference adaptee = null;

        public BayesInferenceApproxAdapter(IBayesSampleInference adaptee)
        {
            this.adaptee = adaptee;
        }

        public BayesInferenceApproxAdapter(IBayesSampleInference adaptee, int N)
            : this(adaptee)
        {
            this.N = N;
        }


        /// <summary>
        /// Return the number of Samples when calling the BayesSampleInference adaptee.
        /// </summary>
        /// <returns>the number of Samples when calling the BayesSampleInference adaptee.</returns>
        public int GetN()
        {
            return N;
        }


        /// <summary>
        /// Return the numver of samples to be generated when calling the BayesSampleInference adaptee.
        /// </summary>
        /// <param name="n">the numver of samples to be generated when calling the BayesSampleInference adaptee.</param>
        public void SetN(int n)
        {
            N = n;
        }


        /// <summary>
        /// Return the BayesSampleInference implementation to be adapted to the BayesInference API.
        /// </summary>
        /// <returns>the BayesSampleInference implementation to be adapted to the BayesInference API.</returns>
        public IBayesSampleInference GetAdaptee()
        {
            return adaptee;
        }

        /// <summary>
        /// Return the BayesSampleInference implementation be be apated to the BayesInference API.
        /// </summary>
        /// <param name="adaptee">the BayesSampleInference implementation be be apated to the BayesInference API.</param>
        public void SetAdaptee(IBayesSampleInference adaptee)
        {
            this.adaptee = adaptee;
        }

        public ICategoricalDistribution Ask(IRandomVariable[] X,
                  AssignmentProposition[] observedEvidence,
                  IBayesianNetwork bn)
        {
            return adaptee.Ask(X, observedEvidence, bn, N);
        }
    }
}
