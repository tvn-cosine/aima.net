namespace aima.net.exceptions
{
    public class IllegalArgumentException : Exception
    {
        public IllegalArgumentException()
            : this(string.Empty)
        { }

        public IllegalArgumentException(string message)
            : base(message)
        { }

        public IllegalArgumentException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
