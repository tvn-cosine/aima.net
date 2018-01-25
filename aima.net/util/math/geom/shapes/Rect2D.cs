﻿using aima.net.util.math.geom.shapes.api;

namespace aima.net.util.math.geom.shapes
{
    /**
     * This class : a rectangle in a two-dimensional Cartesian plot that has its edges parallel to the axis of the plot.<br/>
     * With this condition fast operations are possible for ray-casting and point-inside-testing.
     * 
     * @author Arno von Borries
     * @author Jan Phillip Kretzschmar
     * @author Andreas Walscheid
     *
     */
    public class Rect2D : IGeometric2D
    {
        private readonly Vector2D horizontalVector;
        private readonly Vector2D verticalVector;

        private readonly Point2D lowerLeft;
        private readonly Point2D upperRight;
        private readonly Point2D lowerRight;
        private readonly Point2D upperLeft;

        /**
         * @param lowerLeft the first corner of the rectangle.
         * @param upperRight the second corner of the rectangle.
         */
        public Rect2D(Point2D lowerLeft, Point2D upperRight)
            : this(lowerLeft.getX(), lowerLeft.getY(), upperRight.getX(), upperRight.getY())
        { }

        /**
         * @param x1 the X coordinate of the first corner.
         * @param y1 the Y coordinate of the first corner.
         * @param x2 the X coordinate of the opposite corner.
         * @param y2 the Y coordinate of the opposite corner.
         */
        public Rect2D(double x1, double y1, double x2, double y2)
        {
            if (x1 < x2)
            {
                if (y1 < y2)
                {
                    lowerLeft = new Point2D(x1, y1);
                    upperRight = new Point2D(x2, y2);
                }
                else
                {
                    lowerLeft = new Point2D(x1, y2);
                    upperRight = new Point2D(x2, y1);
                }
            }
            else
            {
                if (y1 < y2)
                {
                    lowerLeft = new Point2D(x2, y1);
                    upperRight = new Point2D(x1, y2);
                }
                else
                {
                    lowerLeft = new Point2D(x2, y2);
                    upperRight = new Point2D(x1, y1);
                }
            }
            lowerRight = new Point2D(upperRight.getX(), lowerLeft.getY());
            upperLeft = new Point2D(lowerLeft.getX(), upperRight.getY());

            horizontalVector = new Vector2D(lowerRight.getX() - lowerLeft.getX(), 0.0d);
            verticalVector = new Vector2D(0.0d, upperLeft.getY() - lowerLeft.getY());
        }

        /**
         * @return the lower left corner of the rectangle.
         */
        public Point2D getLowerLeft()
        {
            return lowerLeft;
        }

        /**
         * @return the upper right corner of the rectangle.
         */
        public Point2D getUpperRight()
        {
            return upperRight;
        }

        /**
         * @return the lower right corner of the rectangle.
         */
        public Point2D getLowerRight()
        {
            return lowerRight;
        }

        /**
         * @return the upper left corner of the rectangle.
         */
        public Point2D getUpperLeft()
        {
            return upperLeft;
        }


        public Point2D randomPoint()
        {
            double x = Util.generateRandomDoubleBetween(lowerLeft.getX(), upperRight.getX());
            double y = Util.generateRandomDoubleBetween(lowerLeft.getY(), upperRight.getY());
            return new Point2D(x, y);
        }


        public bool isInside(Point2D point)
        {
            return lowerLeft.getX() < point.getX() &&
                    lowerLeft.getY() < point.getY() &&
                    upperRight.getX() > point.getX() &&
                    upperRight.getY() > point.getY();
        }


        public bool isInsideBorder(Point2D point)
        {
            return lowerLeft.getX() <= point.getX() &&
                    lowerLeft.getY() <= point.getY() &&
                    upperRight.getX() >= point.getX() &&
                    upperRight.getY() >= point.getY();
        }


