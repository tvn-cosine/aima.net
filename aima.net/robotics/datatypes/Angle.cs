using aima.net.api;
using aima.net.robotics.datatypes.api;
using aima.net.util;

namespace aima.net.robotics.datatypes
{
    /// <summary> 
    /// This class stores an angle as a double. It is used to pass on the angle 
    /// from the IMclRangeReading to the IPose2D.<para />
    /// This implementation can be used for angles in radians. 
    /// The context of this unit is that IPose2D has to use radian angles.<para />
    /// Please note, that all classes that are interacting with the
    /// MCL somehow should use radian angles, too.<para />
    /// Use degreeAngle(), getDegreeValue(), Math.toDegrees()
    /// and Math.toRadians() if you need or have degree angles.
    /// </summary>
    public class Angle : IMclVector, IComparable<Angle>
    { 
        /// <summary>
        /// The zero angle represents 0.0 radians.
        /// </summary>
        public static readonly Angle ZERO_ANGLE = new Angle(0.0D);

        private readonly double value;

        /// <summary> 
        /// </summary>
        /// <param name="value">value the radian value of the angle.</param>
        public Angle(double value)
        {
            this.value = value;
        }

        private static double degreeToRadian(double angle)
        {
            return System.Math.PI * angle / 180.0;
        }

        private static double radianToDegree(double angle)
        {
            return angle * (180.0 / System.Math.PI);
        }
         
        /// <summary>
        /// Creates a new angle based on a degree value.
        /// </summary>
        /// <param name="value">the degree value of the angle.</param>
        /// <returns>the new angle.</returns>
        public static Angle DegreeAngle(double value)
        {
            return new Angle(degreeToRadian(value));
        }

        /// <summary>
        /// Return the radian value of the angle.
        /// </summary>
        /// <returns>the radian value of the angle.</returns>
        public double GetValue()
        {
            return value;
        }

        /// <summary>
        /// Return the degree value of the angle.
        /// </summary>
        /// <returns>the degree value of the angle.</returns>
        public double GetDegreeValue()
        {
            return radianToDegree(value);
        }
         
        public int CompareTo(Angle o)
        {
            if (Util.compareDoubles(this.value, o.value)) return 0;
            if (this.value < o.value) return -1;
            return 1;
        }
    } 
}
