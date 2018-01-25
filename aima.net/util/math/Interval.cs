using aima.net.api;

namespace aima.net.util.math
{
    /// <summary>
    /// Basic supports for Intervals. <para />
    /// See <see href="http://en.wikipedia.org/wiki/Interval_%28mathematics%29" />Interval
    /// </summary>
    /// <typeparam name="C">The object type</typeparam>
    public class Interval<C>
    {
        private IComparable<C> lower = null;
        private bool lowerInclusive = true;
        private IComparable<C> upper = null;
        private bool upperInclusive = true;

        public Interval()
        { }

        /// <summary>
        /// Constructs a closed interval from the two specified end points.
        /// </summary>
        /// <param name="lower">the lower end point of the interval</param>
        /// <param name="upper">the upper end point of the interval</param>
        public Interval(IComparable<C> lower, IComparable<C> upper)
        {
            SetLower(lower);
            SetUpper(upper);
        }

        /// <summary>
        /// Constructs an interval from the two specified end points.
        /// </summary>
        /// <param name="lower">the lower end point of the interval</param>
        /// <param name="lowerInclusive">wether or not the lower end of the interval is inclusive of its value.</param>
        /// <param name="upper">the upper end point of the interval</param>
        /// <param name="upperInclusive">whether or not the upper end of the interval is inclusive of its value.</param>
        public Interval(IComparable<C> lower, bool lowerInclusive,
                        IComparable<C> upper, bool upperInclusive)
        {
            SetLower(lower);
            SetLowerInclusive(lowerInclusive);
            SetUpper(upper);
            SetUpperInclusive(upperInclusive);
        }

        /// <summary>
        /// Returns true if the specified object is between the end points of this interval.
        /// </summary>
        /// <param name="o">the specified object</param>
        /// <returns>true if the specified object is between the end points of this interval.</returns>
        public bool IsInInterval(C o)
        {
            if (null == lower || null == upper)
            {
                return false;
            }

            bool _in = true;

            if (IsLowerInclusive())
            {
                _in = lower.CompareTo(o) <= 0;
            }
            else
            {
                _in = lower.CompareTo(o) < 0;
            }

            if (_in)
            {
                if (IsUpperInclusive())
                {
                    _in = upper.CompareTo(o) >= 0;
                }
                else
                {
                    _in = upper.CompareTo(o) > 0;
                }
            }

            return _in;
        }

        /// <summary>
        /// Returns true if this interval is lower inclusive.
        /// </summary>
        /// <returns>true if this interval is lower inclusive.</returns>
        public bool IsLowerInclusive()
        {
            return lowerInclusive;
        }

        /// <summary>
        /// Returns true if this interval is not lower inclusive.
        /// </summary>
        /// <returns>true if this interval is not lower inclusive.</returns>
        public bool IsLowerExclusive()
        {
            return !lowerInclusive;
        }

        /// <summary>
        /// Sets the interval to lower inclusive or lower exclusive.
        /// </summary>
        /// <param name="inclusive">true represents lower inclusive and false represents lower exclusive.</param>
        public void SetLowerInclusive(bool inclusive)
        {
            this.lowerInclusive = inclusive;
        }

        /// <summary>
        /// Sets the interval to lower exclusive or lower inclusive.
        /// </summary>
        /// <param name="exclusive">true represents lower exclusive and false represents lower inclusive.</param>
        public void SetLowerExclusive(bool exclusive)
        {
            this.lowerInclusive = !exclusive;
        }
         
        /// <summary>
        /// Returns the lower end point of the interval.
        /// </summary>
        /// <returns>the lower end point of the interval.</returns>
        public IComparable<C> GetLower()
        {
            return lower;
        }


        /// <summary>
        /// Sets the lower end point of the interval.
        /// </summary>
        /// <param name="lower">the lower end point of the interval.</param>
        public void SetLower(IComparable<C> lower)
        {
            this.lower = lower;
        }

        /// <summary>
        /// Returns true if this interval is upper inclusive.
        /// </summary>
        /// <returns>true if this interval is upper inclusive.</returns>
        public bool IsUpperInclusive()
        {
            return upperInclusive;
        }

        /// <summary>
        /// Returns true if this interval is upper exclusive.
        /// </summary>
        /// <returns>true if this interval is upper exclusive.</returns>
        public bool IsUpperExclusive()
        {
            return !upperInclusive;
        }

        /// <summary>
        /// Sets the interval to upper inclusive or upper exclusive.
        /// </summary>
        /// <param name="inclusive">true represents upper inclusive and false represents upper exclusive.</param>
        public void SetUpperInclusive(bool inclusive)
        {
            this.upperInclusive = inclusive;
        }

        /// <summary>
        /// Sets the interval to upper exclusive or upper inclusive.
        /// </summary>
        /// <param name="exclusive">true represents upper exclusive and false represents upper inclusive.</param>
        public void SetUpperExclusive(bool exclusive)
        {
            this.upperInclusive = !exclusive;
        }

        /// <summary>
        /// Returns the upper end point of the interval.
        /// </summary>
        /// <returns>the upper end point of the interval.</returns>
        public IComparable<C> GetUpper()
        {
            return upper;
        }

        /// <summary>
        /// Sets the upper end point of the interval.
        /// </summary>
        /// <param name="upper">the upper end point of the interval.</param>
        public void SetUpper(IComparable<C> upper)
        {
            this.upper = upper;
        }

        public override string ToString()
        {
            return (lowerInclusive ? "[" : "(") + lower + ", " + upper  + (upperInclusive ? "]" : ")");
        }
    } 
}
