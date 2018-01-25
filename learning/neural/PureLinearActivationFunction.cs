using aima.net.learning.neural.api;

namespace aima.net.learning.neural
{
    public class PureLinearActivationFunction : IActivationFunction
    { 
        public double Activation(double parameter)
        {
            return parameter;
        }

        public double Deriv(double parameter)
        {  
            return 1;
        }
    }
}
