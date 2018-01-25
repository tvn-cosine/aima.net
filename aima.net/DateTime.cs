using aima.net.api;

namespace aima.net
{
    public class DateTime : IDateTime, IComparable<DateTime>, IEquatable<DateTime>
    {
        public System.DateTime backingDateTime;

        public DateTime(IDateTime input)
        {
            backingDateTime = new System.DateTime(input.GetTicks());
        }

        public DateTime(long ticks)
        {
            backingDateTime = new System.DateTime(ticks);
        }

        public DateTime(int year, int month, int day)
        {
            backingDateTime = new System.DateTime(year, month, day);
        }

        public DateTime(int year, int month, int day, int hour, int minute, int second)
        {
            backingDateTime = new System.DateTime(year, month, day, hour, minute, second);
        }

        public DateTime(int year, int month, int day, int hour, int minute, int second, int millisecond)
        {
            backingDateTime = new System.DateTime(year, month, day, hour, minute, second, millisecond);
        }

        public int GetDay()
        {
            return backingDateTime.Day;
        }

        public int GetDayOfYear()
        {
            return backingDateTime.DayOfYear;
        }

        public int GetHour()
        {
            return backingDateTime.Hour;
        }

        public int GetMillisecond()
        {
            return backingDateTime.Millisecond;
        }

        public int GetMinute()
        {
            return backingDateTime.Minute;
        }

        public int GetMonth()
        {
            return backingDateTime.Month;
        }

        public int GetSecond()
        {
            return backingDateTime.Second;
        }

        public long GetTicks()
        {
            return backingDateTime.Ticks;
        }

        public int GetYear()
        {
            return backingDateTime.Year;
        }

        public override string ToString()
        {
            return backingDateTime.ToString();
        }

        public string ToString(string format)
        {
            return backingDateTime.ToString(format);
        }

        public IDateTime AddDays(double value)
        {
            return new DateTime(backingDateTime.AddDays(value).Ticks);
        }

        public IDateTime AddHours(double value)
        {
            return new DateTime(backingDateTime.AddHours(value).Ticks);
        }

        public IDateTime AddMilliseconds(double value)
        {
            return new DateTime(backingDateTime.AddMilliseconds(value).Ticks);
        }

        public IDateTime AddMinutes(double value)
        {
            return new DateTime(backingDateTime.AddMinutes(value).Ticks);
        }

        public IDateTime AddMonths(int months)
        {
            return new DateTime(backingDateTime.AddMonths(months).Ticks);
        }

        public IDateTime AddSeconds(double value)
        {
            return new DateTime(backingDateTime.AddSeconds(value).Ticks);
        }

        public IDateTime AddTicks(long value)
        {
            return new DateTime(backingDateTime.AddTicks(value).Ticks);
        }

        public IDateTime AddYears(int value)
        {
            return new DateTime(backingDateTime.AddYears(value).Ticks);
        }

        public int CompareTo(IDateTime other)
        {
            return CompareTo(other as DateTime);
        }

        public bool Equals(DateTime other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            else
            {
                return other.backingDateTime.Equals(other.backingDateTime);
            }
        }

        public bool Equals(IDateTime other)
        {
            return Equals(other as DateTime);
        }

        public override bool Equals(object obj)
        {
            if (obj is System.DateTime)
            {
                return backingDateTime == (System.DateTime)obj;
            }
            else
            {
                return Equals(obj as DateTime);
            }
        }

        public int CompareTo(DateTime other)
        {
            if (ReferenceEquals(null, other))
            {
                return 1;
            }
            else
            {
                return backingDateTime.CompareTo(other.backingDateTime);
            }
        }

        public override int GetHashCode()
        {
            return backingDateTime.GetHashCode();
        }

        public static IDateTime Now()
        {
            return new DateTime(System.DateTime.Now.Ticks);
        }
         
        public bool NotEquals(IDateTime other)
        {
            return !Equals(other);
        }

        public bool SmallerThan(DateTime other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            else
            {
                return backingDateTime < other.backingDateTime;
            }
        }

        public bool BiggerThan(DateTime other)
        {
            if (ReferenceEquals(null, other))
            {
                return true;
            }
            else
            {
                return backingDateTime > other.backingDateTime;
            }
        }

        public bool SmallerOrEqualThan(DateTime other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            else
            {
                return backingDateTime <= other.backingDateTime;
            }
        }
        public bool BiggerOrEqualThan(DateTime other)
        {
            if (ReferenceEquals(null, other))
            {
                return true;
            }
            else
            {
                return backingDateTime >= other.backingDateTime;
            }
        }

        public bool SmallerThan(IDateTime other)
        {
            return SmallerThan(other as DateTime);
        }

        public bool BiggerThan(IDateTime other)
        {
            return BiggerThan(other as DateTime);
        }

        public bool SmallerOrEqualThan(IDateTime other)
        {
            return SmallerOrEqualThan(other as DateTime);
        }

        public bool BiggerOrEqualThan(IDateTime other)
        {
            return BiggerOrEqualThan(other as DateTime);
        }
    }
}
