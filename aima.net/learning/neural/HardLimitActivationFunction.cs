using aima.net.learning.neural.api;

namespace aima.net.learning.neural
{
    public class HardLimitActivationFunction : IActivationFunction
    {
        public double Activation(double parameter)
        { 
            if (parameter < 0D)
            {
                return 0D;
            }
            else
            {
                return 1D;
            }
        }

        public double Deriv(double parameter)
        {
            return 0D;
        }
    }
}
