using aima.net.io.api;

namespace aima.net.io
{
    public class TextReader : ITextReader
    {
        public System.IO.TextReader backingReader;

        public TextReader(System.IO.TextReader reader)
        {
            this.backingReader = reader;
        }

        public virtual void Close()
        {
            backingReader.Close();
        }

        public virtual void Dispose()
        {
            backingReader.Dispose();
        }

        public virtual int Peek()
        {
            return backingReader.Peek();
        }

        public virtual int Read()
        {
            return backingReader.Read();
        }

        public virtual string ReadLine()
        {
            return backingReader.ReadLine();
        }

        public virtual string ReadToEnd()
        {
            return backingReader.ReadToEnd();
        }
    }
}
