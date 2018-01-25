using aima.net.collections;
using aima.net.collections.api;
using aima.net.util.math;

namespace aima.net.learning.neural
{
    /// <summary> 
    /// Contains sensitivity matrices and related calculations for each layer.
    /// Used for backprop learning
    /// </summary>
    public class LayerSensitivity
    {
        private readonly Layer layer;

        Matrix sensitivityMatrix;

        public LayerSensitivity(Layer layer)
        {
            Matrix weightMatrix = layer.GetWeightMatrix();
            this.sensitivityMatrix = new Matrix(weightMatrix.GetRowDimension(),
                                                weightMatrix.GetColumnDimension());
            this.layer = layer;
        }

        public Matrix GetSensitivityMatrix()
        {
            return sensitivityMatrix;
        }

        public Matrix SensitivityMatrixFromErrorMatrix(Vector errorVector)
        {
            Matrix derivativeMatrix = CreateDerivativeMatrix(layer.GetLastInducedField());
            Matrix calculatedSensitivityMatrix = derivativeMatrix.Times(errorVector)
                                                                 .Times(-2.0);
            sensitivityMatrix = calculatedSensitivityMatrix.Copy();
            return calculatedSensitivityMatrix;
        }

        public Matrix SensitivityMatrixFromSucceedingLayer(LayerSensitivity nextLayerSensitivity)
        {
            Layer nextLayer = nextLayerSensitivity.GetLayer();
            Matrix derivativeMatrix = CreateDerivativeMatrix(layer.GetLastInducedField());
            Matrix weightTranspose = nextLayer.GetWeightMatrix()
                                              .Transpose();
            Matrix calculatedSensitivityMatrix
                = derivativeMatrix.Times(weightTranspose)
                                  .Times(nextLayerSensitivity.GetSensitivityMatrix());
            sensitivityMatrix = calculatedSensitivityMatrix.Copy();
            return sensitivityMatrix;
        }

        public Layer GetLayer()
        {
            return layer;
        }

        private Matrix CreateDerivativeMatrix(Vector lastInducedField)
        {
            ICollection<double> lst = CollectionFactory.CreateQueue<double>();
            for (int i = 0; i < lastInducedField.Size(); ++i)
            {
                lst.Add(layer.GetActivationFunction()
                             .Deriv(lastInducedField.GetValue(i)));
            }
            return Matrix.createDiagonalMatrix(lst);
        }
    }
}
