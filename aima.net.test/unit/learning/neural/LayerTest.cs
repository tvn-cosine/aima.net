using Microsoft.VisualStudio.TestTools.UnitTesting;
using aima.net.learning.neural;
using aima.net.util.math;

namespace aima.net.test.unit.learning.neural
{
    [TestClass]
    public class LayerTest
    {

        [TestMethod]
        public void testFeedForward()
        {
            // example 11.14 of Neural Network Design by Hagan, Demuth and Beale
            // lots of tedious tests necessary to ensure nn is fundamentally correct
            Matrix weightMatrix1 = new Matrix(2, 1);
            weightMatrix1.Set(0, 0, -0.27);
            weightMatrix1.Set(1, 0, -0.41);

            Vector biasVector1 = new Vector(2);
            biasVector1.SetValue(0, -0.48);
            biasVector1.SetValue(1, -0.13);

            Layer layer1 = new Layer(weightMatrix1, biasVector1,
                    new LogSigActivationFunction());

            Vector inputVector1 = new Vector(1);
            inputVector1.SetValue(0, 1);

            Vector expected = new Vector(2);
            expected.SetValue(0, 0.321);
            expected.SetValue(1, 0.368);

            Vector result1 = layer1.FeedForward(inputVector1);
            Assert.AreEqual(expected.GetValue(0), result1.GetValue(0), 0.001);
            Assert.AreEqual(expected.GetValue(1), result1.GetValue(1), 0.001);

            Matrix weightMatrix2 = new Matrix(1, 2);
            weightMatrix2.Set(0, 0, 0.09);
            weightMatrix2.Set(0, 1, -0.17);

            Vector biasVector2 = new Vector(1);
            biasVector2.SetValue(0, 0.48);

            Layer layer2 = new Layer(weightMatrix2, biasVector2,
                    new PureLinearActivationFunction());
            Vector inputVector2 = layer1.GetLastActivationValues();
            Vector result2 = layer2.FeedForward(inputVector2);
            Assert.AreEqual(0.446, result2.GetValue(0), 0.001);
        }

        [TestMethod]
        public void testSensitivityMatrixCalculationFromErrorVector()
        {
            Matrix weightMatrix1 = new Matrix(2, 1);
            weightMatrix1.Set(0, 0, -0.27);
            weightMatrix1.Set(1, 0, -0.41);

            Vector biasVector1 = new Vector(2);
            biasVector1.SetValue(0, -0.48);
            biasVector1.SetValue(1, -0.13);

            Layer layer1 = new Layer(weightMatrix1, biasVector1,
                    new LogSigActivationFunction());

            Vector inputVector1 = new Vector(1);
            inputVector1.SetValue(0, 1);

            layer1.FeedForward(inputVector1);

            Matrix weightMatrix2 = new Matrix(1, 2);
            weightMatrix2.Set(0, 0, 0.09);
            weightMatrix2.Set(0, 1, -0.17);

            Vector biasVector2 = new Vector(1);
            biasVector2.SetValue(0, 0.48);

            Layer layer2 = new Layer(weightMatrix2, biasVector2,
                    new PureLinearActivationFunction());
            Vector inputVector2 = layer1.GetLastActivationValues();
            layer2.FeedForward(inputVector2);

            Vector errorVector = new Vector(1);
            errorVector.SetValue(0, 1.261);
            LayerSensitivity layer2Sensitivity = new LayerSensitivity(layer2);
            layer2Sensitivity.SensitivityMatrixFromErrorMatrix(errorVector);

            Matrix sensitivityMatrix = layer2Sensitivity.GetSensitivityMatrix();
            Assert.AreEqual(-2.522, sensitivityMatrix.Get(0, 0), 0.0001);
        }

