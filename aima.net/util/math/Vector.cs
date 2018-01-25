using aima.net.collections.api;
using aima.net.exceptions;

namespace aima.net.util.math
{
    /// <summary>
    /// Vector is modelled as a matrix with a single column;
    /// </summary>
    public class Vector : Matrix
    {  
        /// <summary>
        /// Constructs a vector with the specified size.
        /// </summary>
        /// <param name="size">the capacity of the vector</param>
        public Vector(int size)
            : base(size, 1)
        { }
        
        /// <summary>
        /// Constructs a vector with the specified list of values.
        /// </summary>
        /// <param name="lst">a list of values</param>
        public Vector(ICollection<double> lst)
            : base(lst.Size(), 1)
        { 
            for (int i = 0; i < lst.Size();++i)
            {
                SetValue(i, lst.Get(i));
            }
        }
       
        /// <summary>
        /// Returns the value at the specified index.
        /// </summary>
        /// <param name="i">the index of the value to return.</param>
        /// <returns>the value at the specified index.</returns>
        public double GetValue(int i)
        {
            return base.Get(i, 0);
        }
        
        /// <summary>
        /// Sets the value at the specified index.
        /// </summary>
        /// <param name="index">the index of the value to set.</param>
        /// <param name="value">the value to be placed at the index.</param>
        public void SetValue(int index, double value)
        {
            base.Set(index, 0, value);
        }
         
        /// <summary>
        /// Returns a copy of this vector.
        /// </summary>
        /// <returns>a copy of this vector.</returns>
        public Vector CopyVector()
        {
            Vector result = new Vector(GetRowDimension());
            for (int i = 0; i < GetRowDimension();++i)
            {
                result.SetValue(i, GetValue(i));
            }
            return result;
        }
 
        /// <summary>
        /// Returns the number of values in this vector.
        /// </summary>
        /// <returns>the number of values in this vector.</returns>
        public int Size()
        {
            return GetRowDimension();
        }
       
        /// <summary>
        /// Returns the result of vector subtraction.
        /// </summary>
        /// <param name="v">the vector to subtract</param>
        /// <returns>the result of vector subtraction.</returns>
        public Vector Minus(Vector v)
        {
            Vector result = new Vector(Size());
            for (int i = 0; i < Size();++i)
            {
                result.SetValue(i, GetValue(i) - v.GetValue(i));
            }
            return result;
        }
         
        /// <summary>
        /// Returns the result of vector addition.
        /// </summary>
        /// <param name="v">the vector to add</param>
        /// <returns>the result of vector addition.</returns>
        public Vector Plus(Vector v)
        {
            Vector result = new Vector(Size());
            for (int i = 0; i < Size();++i)
            {
                result.SetValue(i, GetValue(i) + v.GetValue(i));
            }
            return result;
        }

        /// <summary>
        /// Returns the index at which the maximum value in this vector is located.
        /// </summary>
        /// <returns>the index at which the maximum value in this vector is located.</returns>
        /// <exception cref="RuntimeException">if the vector does not contain any values.</exception>
        public int IndexHavingMaxValue()
        {
            if (Size() <= 0)
            {
                throw new RuntimeException("can't perform this op on empty vector");
            }
            int res = 0;
            for (int i = 0; i < Size();++i)
            {
                if (GetValue(i) > GetValue(res))
                {
                    res = i;
                }
            }
            return res;
        }
    } 
}
