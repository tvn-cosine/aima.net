using System.Collections.Generic;
using aima.net.text.patternmatching;

namespace aima.net.text.patternmatching.api
{
    public interface IPatternMatchingMachine
    {
        ICollection<IPattern> Match(IEnumerable<char> input); 
        void Match(IEnumerable<char> input, PatternFoundDelegate patternMatchFound);
    }
}
