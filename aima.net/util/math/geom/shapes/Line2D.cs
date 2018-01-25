using aima.net.util.math.geom.shapes.api;

namespace aima.net.util.math.geom.shapes
{
    /**
     * This class : a line in a two-dimensional Cartesian plot.<br/>
     * Every line consists of a starting point and an ending point represented by a {@link Point2D}.<br/>
     * In addition the line between these two points can be represented as a {@link Vector2D}. 
     * 
     * @author Arno von Borries
     * @author Jan Phillip Kretzschmar
     * @author Andreas Walscheid
     *
     */
    public class Line2D : IGeometric2D
    {
        private readonly Point2D start;
        private readonly Vector2D line;
        private readonly Point2D end;

        private readonly bool zeroX;
        private readonly bool zeroY;

        /**
         * @param start the starting point of the line.
         * @param line the vector representing the line.
         */
        public Line2D(Point2D start, Vector2D line)
        {
            this.start = start;
            this.line = line;
            this.end = start.add(line);

            this.zeroX = Util.compareDoubles(line.getX(), 0.0d);
            this.zeroY = Util.compareDoubles(line.getY(), 0.0d);
        }

        /**
         * @param start the starting point of the line.
         * @param end the ending point of the line.
         */
        public Line2D(Point2D start, Point2D end)
        {
            this.start = start;
            this.line = start.vec(end);
            this.end = end;

            this.zeroX = Util.compareDoubles(line.getX(), 0.0d);
            this.zeroY = Util.compareDoubles(line.getY(), 0.0d);
        }

        /**
         * @param startX the X coordinate of the starting point of the line.
         * @param startY the Y coordinate of the starting point of the line.
         * @param endX the X coordinate of the ending point of the line.
         * @param endY the Y coordinate of the ending point of the line.
         */
        public Line2D(double startX, double startY, double endX, double endY)
        {
            this.start = new Point2D(startX, startY);
            this.line = new Vector2D(endX - startX, endY - startY);
            this.end = new Point2D(endX, endY);

            this.zeroX = Util.compareDoubles(line.getX(), 0.0d);
            this.zeroY = Util.compareDoubles(line.getY(), 0.0d);
        }

        /**
         * @return the starting point of the line.
         */
        public Point2D getStart()
        {
            return start;
        }

        /**
         * @return the vector representing the line.
         */
        public Vector2D getDirection()
        {
            return line;
        }

        /**
         * @return the ending point of the line.
         */
        public Point2D getEnd()
        {
            return end;
        }


        public Point2D randomPoint()
        {
            if (zeroX && zeroY)
            {
                return start.Clone();
            }
            else if (zeroX)
            {
                return new Point2D(start.getX(), Util.generateRandomDoubleBetween(start.getY(), end.getY()));
            }
            else if (zeroY)
            {
                return new Point2D(Util.generateRandomDoubleBetween(start.getX(), end.getX()), start.getY());
            }
            else
            {
                double x = Util.generateRandomDoubleBetween(start.getX(), end.getX());
                double y = ((x - start.getX()) / line.getX()) * line.getY() + start.getY();
                return new Point2D(x, y);
            }
        }


        public bool isInside(Point2D point)
        {
            return false;
        }


        public bool isInsideBorder(Point2D point)
        {
            if (zeroX && zeroY)
            {
                return start.Equals(point);
            }
            else if (zeroX)
            {
                double len = (point.getY() - start.getY()) / line.getY();
                return len <= 1 && len >= 0 && Util.compareDoubles(start.getX(), point.getX());
            }
            else if (zeroY)
            {
                double len = (point.getX() - start.getX()) / line.getX();
                return len <= 1 && len >= 0 && Util.compareDoubles(start.getY(), point.getY());
            }
            else
            {
                double len1 = (point.getX() - start.getX()) / line.getX();
                double len2 = (point.getY() - start.getY()) / line.getY();
                return len1 <= 1 && len1 >= 0 && Util.compareDoubles(len1, len2);
            }
        }


        public double rayCast(Ray2D ray)
        {
            if (!ray.getDirection().isParallel(line))
            {
                double divisor = (ray.getDirection().getY() * line.getX() - ray.getDirection().getX() * line.getY());
                if (Util.compareDoubles(divisor, 0.0d)) return double.PositiveInfinity;
                double len1 = (start.getY() * line.getX() - ray.getStart().getY() * line.getX() - start.getX() * line.getY() + ray.getStart().getX() * line.getY()) / divisor;
                if (len1 > 0)
                {
                    double len2 = (ray.getDirection().getY() * ray.getStart().getX() - ray.getDirection().getY() * start.getX() - ray.getDirection().getX() * ray.getStart().getY() + ray.getDirection().getX() * start.getY()) / divisor;
                    if (len2 >= 0 && len2 <= 1) return len1 * ray.getDirection().length();
                }
            }
            else
            {
                Vector2D startVec = ray.getStart().vec(start);
                if (ray.getDirection().isAbsoluteParallel(startVec))
                {
                    return startVec.length();
                }
                else
                {
                    Vector2D endVec = ray.getStart().vec(end);
                    if (ray.getDirection().isAbsoluteParallel(endVec))
                    {
                        return endVec.length();
                    }
                }
            }
            return double.PositiveInfinity;
        }


        public Rect2D getBounds()
        {
            return new Rect2D(start.getX(), start.getY(), end.getX(), end.getY());
        }

        IGeometric2D IGeometric2D.transform(TransformMatrix2D matrix)
        {
            return transform(matrix);
        }

        public Line2D transform(TransformMatrix2D matrix)
        {
            Point2D startNew = matrix.multiply(start),
                      endNew = matrix.multiply(end);
            return new Line2D(startNew, endNew);
        }
    }

}
