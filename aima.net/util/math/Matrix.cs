using aima.net;
using aima.net.api;
using aima.net.collections.api;
using aima.net.exceptions;
using aima.net.text;
using aima.net.text.api;

namespace aima.net.util.math
{
    /// <summary>
    /// The Matrix Class provides the fundamental operations of numerical linear
    /// algebra. Various constructors create Matrices from two dimensional arrays of
    /// double precision floating point numbers. Various "gets" and "sets" provide
    /// access to submatrices and matrix elements. Several methods implement basic
    /// matrix arithmetic, including matrix addition and multiplication, matrix
    /// norms, and element-by-element array operations. Methods for reading and
    /// printing matrices are also included. All the operations in this version of
    /// the Matrix Class involve real matrices. Complex matrices may be handled in a
    /// future version.
    /// <para />
    /// Five fundamental matrix decompositions, which consist of pairs or triples of
    /// matrices, permutation vectors, and the like, produce results in five
    /// decomposition classes. These decompositions are accessed by the Matrix class
    /// to compute solutions of simultaneous linear equations, determinants, inverses
    /// and other matrix functions. The five decompositions are:
    /// <para />
    ///  
    /// * Cholesky Decomposition of symmetric, positive definite matrices.<para />
    /// * LU Decomposition of rectangular matrices.<para />
    /// * QR Decomposition of rectangular matrices.<para />
    /// * Singular Value Decomposition of rectangular matrices.<para />
    /// * Eigenvalue Decomposition of both symmetric and nonsymmetric square
    /// matrices.<para />
    /// <para />
    /// Example of use:
    /// <para />
    /// Solve a linear system A x = b and compute the residual norm, ||b - A x||.
    /// <para />
    /// double[][] vals = { { 1., 2., 3 }, { 4., 5., 6. }, { 7., 8., 10. } };<para />
    /// Matrix A = new Matrix(vals);<para />
    /// Matrix b = Matrix.Random(3, 1);<para />
    /// Matrix x = A.Solve(b);<para />
    /// Matrix r = A.Times(x).Minus(b);<para />
    /// double rnorm = r.NormInf();  
    /// </summary>
    public class Matrix : ICloneable<object>
    {
        /// <summary>
        /// Array for internal storage of elements.
        /// </summary>
        private readonly double[,] A;

        /// <summary>
        /// Row dimensions.
        /// </summary>
        private readonly int m;
        /// <summary>
        /// Column dimensions.
        /// </summary>
        private readonly int n;

        /// <summary>
        /// Construct a diagonal Matrix from the given List of doubles
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static Matrix createDiagonalMatrix(ICollection<double> values)
        {
            Matrix m = new Matrix(values.Size(), values.Size(), 0);
            for (int i = 0; i < values.Size(); ++i)
            {
                m.Set(i, i, values.Get(i));
            }
            return m;
        }

        /// <summary>
        /// Construct an m-by-n matrix of zeros.
        /// </summary>
        /// <param name="m">Number of rows.</param>
        /// <param name="n">Number of colums.</param> 
        public Matrix(int m, int n)
        {
            this.m = m;
            this.n = n;
            A = new double[m, n];
        }

        /// <summary>
        /// Construct an m-by-n matrix of zeros.
        /// </summary>
        /// <param name="m">Number of rows.</param>
        /// <param name="n">Number of colums.</param> 
        /// <param name="s">Fill the matrix with this scalar value.</param>
        public Matrix(int m, int n, double s)
        {
            this.m = m;
            this.n = n;
            A = new double[m, n];
            for (int i = 0; i < m; ++i)
            {
                for (int j = 0; j < n; j++)
                {
                    A[i, j] = s;
                }
            }
        }

        /// <summary>
        /// Construct a matrix from a 2-D array.
        /// </summary>
        /// <param name="A">Two-dimensional array of doubles.</param>
        public Matrix(double[,] A)
        {
            m = A.GetLength(0);
            n = A.GetLength(1);
            this.A = A;
        }

        /// <summary>
        /// Construct a matrix quickly without checking arguments.
        /// </summary>
        /// <param name="A">Two-dimensional array of doubles.</param>
        /// <param name="m">Number of rows.</param>
        /// <param name="n">Number of colums.</param>
        public Matrix(double[,] A, int m, int n)
        {
            this.A = A;
            this.m = m;
            this.n = n;
        }

