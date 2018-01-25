using aima.net.exceptions;
using aima.net.learning.framework;
using aima.net.learning.neural;
using aima.net.learning.neural.api;
using aima.net.learning.neural.examples;
using aima.net.util;

namespace aima.net.demo.learning.chapter18
{
    public class PerceptronDemo
    {
        static void Main(params string[] args)
        {
            System.Console.WriteLine(Util.ntimes("*", 100));
            System.Console.WriteLine("\n Perceptron Demo - Running Perceptron on Iris data Set with 10 epochs of learning ");
            System.Console.WriteLine(Util.ntimes("*", 100));
            perceptronDemo();
        }

        static void perceptronDemo()
        {
            try
            { 
                DataSet irisDataSet = DataSetFactory.getIrisDataSet();
                INumerizer numerizer = new IrisDataSetNumerizer();
                NeuralNetworkDataSet innds = new IrisNeuralNetworkDataSet();

                innds.CreateExamplesFromDataSet(irisDataSet, numerizer);

                Perceptron perc = new Perceptron(3, 4);

                perc.TrainOn(innds, 10);

                innds.RefreshDataset();
                int[] result = perc.TestOnDataSet(innds);
                System.Console.WriteLine(result[0] + " right, " + result[1] + " wrong");
            }
            catch (Exception e)
            {
                throw e;
            } 
        }
    }
}
