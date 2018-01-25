using aima.net.agent.api;
using aima.net.util;

namespace aima.net.agent
{ 
    public class DynamicState : ObjectWithDynamicAttributes, IState
    {
        public const string TYPE = "State";

        public DynamicState()
        { }

        public override string DescribeType()
        {
            return TYPE;
        }
    }
}