        /// <summary>
        /// Construct a matrix from a one-dimensional packed array
        /// </summary>
        /// <param name="vals">One-dimensional array of doubles, packed by columns (ala Fortran).</param>
        /// <param name="m">Number of rows.</param>
        public Matrix(double[] vals, int m)
        {
            this.m = m;
            n = (m != 0 ? vals.Length / m : 0);
            if (m * n != vals.Length)
            {
                throw new IllegalArgumentException("Array length must be a multiple of m.");
            }
            A = new double[m, n];
            for (int i = 0; i < m; ++i)
            {
                for (int j = 0; j < n; j++)
                {
                    A[i, j] = vals[i + j * m];
                }
            }
        }

        /// <summary>
        /// Construct a matrix from a copy of a 2-D array.
        /// </summary>
        /// <param name="A">Two-dimensional array of doubles.</param>
        /// <returns></returns>
        public static Matrix ConstructWithCopy(double[,] A)
        {
            int m = A.GetLength(0);
            int n = A.GetLength(1);
            Matrix X = new Matrix(m, n);
            double[,] C = X.GetArray();
            for (int i = 0; i < m; ++i)
            {
                for (int j = 0; j < n; j++)
                {
                    C[i, j] = A[i, j];
                }
            }
            return X;
        }

        /// <summary>
        /// Make a deep copy of a matrix
        /// </summary>
        /// <returns></returns>
        public Matrix Copy()
        {
            Matrix X = new Matrix(m, n);
            double[,] C = X.GetArray();
            for (int i = 0; i < m; ++i)
            {
                for (int j = 0; j < n; j++)
                {
                    C[i, j] = A[i, j];
                }
            }
            return X;
        }

        /// <summary>
        /// Clone the Matrix object.
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return this.Copy();
        }

        /// <summary>
        /// Access the internal two-dimensional array.
        /// </summary>
        /// <returns>Pointer to the two-dimensional array of matrix elements.</returns>
        public double[,] GetArray()
        {
            return A;
        }

        /// <summary>
        /// Copy the internal two-dimensional array.
        /// </summary>
        /// <returns>Two-dimensional array copy of matrix elements.</returns>
        public double[,] GetArrayCopy()
        {
            double[,] C = new double[m, n];
            for (int i = 0; i < m; ++i)
            {
                for (int j = 0; j < n; j++)
                {
                    C[i, j] = A[i, j];
                }
            }
            return C;
        }

        /// <summary>
        /// Make a one-dimensional column packed copy of the internal array.
        /// </summary>
        /// <returns>Matrix elements packed in a one-dimensional array by columns.</returns>
        public double[] GetColumnPackedCopy()
        {
            double[] vals = new double[m * n];
            for (int i = 0; i < m; ++i)
            {
                for (int j = 0; j < n; j++)
                {
                    vals[i + j * m] = A[i, j];
                }
            }
            return vals;
        }

        /// <summary>
        /// Make a one-dimensional row packed copy of the internal array.
        /// </summary>
        /// <returns>Matrix elements packed in a one-dimensional array by rows.</returns>
        public double[] GetRowPackedCopy()
        {
            double[] vals = new double[m * n];
            for (int i = 0; i < m; ++i)
            {
                for (int j = 0; j < n; j++)
                {
                    vals[i * n + j] = A[i, j];
                }
            }
            return vals;
        }

        /// <summary>
        /// Get row dimension.
        /// </summary>
        /// <returns>m, the number of rows.</returns>
        public int GetRowDimension()
        {
            return m;
        }

        /// <summary>
        /// Get column dimension.
        /// </summary>
        /// <returns>n, the number of columns.</returns>
        public int GetColumnDimension()
        {
            return n;
        }

        /// <summary>
        /// Get a single element.
        /// </summary>
        /// <param name="i">Row index.</param>
        /// <param name="j">Column index.</param>
        /// <returns>A(i,j)</returns>
        public double Get(int i, int j)
        {
            return A[i, j];
        }

