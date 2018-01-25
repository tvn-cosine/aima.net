using aima.net.exceptions;

namespace aima.net.util.math
{
    /// <summary>
    /// LU Decomposition. <para />
    /// For an m-by-n matrix A with m >= n, the LU decomposition is an m-by-n unit
    /// lower triangular matrix L, an n-by-n upper triangular matrix U, and a
    /// permutation vector piv of length m so that A(piv,:) = L*U. If m &lt; n, then L
    /// is m-by-m and U is m-by-n.
    /// <para />
    /// The LU decompostion with pivoting always exists, even if the matrix is
    /// singular, so the constructor will never fail. The primary use of the LU
    /// decomposition is in the solution of square systems of simultaneous linear
    /// equations. This will fail if isNonsingular() returns false.
    /// </summary>
    public class LUDecomposition
    {
        /// <summary>
        /// Array for internal storage of decomposition.
        /// </summary>
        private readonly double[,] LU;

        //  Row and column dimensions, and pivot sign. 
        /// <summary>
        /// column dimension.
        /// </summary>
        private readonly int m;
        /// <summary>
        /// row dimension.
        /// </summary>
        private readonly int n;
        /// <summary>
        /// pivot sign.
        /// </summary>
        private int pivsign;

        /// <summary>
        /// Internal storage of pivot vector.
        /// </summary>
        private readonly int[] piv;

        /// <summary>
        /// LU Decomposition, a structure to access L, U and piv.
        /// </summary>
        /// <param name="A">Rectangular matrix</param>
        public LUDecomposition(Matrix A)
        {
            // Use a "left-looking", dot-product, Crout/Doolittle algorithm. 
            LU = A.GetArrayCopy();
            m = A.GetRowDimension();
            n = A.GetColumnDimension();
            piv = new int[m];
            for (int i = 0; i < m; ++i)
            {
                piv[i] = i;
            }
            pivsign = 1;
            double[] LUcolj = new double[m];

            // Outer loop. 
            for (int j = 0; j < n; j++)
            {
                // Make a copy of the j-th column to localize references. 
                for (int i = 0; i < m; ++i)
                {
                    LUcolj[i] = LU[i, j];
                }

                // Apply previous transformations. 
                for (int i = 0; i < m; ++i)
                {
                    // Most of the time is spent in the following dot product. 
                    int kmax = System.Math.Min(i, j);
                    double s = 0.0;
                    for (int k = 0; k < kmax; k++)
                    {
                        s += LU[i, k] * LUcolj[k];
                    }

                    LU[i, j] = LUcolj[i] -= s;
                }

                // Find pivot and exchange if necessary. 
                int p = j;
                for (int i = j + 1; i < m; ++i)
                {
                    if (System.Math.Abs(LUcolj[i]) > System.Math.Abs(LUcolj[p]))
                    {
                        p = i;
                    }
                }
                if (p != j)
                {
                    int k = 0;
                    for (k = 0; k < n; k++)
                    {
                        double t = LU[p, k];
                        LU[p, k] = LU[j, k];
                        LU[j, k] = t;
                    }
                    k = piv[p];
                    piv[p] = piv[j];
                    piv[j] = k;
                    pivsign = -pivsign;
                }

                // Compute multipliers. 
                if (j < m & LU[j, j] != 0.0)
                {
                    for (int i = j + 1; i < m; ++i)
                    {
                        LU[i, j] /= LU[j, j];
                    }
                }
            }
        }
 
