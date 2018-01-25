using aima.net.learning.framework;
using aima.net.learning.neural.api;
using aima.net.util.math;

namespace aima.net.learning.neural
{
    public class FeedForwardNeuralNetwork : IFunctionApproximator
    {
        public const string UPPER_LIMIT_WEIGHTS = "upper_limit_weights";
        public const string LOWER_LIMIT_WEIGHTS = "lower_limit_weights";
        public const string NUMBER_OF_OUTPUTS = "number_of_outputs";
        public const string NUMBER_OF_HIDDEN_NEURONS = "number_of_hidden_neurons";
        public const string NUMBER_OF_INPUTS = "number_of_inputs";

        private readonly Layer hiddenLayer;
        private readonly Layer outputLayer;

        private INeuralNetworkTrainingScheme trainingScheme;

        /// <summary>
        /// Constructor to be used for non testing code.
        /// </summary>
        /// <param name="config"></param>
        public FeedForwardNeuralNetwork(NeuralNetworkConfig config)
        { 
            int numberOfInputNeurons = config
                    .GetParameterAsInteger(NUMBER_OF_INPUTS);
            int numberOfHiddenNeurons = config
                    .GetParameterAsInteger(NUMBER_OF_HIDDEN_NEURONS);
            int numberOfOutputNeurons = config
                    .GetParameterAsInteger(NUMBER_OF_OUTPUTS);

            double lowerLimitForWeights = config
                    .GetParameterAsDouble(LOWER_LIMIT_WEIGHTS);
            double upperLimitForWeights = config
                    .GetParameterAsDouble(UPPER_LIMIT_WEIGHTS);

            hiddenLayer = new Layer(numberOfHiddenNeurons, numberOfInputNeurons,
                    lowerLimitForWeights, upperLimitForWeights,
                    new LogSigActivationFunction());

            outputLayer = new Layer(numberOfOutputNeurons, numberOfHiddenNeurons,
                    lowerLimitForWeights, upperLimitForWeights,
                    new PureLinearActivationFunction()); 
        }

        /// <summary>
        /// ONLY for testing to set up a network with known weights in future use to
        /// deserialize networks after adding variables for pen weightupdate,
        /// lastnput etc
        /// </summary>
        /// <param name="hiddenLayerWeights"></param>
        /// <param name="hiddenLayerBias"></param>
        /// <param name="outputLayerWeights"></param>
        /// <param name="outputLayerBias"></param>
        public FeedForwardNeuralNetwork(Matrix hiddenLayerWeights,
                                        Vector hiddenLayerBias,
                                        Matrix outputLayerWeights,
                                        Vector outputLayerBias)
        {
            hiddenLayer = new Layer(hiddenLayerWeights, hiddenLayerBias, new LogSigActivationFunction());
            outputLayer = new Layer(outputLayerWeights, outputLayerBias, new PureLinearActivationFunction());
        }

        public void ProcessError(Vector error)
        {
            trainingScheme.ProcessError(this, error);
        }

        public Vector ProcessInput(Vector input)
        {
            return trainingScheme.ProcessInput(this, input);
        }

        public void TrainOn(NeuralNetworkDataSet innds, int numberofEpochs)
        {
            for (int i = 0; i < numberofEpochs; ++i)
            {
                innds.RefreshDataset();
                while (innds.HasMoreExamples())
                {
                    NeuralNetworkExample nne = innds.GetExampleAtRandom();
                    ProcessInput(nne.GetInput());
                    Vector error = GetOutputLayer()
                            .ErrorVectorFrom(nne.GetTarget());
                    ProcessError(error);
                }
            }
        }

        public Vector Predict(NeuralNetworkExample nne)
        {
            return ProcessInput(nne.GetInput());
        }

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

        public virtual void TestOn(DataSet ds) { }

        public Matrix GetHiddenLayerWeights()
        { 
            return hiddenLayer.GetWeightMatrix();
        }

        public Vector GetHiddenLayerBias()
        { 
            return hiddenLayer.GetBiasVector();
        }

        public Matrix GetOutputLayerWeights()
        { 
            return outputLayer.GetWeightMatrix();
        }

        public Vector GetOutputLayerBias()
        { 
            return outputLayer.GetBiasVector();
        }

        public Layer GetHiddenLayer()
        {
            return hiddenLayer;
        }

        public Layer GetOutputLayer()
        {
            return outputLayer;
        }

        public void SetTrainingScheme(INeuralNetworkTrainingScheme trainingScheme)
        {
            this.trainingScheme = trainingScheme;
            trainingScheme.SetNeuralNetwork(this);
        }
    }
}
