namespace aima.net.exceptions
{
    public class Exception : System.Exception
    {
        private readonly string message;
        private readonly Exception innerException;

        public Exception(string message)
            : this(message, null)
        { }

        public Exception(string message, Exception innerException)
            : base(message, innerException)
        {
            this.message = message;
            this.innerException = innerException;
        }

        public string GetMessage()
        {
            return this.message;
        }

        public Exception GetInnerException()
        {
            return this.innerException;
        }
    }
}
