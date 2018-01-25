using aima.net.api;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.exceptions;
using aima.net.learning.framework.api;

namespace aima.net.learning.framework
{
    public class Example : IStringable, IEquatable
    {
        IMap<string, IAttribute> attributes;

        private IAttribute targetAttribute;

        public Example(IMap<string, IAttribute> attributes,
                       IAttribute targetAttribute)
        {
            this.attributes = attributes;
            this.targetAttribute = targetAttribute;
        }

        public string getAttributeValueAsString(string attributeName)
        {
            return attributes.Get(attributeName).ValueAsString();
        }

        public double getAttributeValueAsDouble(string attributeName)
        {
            IAttribute attribute = attributes.Get(attributeName);
            if (attribute == null || !(attribute is NumericAttribute))
            {
                throw new RuntimeException("cannot return numerical value for non numeric attribute");
            }
            return ((NumericAttribute)attribute).valueAsDouble();
        }

        public override string ToString()
        {
            return attributes.ToString();
        }

        public string targetValue()
        {
            return getAttributeValueAsString(targetAttribute.Name());
        }

        public override bool Equals(object o)
        {
            if (this == o)
            {
                return true;
            }
            if ((o == null) || (this.GetType() != o.GetType()))
            {
                return false;
            }
            Example other = (Example)o;
            return attributes.Equals(other.attributes);
        }

        public override int GetHashCode()
        {
            return attributes.GetHashCode();
        }

        public Example numerize(IMap<string, IMap<string, int>> attrValueToNumber)
        {
            IMap<string, IAttribute> numerizedExampleData = CollectionFactory.CreateInsertionOrderedMap<string, IAttribute>();
            foreach (string key in attributes.GetKeys())
            {
                IAttribute attribute = attributes.Get(key);
                if (attribute is StringAttribute)
                {
                    int correspondingNumber = attrValueToNumber.Get(key).Get(attribute.ValueAsString());
                    NumericAttributeSpecification spec = new NumericAttributeSpecification(key);
                    numerizedExampleData.Put(key, new NumericAttribute(correspondingNumber, spec));
                }
                else
                {// Numeric Attribute
                    numerizedExampleData.Put(key, attribute);
                }
            }
            return new Example(numerizedExampleData, numerizedExampleData.Get(targetAttribute.Name()));
        }
    }
}
