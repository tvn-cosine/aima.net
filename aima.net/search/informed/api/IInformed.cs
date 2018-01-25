using aima.net.search.framework; 
using aima.net.util.api;

namespace aima.net.search.informed.api
{
    /// <summary>
    /// Search algorithms which make use of heuristics to guide the search are expected to implement this interface.
    /// </summary>
    /// <typeparam name="S"></typeparam>
    /// <typeparam name="A"></typeparam>
    public interface IInformed<S, A>
    {
        void setHeuristicFunction(IToDoubleFunction<Node<S, A>> h);
    } 
}
