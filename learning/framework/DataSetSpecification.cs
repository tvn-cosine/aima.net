using aima.net.collections;
using aima.net.collections.api;
using aima.net.exceptions;
using aima.net.learning.framework.api;

namespace aima.net.learning.framework
{ 
    public class DataSetSpecification
    {
        ICollection<IAttributeSpecification> attributeSpecifications;

        private string targetAttribute;

        public DataSetSpecification()
        {
            this.attributeSpecifications = CollectionFactory.CreateQueue<IAttributeSpecification>();
        }

        public virtual bool isValid(ICollection<string> uncheckedAttributes)
        {
            if (attributeSpecifications.Size() != uncheckedAttributes.Size())
            {
                throw new RuntimeException("size mismatch specsize = "
                        + attributeSpecifications.Size() + " attrbutes size = "
                        + uncheckedAttributes.Size());
            }

            for (int i = 0; i < attributeSpecifications.Size(); ++i)
            {
                if (!(attributeSpecifications.Get(i).IsValid(uncheckedAttributes.Get(i))))
                {
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// Returns the targetAttribute.
        /// </summary>
        /// <returns>Returns the targetAttribute.</returns>
        public virtual string getTarget()
        {
            return targetAttribute;
        }

        public virtual ICollection<string> getPossibleAttributeValues(string attributeName)
        {
            foreach (IAttributeSpecification _as in attributeSpecifications)
            {
                if (_as.GetAttributeName().Equals(attributeName))
                {
                    return ((StringAttributeSpecification)_as).possibleAttributeValues();
                }
            }
            throw new RuntimeException("No such attribute" + attributeName);
        }

        public virtual ICollection<string> getAttributeNames()
        {
            ICollection<string> names = CollectionFactory.CreateQueue<string>();
            foreach (IAttributeSpecification _as in attributeSpecifications)
            {
                names.Add(_as.GetAttributeName());
            }
            return names;
        }

        public virtual void defineStringAttribute(string name, string[] attributeValues)
        {
            attributeSpecifications.Add(new StringAttributeSpecification(name, attributeValues));
            setTarget(name);// target defaults to last column added
        }

        /// <summary>
        /// The targetAttribute to set.
        /// </summary>
        /// <param name="target">The targetAttribute to set.</param>
        public virtual void setTarget(string target)
        {
            this.targetAttribute = target;
        }

        public virtual IAttributeSpecification getAttributeSpecFor(string name)
        {
            foreach (IAttributeSpecification spec in attributeSpecifications)
            {
                if (spec.GetAttributeName().Equals(name))
                {
                    return spec;
                }
            }
            throw new RuntimeException("no attribute spec for  " + name);
        }

        public virtual void defineNumericAttribute(string name)
        {
            attributeSpecifications.Add(new NumericAttributeSpecification(name));
        }

        public virtual ICollection<string> getNamesOfStringAttributes()
        {
            ICollection<string> names = CollectionFactory.CreateQueue<string>();
            foreach (IAttributeSpecification spec in attributeSpecifications)
            {
                if (spec is StringAttributeSpecification)
                {
                    names.Add(spec.GetAttributeName());
                }
            }
            return names;
        }

        public override string ToString()
        {
            return attributeSpecifications.ToString();
        }
    } 
}
