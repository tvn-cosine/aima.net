namespace aima.net.exceptions
{
    public class IllegalStateException : Exception
    {
        public IllegalStateException()
            : this(string.Empty)
        { }

        public IllegalStateException(string message)
            : base(message)
        { }

        public IllegalStateException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
