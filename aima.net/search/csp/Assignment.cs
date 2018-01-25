using aima.net;
using aima.net.api;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.exceptions;
using aima.net.text;
using aima.net.text.api;
using aima.net.search.csp.api;

namespace aima.net.search.csp
{
    /// <summary>
    /// An assignment assigns values to some or all variables of a CSP.
    /// </summary>
    /// <typeparam name="VAR"></typeparam>
    /// <typeparam name="VAL"></typeparam>
    public class Assignment<VAR, VAL> : ICloneable<Assignment<VAR, VAL>>
        where VAR : Variable
    {
        /// <summary>
        /// Maps variables to their assigned values.
        /// </summary>
        private IMap<VAR, VAL> variableToValueMap = CollectionFactory.CreateInsertionOrderedMap<VAR, VAL>();

        public ICollection<VAR> getVariables()
        {
            return CollectionFactory.CreateQueue<VAR>(variableToValueMap.GetKeys());
        }

        public VAL getValue(VAR var)
        {
            return variableToValueMap.Get(var);
        }

        public VAL add(VAR var, VAL value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value cannot be null");
            }

            variableToValueMap.Put(var, value);
            return value;
        }

        public VAL remove(VAR var)
        {
            VAL val = variableToValueMap.Get(var);
            variableToValueMap.Remove(var);
            return val;
        }

        public bool contains(VAR var)
        {
            return variableToValueMap.ContainsKey(var);
        }

        /// <summary>
        /// Returns true if this assignment does not violate any constraints of constraints.
        /// </summary>
        /// <param name="constraints"></param>
        /// <returns></returns>
        public bool isConsistent(ICollection<IConstraint<VAR, VAL>> constraints)
        {
            foreach (IConstraint<VAR, VAL> cons in constraints)
                if (!cons.isSatisfiedWith(this))
                    return false;
            return true;
        }

        /// <summary>
        /// Returns true if this assignment assigns values to every variable of vars.
        /// </summary>
        /// <param name="vars"></param>
        /// <returns></returns>
        public bool isComplete(ICollection<VAR> vars)
        {
            foreach (VAR var in vars)
                if (!contains(var))
                    return false;
            return true;
        }

        /// <summary>
        /// Returns true if this assignment is consistent as well as complete with
        /// respect to the given CSP.
        /// </summary>
        /// <param name="csp"></param>
        /// <returns></returns>
        public bool isSolution(CSP<VAR, VAL> csp)
        {
            return isConsistent(csp.getConstraints()) && isComplete(csp.getVariables());
        }

        public Assignment<VAR, VAL> Clone()
        {
            Assignment<VAR, VAL> result;

            result = new Assignment<VAR, VAL>();
            result.variableToValueMap = CollectionFactory.CreateMap<VAR, VAL>(variableToValueMap);

            return result;
        }


        public override string ToString()
        {
            bool comma = false;
            IStringBuilder result = TextFactory.CreateStringBuilder("{");
            foreach (var entry in variableToValueMap)
            {
                if (comma)
                    result.Append(", ");
                result.Append(entry.GetKey()).Append("=").Append(entry.GetValue());
                comma = true;
            }
            result.Append("}");
            return result.ToString();
        }
    }
}
