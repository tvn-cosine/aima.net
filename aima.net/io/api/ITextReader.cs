using aima.net.api;

namespace aima.net.io.api
{
    public interface ITextReader : IDisposable
    {
        void Close();
        int Peek();
        int Read();
        string ReadLine();
        string ReadToEnd();
    }
}
