using System;
using System.Collections.Generic;

namespace aima.net.text.patternmatching.api
{
    public interface IPattern : IEnumerable<char>, 
                                IEquatable<IPattern>
    { }
}
