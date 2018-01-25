namespace aima.net.search.local
{
    public class Scheduler
    {
        private readonly int k, limit;
        private readonly double lam;

        public Scheduler(int k, double lam, int limit)
        {
            this.k = k;
            this.lam = lam;
            this.limit = limit;
        }

        public Scheduler()
        {
            this.k = 20;
            this.lam = 0.045;
            this.limit = 100;
        }

        public double getTemp(int t)
        {
            if (t < limit)
                return k * System.Math.Exp((-1) * lam * t);
            else
                return 0.0;
        }
    }

}