        /// <summary>
        /// LU Decomposition, computed by Gaussian elimination. <para />
        /// This constructor computes L and U with the "daxpy"-based elimination algorithm
        /// used in LINPACK and MATLAB.<para />We suspect the dot-product, Crout algorithm will be faster. 
        /// We have temporarily included this constructor until timing experiments confirm this 
        /// suspicion. 
        /// </summary>
        /// <param name="A">A Rectangular matrix</param>
        /// <param name="linpackflag">linpackflag Use Gaussian elimination. Actual value ignored.</param>
        public LUDecomposition(Matrix A, bool linpackflag)
        {
            // Initialize. 
            LU = A.GetArrayCopy();
            m = A.GetRowDimension();
            n = A.GetColumnDimension();
            piv = new int[m];
            for (int i = 0; i < m; ++i)
            {
                piv[i] = i;
            }
            pivsign = 1;
            // Main loop. 
            for (int k = 0; k < n; k++)
            {
                // Find pivot. 
                int p = k;
                for (int i = k + 1; i < m; ++i)
                {
                    if (System.Math.Abs(LU[i, k]) > System.Math.Abs(LU[p, k]))
                    {
                        p = i;
                    }
                }
                // Exchange if necessary. 
                if (p != k)
                {
                    for (int j = 0; j < n; j++)
                    {
                        double tr = LU[p, j];
                        LU[p, j] = LU[k, j];
                        LU[k, j] = tr;
                    }
                    int t = piv[p];
                    piv[p] = piv[k];
                    piv[k] = t;
                    pivsign = -pivsign;
                }
                // Compute multipliers and eliminate k-th column. 
                if (LU[k, k] != 0.0)
                {
                    for (int i = k + 1; i < m; ++i)
                    {
                        LU[i, k] /= LU[k, k];
                        for (int j = k + 1; j < n; j++)
                        {
                            LU[i, j] -= LU[i, k] * LU[k, j];
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Is the matrix nonsingular?
        /// </summary>
        /// <returns>true if U, and hence A, is nonsingular.</returns>
        public bool IsNonsingular()
        {
            for (int j = 0; j < n; j++)
            {
                if (LU[j, j] == 0)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Return lower triangular factor
        /// </summary>
        /// <returns>L</returns>
        public Matrix GetL()
        {
            Matrix X = new Matrix(m, n);
            double[,] L = X.GetArray();
            for (int i = 0; i < m; ++i)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i > j)
                    {
                        L[i, j] = LU[i, j];
                    }
                    else if (i == j)
                    {
                        L[i, j] = 1.0;
                    }
                    else
                    {
                        L[i, j] = 0.0;
                    }
                }
            }
            return X;
        }

        /// <summary>
        /// Return upper triangular factor
        /// </summary>
        /// <returns>U</returns>
        public Matrix GetU()
        {
            Matrix X = new Matrix(n, n);
            double[,] U = X.GetArray();
            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i <= j)
                    {
                        U[i, j] = LU[i, j];
                    }
                    else
                    {
                        U[i, j] = 0.0;
                    }
                }
            }
            return X;
        }


        /// <summary>
        /// pivot permutation vector
        /// </summary>
        /// <returns>piv</returns>
        public int[] GetPivot()
        {
            int[] p = new int[m];
            for (int i = 0; i < m; ++i)
            {
                p[i] = piv[i];
            }
            return p;
        }

        /// <summary>
        /// Return pivot permutation vector as a one-dimensional double array
        /// </summary>
        /// <returns>(double) piv</returns>
        public double[] GetDoublePivot()
        {
            double[] vals = new double[m];
            for (int i = 0; i < m; ++i)
            {
                vals[i] = piv[i];
            }
            return vals;
        }

        /// <summary>
        /// Determinant
        /// </summary>
        /// <returns>det(A)</returns>
        /// <exception cref="IllegalArgumentException">Matrix must be square</exception>
        public double Det()
        {
            if (m != n)
            {
                throw new IllegalArgumentException("Matrix must be square.");
            }
            double d = pivsign;
            for (int j = 0; j < n; j++)
            {
                d *= LU[j, j];
            }
            return d;
        }

        /// <summary>
        /// Solve A*X = B
        /// </summary>
        /// <param name="B">A Matrix with as many rows as A and any number of columns.</param>
        /// <returns>X so that L*U*X = B(piv,:)</returns>
        /// <exception cref="RuntimeException">Matrix is singular.</exception>
        /// <exception cref="IllegalArgumentException">Matrix row dimensions must agree.</exception>
        public Matrix Solve(Matrix B)
        {
            if (B.GetRowDimension() != m)
            {
                throw new IllegalArgumentException(
                        "Matrix row dimensions must agree.");
            }
            if (!this.IsNonsingular())
            {
                throw new RuntimeException("Matrix is singular.");
            }

            // Copy right hand side with pivoting
            int nx = B.GetColumnDimension();
            Matrix Xmat = B.GetMatrix(piv, 0, nx - 1);
            double[,] X = Xmat.GetArray();

            // Solve L*Y = B(piv,:)
            for (int k = 0; k < n; k++)
            {
                for (int i = k + 1; i < n; ++i)
                {
                    for (int j = 0; j < nx; j++)
                    {
                        X[i, j] -= X[k, j] * LU[i, k];
                    }
                }
            }
            // Solve U*X = Y;
            for (int k = n - 1; k >= 0; k--)
            {
                for (int j = 0; j < nx; j++)
                {
                    X[k, j] /= LU[k, k];
                }
                for (int i = 0; i < k; ++i)
                {
                    for (int j = 0; j < nx; j++)
                    {
                        X[i, j] -= X[k, j] * LU[i, k];
                    }
                }
            }
            return Xmat;
        }
    }
}
