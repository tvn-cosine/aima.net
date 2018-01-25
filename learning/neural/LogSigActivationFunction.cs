using aima.net.learning.neural.api;

namespace aima.net.learning.neural
{
    public class LogSigActivationFunction : IActivationFunction
    { 
        public double Activation(double parameter)
        { 
            return 1.0 / (1.0 + System.Math.Pow(System.Math.E, (-1.0 * parameter)));
        }

        public double Deriv(double parameter)
        {
            // parameter = induced field
            // e == activation
            double e = 1.0 / (1.0 + System.Math.Pow(System.Math.E, (-1.0 * parameter)));
            return e * (1.0 - e);
        }
    }
}
