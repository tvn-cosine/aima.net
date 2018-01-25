namespace aima.net.learning.neural.api
{
    public interface IActivationFunction
    {
        double Activation(double parameter); 
        double Deriv(double parameter);
    }
}
