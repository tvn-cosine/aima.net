using aima.net.learning.neural.api;
using aima.net.util.math;

namespace aima.net.learning.neural
{
    public class Perceptron : IFunctionApproximator
    {
        private readonly Layer layer;
        private Vector lastInput;

        public Perceptron(int numberOfNeurons, int numberOfInputs)
        {
            this.layer = new Layer(numberOfNeurons, numberOfInputs, 2.0, -2.0, new HardLimitActivationFunction()); 
        }

        public Vector ProcessInput(Vector input)
        {
            lastInput = input;
            return layer.FeedForward(input);
        }

        public void ProcessError(Vector error)
        {
            Matrix weightUpdate = error.Times(lastInput.Transpose());
            layer.AcceptNewWeightUpdate(weightUpdate);

            Vector biasUpdate = layer.GetBiasVector().Plus(error);
            layer.AcceptNewBiasUpdate(biasUpdate);

        }
         
        /// <summary>
        /// Induces the layer of this perceptron from the specified set of examples
        /// </summary>
        /// <param name="innds">a set of training examples for constructing the layer of this perceptron.</param>
        /// <param name="numberofEpochs">the number of training epochs to be used.</param>
        public void TrainOn(NeuralNetworkDataSet innds, int numberofEpochs)
        {
            for (int i = 0; i < numberofEpochs; ++i)
            {
                innds.RefreshDataset();
                while (innds.HasMoreExamples())
                {
                    NeuralNetworkExample nne = innds.GetExampleAtRandom();
                    ProcessInput(nne.GetInput());
                    Vector error = layer.ErrorVectorFrom(nne.GetTarget());
                    ProcessError(error);
                }
            }
        }
         
        /// <summary>
        /// Returns the outcome predicted for the specified example
        /// </summary>
        /// <param name="nne">an example</param>
        /// <returns>the outcome predicted for the specified example</returns>
        public Vector Predict(NeuralNetworkExample nne)
        {
            return ProcessInput(nne.GetInput());
        }
       
        /// <summary>
        /// Returns the accuracy of the hypothesis on the specified set of examples
        /// </summary>
        /// <param name="nnds">the neural network data set to be tested on.</param>
        /// <returns>the accuracy of the hypothesis on the specified set of examples</returns>
        public int[] TestOnDataSet(NeuralNetworkDataSet nnds)
        {
            int[] result = new int[] { 0, 0 };
            nnds.RefreshDataset();
            while (nnds.HasMoreExamples())
            {
                NeuralNetworkExample nne = nnds.GetExampleAtRandom();
                Vector prediction = Predict(nne);
                if (nne.IsCorrect(prediction))
                {
                    result[0] = result[0] + 1;
                }
                else
                {
                    result[1] = result[1] + 1;
                }
            }
            return result;
        }
    }
}
