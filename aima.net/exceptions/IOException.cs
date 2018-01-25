namespace aima.net.exceptions
{
    public class IOException : Exception
    {
        public IOException()
            : this(string.Empty)
        { }

        public IOException(string message)
            : base(message)
        { }

        public IOException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
