namespace aima.net.exceptions
{
    public class IndexOutOfRangeException : Exception
    {
        public IndexOutOfRangeException()
            : this(string.Empty)
        { }

        public IndexOutOfRangeException(string message)
            : base(message)
        { }

        public IndexOutOfRangeException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