        [TestMethod]
        public void testSensitivityMatrixCalculationFromSucceedingLayer()
        {
            Matrix weightMatrix1 = new Matrix(2, 1);
            weightMatrix1.Set(0, 0, -0.27);
            weightMatrix1.Set(1, 0, -0.41);

            Vector biasVector1 = new Vector(2);
            biasVector1.SetValue(0, -0.48);
            biasVector1.SetValue(1, -0.13);

            Layer layer1 = new Layer(weightMatrix1, biasVector1,
                    new LogSigActivationFunction());
            LayerSensitivity layer1Sensitivity = new LayerSensitivity(layer1);

            Vector inputVector1 = new Vector(1);
            inputVector1.SetValue(0, 1);

            layer1.FeedForward(inputVector1);

            Matrix weightMatrix2 = new Matrix(1, 2);
            weightMatrix2.Set(0, 0, 0.09);
            weightMatrix2.Set(0, 1, -0.17);

            Vector biasVector2 = new Vector(1);
            biasVector2.SetValue(0, 0.48);

            Layer layer2 = new Layer(weightMatrix2, biasVector2,
                    new PureLinearActivationFunction());
            Vector inputVector2 = layer1.GetLastActivationValues();
            layer2.FeedForward(inputVector2);

            Vector errorVector = new Vector(1);
            errorVector.SetValue(0, 1.261);
            LayerSensitivity layer2Sensitivity = new LayerSensitivity(layer2);
            layer2Sensitivity.SensitivityMatrixFromErrorMatrix(errorVector);

            layer1Sensitivity
                    .SensitivityMatrixFromSucceedingLayer(layer2Sensitivity);
            Matrix sensitivityMatrix = layer1Sensitivity.GetSensitivityMatrix();

            Assert.AreEqual(2, sensitivityMatrix.GetRowDimension());
            Assert.AreEqual(1, sensitivityMatrix.GetColumnDimension());
            Assert.AreEqual(-0.0495, sensitivityMatrix.Get(0, 0), 0.001);
            Assert.AreEqual(0.0997, sensitivityMatrix.Get(1, 0), 0.001);
        }

        [TestMethod]
        public void testWeightUpdateMatrixesFormedCorrectly()
        {
            Matrix weightMatrix1 = new Matrix(2, 1);
            weightMatrix1.Set(0, 0, -0.27);
            weightMatrix1.Set(1, 0, -0.41);

            Vector biasVector1 = new Vector(2);
            biasVector1.SetValue(0, -0.48);
            biasVector1.SetValue(1, -0.13);

            Layer layer1 = new Layer(weightMatrix1, biasVector1,
                    new LogSigActivationFunction());
            LayerSensitivity layer1Sensitivity = new LayerSensitivity(layer1);

            Vector inputVector1 = new Vector(1);
            inputVector1.SetValue(0, 1);

            layer1.FeedForward(inputVector1);

            Matrix weightMatrix2 = new Matrix(1, 2);
            weightMatrix2.Set(0, 0, 0.09);
            weightMatrix2.Set(0, 1, -0.17);

            Vector biasVector2 = new Vector(1);
            biasVector2.SetValue(0, 0.48);

            Layer layer2 = new Layer(weightMatrix2, biasVector2,
                    new PureLinearActivationFunction());
            Vector inputVector2 = layer1.GetLastActivationValues();
            layer2.FeedForward(inputVector2);

            Vector errorVector = new Vector(1);
            errorVector.SetValue(0, 1.261);
            LayerSensitivity layer2Sensitivity = new LayerSensitivity(layer2);
            layer2Sensitivity.SensitivityMatrixFromErrorMatrix(errorVector);

            layer1Sensitivity
                    .SensitivityMatrixFromSucceedingLayer(layer2Sensitivity);

            Matrix weightUpdateMatrix2 = BackPropagationLearning.CalculateWeightUpdates(
                    layer2Sensitivity, layer1.GetLastActivationValues(), 0.1);
            Assert.AreEqual(0.0809, weightUpdateMatrix2.Get(0, 0), 0.001);
            Assert.AreEqual(0.0928, weightUpdateMatrix2.Get(0, 1), 0.001);

            Matrix lastWeightUpdateMatrix2 = layer2.GetLastWeightUpdateMatrix();
            Assert.AreEqual(0.0809, lastWeightUpdateMatrix2.Get(0, 0), 0.001);
            Assert.AreEqual(0.0928, lastWeightUpdateMatrix2.Get(0, 1), 0.001);

            Matrix penultimateWeightUpdatematrix2 = layer2
                    .GetPenultimateWeightUpdateMatrix();
            Assert.AreEqual(0.0, penultimateWeightUpdatematrix2.Get(0, 0),
                    0.001);
            Assert.AreEqual(0.0, penultimateWeightUpdatematrix2.Get(0, 1),
                    0.001);

            Matrix weightUpdateMatrix1 = BackPropagationLearning.CalculateWeightUpdates(
                    layer1Sensitivity, inputVector1, 0.1);
            Assert.AreEqual(0.0049, weightUpdateMatrix1.Get(0, 0), 0.001);
            Assert.AreEqual(-0.00997, weightUpdateMatrix1.Get(1, 0), 0.001);

            Matrix lastWeightUpdateMatrix1 = layer1.GetLastWeightUpdateMatrix();
            Assert.AreEqual(0.0049, lastWeightUpdateMatrix1.Get(0, 0), 0.001);
            Assert.AreEqual(-0.00997, lastWeightUpdateMatrix1.Get(1, 0), 0.001);
            Matrix penultimateWeightUpdatematrix1 = layer1
                    .GetPenultimateWeightUpdateMatrix();
            Assert.AreEqual(0.0, penultimateWeightUpdatematrix1.Get(0, 0),
                    0.001);
            Assert.AreEqual(0.0, penultimateWeightUpdatematrix1.Get(1, 0),
                    0.001);
        }

