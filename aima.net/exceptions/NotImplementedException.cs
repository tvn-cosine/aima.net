namespace aima.net.exceptions
{
    public class NotImplementedException : Exception
    {
        public NotImplementedException()
            : this(string.Empty)
        { }

        public NotImplementedException(string message)
            : base(message)
        { }

        public NotImplementedException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
