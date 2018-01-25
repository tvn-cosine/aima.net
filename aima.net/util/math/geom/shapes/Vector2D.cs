using aima.net.api; 

namespace aima.net.util.math.geom.shapes
{
    /**
     * This class represents a vector in a two-dimensional Cartesian plot.<br/>
     * Simple arithmetic operations are supported.
     * 
     * @author Arno von Borries
     * @author Jan Phillip Kretzschmar
     * @author Andreas Walscheid
     *
     */
    public class Vector2D : IEquatable<Vector2D>
    {
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /**
         * This is a vector that is parallel to the X axis.
         */
        public static readonly Vector2D X_VECTOR = new Vector2D(1.0d, 0.0d);
        /**
         * This is a vector that is parallel to the Y axis.
         */
        public static readonly Vector2D Y_VECTOR = new Vector2D(0.0d, 1.0d);
        /**
         * This is the zero vector. It does not have a direction neither a length.
         */
        public static readonly Vector2D ZERO_VECTOR = new Vector2D(0.0d, 0.0d);

        private readonly double x;
        private readonly double y;

        /**
         * @param x the X parameter of the vector.
         * @param y the Y parameter of the vector.
         */
        public Vector2D(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        /**
         * Calculates a vector based on the length and the heading of the vector.
         * @param length the length of the vector.
         * @param heading the angle at which the vector points.
         * @return a new vector derived from its polar representation.
         */
        public static Vector2D calculateFromPolar(double length, double heading)
        {
            double x = length * System.Math.Cos(heading);
            double y = length * System.Math.Sin(heading);
            return new Vector2D(x, y);
        }

        /**
         * @return the X parameter of the vector.
         */
        public double getX()
        {
            return x;
        }

        /**
         * @return the Y parameter of the vector.
         */
        public double getY()
        {
            return y;
        }

        /**
         * Adds a vector onto this vector.
         * @param op2 the vector to be added.
         * @return the new calculated vector.
         */
        public Vector2D add(Vector2D op2)
        {
            return new Vector2D(this.x + op2.x, this.y + op2.y);
        }

        /**
         * Subtracts a vector from this vector.
         * @param op2 the vector to be subtracted.
         * @return the new calculated vector.
         */
        public Vector2D sub(Vector2D op2)
        {
            return new Vector2D(this.x - op2.x, this.y - op2.y);
        }

        /**
         * Multiplies a vector with a double.
         * @param n the times the vector is to be taken.
         * @return the new calculated vector.
         */
        public Vector2D multiply(double n)
        {
            return new Vector2D(this.x * n, this.y * n);
        }

        /**
         * Inverts this vector.
         * @return the inverted vector.
         */
        public Vector2D invert()
        {
            return new Vector2D(-this.x, -this.y);
        }

        /**
         * Checks whether this vector and another vector are parallel to each other.<br/>
         * If one of the vectors is the zero vector this method always returns {@code true}.
         * @param op2 the second vector.
         * @return {@code true} if the two vectors are parallel.
         */
        public bool isAbsoluteParallel(Vector2D op2)
        {
            return (this.y * op2.getX() - this.x * op2.getY() == 0);
        }

        /**
         * Checks whether this vector and another vector are parallel to each other or rotated by 180 degrees to each other.<br/>
         * If one of the vectors is the zero vector this method always returns {@code true}.
         * @param op2 the second vector.
         * @return {@code true} if the two vectors are parallel.
         */
        public bool isParallel(Vector2D op2)
        {
              double angle = angleTo(op2);
            return Util.compareDoubles(angle, 0.0d) || Util.compareDoubles(angle, System.Math.PI);
        }

        /**
         * Calculates the angle between two vectors in radians.<br/>
         * Both vectors must be different from the zero-vector.
         * @param op2 the second vector.
         * @return the angle in radians.
         */
        public double angleTo(Vector2D op2)
        {
              double result = System.Math.Atan2(op2.getY(), op2.getX()) - System.Math.Atan2(this.y, this.x);
            return result < 0 ? result + 2 * System.Math.PI : result;
        }

        /**
         * Calculates the length of the vector.
         * @return the length of the vector.
         */
        public double length()
        {
            return System.Math.Sqrt(this.x * this.x + this.y * this.y);
        }

        /**
         * Checks equality for this vector with another vector.
         * @param op2 the second vector.
         * @return true if the vectors are equal in direction and length.
         */
        public bool Equals(Vector2D op2)
        {
            if (op2 == null) return false;
            return Util.compareDoubles(this.x, op2.x) && Util.compareDoubles(this.y, op2.y);
        }


        public override bool Equals(object o)
        {
            if (o is Vector2D)
                return this.Equals((Vector2D)o);
            return false;
        }
    }

}
