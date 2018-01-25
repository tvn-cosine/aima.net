namespace aima.net.exceptions
{
    public class NotSupportedException : Exception
    {
        public NotSupportedException()
            : this(string.Empty)
        { }

        public NotSupportedException(string message)
            : base(message)
        { }

        public NotSupportedException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