        [TestMethod]
        public void testBiasUpdateMatrixesFormedCorrectly()
        {
            Matrix weightMatrix1 = new Matrix(2, 1);
            weightMatrix1.Set(0, 0, -0.27);
            weightMatrix1.Set(1, 0, -0.41);

            Vector biasVector1 = new Vector(2);
            biasVector1.SetValue(0, -0.48);
            biasVector1.SetValue(1, -0.13);

            Layer layer1 = new Layer(weightMatrix1, biasVector1,
                    new LogSigActivationFunction());
            LayerSensitivity layer1Sensitivity = new LayerSensitivity(layer1);

            Vector inputVector1 = new Vector(1);
            inputVector1.SetValue(0, 1);

            layer1.FeedForward(inputVector1);

            Matrix weightMatrix2 = new Matrix(1, 2);
            weightMatrix2.Set(0, 0, 0.09);
            weightMatrix2.Set(0, 1, -0.17);

            Vector biasVector2 = new Vector(1);
            biasVector2.SetValue(0, 0.48);

            Layer layer2 = new Layer(weightMatrix2, biasVector2,
                    new PureLinearActivationFunction());
            LayerSensitivity layer2Sensitivity = new LayerSensitivity(layer2);
            Vector inputVector2 = layer1.GetLastActivationValues();
            layer2.FeedForward(inputVector2);

            Vector errorVector = new Vector(1);
            errorVector.SetValue(0, 1.261);
            layer2Sensitivity.SensitivityMatrixFromErrorMatrix(errorVector);

            layer1Sensitivity
                    .SensitivityMatrixFromSucceedingLayer(layer2Sensitivity);

            Vector biasUpdateVector2 = BackPropagationLearning.CalculateBiasUpdates(
                    layer2Sensitivity, 0.1);
            Assert.AreEqual(0.2522, biasUpdateVector2.GetValue(0), 0.001);

            Vector lastBiasUpdateVector2 = layer2.GetLastBiasUpdateVector();
            Assert.AreEqual(0.2522, lastBiasUpdateVector2.GetValue(0), 0.001);

            Vector penultimateBiasUpdateVector2 = layer2
                    .GetPenultimateBiasUpdateVector();
            Assert.AreEqual(0.0, penultimateBiasUpdateVector2.GetValue(0),
                    0.001);

            Vector biasUpdateVector1 = BackPropagationLearning.CalculateBiasUpdates(
                    layer1Sensitivity, 0.1);
            Assert.AreEqual(0.00495, biasUpdateVector1.GetValue(0), 0.001);
            Assert.AreEqual(-0.00997, biasUpdateVector1.GetValue(1), 0.001);

            Vector lastBiasUpdateVector1 = layer1.GetLastBiasUpdateVector();

            Assert.AreEqual(0.00495, lastBiasUpdateVector1.GetValue(0), 0.001);
            Assert.AreEqual(-0.00997, lastBiasUpdateVector1.GetValue(1), 0.001);

            Vector penultimateBiasUpdateVector1 = layer1
                    .GetPenultimateBiasUpdateVector();
            Assert.AreEqual(0.0, penultimateBiasUpdateVector1.GetValue(0),
                    0.001);
            Assert.AreEqual(0.0, penultimateBiasUpdateVector1.GetValue(1),
                    0.001);
        }

