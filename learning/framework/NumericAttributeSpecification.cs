using aima.net.text;
using aima.net.learning.framework.api;
using aima.net.text.api;

namespace aima.net.learning.framework
{ 
    public class NumericAttributeSpecification : IAttributeSpecification
    {
        // a simple attribute representing a number represented as a double .
        private string name;

        public NumericAttributeSpecification(string name)
        {
            this.name = name;
        }

        public bool IsValid(string s)
        { 
            return TextFactory.IsValidDouble(s);
        }

        public string GetAttributeName()
        {
            return name;
        }

        public IAttribute CreateAttribute(string rawValue)
        {
            return new NumericAttribute(TextFactory.ParseDouble(rawValue), this);
        }

        public override string ToString()
        {
            IStringBuilder sb = TextFactory.CreateStringBuilder();
            sb.Append('[');
            sb.Append(name); 
            sb.Append("]");
            return sb.ToString();
        }
    }
}
