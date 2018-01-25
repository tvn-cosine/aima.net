using aima.net.collections;
using aima.net.collections.api;
using aima.net.probability.hmm;
using aima.net.probability.hmm.api; 
using aima.net.util.math;

namespace aima.net.probability.example
{
    public class HMMExampleFactory
    { 
        public static IHiddenMarkovModel getUmbrellaWorldModel()
        {
            Matrix transitionModel = new Matrix(new double[,] { { 0.7, 0.3 }, { 0.3, 0.7 } });
            IMap<object, Matrix> sensorModel = CollectionFactory.CreateInsertionOrderedMap<object, Matrix>();
            sensorModel.Put(true, new Matrix(new double[,] { { 0.9, 0.0 }, { 0.0, 0.2 } }));
            sensorModel.Put(false, new Matrix(new double[,] { { 0.1, 0.0 }, { 0.0, 0.8 } }));
            Matrix prior = new Matrix(new double[] { 0.5, 0.5 }, 2);
            return new HiddenMarkovModel(ExampleRV.RAIN_t_RV, transitionModel, sensorModel, prior);
        }
    }
}
