using aima.net.learning.neural.api;
using aima.net.util.math;
using aima.net.exceptions;

namespace aima.net.learning.neural
{
    public class BackPropagationDeepLearning : INeuralNetworkTrainingScheme
    {
        private readonly double learningRate;
        private readonly double momentum;

        private Layer[] hiddenLayers;
        private Layer outputLayer;

        private LayerSensitivity[] hiddenSensitivities;
        private LayerSensitivity outputSensitivity;
          
        public BackPropagationDeepLearning(double learningRate, double momentum)
        {
            this.learningRate = learningRate;
            this.momentum = momentum;
        }

        public void SetNeuralNetwork(IFunctionApproximator fapp)
        {
            FeedForwardDeepNeuralNetwork ffnn = fapp as FeedForwardDeepNeuralNetwork;
            if (null == ffnn)
            {
                throw new Exception("Only supporting FeedForwardDeepNeuralNetwork at this stage.");
            }

            this.hiddenLayers = new Layer[ffnn.GetNumberOfHiddenLayers()];
            for (int i = 0; i < hiddenLayers.Length; ++i)
            {
                this.hiddenLayers[i] = ffnn.GetHiddenLayer(i);
            }

            this.outputLayer = ffnn.GetOutputLayer();

            this.hiddenSensitivities = new LayerSensitivity[ffnn.GetNumberOfHiddenLayers()];
            for (int i = 0; i < hiddenLayers.Length; ++i)
            {
                this.hiddenSensitivities[i] = new LayerSensitivity(ffnn.GetHiddenLayer(i));
            }

            this.outputSensitivity = new LayerSensitivity(outputLayer);
        }

        public Vector ProcessInput(IFunctionApproximator network, Vector input)
        {
            hiddenLayers[0].FeedForward(input);

            for (int i = 1; i < hiddenLayers.Length; ++i)
            {
                hiddenLayers[i].FeedForward(hiddenLayers[i - 1].GetLastActivationValues());
            }

            outputLayer.FeedForward(hiddenLayers[hiddenLayers.Length - 1].GetLastActivationValues());

            return outputLayer.GetLastActivationValues();
        }

        public void ProcessError(IFunctionApproximator network,
                                 Vector error)
        {
            // TODO calculate total error somewhere
            // create Sensitivity Matrices
            outputSensitivity.SensitivityMatrixFromErrorMatrix(error);

            hiddenSensitivities[hiddenSensitivities.Length - 1].SensitivityMatrixFromSucceedingLayer(outputSensitivity);
            for (int i = hiddenSensitivities.Length - 2; i >= 0; --i)
            {
                hiddenSensitivities[i].SensitivityMatrixFromSucceedingLayer(hiddenSensitivities[i + 1]);
            }

            // calculate weight Updates
            CalculateWeightUpdates(outputSensitivity, hiddenLayers[hiddenLayers.Length - 1].GetLastActivationValues(),
                learningRate, momentum);
            for (int i = hiddenLayers.Length - 1; i > 0; --i)
            {
                CalculateWeightUpdates(hiddenSensitivities[i], hiddenLayers[i - 1].GetLastActivationValues(),
                    learningRate, momentum);
            }

            CalculateWeightUpdates(hiddenSensitivities[0], hiddenLayers[0].GetLastInputValues(), learningRate, momentum);

            // calculate Bias Updates
            CalculateBiasUpdates(outputSensitivity, learningRate, momentum);
            for (int i = hiddenLayers.Length - 1; i >= 0; --i)
            {
                CalculateBiasUpdates(hiddenSensitivities[i], learningRate, momentum);
            }

            // update weightsAndBiases
            outputLayer.UpdateWeights();
            outputLayer.UpdateBiases();

            for (int i = hiddenLayers.Length - 1; i >= 0; --i)
            {
                hiddenLayers[i].UpdateWeights();
                hiddenLayers[i].UpdateBiases();
            } 
        }

        public Matrix CalculateWeightUpdates(LayerSensitivity layerSensitivity,
                                             Vector previousLayerActivationOrInput,
                                             double alpha,
                                             double momentum)
        {
            Layer layer = layerSensitivity.GetLayer();
            Matrix activationTranspose = previousLayerActivationOrInput.Transpose();
            Matrix momentumLessUpdate
                = layerSensitivity.GetSensitivityMatrix()
                                  .Times(activationTranspose)
                                  .Times(alpha)
                                  .Times(-1.0);
            Matrix updateWithMomentum
                = layer.GetLastWeightUpdateMatrix()
                       .Times(momentum)
                       .Plus(momentumLessUpdate.Times(1.0 - momentum));
            layer.AcceptNewWeightUpdate(updateWithMomentum.Copy());

            return updateWithMomentum;
        }

        public static Matrix CalculateWeightUpdates(LayerSensitivity layerSensitivity,
                                                    Vector previousLayerActivationOrInput,
                                                    double alpha)
        {
            Layer layer = layerSensitivity.GetLayer();
            Matrix activationTranspose = previousLayerActivationOrInput.Transpose();
            Matrix weightUpdateMatrix
                = layerSensitivity.GetSensitivityMatrix()
                                  .Times(activationTranspose)
                                  .Times(alpha)
                                  .Times(-1.0);
            layer.AcceptNewWeightUpdate(weightUpdateMatrix.Copy());

            return weightUpdateMatrix;
        }

        public Vector CalculateBiasUpdates(LayerSensitivity layerSensitivity,
                                           double alpha,
                                           double momentum)
        {
            Layer layer = layerSensitivity.GetLayer();
            Matrix biasUpdateMatrixWithoutMomentum = layerSensitivity.GetSensitivityMatrix().Times(alpha).Times(-1.0);

            Matrix biasUpdateMatrixWithMomentum
                = layer.GetLastBiasUpdateVector()
                       .Times(momentum)
                       .Plus(biasUpdateMatrixWithoutMomentum
                       .Times(1.0 - momentum));
            Vector result = new Vector(biasUpdateMatrixWithMomentum.GetRowDimension());
            for (int i = 0; i < biasUpdateMatrixWithMomentum.GetRowDimension(); ++i)
            {
                result.SetValue(i, biasUpdateMatrixWithMomentum.Get(i, 0));
            }
            layer.AcceptNewBiasUpdate(result.CopyVector());
            return result;
        }

        public static Vector CalculateBiasUpdates(LayerSensitivity layerSensitivity,
                                                  double alpha)
        {
            Layer layer = layerSensitivity.GetLayer();
            Matrix biasUpdateMatrix
                = layerSensitivity.GetSensitivityMatrix()
                                  .Times(alpha)
                                  .Times(-1.0);

            Vector result = new Vector(biasUpdateMatrix.GetRowDimension());
            for (int i = 0; i < biasUpdateMatrix.GetRowDimension(); ++i)
            {
                result.SetValue(i, biasUpdateMatrix.Get(i, 0));
            }
            layer.AcceptNewBiasUpdate(result.CopyVector());
            return result;
        }
    }
}
