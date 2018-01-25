namespace aima.net.expressions
{
    public class Operand<T> : ExpressionObject
    {
        public Operand(string description, T value)
            : base(description)
        {
            Value = value;
        }

        public virtual T Value { get; }

        public static implicit operator Operand<T>(T input)
        {
            return new Operand<T>("Operand", input);
        }

        public static implicit operator T(Operand<T> input)
        {
            return input.Value;
        }
    }
}