        /// <summary>
        /// Get a submatrix.
        /// </summary>
        /// <param name="i0">Initial row index</param>
        /// <param name="i1">Final row index</param>
        /// <param name="j0">Initial column index</param>
        /// <param name="j1">Final column index</param>
        /// <returns>A(i0:i1,j0:j1)</returns>
        /// <exception cref="ArrayIndexOutOfBoundsException"></exception>
        public Matrix GetMatrix(int i0, int i1, int j0, int j1)
        {
            Matrix X = new Matrix(i1 - i0 + 1, j1 - j0 + 1);
            double[,] B = X.GetArray();
            try
            {
                for (int i = i0; i <= i1; ++i)
                {
                    for (int j = j0; j <= j1; j++)
                    {
                        B[i - i0, j - j0] = A[i, j];
                    }
                }
            }
            catch (ArrayIndexOutOfBoundsException)
            {
                throw new ArrayIndexOutOfBoundsException("Submatrix indices");
            }
            return X;
        }

        /// <summary>
        /// Get a submatrix.
        /// </summary>
        /// <param name="r">Array of row indices.</param>
        /// <param name="c">Array of column indices.</param>
        /// <returns>A(r(:),c(:))</returns>
        /// <exception cref="ArrayIndexOutOfBoundsException"></exception>
        public Matrix GetMatrix(int[] r, int[] c)
        {
            Matrix X = new Matrix(r.Length, c.Length);
            double[,] B = X.GetArray();
            try
            {
                for (int i = 0; i < r.Length; ++i)
                {
                    for (int j = 0; j < c.Length; j++)
                    {
                        B[i, j] = A[r[i], c[j]];
                    }
                }
            }
            catch (ArrayIndexOutOfBoundsException)
            {
                throw new ArrayIndexOutOfBoundsException("Submatrix indices");
            }
            return X;
        }

        /// <summary>
        /// Get a submatrix.
        /// </summary>
        /// <param name="i0">Initial row index</param>
        /// <param name="i1">Final row index</param>
        /// <param name="c">Array of column indices.</param>
        /// <returns>A(i0:i1,c(:))</returns>
        /// <exception cref="ArrayIndexOutOfBoundsException"></exception>
        public Matrix GetMatrix(int i0, int i1, int[] c)
        {
            Matrix X = new Matrix(i1 - i0 + 1, c.Length);
            double[,] B = X.GetArray();
            try
            {
                for (int i = i0; i <= i1; ++i)
                {
                    for (int j = 0; j < c.Length; j++)
                    {
                        B[i - i0, j] = A[i, c[j]];
                    }
                }
            }
            catch (ArrayIndexOutOfBoundsException)
            {
                throw new ArrayIndexOutOfBoundsException("Submatrix indices");
            }
            return X;
        }

        /// <summary>
        /// Get a submatrix.
        /// </summary>
        /// <param name="r">Array of row indices.</param>
        /// <param name="j0">Initial column index</param>
        /// <param name="j1">Final column index</param>
        /// <returns>A(r(:),j0:j1)</returns>
        /// <exception cref="ArrayIndexOutOfBoundsException"></exception>
        public Matrix GetMatrix(int[] r, int j0, int j1)
        {
            Matrix X = new Matrix(r.Length, j1 - j0 + 1);
            double[,] B = X.GetArray();
            try
            {
                for (int i = 0; i < r.Length; ++i)
                {
                    for (int j = j0; j <= j1; j++)
                    {
                        B[i, j - j0] = A[r[i], j];
                    }
                }
            }
            catch (ArrayIndexOutOfBoundsException)
            {
                throw new ArrayIndexOutOfBoundsException("Submatrix indices");
            }
            return X;
        }

        /// <summary>
        /// Set a single element.
        /// </summary>
        /// <param name="i">Row index.</param>
        /// <param name="j">Column index.</param>
        /// <param name="s">A(i,j)</param>
        public void Set(int i, int j, double s)
        {
            A[i, j] = s;
        }

        /// <summary>
        /// Set a submatrix.
        /// </summary>
        /// <param name="i0">Initial row index</param>
        /// <param name="i1">Final row index</param>
        /// <param name="j0">Initial column index</param>
        /// <param name="j1">Final column index</param>
        /// <param name="X">A(i0:i1,j0:j1)</param>
        /// <exception cref="ArrayIndexOutOfBoundsException"></exception>
        public void SetMatrix(int i0, int i1, int j0, int j1, Matrix X)
        {
            try
            {
                for (int i = i0; i <= i1; ++i)
                {
                    for (int j = j0; j <= j1; j++)
                    {
                        A[i, j] = X.Get(i - i0, j - j0);
                    }
                }
            }
            catch (ArrayIndexOutOfBoundsException)
            {
                throw new ArrayIndexOutOfBoundsException("Submatrix indices");
            }
        }

