using aima.net.exceptions;
using aima.net.learning.framework;
using aima.net.learning.neural;
using aima.net.learning.neural.api;
using aima.net.learning.neural.examples;
using aima.net.util;

namespace aima.net.demo.learning.chapter18
{
    public class BackPropogationDemo
    {
        internal static void Main(params string[] args)
        {
            System.Console.WriteLine(Util.ntimes("*", 100));
            System.Console.WriteLine("\n BackpropagationDemo  - Running BackProp on Iris data Set with 1000 epochs of learning ");
            System.Console.WriteLine(Util.ntimes("*", 100));
            backPropogationDemo();
        }

        internal static void backPropogationDemo()
        {
            try
            {
                DataSet irisDataSet = DataSetFactory.getIrisDataSet();
                INumerizer numerizer = new IrisDataSetNumerizer();
                NeuralNetworkDataSet innds = new IrisNeuralNetworkDataSet();

                innds.CreateExamplesFromDataSet(irisDataSet, numerizer);

                NeuralNetworkConfig config = new NeuralNetworkConfig();
                config.SetConfig(FeedForwardNeuralNetwork.NUMBER_OF_INPUTS, 4);
                config.SetConfig(FeedForwardNeuralNetwork.NUMBER_OF_OUTPUTS, 3);
                config.SetConfig(FeedForwardNeuralNetwork.NUMBER_OF_HIDDEN_NEURONS,
                        6);
                config.SetConfig(FeedForwardNeuralNetwork.LOWER_LIMIT_WEIGHTS, -2.0);
                config.SetConfig(FeedForwardNeuralNetwork.UPPER_LIMIT_WEIGHTS, 2.0);

                FeedForwardNeuralNetwork ffnn = new FeedForwardNeuralNetwork(config);
                ffnn.SetTrainingScheme(new BackPropagationLearning(0.1, 0.9));

                ffnn.TrainOn(innds, 1000);

                innds.RefreshDataset();
                int[] result = ffnn.TestOnDataSet(innds);
                System.Console.WriteLine(result[0] + " right, " + result[1] + " wrong");
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
