using System;
using System.Collections.Generic;

namespace aima.net.expressions
{
    public class ShuntingYard<T>
    {
        internal const string noLeftParenthesisFound = "Expected Left parenthesis not found.";
        internal const string mismatchedParenthesis = "Mismatched parenthesis.";

        public PostFixExpression<T> ToPostFix(InfixExpression<T> expression)
        {
            var outputQueue = new PostFixExpression<T>();
            var operatorStack = new Stack<ExpressionObject>();

            foreach (var token in expression)
            {
                if (token is Operand<T>)
                {
                    outputQueue.Enqueue(token);
                }
                else if (token is Function)
                {
                    operatorStack.Push(token);
                }
                else if (token == ExpressionObject.FunctionSeperator)
                {
                    popUntilLeftParenthesisFound(operatorStack, outputQueue);
                }
                else if (token is Operator<T>)
                {
                    if (operatorStack.Count == 0)
                        operatorStack.Push(token);
                    else
                    {
                        var o1 = token as Operator<T>;
                        while (operatorStack.Count > 0 &&
                               operatorStack.Peek() is Operator<T> &&
                               ((o1.Associativity == Associativity.Left && o1.Presedence <= (operatorStack.Peek() as Operator<T>).Presedence) ||
                                (o1.Associativity == Associativity.Right && o1.Presedence < (operatorStack.Peek() as Operator<T>).Presedence)))
                        {
                            outputQueue.Enqueue(operatorStack.Pop());
                        }
                        operatorStack.Push(o1);
                    }
                }
                else if (token == ExpressionObject.LeftParenthesis)
                {
                    operatorStack.Push(token);
                }
                else if (token == ExpressionObject.RightParenthesis)
                {
                    popUntilLeftParenthesisFound(operatorStack, outputQueue);
                    operatorStack.Pop();
                    if (operatorStack.Peek() is Function)
                    {
                        outputQueue.Enqueue(operatorStack.Pop());
                    }
                }
            }

            while (operatorStack.Count > 0)
            {
                if (operatorStack.Peek() == ExpressionObject.LeftParenthesis)
                    throw new ArgumentException(mismatchedParenthesis);
                else
                    outputQueue.Enqueue(operatorStack.Pop());
            }

            return outputQueue;
        }

        private void popUntilLeftParenthesisFound(Stack<ExpressionObject> operatorStack, Queue<ExpressionObject> outputQueue)
        {
            if (operatorStack.Count == 0)
                throw new ArgumentException(mismatchedParenthesis, new ArgumentException(noLeftParenthesisFound));

            while (operatorStack.Peek() != ExpressionObject.LeftParenthesis)
            {
                outputQueue.Enqueue(operatorStack.Pop());

                if (operatorStack.Count == 0)
                    throw new ArgumentException(mismatchedParenthesis, new ArgumentException(noLeftParenthesisFound));
            }
        }
    }
}
