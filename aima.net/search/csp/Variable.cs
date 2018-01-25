namespace aima.net.search.csp
{
    /// <summary>
    /// A variable is a distinguishable object with a name.
    /// </summary>
    public class Variable
    {
        private readonly string name;

        public Variable(string name)
        {
            this.name = name;
        }

        public string getName()
        {
            return name;
        }

        public override string ToString()
        {
            return name;
        }

        /// <summary>
        /// Variables with equal names are equal.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return obj is Variable && this.name.Equals(((Variable)obj).name);
        }
         
        public override int GetHashCode()
        {
            return name.GetHashCode();
        }
    } 
}