        /// <summary>
        /// Set a submatrix.
        /// </summary>
        /// <param name="r">Array of row indices.</param>
        /// <param name="c">Array of column indices.</param>
        /// <param name="X">A(r(:),c(:))</param>
        /// <exception cref="ArrayIndexOutOfBoundsException"></exception>
        public void SetMatrix(int[] r, int[] c, Matrix X)
        {
            try
            {
                for (int i = 0; i < r.Length; ++i)
                {
                    for (int j = 0; j < c.Length; j++)
                    {
                        A[r[i], c[j]] = X.Get(i, j);
                    }
                }
            }
            catch (ArrayIndexOutOfBoundsException)
            {
                throw new ArrayIndexOutOfBoundsException("Submatrix indices");
            }
        }

        /// <summary>
        /// Set a submatrix.
        /// </summary>
        /// <param name="r">Array of row indices.</param>
        /// <param name="j0">Initial column index</param>
        /// <param name="j1">Final column index</param>
        /// <param name="X">A(r(:),j0:j1)</param>
        /// <exception cref="ArrayIndexOutOfBoundsException"></exception>
        public void SetMatrix(int[] r, int j0, int j1, Matrix X)
        {
            try
            {
                for (int i = 0; i < r.Length; ++i)
                {
                    for (int j = j0; j <= j1; j++)
                    {
                        A[r[i], j] = X.Get(i, j - j0);
                    }
                }
            }
            catch (ArrayIndexOutOfBoundsException)
            {
                throw new ArrayIndexOutOfBoundsException("Submatrix indices");
            }
        }

        /// <summary>
        /// Set a submatrix.
        /// </summary>
        /// <param name="i0">Initial row index</param>
        /// <param name="i1">Final row index</param>
        /// <param name="c">Array of column indices.</param>
        /// <param name="X">A(i0:i1,c(:))</param>
        /// <exception cref="ArrayIndexOutOfBoundsException"></exception>
        public void SetMatrix(int i0, int i1, int[] c, Matrix X)
        {
            try
            {
                for (int i = i0; i <= i1; ++i)
                {
                    for (int j = 0; j < c.Length; j++)
                    {
                        A[i, c[j]] = X.Get(i - i0, j);
                    }
                }
            }
            catch (ArrayIndexOutOfBoundsException)
            {
                throw new ArrayIndexOutOfBoundsException("Submatrix indices");
            }
        }

        /// <summary>
        /// Matrix transpose.
        /// </summary>
        /// <returns>A'</returns>
        public Matrix Transpose()
        {
            Matrix X = new Matrix(n, m);
            double[,] C = X.GetArray();
            for (int i = 0; i < m; ++i)
            {
                for (int j = 0; j < n; j++)
                {
                    C[j, i] = A[i, j];
                }
            }
            return X;
        }

        /// <summary>
        /// One norm
        /// </summary>
        /// <returns>maximum column sum.</returns>
        public double Norm1()
        {
            double f = 0;
            for (int j = 0; j < n; j++)
            {
                double s = 0;
                for (int i = 0; i < m; ++i)
                {
                    s += System.Math.Abs(A[i, j]);
                }
                f = System.Math.Max(f, s);
            }
            return f;
        }

        // /// <summary>
        // /// Two norm
        // /// </summary>
        // /// <returns>maximum singular value.</returns>
        // /// public double Norm2() 
        // /// {
        // ///     return (new SingularValueDecomposition(this).Norm2());
        // /// }

        /// <summary>
        /// Infinity norm
        /// </summary>
        /// <returns>maximum row sum.</returns>
        public double NormInf()
        {
            double f = 0;
            for (int i = 0; i < m; ++i)
            {
                double s = 0;
                for (int j = 0; j < n; j++)
                {
                    s += System.Math.Abs(A[i, j]);
                }
                f = System.Math.Max(f, s);
            }
            return f;
        }
        /// <summary>
        /// Frobenius norm
        /// </summary>
        /// <returns>sqrt of sum of squares of all elements.</returns>
        public double NormF()
        {
            double f = 0;
            for (int i = 0; i < m; ++i)
            {
                for (int j = 0; j < n; j++)
                {
                    f = System.Math.Sqrt(f * f + A[i, j] * A[i, j]);
                }
            }
            return f;
        }

