using aima.net.api;

namespace aima.net
{
    public static class CommonFactory
    {
        public static IRandom CreateMockRandom(double[] values)
        {
            return new MockRandom(values);
        }

        public static IRandom CreateRandom()
        {
            return new Random();
        }

        public static IDateTime Now()
        {
            return DateTime.Now();
        }

        public static IDateTime CreateDateTime(IDateTime input)
        {
            return new DateTime(input);
        }

        public static IDateTime CreateDateTime(long ticks)
        {
            return new DateTime(ticks);
        }

        public static IDateTime CreateDateTime(int year, int month, int day)
        {
            return new DateTime(year, month, day);
        }

        public static IDateTime CreateDateTime(int year, int month, int day, int hour, int minute, int second)
        {
            return new DateTime(year, month, day, hour, minute, second);
        }

        public static IDateTime CreateDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond)
        {
            return new DateTime(year, month, day, hour, minute, second, millisecond);
        }

        public static IStopWatch CreateStopWatch()
        {
            return new StopWatch();
        }
    }
}
