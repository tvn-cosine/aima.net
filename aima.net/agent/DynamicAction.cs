using aima.net.agent.api;
using aima.net.util;

namespace aima.net.agent
{
    public class DynamicAction : ObjectWithDynamicAttributes, IAction
    {
        public const string ATTRIBUTE_NAME = "name";
        public const string TYPE = "Action";

        private readonly bool isNoOp;

        public DynamicAction(string name)
            : this(name, false)
        { }

        public DynamicAction(string name, bool isNoOp)
        {
            this.SetAttribute(ATTRIBUTE_NAME, name);
            this.isNoOp = isNoOp;
        }

        /// <summary>
        /// Returns the value of the name attribute.
        /// </summary>
        /// <returns></returns>
        public virtual string GetName()
        {
            return (string)GetAttribute(ATTRIBUTE_NAME);
        }

        public virtual bool IsNoOp()
        {
            return isNoOp;
        }

        public override string DescribeType()
        {
            return TYPE;
        }

        public static readonly DynamicAction NO_OP = new DynamicAction("NoOp", true);
    }
}