        /// <summary>
        /// Unary minus
        /// </summary>
        /// <returns>-A</returns>
        public Matrix Uminus()
        {
            Matrix X = new Matrix(m, n);
            double[,] C = X.GetArray();
            for (int i = 0; i < m; ++i)
            {
                for (int j = 0; j < n; j++)
                {
                    C[i, j] = -A[i, j];
                }
            }
            return X;
        }

        /// <summary>
        /// C = A + B
        /// </summary>
        /// <param name="B">another matrix</param>
        /// <returns>A + B</returns>
        public Matrix Plus(Matrix B)
        {
            checkMatrixDimensions(B);
            Matrix X = new Matrix(m, n);
            double[,] C = X.GetArray();
            for (int i = 0; i < m; ++i)
            {
                for (int j = 0; j < n; j++)
                {
                    C[i, j] = A[i, j] + B.A[i, j];
                }
            }
            return X;
        }

        /// <summary>
        /// A = A + B
        /// </summary>
        /// <param name="B">another matrix</param>
        /// <returns>A + B</returns>
        public Matrix PlusEquals(Matrix B)
        {
            checkMatrixDimensions(B);
            for (int i = 0; i < m; ++i)
            {
                for (int j = 0; j < n; j++)
                {
                    A[i, j] = A[i, j] + B.A[i, j];
                }
            }
            return this;
        }

        /// <summary>
        /// C = A - B
        /// </summary>
        /// <param name="B">another matrix</param>
        /// <returns>A - B</returns>
        public Matrix Minus(Matrix B)
        {
            checkMatrixDimensions(B);
            Matrix X = new Matrix(m, n);
            double[,] C = X.GetArray();
            for (int i = 0; i < m; ++i)
            {
                for (int j = 0; j < n; j++)
                {
                    C[i, j] = A[i, j] - B.A[i, j];
                }
            }
            return X;
        }

        /// <summary>
        /// A = A - B
        /// </summary>
        /// <param name="B">another matrix</param>
        /// <returns>A - B</returns>
        public Matrix MinusEquals(Matrix B)
        {
            checkMatrixDimensions(B);
            for (int i = 0; i < m; ++i)
            {
                for (int j = 0; j < n; j++)
                {
                    A[i, j] = A[i, j] - B.A[i, j];
                }
            }
            return this;
        }

        /// <summary>
        /// Element-by-element multiplication, C = A.*B
        /// </summary>
        /// <param name="B">another matrix</param>
        /// <returns>A.*B</returns>
        public Matrix ArrayTimes(Matrix B)
        {
            checkMatrixDimensions(B);
            Matrix X = new Matrix(m, n);
            double[,] C = X.GetArray();
            for (int i = 0; i < m; ++i)
            {
                for (int j = 0; j < n; j++)
                {
                    C[i, j] = A[i, j] * B.A[i, j];
                }
            }
            return X;
        }

        /// <summary>
        /// Element-by-element multiplication in place, A = A.* B
        /// </summary>
        /// <param name="B">another matrix</param>
        /// <returns>A.*B</returns>
        public Matrix ArrayTimesEquals(Matrix B)
        {
            checkMatrixDimensions(B);
            for (int i = 0; i < m; ++i)
            {
                for (int j = 0; j < n; j++)
                {
                    A[i, j] = A[i, j] * B.A[i, j];
                }
            }
            return this;
        }

        /// <summary>
        /// Element-by-element right division, C = A./B
        /// </summary>
        /// <param name="B">another matrix</param>
        /// <returns>A./B</returns>
        public Matrix ArrayRightDivide(Matrix B)
        {
            checkMatrixDimensions(B);
            Matrix X = new Matrix(m, n);
            double[,] C = X.GetArray();
            for (int i = 0; i < m; ++i)
            {
                for (int j = 0; j < n; j++)
                {
                    C[i, j] = A[i, j] / B.A[i, j];
                }
            }
            return X;
        }

