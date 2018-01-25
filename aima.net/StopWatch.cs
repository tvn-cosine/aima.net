using aima.net.api;

namespace aima.net
{
    public class StopWatch : System.Diagnostics.Stopwatch, IStopWatch
    { 
        public long GetElapsedMilliseconds()
        {
            return this.ElapsedMilliseconds;
        }

        public long GetElapsedTicks()
        {
            return this.ElapsedTicks;
        }

        public bool GetIsRunning()
        {
            return this.IsRunning;
        }
    }
}
