using aima.net.learning.framework;
using aima.net.learning.neural.api;
using aima.net.util.math;
using aima.net.exceptions;

namespace aima.net.learning.neural
{
    public class FeedForwardDeepNeuralNetwork : IFunctionApproximator
    { 
        public const string UPPER_LIMIT_WEIGHTS = "upper_limit_weights";
        public const string LOWER_LIMIT_WEIGHTS = "lower_limit_weights";
        public const string NUMBER_OF_OUTPUTS = "number_of_outputs";
        public const string NUMBER_OF_INPUTS = "number_of_inputs";

        public const string NUMBER_OF_HIDDEN_LAYERS = "number_of_hidden_layers";
        public const string NUMBER_OF_HIDDEN_NEURONS_PER_LAYER = "number_of_hidden_neurons_per_layer";

        private readonly Layer outputLayer;
        private readonly Layer[] hiddenLayers;
        private readonly NeuralNetworkConfig config;

        private INeuralNetworkTrainingScheme trainingScheme;

        public int GetNumberOfHiddenLayers()
        {
            return hiddenLayers.Length;
        }

        /// <summary>
        /// Constructor to be used for non testing code.
        /// </summary>
        /// <param name="config"></param>
        public FeedForwardDeepNeuralNetwork(NeuralNetworkConfig config, IActivationFunction activationFunction)
        {
            if (config.GetParameterAsInteger(NUMBER_OF_HIDDEN_LAYERS) < 1)
            {
                throw new ArgumentOutOfRangeException("NUMBER_OF_HIDDEN_LAYERS must be >= 1");
            }

            if (null == activationFunction)
            {
                activationFunction = new LogSigActivationFunction();
            }
            this.config = config;

            int numberOfInputNeurons = config.GetParameterAsInteger(NUMBER_OF_INPUTS);
            int numberOfOutputNeurons = config.GetParameterAsInteger(NUMBER_OF_OUTPUTS);
            
            double lowerLimitForWeights = config.GetParameterAsDouble(LOWER_LIMIT_WEIGHTS);
            double upperLimitForWeights = config.GetParameterAsDouble(UPPER_LIMIT_WEIGHTS);

            hiddenLayers = new Layer[config.GetParameterAsInteger(NUMBER_OF_HIDDEN_LAYERS)];
            
            //TODO: Create Hidden layers here
        }

        /// <summary>
        /// ONLY for testing to set up a network with known weights in future use to
        /// deserialize networks after adding variables for pen weightupdate,
        /// lastnput etc
        /// </summary>
        /// <param name="hiddenLayersWeights"></param>
        /// <param name="hiddenLayersBias"></param>
        /// <param name="outputLayerWeights"></param>
        /// <param name="outputLayerBias"></param>
        public FeedForwardDeepNeuralNetwork(Matrix[] hiddenLayersWeights,
                                            Vector[] hiddenLayersBias,
                                            Matrix outputLayerWeights,
                                            Vector outputLayerBias)
        {
            if (hiddenLayersWeights.Length != config.GetParameterAsInteger(NUMBER_OF_HIDDEN_LAYERS)
              || hiddenLayersBias.Length != config.GetParameterAsInteger(NUMBER_OF_HIDDEN_LAYERS))
            {
                throw new ArgumentOutOfRangeException("hiddenLayerWeights,hiddenLayerBias != NUMBER_OF_HIDDEN_LAYERS");
            }

            hiddenLayers = new Layer[config.GetParameterAsInteger(NUMBER_OF_HIDDEN_LAYERS)];
            for (int i = 0; i < hiddenLayers.Length; ++i)
            {
                hiddenLayers[i] = new Layer(hiddenLayersWeights[i], hiddenLayersBias[i], new LogSigActivationFunction());
            }

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
                     
                    Vector error = GetOutputLayer().ErrorVectorFrom(nne.GetTarget()); 
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

        public Matrix GetHiddenLayerWeights(int layer)
        {
            if (layer < 0 || layer > hiddenLayers.Length)
            {
                throw new ArgumentOutOfRangeException("layer does not exist");
            }

            return hiddenLayers[layer].GetWeightMatrix();
        }

        public Vector GetHiddenLayerBias(int layer)
        {
            if (layer < 0 || layer > hiddenLayers.Length)
            {
                throw new ArgumentOutOfRangeException("layer does not exist");
            }

            return hiddenLayers[layer].GetBiasVector();
        }

        public Matrix GetOutputLayerWeights()
        {
            return outputLayer.GetWeightMatrix();
        }

        public Layer GetHiddenLayer(int layer)
        {
            if (layer < 0 || layer > hiddenLayers.Length)
            {
                throw new ArgumentOutOfRangeException("layer does not exist");
            }

            return hiddenLayers[layer];
        }

        public Vector GetOutputLayerBias()
        {
            return outputLayer.GetBiasVector();
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