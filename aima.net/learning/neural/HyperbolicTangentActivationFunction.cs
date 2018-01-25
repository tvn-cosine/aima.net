using aima.net.learning.neural.api;

namespace aima.net.learning.neural
{
    public class HyperbolicTangentActivationFunction : IActivationFunction
    {
        public double Activation(double parameter)
        {
            return (2D / (1D + (System.Math.Pow(System.Math.E, (-2D * parameter))))) - 1D;
        }

        public double Deriv(double parameter)
        {
            return 1D - System.Math.Pow(Activation(parameter), 2);
        }
    }
}
