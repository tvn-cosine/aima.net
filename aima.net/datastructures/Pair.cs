using aima.net.api;

namespace aima.net.datastructures
{ 
    public class Pair<X, Y> : IEquatable, IHashable, IStringable
    {
        private readonly X a;
        private readonly Y b;
         
        /// <summary>
        /// Constructs a Pair from two given elements
        /// </summary>
        /// <param name="a">the first element</param>
        /// <param name="b">the second element</param>
        public Pair(X a, Y b)
        {
            this.a = a;
            this.b = b;
        }
         
        /// <summary>
        /// Returns the first element of the pair
        /// </summary>
        /// <returns>the first element of the pair</returns>
        public X GetFirst()
        {
            return a;
        }
         
        /// <summary>
        /// Returns the second element of the pair
        /// </summary>
        /// <returns>the second element of the pair</returns>
        public Y getSecond()
        {
            return b;
        }

        public override bool Equals(object o)
        {
            if (o is Pair<X, Y>)
            {
                Pair<X, Y> p = (Pair<X, Y>)o;
                return a.Equals(p.a)
                    && b.Equals(p.b);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return a.GetHashCode() + 31 * b.GetHashCode();
        }

        public override string ToString()
        {
            return "< " 
                  + GetFirst().ToString() 
                  + " , " + getSecond().ToString()
                  + " > ";
        }
    } 
}
