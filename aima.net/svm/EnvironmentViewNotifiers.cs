using aima.net.agent.api;

namespace aima.net.svm
{ 
    public class EnvironmentViewNotifierConsole : IEnvironmentViewNotifier
    {
        public void NotifyViews(string message)
        {
            System.Console.WriteLine(message);
        }
    }
     
    public class EnvironmentViewNotifierNone : IEnvironmentViewNotifier
    {
        public void NotifyViews(string message)
        { } 
    }
}
