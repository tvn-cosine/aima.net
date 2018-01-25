using aima.net.api;

namespace aima.net.text.api
{
    public interface IRegularExpression : IStringable
    {
        bool IsMatch(string input);
        bool IsMatch(string input, int startat);
        string[] Matches(string input);
        string Replace(string input, string replacement);
        string Replace(string input, string replacement, int count);
        string Replace(string input, string replacement, int count, int startat);
        string[] Split(string input);
        string[] Split(string input, int count);
        string[] Split(string input, int count, int startat);  
    }
}