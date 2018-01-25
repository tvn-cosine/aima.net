using aima.net.util.math;

namespace aima.net.learning.neural.api
{
    public interface INeuralNetworkTrainingScheme
    {
        Vector ProcessInput(IFunctionApproximator network, Vector input); 
        void ProcessError(IFunctionApproximator network, Vector error); 
        void SetNeuralNetwork(IFunctionApproximator ffnn);
    }
}
