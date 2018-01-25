using System;
using System.Collections.Generic;

namespace aima.net.expressions
{
    public interface IFunction
    {
        Action<Stack<ExpressionObject>> F { get; }
    }
}
