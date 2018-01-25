using System.Collections.Generic;
using System.Text;

namespace aima.net.expressions
{
    public class InfixExpression<T> : List<ExpressionObject>, ICalculate<T>
    {
        public Operand<T> Calculate()
        {
            var sya = new ShuntingYard<T>();
            var pf = sya.ToPostFix(this);

            return pf.Calculate();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var i in this)
            {
                sb.AppendFormat(" {0}", i.ToString());
            }

            return sb.ToString();
        }
    }
}
