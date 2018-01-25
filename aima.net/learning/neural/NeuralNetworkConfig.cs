using aima.net.collections;
using aima.net.collections.api;

namespace aima.net.learning.neural
{
    /// <summary>
    /// A holder for config data for neural networks and possibly for other learning systems.
    /// </summary>
    public class NeuralNetworkConfig
    {
        private readonly IMap<string, object> hash;

        public NeuralNetworkConfig(IMap<string, object> hash)
        {
            this.hash = hash;
        }

        public NeuralNetworkConfig()
        {
            this.hash = CollectionFactory.CreateInsertionOrderedMap<string, object>();
        }

        public double GetParameterAsDouble(string key)
        { 
            return (double)hash.Get(key);
        }

        public int GetParameterAsInteger(string key)
        { 
            return (int)hash.Get(key);
        }

        public void SetConfig(string key, double value)
        {
            hash.Put(key, value);
        }

        public void SetConfig(string key, int value)
        {
            hash.Put(key, value);
        }
    } 
}
