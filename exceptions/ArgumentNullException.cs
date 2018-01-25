namespace aima.net.exceptions
{
    public class ArgumentNullException : Exception
    {
        public ArgumentNullException()
            : this(string.Empty)
        { }

        public ArgumentNullException(string message)
            : base(message)
        { }

        public ArgumentNullException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
