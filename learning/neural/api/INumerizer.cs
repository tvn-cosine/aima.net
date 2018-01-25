using aima.net.collections.api;
using aima.net.datastructures;
using aima.net.learning.framework;

namespace aima.net.learning.neural.api
{
    /// <summary>
    /// A Numerizer understands how to convert an example from a particular data set
    /// into a Pair of lists of doubles. The first represents the input
    /// to the neural network, and the second represents the desired output. See
    /// IrisDataSetNumerizer for a concrete example
    /// </summary>
    public interface INumerizer
    {
        Pair<ICollection<double>, ICollection<double>> Numerize(Example e);

        string Denumerize(ICollection<double> outputValue);
    }
}
