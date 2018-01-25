namespace aima.net.api
{ 
    public interface IEquatable<T>  
    {
        bool Equals(T other);
    }

    public interface IEquatable : IEquatable<object>, IHashable
    { }
}
