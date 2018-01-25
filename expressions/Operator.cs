using System;
using System.Collections.Generic;

namespace aima.net.expressions
{
    public class Operator<T> : ExpressionObject, IFunction
    {
        public Operator(string description, byte presedence, Associativity associativity, Action<Stack<ExpressionObject>> f)
            : base(description)
        {
            Presedence = presedence;
            Associativity = associativity;
            F = f;
        }

        public readonly byte Presedence;
        public readonly Associativity Associativity;
        public Action<Stack<ExpressionObject>> F { get; }
    }

    public class BooleanOperator : Operator<bool>
    {
        private BooleanOperator(string description, byte presedence, Associativity associativity, Action<Stack<ExpressionObject>> f)
            : base(description, presedence, associativity, f)
        { }
         
        public static readonly BooleanOperator Not = new BooleanOperator("!", 3, Associativity.Right, not);
        public static readonly BooleanOperator And = new BooleanOperator("&&", 2, Associativity.Left, and);
        public static readonly BooleanOperator Or = new BooleanOperator("||", 1, Associativity.Left, or);

        private static Operand<bool> popFromAnswerStack(Stack<ExpressionObject> answerStack)
        {
            var x = answerStack.Pop() as Operand<bool>;
            if (x == null)
                throw new ArgumentNullException("Could not cast to Operand<bool>.");

            return x;
        }

        private static void not(Stack<ExpressionObject> answerStack)
        {
            var x = popFromAnswerStack(answerStack);

            Operand<bool> answer = (!x);
            answerStack.Push(answer);
        }
         
        private static void and(Stack<ExpressionObject> answerStack)
        {
            var x = popFromAnswerStack(answerStack);
            var y = popFromAnswerStack(answerStack);

            Operand<bool> answer = (y && x);
            answerStack.Push(answer);
        }
         
        private static void or(Stack<ExpressionObject> answerStack)
        {
            var x = popFromAnswerStack(answerStack);
            var y = popFromAnswerStack(answerStack);

            Operand<bool> answer = (y || x);
            answerStack.Push(answer);
        }
    }
}
