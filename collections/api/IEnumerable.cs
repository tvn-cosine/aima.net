namespace aima.net.collections.api
{
    public interface IEnumerable<T>
    {
        IEnumerator<T> GetEnumerator();
    }
}
