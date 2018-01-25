using aima.net.api;

namespace aima.net.environment.connectfour
{
    public class ActionValuePair<A> : IComparable<ActionValuePair<A>>
    {
        private A action;
        private double value;

        public static ActionValuePair<A> createFor(A action, double utility)
        {
            return new ActionValuePair<A>(action, utility);
        }

        public ActionValuePair(A action, double utility)
        {
            this.action = action;
            this.value = utility;
        }

        public A getAction()
        {
            return action;
        }

        public double getValue()
        {
            return value;
        }

        public int CompareTo(ActionValuePair<A> pair)
        {
            if (value < pair.value)
                return 1;
            else if (value > pair.value)
                return -1;
            else
                return 0;
        }
    }
}