        public double rayCast(Ray2D ray)
        {
            double result = double.PositiveInfinity;
            if (!Util.compareDoubles(ray.getDirection().getY(), 0.0d))
            {
                //check for the horizontal sides
                double divisor = (ray.getDirection().getY() * horizontalVector.getX());
                if (!Util.compareDoubles(divisor, 0.0d))
                {
                    double rayLen1 = (lowerLeft.getY() * horizontalVector.getX() - ray.getStart().getY() * horizontalVector.getX()) / divisor;
                    if (rayLen1 > 0)
                    {
                        double sideLen = (ray.getDirection().getY() * ray.getStart().getX() - ray.getDirection().getY() * lowerLeft.getX() - ray.getDirection().getX() * ray.getStart().getY() + ray.getDirection().getX() * lowerLeft.getY()) / divisor;
                        if (sideLen >= 0 && sideLen <= 1) result = rayLen1;
                    }
                    double rayLen2 = (upperLeft.getY() * horizontalVector.getX() - ray.getStart().getY() * horizontalVector.getX()) / divisor;
                    if (rayLen2 > 0)
                    {
                        double sideLen = (ray.getDirection().getY() * ray.getStart().getX() - ray.getDirection().getY() * upperLeft.getX() - ray.getDirection().getX() * ray.getStart().getY() + ray.getDirection().getX() * upperLeft.getY()) / divisor;
                        if (sideLen >= 0 && sideLen <= 1) result = result > rayLen2 ? rayLen2 : result;
                    }
                }
            }
            if (!Util.compareDoubles(ray.getDirection().getX(), 0.0d))
            {
                //check for the vertical sides
                double divisor = (-ray.getDirection().getX() * verticalVector.getY());
                if (!Util.compareDoubles(divisor, 0.0d))
                {
                    double rayLen3 = (-lowerLeft.getX() * verticalVector.getY() + ray.getStart().getX() * verticalVector.getY()) / divisor;
                    if (rayLen3 > 0)
                    {
                        double sideLen = (ray.getDirection().getY() * ray.getStart().getX() - ray.getDirection().getY() * lowerLeft.getX() - ray.getDirection().getX() * ray.getStart().getY() + ray.getDirection().getX() * lowerLeft.getY()) / divisor;
                        if (sideLen >= 0 && sideLen <= 1) result = result > rayLen3 ? rayLen3 : result;
                    }
                    double rayLen4 = (-lowerRight.getX() * verticalVector.getY() + ray.getStart().getX() * verticalVector.getY()) / divisor;
                    if (rayLen4 > 0)
                    {
                        double sideLen = (ray.getDirection().getY() * ray.getStart().getX() - ray.getDirection().getY() * lowerRight.getX() - ray.getDirection().getX() * ray.getStart().getY() + ray.getDirection().getX() * lowerRight.getY()) / divisor;
                        if (sideLen >= 0 && sideLen <= 1) result = result > rayLen4 ? rayLen4 : result;
                    }
                }
            }
            return result * ray.getDirection().length();
        }


        public Rect2D getBounds()
        {
            return this;
        }


        public IGeometric2D transform(TransformMatrix2D matrix)
        {
            Point2D lowerLeftNew = matrix.multiply(lowerLeft);
            Point2D upperRightNew = matrix.multiply(upperRight);
            Point2D upperLeftNew = matrix.multiply(upperLeft);
            Point2D lowerRightNew = matrix.multiply(lowerRight);
            if (!Util.compareDoubles(lowerLeftNew.getY(), lowerRightNew.getY()) || !Util.compareDoubles(upperLeftNew.getY(), upperRightNew.getY()) || !Util.compareDoubles(lowerLeftNew.getX(), upperLeftNew.getX()) || !Util.compareDoubles(lowerRightNew.getX(), upperRightNew.getX()))
            {
                Point2D[] vertexes = new Point2D[4];
                vertexes[0] = lowerLeftNew;
                vertexes[1] = lowerRightNew;
                vertexes[2] = upperRightNew;
                vertexes[3] = upperLeftNew;
                return new Polyline2D(vertexes, true);
            }
            return new Rect2D(lowerLeftNew, upperRightNew);
        }
    }

}
