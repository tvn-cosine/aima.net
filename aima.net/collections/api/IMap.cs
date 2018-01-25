using aima.net.api;
using aima.net.datastructures;

namespace aima.net.collections.api
{
    public interface IMap<KEY, VALUE> : ICollection<KeyValuePair<KEY, VALUE>>, 
        IEnumerable<KeyValuePair<KEY, VALUE>>, IHashable, 
        IStringable 
    {
        VALUE Get(KEY key);
        ISet<KEY> GetKeys();
        ICollection<VALUE> GetValues();
       
        void Put(KEY key, VALUE value);
        void PutAll(IMap<KEY, VALUE> map);
        bool ContainsKey(KEY key); 
        bool Remove(KEY key);  
    }
}