        [TestMethod]
        public void testWeightsAndBiasesUpdatedCorrectly()
        {
            Matrix weightMatrix1 = new Matrix(2, 1);
            weightMatrix1.Set(0, 0, -0.27);
            weightMatrix1.Set(1, 0, -0.41);

            Vector biasVector1 = new Vector(2);
            biasVector1.SetValue(0, -0.48);
            biasVector1.SetValue(1, -0.13);

            Layer layer1 = new Layer(weightMatrix1, biasVector1,
                    new LogSigActivationFunction());
            LayerSensitivity layer1Sensitivity = new LayerSensitivity(layer1);

            Vector inputVector1 = new Vector(1);
            inputVector1.SetValue(0, 1);

            layer1.FeedForward(inputVector1);

            Matrix weightMatrix2 = new Matrix(1, 2);
            weightMatrix2.Set(0, 0, 0.09);
            weightMatrix2.Set(0, 1, -0.17);

            Vector biasVector2 = new Vector(1);
            biasVector2.SetValue(0, 0.48);

            Layer layer2 = new Layer(weightMatrix2, biasVector2,
                    new PureLinearActivationFunction());
            Vector inputVector2 = layer1.GetLastActivationValues();
            layer2.FeedForward(inputVector2);

            Vector errorVector = new Vector(1);
            errorVector.SetValue(0, 1.261);
            LayerSensitivity layer2Sensitivity = new LayerSensitivity(layer2);
            layer2Sensitivity.SensitivityMatrixFromErrorMatrix(errorVector);

            layer1Sensitivity
                    .SensitivityMatrixFromSucceedingLayer(layer2Sensitivity);

            BackPropagationLearning.CalculateWeightUpdates(layer2Sensitivity,
                    layer1.GetLastActivationValues(), 0.1);

            BackPropagationLearning.CalculateBiasUpdates(layer2Sensitivity, 0.1);

            BackPropagationLearning.CalculateWeightUpdates(layer1Sensitivity,
                    inputVector1, 0.1);

            BackPropagationLearning.CalculateBiasUpdates(layer1Sensitivity, 0.1);

            layer2.UpdateWeights();
            Matrix newWeightMatrix2 = layer2.GetWeightMatrix();
            Assert.AreEqual(0.171, newWeightMatrix2.Get(0, 0), 0.001);
            Assert.AreEqual(-0.0772, newWeightMatrix2.Get(0, 1), 0.001);

            layer2.UpdateBiases();
            Vector newBiasVector2 = layer2.GetBiasVector();
            Assert.AreEqual(0.7322, newBiasVector2.GetValue(0), 0.00001);

            layer1.UpdateWeights();
            Matrix newWeightMatrix1 = layer1.GetWeightMatrix();

            Assert.AreEqual(-0.265, newWeightMatrix1.Get(0, 0), 0.001);
            Assert.AreEqual(-0.419, newWeightMatrix1.Get(1, 0), 0.001);

            layer1.UpdateBiases();
            Vector newBiasVector1 = layer1.GetBiasVector();

            Assert.AreEqual(-0.475, newBiasVector1.GetValue(0), 0.001);
            Assert.AreEqual(-0.139, newBiasVector1.GetValue(1), 0.001);
        }
    }

}
