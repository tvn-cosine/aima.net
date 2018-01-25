using aima.net.api;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.datastructures;
using aima.net.exceptions;

namespace aima.net.util
{
    /**
     * A basic implementation of a disjoint-set data structure for maintaining a
     * collection of disjoint dynamic sets. This is based on the algorithm
     * description in Chapter 21 of 'Introduction to Algorithm 2nd Edition' (by
     * Cormen, Leriserson, Rivest, and Stein) for Disjoint-sets taking into account
     * some of the heuristic ideas described. However, this implementation relies on
     * the constant time performance of the HashMap's get and put operations, and
     * HashSet's add, remove, contains and size operations as an alternative
     * implementation approach. This provides a cleaner separation between the
     * elements and the implementation (i.e. the idea of a representative for a
     * particular disjoint set is not used) and an easier to use API (i.e. direct
     * access to the disjoint set that an element belongs to and no need to
     * understand how the internals work with respect to a representative) but will
     * not perform as fast (proper analysis required but likely O(m + n lg n) as
     * detailed in Theorem 21.1 on page 504 of 'Introduction to Algorithm 2nd
     * Edition').
     * 
     * Note: the internal implementation of this class can likely be improved to use
     * all the techniques outlined in section 21.3 in order to get optimal
     * performance.
     * 
     * @author Ciaran O'Reilly
     * 
     * @param <E>
     *            the type of elements to be contained by the disjoint sets.
     */
    public class DisjointSets<E>
    { 
        private IMap<E, ISet<E>> elementToSet = CollectionFactory.CreateInsertionOrderedMap<E, ISet<E>>();
        private ISet<ISet<E>> disjointSets = CollectionFactory.CreateSet<ISet<E>>();

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public DisjointSets()
        {

        }

        /**
         * Constructor.
         * 
         * @param initialElements
         *            a collection of elements, each of which will be assigned to
         *            their own disjoint set via makeSet().
         */
        public DisjointSets(ICollection<E> initialElements)
        {
            foreach (E element in initialElements)
            {
                makeSet(element);
            }
        }

        /**
         * Constructor.
         * 
         * @param elements
         *            a collection of elements, each of which will be assigned to
         *            their own disjoint set via makeSet().
         */
        public DisjointSets(params E[] elements)
        {
            foreach (E element in elements)
            {
                makeSet(element);
            }
        }

        /**
         * Create a disjoint set for the element passed in. This method should be
         * called for all elements before any of the other API methods are called
         * (i.e. construct a disjoint set for each element first and then union them
         * together as appropriate).
         * 
         * @param element
         *            the element for which a new disjoint set should be
         *            constructed.
         */
        public void makeSet(E element)
        {
            if (!elementToSet.ContainsKey(element))
            {
                // Note: It is necessary to use an identity based hash set
                // whose equal and hashCode method are based on the Sets
                // identity and not its elements as we are adding
                // this set to a set but changing its values as unions
                // occur.
                ISet<E> set = new IdentityHashSet<E>();
                set.Add(element);
                elementToSet.Put(element, set);
                disjointSets.Add(set);
            }
        }

        /**
         * Union two disjoint sets together if the arguments currently belong to two
         * different sets.
         *
         * @throws IllegalArgumentException
         *             if element1 or element 2 is not already associated with a
         *             disjoint set (i.e. makeSet() was not called for the argument
         *             beforehand).
         */
        public void union(E element1, E element2)
        {
            ISet<E> set1 = elementToSet.Get(element1);
            if (set1 == null)
            {
                throw new IllegalArgumentException("element 1 is not associated with a disjoint set, call makeSet() first.");
            }
            ISet<E> set2 = elementToSet.Get(element2);
            if (set2 == null)
            {
                throw new IllegalArgumentException("element 2 is not associated with a disjoint set, call makeSet() first.");
            }
            if (set1 != set2)
            {
                // simple weighted union heuristic
                if (set1.Size() < set2.Size())
                {
                    set2.AddAll(set1);
                    foreach (E element in set1)
                    {
                        ISet<E> previousElement = elementToSet.Get(element);
                        elementToSet.Put(element, set2);
                        disjointSets.Remove(previousElement);
                    }
                }
                else
                {
                    // i.e. set1 >= set2
                    set1.AddAll(set2);
                    foreach (E element in set2)
                    {
                        ISet<E> previousElement = elementToSet.Get(element);
                        elementToSet.Put(element, set1);
                        disjointSets.Remove(previousElement);
                    }
                }
            }
        }

        /**
         * Find the disjoint set that an element belongs to.
         * 
         * @param element
         *            the element whose disjoint set is being sought.
         * @return the disjoint set for the element or null if makeSet(element) was
         *         not previously called.
         */
        public ISet<E> find(E element)
        {
            // Note: Instantiate normal sets to ensure IdentityHashSet
            // is not exposed outside of this class.
            // This also ensures the internal logic cannot
            // be corrupted externally due to changing sets.
            return CollectionFactory.CreateSet<E>(elementToSet.Get(element));
        }

        /**
         * 
         * @return a map for each element and the corresponding disjoint set that it
         *         belongs to.
         */
        public IMap<E, ISet<E>> getElementToDisjointSet()
        {
            // Note: Instantiate normal sets to ensure IdentityHashSet
            // is not exposed outside of this class.
            // This also ensures the internal logic cannot
            // be corrupted externally due to changing sets.
            IMap<E, ISet<E>> result = CollectionFactory.CreateInsertionOrderedMap<E, ISet<E>>();
            foreach (KeyValuePair<E, ISet<E>> entry in elementToSet)
            {
                result.Put(entry.GetKey(), CollectionFactory.CreateSet<E>(entry.GetValue()));
            }
            return result;
        }

        /**
         * 
         * @return the set of disjoint sets being maintained.
         */
        public ISet<ISet<E>> getDisjointSets()
        {
            // Note: Instantiate normal sets to ensure IdentityHashSet
            // is not exposed outside of this class.
            // This also ensures the internal logic cannot
            // be corrupted externally due to changing sets.
            ISet<ISet<E>> result = CollectionFactory.CreateSet<ISet<E>>();
            foreach (ISet<E> disjointSet in disjointSets)
                result.Add(CollectionFactory.CreateSet<E>(disjointSet));
            return result;
        }

        /**
         * 
         * @return the number of disjoint sets.
         */
        public int numberDisjointSets()
        {
            return disjointSets.Size();
        }

        /**
         * Remove all the disjoint sets.
         */
        public void clear()
        {
            elementToSet.Clear();
            disjointSets.Clear();
        }

        //
        // PRIVATE METHODS
        //

        // Override hashCode and equals so that
        // the Set itself and not its elements
        // are used to determine its hashCode
        // and equality.
        private class IdentityHashSet<H> : Set<H>, IHashable, IEquatable
        {
            public override int GetHashCode()
            {
                return base.backingSet.GetHashCode();
            }

            public override bool Equals(object o)
            {
                return this == o;
            }
        }
    } 
}
