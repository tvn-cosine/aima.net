using aima.net.agent.api; 
using aima.net.collections.api;
using aima.net.util;

namespace aima.net.demo.search
{
    public abstract class SearchDemoBase
    { 
        protected static void printInstrumentation(Properties properties)
        {
            foreach (object o in properties.GetKeys())
            {
                string key = (string)o;
                string property = (string)properties.getProperty(key);
                System.Console.WriteLine(key + " : " + property);
            } 
        }

        protected static void printActions(ICollection<IAction> actions)
        {
            foreach (IAction action in actions)
            {
                System.Console.WriteLine(action);
            }
        }
    }
}
