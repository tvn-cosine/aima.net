using aima.net.exceptions;

namespace aima.net
{
    public class EventObject
    {
        /// <summary>
        /// The object on which the Event initially occurred.
        /// </summary>
        protected object source;
         
        /// <summary>
        /// Constructs a prototypical Event.
        /// </summary>
        /// <param name="source">The object on which the Event initially occurred.</param>
        public EventObject(object source)
        {
            if (source == null)
                throw new IllegalArgumentException("null source");

            this.source = source;
        }

        /// <summary>
        /// The object on which the Event initially occurred.
        /// </summary>
        /// <returns>The object on which the Event initially occurred.</returns>
        public object GetSource()
        {
            return source;
        }

        /// <summary>
        /// Returns a string representation of this EventObject.
        /// </summary>
        /// <returns>Returns a string representation of this EventObject.</returns>
        public override string ToString()
        {
            return GetType().Name + "[source=" + source + "]";
        }
    } 
}
