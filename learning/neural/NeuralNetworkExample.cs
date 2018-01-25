using aima.net.collections;
using aima.net.collections.api;
using aima.net.util.math;

namespace aima.net.learning.neural
{
    public class NeuralNetworkExample
    {
        private readonly ICollection<double> normalizedInput, normalizedTarget;

        public NeuralNetworkExample(ICollection<double> normalizedInput, ICollection<double> normalizedTarget)
        {
            this.normalizedInput = normalizedInput;
            this.normalizedTarget = normalizedTarget;
        }

        public NeuralNetworkExample CopyExample()
        {
            ICollection<double> newInput = CollectionFactory.CreateQueue<double>();
            ICollection<double> newTarget = CollectionFactory.CreateQueue<double>();
            foreach (double d in normalizedInput)
            {
                newInput.Add(d);
            }
            foreach (double d in normalizedTarget)
            {
                newTarget.Add(d);
            }
            return new NeuralNetworkExample(newInput, newTarget);
        }

        public Vector GetInput()
        {
            Vector v = new Vector(normalizedInput);
            return v; 
        }

        public Vector GetTarget()
        {
            Vector v = new Vector(normalizedTarget);
            return v; 
        }

        public bool IsCorrect(Vector prediction)
        {
            // compares the index having greatest value in target to indec having greatest value in prediction. 
            // If identical, correct
            return GetTarget().IndexHavingMaxValue() == prediction.IndexHavingMaxValue();
        }
    } 
}
