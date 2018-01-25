using aima.net.api;

namespace aima.net.datastructures
{ 
    public class Triplet<X, Y, Z> : IEquatable, IHashable, IStringable
    {
        private readonly X x;
        private readonly Y y;
        private readonly Z z;
        
        /// <summary>
        /// Constructs a triplet with three specified elements.
        /// </summary>
        /// <param name="x">the first element of the triplet.</param>
        /// <param name="y">the second element of the triplet.</param>
        /// <param name="z">the third element of the triplet.</param>
        public Triplet(X x, Y y, Z z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
         
        /// <summary>
        /// Returns the first element of the triplet.
        /// </summary>
        /// <returns>the first element of the triplet.</returns>
        public X GetFirst()
        {
            return x;
        }
         
        /// <summary>
        /// Returns the second element of the triplet.
        /// </summary>
        /// <returns>the second element of the triplet.</returns>
        public Y GetSecond()
        {
            return y;
        }
         
        /// <summary>
        /// Returns the third element of the triplet.
        /// </summary>
        /// <returns>the third element of the triplet.</returns>
        public Z getThird()
        {
            return z;
        }

        public override bool Equals(object o)
        {
            if (o is Triplet<X, Y, Z>)
            {
                Triplet<X, Y, Z> other = (Triplet<X, Y, Z>)o;
                return (x.Equals(other.x))
                    && (y.Equals(other.y))
                    && (z.Equals(other.z));
            }
            return false;
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() + 31 * y.GetHashCode() + 31 * z.GetHashCode();
        }

        public override string ToString()
        {
            return "< " 
                 + x.ToString() 
                 + " , " 
                 + y.ToString() + " , "
                 + z.ToString() + " >";
        }
    } 
}
