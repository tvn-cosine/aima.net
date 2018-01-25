using aima.net.agent.api;
using aima.net.api;
using aima.net.exceptions;
using aima.net.util;

namespace aima.net.agent
{
    public class DynamicPercept : ObjectWithDynamicAttributes, IPercept, IHashable, IEquatable
    {
        public DynamicPercept()
        { }

        public override string DescribeType()
        {
            return "Percept";
        }

        /// <summary>
        /// Constructs a DynamicPercept with one attribute
        /// </summary>
        /// <param name="key1">the attribute key</param>
        /// <param name="value1">the attribute value</param>
        public DynamicPercept(object key1, object value1)
        {
            SetAttribute(key1, value1);
        }

        /// <summary>
        /// Constructs a DynamicPercept with two attributes
        /// </summary>
        /// <param name="key1">the first attribute key</param>
        /// <param name="value1">the first attribute value</param>
        /// <param name="key2">the second attribute key</param>
        /// <param name="value2">the second attribute value</param>
        public DynamicPercept(object key1, object value1, object key2, object value2)
        {
            SetAttribute(key1, value1);
            SetAttribute(key2, value2);
        }

        /// <summary>
        /// Constructs a DynamicPercept with an array of attributes
        /// </summary>
        /// <param name="keys">the array of attribute keys</param>
        /// <param name="values">the array of attribute values</param>
        public DynamicPercept(object[] keys, object[] values)
        {
            if (keys.Length != values.Length)
            {
                throw new ArgumentOutOfRangeException("keys.Length != values.Length", null);
            }

            for (int i = 0; i < keys.Length; ++i)
            {
                SetAttribute(keys[i], values[i]);
            }
        }

        public override bool Equals(object o)
        {
            if (null == (o as DynamicPercept)) return false;
            return ToString().Equals(o.ToString());
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}
