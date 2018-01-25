using aima.net.robotics.datatypes.api;

namespace aima.net.robotics.datatypes
{
    /// <summary>
    /// This class : a single linear reading of a range.<para />
    /// In addition to the range it stores an Angle by which the 
    /// heading was rotated for the measurement of the range.
    /// </summary>
    public abstract class AbstractRangeReading : IMclRangeReading<AbstractRangeReading, Angle>
    {
        private readonly double value;
        private readonly Angle angle;
        
        /// <summary>
        /// Constructor for a range reading with an angle assumed to be zero.
        /// </summary>
        /// <param name="value">the actual range that was measured.</param>
        public AbstractRangeReading(double value)
        {
            this.value = value;
            this.angle = Angle.ZERO_ANGLE;
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">the actual range that was measured.</param>
        /// <param name="angle">the angle by which the heading was rotated for the measurement.</param>
        public AbstractRangeReading(double value, Angle angle)
        {
            this.value = value;
            this.angle = angle;
        }

        /// <summary>
        /// Returns the range that was measured.
        /// </summary>
        /// <returns>the range that was measured.</returns>
        public double GetValue()
        {
            return value;
        }
         
        public Angle GetAngle()
        {
            return angle;
        }
         
        public override string ToString()
        {
            return string.Format("{0}.2f", value) + "@" + string.Format("{0}.2f", angle.GetDegreeValue()) + "\u00BA";
        }

        public abstract double CalculateWeight(AbstractRangeReading secondRange);
    } 
}
