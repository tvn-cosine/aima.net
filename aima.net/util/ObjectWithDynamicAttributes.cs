using aima.net.api;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.text;
using aima.net.text.api;

namespace aima.net.util
{ 
    public abstract class ObjectWithDynamicAttributes : IEquatable, IHashable, IStringable
    {
        private IMap<object, object> attributes = CollectionFactory.CreateInsertionOrderedMap<object, object>();
         
        /// <summary>
        /// By default, returns the simple name of the underlying class as given in the source code.
        /// </summary>
        /// <returns>the simple name of the underlying class</returns>
        public virtual string DescribeType()
        {
            return GetType().Name;
        }
         
        /// <summary>
        /// Returns a string representation of the object's current attributes
        /// </summary>
        /// <returns>a string representation of the object's current attributes</returns>
        public virtual string DescribeAttributes()
        {
            IStringBuilder sb = TextFactory.CreateStringBuilder();

            sb.Append("[");
            bool first = true;
            foreach (object key in attributes.GetKeys())
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    sb.Append(", ");
                }

                sb.Append(key);
                sb.Append("==");
                sb.Append(attributes.Get(key));
            }
            sb.Append("]");

            return sb.ToString();
        }
         
        /// <summary>
        /// Returns an unmodifiable view of the object's key set
        /// </summary>
        /// <returns>an unmodifiable view of the object's key set</returns>
        public virtual ISet<object> GetKeys()
        {
            return CollectionFactory.CreateReadOnlySet<object>(attributes.GetKeys());
        }
         
        /// <summary>
        /// Associates the specified value with the specified attribute key. If the
        /// ObjectWithDynamicAttributes previously contained a mapping for the
        /// attribute key, the old value is replaced.
        /// </summary>
        /// <param name="key">the attribute key</param>
        /// <param name="value">the attribute value</param>
        public virtual void SetAttribute(object key, object value)
        {
            attributes.Put(key, value);
        }
         
        /// <summary>
        /// Returns the value of the specified attribute key, or null if the
        /// attribute was not found.
        /// </summary>
        /// <param name="key">the attribute key</param>
        /// <returns>the value of the specified attribute name, or null if not found.</returns>
        public virtual object GetAttribute(object key)
        {
            return attributes.Get(key);
        }
         
        /// <summary>
        /// Removes the attribute with the specified key from this
        /// ObjectWithDynamicAttributes.
        /// </summary>
        /// <param name="key">the attribute key</param>
        public virtual void RemoveAttribute(object key)
        {
            attributes.Remove(key);
        }

        public override bool Equals(object o)
        {
            return o != null
                && GetType() == o.GetType()
                && attributes.Equals(((ObjectWithDynamicAttributes)o).attributes);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            return DescribeType() + DescribeAttributes();
        }
    }
}
