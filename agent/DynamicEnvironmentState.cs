using aima.net.agent.api;
using aima.net.util;

namespace aima.net.agent
{
    public class DynamicEnvironmentState : ObjectWithDynamicAttributes, IEnvironmentState
    {
        public const string TYPE = "EnvironmentState";

        public DynamicEnvironmentState()
        { }

        public override string DescribeType()
        {
            return TYPE;
        }
    }
}
