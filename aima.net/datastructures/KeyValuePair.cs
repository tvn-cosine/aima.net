namespace aima.net.datastructures
{
    public struct KeyValuePair<KEY, VALUE>
    {
        private readonly KEY key;
        private readonly VALUE value;

        public KeyValuePair(KEY key, VALUE value)
        {
            this.key = key;
            this.value = value;
        }

        public KEY GetKey()
        {
            return this.key;
        }

        public VALUE GetValue()
        {
            return this.value;
        }
         
        public override bool Equals(object o)
        {
            if (o is KeyValuePair<KEY, VALUE>)
            {
                KeyValuePair<KEY, VALUE> p = (KeyValuePair<KEY, VALUE>)o;
                return GetKey().Equals(p.GetKey())
                    && GetValue().Equals(p.GetValue());
            }
            return false;
        }

        public override int GetHashCode()
        {
            return GetKey().GetHashCode() 
                 + 31 
                 * GetValue().GetHashCode();
        }

        public override string ToString()
        {
            return "< "
                  + GetKey().ToString()
                  + " , " + GetValue().ToString()
                  + " > ";
        }
    }
}
