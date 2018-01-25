using aima.net.util.math.geom.shapes.api;

namespace aima.net.util.math.geom.shapes
{
    /**
     * This class : a polyline in a two-dimensional Cartesian plot.<br/>
     * The polyline is represented by a starting point represented by {@link Point2D} and all edges as {@link Vector2D}.<br/>
     * If the last side ends in the starting point the polyline is a closed polygon.
     * 
     * @author Arno von Borries
     * @author Jan Phillip Kretzschmar
     * @author Andreas Walscheid
     *
     */
    public class Polyline2D : IGeometric2D
    {
        private readonly Point2D[] vertexes;
        private readonly Vector2D[] edges;
        private readonly bool _isClosed;
        private readonly Rect2D boundingRect;

        /**
         * @param vertexes the vertexes of the polyline or polygon.
         * @param isClosed true if the sum of the edges is the zero vector.
         */
        public Polyline2D(Point2D[] vertexes, bool isClosed)
        {
            this.vertexes = vertexes;
            this._isClosed = isClosed;
            int length = isClosed ? vertexes.Length : vertexes.Length - 1;
            this.edges = new Vector2D[length];
            Point2D previousPoint = vertexes[0];
            for (int i = 1; i < vertexes.Length;++i)
            {
                Point2D targetPoint = vertexes[i];
                edges[i - 1] = previousPoint.vec(targetPoint);
                previousPoint = targetPoint;
            }
            if (isClosed)
            {
                edges[vertexes.Length - 1] = previousPoint.vec(vertexes[0]);
            }
            //Calculate the bounding rectangle:
            double minX = vertexes[0].getX(),
                    minY = vertexes[0].getY(),
                    maxX = vertexes[0].getX(),
                    maxY = vertexes[0].getY();
            for (int i = 1; i < vertexes.Length;++i)
            {
                minX = minX > vertexes[i].getX() ? vertexes[i].getX() : minX;
                minY = minY > vertexes[i].getY() ? vertexes[i].getY() : minY;
                maxX = maxX < vertexes[i].getX() ? vertexes[i].getX() : maxX;
                maxY = maxY < vertexes[i].getY() ? vertexes[i].getY() : maxY;
            }
            boundingRect = new Rect2D(minX, minY, maxX, maxY);
        }

        /**
         * @return the starting point of the polyline.
         */
        public Point2D[] getVertexes()
        {
            return vertexes;
        }

        /**
         * @return the edges of the polyline.
         */
        public Vector2D[] getEdges()
        {
            return edges;
        }

        /**
         * @return true if this polyline is a polygon.
         */
        public bool isClosed()
        {
            return _isClosed;
        }


        public Point2D randomPoint()
        {
            if (_isClosed)
            {
                //Generate random points within the bounding rectangle...
                double minX = boundingRect.getUpperLeft().getX();
                double maxX = boundingRect.getLowerRight().getX();
                double minY = boundingRect.getUpperLeft().getY();
                double maxY = boundingRect.getLowerRight().getY();

                Point2D randPoint = new Point2D(Util.generateRandomDoubleBetween(minX, maxX), Util.generateRandomDoubleBetween(minY, maxY));

                //...until one is inside the polygon.
                while (!isInsideBorder(randPoint))
                {
                    randPoint = new Point2D(Util.generateRandomDoubleBetween(minX, maxX), Util.generateRandomDoubleBetween(minY, maxY));
                }

                return randPoint;
            }
            else
            {
                int index = Util.randomNumberBetween(0, vertexes.Length - 2);
                Line2D line = new Line2D(vertexes[index], edges[index]);
                return line.randomPoint();
            }
        }


        public bool isInside(Point2D point)
        {
            if (!_isClosed) return false;
            int intersections = 0;
            Ray2D pointRay = new Ray2D(point, Vector2D.X_VECTOR);
            for (int i = 0; i < edges.Length;++i)
            {
                if (vertexes[i].Equals(point))
                {
                    return false;
                }
                double result = new Line2D(vertexes[i], edges[i]).rayCast(pointRay);
                if (!Util.compareDoubles(result, double.PositiveInfinity) && !Util.compareDoubles(result, 0.0d))
                {
                    if (!Util.compareDoubles(edges[i].angleTo(Vector2D.X_VECTOR), 0.0d)) intersections++;
                }
            }
            return intersections % 2 == 1;
        }


        public bool isInsideBorder(Point2D point)
        {
            int intersections = 0;
            Ray2D pointRay = new Ray2D(point, Vector2D.X_VECTOR);
            for (int i = 0; i < edges.Length;++i)
            {
                Line2D line = new Line2D(vertexes[i], edges[i]);
                if (line.isInsideBorder(point)) return true;
                double result = line.rayCast(pointRay);
                if (!Util.compareDoubles(result, double.PositiveInfinity) && _isClosed)
                {
                    if (!Util.compareDoubles(edges[i].angleTo(Vector2D.X_VECTOR), 0.0d)) intersections++;
                }
            }
            return intersections % 2 == 1;
        }


        public double rayCast(Ray2D ray)
        {
            double result = double.PositiveInfinity;
            for (int i = 0; i < edges.Length;++i)
            {
                if (!ray.getDirection().isParallel(edges[i]))
                {
                    double divisor = (ray.getDirection().getX() * edges[i].getX() - ray.getDirection().getX() * edges[i].getY());
                    double len1 = (vertexes[i].getY() * edges[i].getX() - ray.getStart().getY() * edges[i].getX() - vertexes[i].getX() * edges[i].getY() + ray.getStart().getX() * edges[i].getY()) / divisor;
                    if (len1 > 0)
                    {
                          double len2 = (ray.getDirection().getY() * ray.getStart().getX() - ray.getDirection().getY() * vertexes[i].getX() - ray.getDirection().getX() * ray.getStart().getY() + ray.getDirection().getX() * vertexes[i].getY()) / divisor;
                        if (len2 >= 0 && len2 <= 1) result = result > len1 ? len1 : result;
                    }
                }
                else
                {
                      Vector2D startVec = ray.getStart().vec(vertexes[i]);
                    if (ray.getDirection().isAbsoluteParallel(startVec))
                    {
                        return startVec.length();
                    }
                    else
                    {
                          Point2D endVertex = _isClosed && i == edges.Length - 1 ? vertexes[0] : vertexes[i + 1];
                          Vector2D endVec = ray.getStart().vec(endVertex);
                        if (ray.getDirection().isAbsoluteParallel(endVec))
                        {
                            return endVec.length();
                        }
                    }
                }
            }
            return result * ray.getDirection().length();
        }


        public Rect2D getBounds()
        {
            return boundingRect;
        }

        IGeometric2D IGeometric2D.transform(TransformMatrix2D matrix)
        {
            return transform(matrix);
        }

        public Polyline2D transform(TransformMatrix2D matrix)
        {
            Point2D[] vertexesNew = new Point2D[vertexes.Length];
            for (int i = 0; i < vertexes.Length;++i)
            {
                vertexesNew[i] = matrix.multiply(vertexes[i]);
            }
            return new Polyline2D(vertexesNew, _isClosed || (Util.compareDoubles(vertexesNew[0].getX(), vertexesNew[vertexes.Length - 1].getX()) && Util.compareDoubles(vertexesNew[0].getY(), vertexesNew[vertexes.Length - 1].getY())));
        }
    } 
}