        /// <summary>
        /// Element-by-element right division in place, A = A./B
        /// </summary>
        /// <param name="B">another matrix</param>
        /// <returns>A./B</returns>
        public Matrix ArrayRightDivideEquals(Matrix B)
        {
            checkMatrixDimensions(B);
            for (int i = 0; i < m; ++i)
            {
                for (int j = 0; j < n; j++)
                {
                    A[i, j] = A[i, j] / B.A[i, j];
                }
            }
            return this;
        }

        /// <summary>
        /// Element-by-element left division, C = A.\B
        /// </summary>
        /// <param name="B">another matrix</param>
        /// <returns>A.\B</returns>
        public Matrix ArrayLeftDivide(Matrix B)
        {
            checkMatrixDimensions(B);
            Matrix X = new Matrix(m, n);
            double[,] C = X.GetArray();
            for (int i = 0; i < m; ++i)
            {
                for (int j = 0; j < n; j++)
                {
                    C[i, j] = B.A[i, j] / A[i, j];
                }
            }
            return X;
        }

        /// <summary>
        /// Element-by-element left division in place, A = A.\B
        /// </summary>
        /// <param name="B">another matrix</param>
        /// <returns>A.\B</returns>
        public Matrix ArrayLeftDivideEquals(Matrix B)
        {
            checkMatrixDimensions(B);
            for (int i = 0; i < m; ++i)
            {
                for (int j = 0; j < n; j++)
                {
                    A[i, j] = B.A[i, j] / A[i, j];
                }
            }
            return this;
        }

        /// <summary>
        /// Multiply a matrix by a scalar, C = s*A
        /// </summary>
        /// <param name="s">scalar</param>
        /// <returns>s*A</returns>
        public Matrix Times(double s)
        {
            Matrix X = new Matrix(m, n);
            double[,] C = X.GetArray();
            for (int i = 0; i < m; ++i)
            {
                for (int j = 0; j < n; j++)
                {
                    C[i, j] = s * A[i, j];
                }
            }
            return X;
        }

        /// <summary>
        /// Multiply a matrix by a scalar in place, A = s*A
        /// </summary>
        /// <param name="s">scalar</param>
        /// <returns>replace A by s*A</returns>
        public Matrix TimesEquals(double s)
        {
            for (int i = 0; i < m; ++i)
            {
                for (int j = 0; j < n; j++)
                {
                    A[i, j] = s * A[i, j];
                }
            }
            return this;
        }

        /// <summary>
        /// Linear algebraic matrix multiplication, A * B
        /// </summary>
        /// <param name="B">another matrix</param>
        /// <returns>Matrix product, A * B</returns>
        /// <exception cref="IllegalArgumentException">Matrix inner dimensions must agree.</exception>
        public Matrix Times(Matrix B)
        {
            if (B.m != n)
            {
                throw new IllegalArgumentException("Matrix inner dimensions must agree.");
            }
            Matrix X = new Matrix(m, B.n);
            double[,] C = X.GetArray();
            double[] Bcolj = new double[n];
            for (int j = 0; j < B.n; j++)
            {
                for (int k = 0; k < n; k++)
                {
                    Bcolj[k] = B.A[k, j];
                }
                for (int i = 0; i < m; ++i)
                {
                    double s = 0;
                    for (int k = 0; k < n; k++)
                    {
                        s += A[i, k] * Bcolj[k];
                    }
                    C[i, j] = s;
                }
            }
            return X;
        }

        /// <summary>
        /// LU Decomposition
        /// </summary>
        /// <returns>LU Decomposition</returns>
        public LUDecomposition LU()
        {
            return new LUDecomposition(this);
        }

        /// <summary>
        /// Solve A*X = B
        /// </summary>
        /// <param name="B">right hand side</param>
        /// <returns>solution if A is square, least squares solution otherwise</returns>
        public Matrix Solve(Matrix B)
        {
            // assumed m == n
            return new LUDecomposition(this).Solve(B);

        }

        /// <summary>
        /// Solve X*A = B, which is also A'*X' = B'
        /// </summary>
        /// <param name="B">right hand side</param>
        /// <returns>solution if A is square, least squares solution otherwise.</returns>
        public Matrix SolveTranspose(Matrix B)
        {
            return Transpose().Solve(B.Transpose());
        }

