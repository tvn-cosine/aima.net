using System.Collections.Generic;
using System.Text;

namespace aima.net.expressions
{
    public class PostFixExpression<T> : Queue<ExpressionObject>, ICalculate<T>
    {
        public Operand<T> Calculate()
        {
            var calculationStack = new Stack<ExpressionObject>();
            foreach (var item in this)
            {
                if (item is Operand<T>)
                {
                    calculationStack.Push(item);
                }
                else if (item is Operator<T> || item is Function)
                {
                    var o = item as IFunction;
                    o.F(calculationStack);
                }
            }
            return calculationStack.Pop() as Operand<T>;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var i in this)
            {
                sb.AppendFormat("{0} ", i.ToString());
            }

            return sb.ToString();
        }
    }
}
