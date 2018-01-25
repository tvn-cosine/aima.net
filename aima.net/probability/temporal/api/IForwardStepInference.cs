using aima.net.collections.api;
using aima.net.probability.api;
using aima.net.probability.proposition;

namespace aima.net.probability.temporal.api
{
    /// <summary>  
    /// The FORWARD operator is defined by Equation (15.5).<para />
    /// P(Xt+1 | e1:t+1) 
    /// = αP(et+1 | Xt+1)+xtP(Xt+1 | xt, e1:t)P(xt | e1:t 
    /// = αP(et+1 | Xt+1)+xtP(Xt+1 | xt)P(xt | e1:t) (Markov Assumption)
    /// </summary>
    public interface IForwardStepInference
    { 
        /// <summary>
        /// The FORWARD operator is defined by Equation (15.5).<para />
        /// P(Xt+1 | e1:t+1) 
        /// = αP(et+1 | Xt+1)+xtP(Xt+1 | xt, e1:t)P(xt | e1:t 
        /// = αP(et+1 | Xt+1)+xtP(Xt+1 | xt)P(xt | e1:t) (Markov Assumption)
        /// </summary>
        /// <param name="f1_t">f1:t</param>
        /// <param name="e_tp1">et+1</param>
        /// <returns>f1:t+1 </returns>
        ICategoricalDistribution forward(ICategoricalDistribution f1_t, ICollection<AssignmentProposition> e_tp1);
    } 
}
