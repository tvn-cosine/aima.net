namespace aima.net.exceptions
{
    public class NumberFormatException : Exception
    {
        public NumberFormatException()
            : this(string.Empty)
        { }

        public NumberFormatException(string message)
            : base(message)
        { }

        public NumberFormatException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
