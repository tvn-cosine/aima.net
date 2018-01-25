using System.Collections.Generic;
using aima.net.text.patternmatching.api;

namespace aima.net.text.patternmatching
{
    public delegate void PatternFoundDelegate(IPatternMatchingMachine sender, ISet<IPattern> patternsFound, uint position);
}
