using aima.net.collections.api;
using aima.net.probability.api;
using aima.net.probability.proposition;

namespace aima.net.probability.temporal.api 
{
    /// <summary> 
    /// The BACKWARD operator<para />
    /// bk+1:t = P(ek+1:t | Xk) 
    /// is defined by Equation (15.9).<para />
    ///  
    /// P(ek+1:t | Xk) 
    /// = +xk+1P(ek+1:t | Xk, xk+1)P(xk+1 | Xk) (conditioning on Xk+1)
    /// = +xk+1P(ek+1:t | xk+1)P(xk+1 | Xk) (by conditional independence)
    /// = +xk+1P(ek+1, ek+2:t | xk+1)P(xk+1 | Xk)
    /// = +xk+1P(ek+1 | xk+1)P(ek+2:t | xk+1)P(xk+1 | Xk)
    /// </summary>
    public interface IBackwardStepInference
    {
        /// <summary>
        /// The BACKWARD operator<para />
        /// bk+1:t = P(ek+1:t | Xk) 
        /// is defined by Equation (15.9).<para />
        ///  
        /// P(ek+1:t | Xk) 
        /// = +xk+1P(ek+1:t | Xk, xk+1)P(xk+1 | Xk) (conditioning on Xk+1)
        /// = +xk+1P(ek+1:t | xk+1)P(xk+1 | Xk) (by conditional independence)
        /// = +xk+1P(ek+1, ek+2:t | xk+1)P(xk+1 | Xk)
        /// = +xk+1P(ek+1 | xk+1)P(ek+2:t | xk+1)P(xk+1 | Xk)
        /// </summary>
        /// <param name="b_kp2t">bk+2:t</param>
        /// <param name="e_kp1t">ek+1:t</param>
        /// <returns>bk+1:t</returns>
        ICategoricalDistribution backward(ICategoricalDistribution b_kp2t, ICollection<AssignmentProposition> e_kp1t);
    }

}
