namespace aima.net.api
{
    public interface IDateTime : IStringable, IComparable<IDateTime>, IEquatable<IDateTime>
    {
        int GetDay();
        int GetDayOfYear();
        int GetHour();
        int GetMillisecond();
        int GetMinute();
        int GetMonth();
        int GetSecond();
        long GetTicks();
        int GetYear();
        string ToString(string format); 
        IDateTime AddDays(double value);
        IDateTime AddHours(double value);
        IDateTime AddMilliseconds(double value);
        IDateTime AddMinutes(double value);
        IDateTime AddMonths(int months);
        IDateTime AddSeconds(double value);
        IDateTime AddTicks(long value);
        IDateTime AddYears(int value);
         
        bool NotEquals(IDateTime other);
        bool SmallerThan(IDateTime other);
        bool BiggerThan(IDateTime other);
        bool SmallerOrEqualThan(IDateTime other);
        bool BiggerOrEqualThan(IDateTime other);
    }
}