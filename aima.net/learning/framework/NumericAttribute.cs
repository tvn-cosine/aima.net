using aima.net.learning.framework.api;

namespace aima.net.learning.framework
{
    public class NumericAttribute : IAttribute
    {
        double value;

        private NumericAttributeSpecification spec;

        public NumericAttribute(double rawValue, NumericAttributeSpecification spec)
        {
            this.value = rawValue;
            this.spec = spec;
        }

        public string ValueAsString()
        {
            return value.ToString();
        }

        public string Name()
        {
            return spec.GetAttributeName().Trim();
        }

        public double valueAsDouble()
        {
            return value;
        }

        public override string ToString()
        {
            return value.ToString("N5");
        }
    }
}
