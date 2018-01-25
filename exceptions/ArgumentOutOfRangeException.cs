namespace aima.net.exceptions
{
    public class ArgumentOutOfRangeException : Exception
    {
        public ArgumentOutOfRangeException()
            : this(string.Empty)
        { }

        public ArgumentOutOfRangeException(string message)
            : base(message)
        { }

        public ArgumentOutOfRangeException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
