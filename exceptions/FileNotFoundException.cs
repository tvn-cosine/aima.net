namespace aima.net.exceptions
{
    public class FileNotFoundException : IOException
    {
        public FileNotFoundException()
            : this(string.Empty)
        { }

        public FileNotFoundException(string message)
            : base(message)
        { }

        public FileNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
