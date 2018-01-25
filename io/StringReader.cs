using aima.net.io.api;

namespace aima.net.io
{
    public class StringReader : TextReader, IStringReader
    {
        public StringReader(string input)
            : base(new System.IO.StringReader(input))
        {

        }
    }
}