        /// <summary>
        /// Matrix inverse or pseudoinverse
        /// </summary>
        /// <returns>inverse(A) if A is square, pseudoinverse otherwise.</returns>
        public Matrix Inverse()
        {
            return Solve(Identity(m, m));
        }

        /// <summary>
        /// Matrix determinant
        /// </summary>
        /// <returns>Matrix determinant</returns>
        public double Det()
        {
            return new LUDecomposition(this).Det();
        }

        ////  /// <summary>
        ////  /// Matrix rank
        ////  /// </summary>
        ////  /// <returns>effective numerical rank, obtained from SVD.</returns>
        ////  // public int Rank() 
        ////  // {
        ////  //    return new SingularValueDecomposition(this).rank();
        ////  // }

        ////  /// <summary>
        ////  /// Matrix condition (2 norm)
        ////  /// </summary>
        ////  /// <returns>ratio of largest to smallest singular value.</returns>
        ////  // public double Cond() 
        ////  // {
        ////  //    return new SingularValueDecomposition(this).cond();
        ////  // } 

        /// <summary>
        /// Matrix trace.
        /// </summary>
        /// <returns>sum of the diagonal elements.</returns>
        public double Trace()
        {
            double t = 0;
            for (int i = 0; i < System.Math.Min(m, n); ++i)
            {
                t += A[i, i];
            }
            return t;
        }

        /// <summary>
        /// Generate matrix with random elements
        /// </summary>
        /// <param name="m">Number of rows.</param>
        /// <param name="n">Number of colums.</param>
        /// <returns>An m-by-n matrix with uniformly distributed random elements.</returns>
        public static Matrix Random(int m, int n)
        {
            IRandom _random = CommonFactory.CreateRandom();
            Matrix A = new Matrix(m, n);
            double[,] X = A.GetArray();
            for (int i = 0; i < m; ++i)
            {
                for (int j = 0; j < n; j++)
                {
                    X[i, j] = _random.NextDouble();
                }
            }
            return A;
        }

        /// <summary>
        /// Generate identity matrix
        /// </summary>
        /// <param name="m">Number of rows.</param>
        /// <param name="n">Number of colums.</param>
        /// <returns>An m-by-n matrix with ones on the diagonal and zeros elsewhere.</returns>
        public static Matrix Identity(int m, int n)
        {
            Matrix A = new Matrix(m, n);
            double[,] X = A.GetArray();
            for (int i = 0; i < m; ++i)
            {
                for (int j = 0; j < n; j++)
                {
                    X[i, j] = (i == j ? 1.0 : 0.0);
                }
            }
            return A;
        }

        /// <summary>
        /// Print the matrix to the output stream. Line the elements up in columns.
        /// Use the format object, and right justify within columns of width
        /// characters. Note that is the matrix is to be read back in, you probably
        /// will want to use a NumberFormat that is set to US Locale.
        /// </summary>
        /// <param name="output">the output stream.</param>
        /// <param name="width">Column width.</param>
        public void Print(IStringBuilder output, int width)
        {
            output.AppendLine(); // start on new line.
            for (int i = 0; i < m; ++i)
            {
                for (int j = 0; j < n; j++)
                {
                    string s = A[i, j].ToString(); // format the number
                    int padding = System.Math.Max(1, width - s.Length); // At _least_ 1 space
                    for (int k = 0; k < padding; ++k)
                    {
                        output.Append(' ');
                    }
                    output.Append(s);
                }
                output.AppendLine();
            }
            output.AppendLine(); // end with blank line.
        }

        public override string ToString()
        {
            IStringBuilder buf = TextFactory.CreateStringBuilder();
            for (int i = 0; i < GetRowDimension(); ++i)
            {

                for (int j = 0; j < GetColumnDimension(); j++)
                {
                    buf.Append(Get(i, j));
                    buf.Append(" ");
                }
                buf.Append("\n");
            }

            return buf.ToString();
        }

        /// <summary>
        /// Check if size(A) == size(B)
        /// </summary>
        /// <param name="B"></param>
        private void checkMatrixDimensions(Matrix B)
        {
            if (B.m != m || B.n != n)
            {
                throw new IllegalArgumentException("Matrix dimensions must agree.");
            }
        }
    }
}
