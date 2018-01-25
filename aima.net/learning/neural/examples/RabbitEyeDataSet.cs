using aima.net.collections; 

namespace aima.net.learning.neural.examples
{
    public class RabbitEyeDataSet : NeuralNetworkDataSet
    {
        public override void SetTargetColumns()
        {
            // assumed that data from file has been pre processed
            // TODO this should be
            // somewhere else,in the
            // super class.
            // Type != class Aargh! I want more
            // powerful type systems
            targetColumnNumbers = CollectionFactory.CreateQueue<int>();

            targetColumnNumbers.Add(1); // using zero based indexing
        }
    }
}
