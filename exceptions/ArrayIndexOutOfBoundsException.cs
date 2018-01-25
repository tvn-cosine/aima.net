namespace aima.net.exceptions
{
    public class ArrayIndexOutOfBoundsException : Exception
    {
        public ArrayIndexOutOfBoundsException()
            : this(string.Empty)
        { }

        public ArrayIndexOutOfBoundsException(string message)
            : base(message)
        { }

        public ArrayIndexOutOfBoundsException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
