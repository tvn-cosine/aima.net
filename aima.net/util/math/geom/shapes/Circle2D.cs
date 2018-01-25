using aima.net.util.math.geom.shapes.api;

namespace aima.net.util.math.geom.shapes
{
    /**
     * This class : a circle in a two-dimensional Cartesian plot.<br/>
     * The circle is represented by a {@link Point2D} center and a {@code double} radius.
     * 
     * @author Arno von Borries
     * @author Jan Phillip Kretzschmar
     * @author Andreas Walscheid
     *
     */
    public class Circle2D : IGeometric2D
    {
        private readonly Point2D center;
        private readonly double radius;

        /**
         * @param center the center point of the circle.
         * @param radius the radius of the circle.
         */
        public Circle2D(Point2D center, double radius)
        {
            this.center = center;
            this.radius = radius;
        }

        /**
         * @param cx the x coordinate of the center point of the circle.
         * @param cy the y coordinate of the center point of the circle.
         * @param radius the radius of the circle.
         */
        public Circle2D(double cx, double cy, double radius)
        {
            this.center = new Point2D(cx, cy);
            this.radius = radius;
        }

        /**
         * @return the center point of the circle.
         */
        public Point2D getCenter()
        {
            return center;
        }

        /**
         * @return the radius of the circle.
         */
        public double getRadius()
        {
            return radius;
        }


        public Point2D randomPoint()
        {
            double x = Util.generateRandomDoubleBetween(-radius, radius);
            double maxY = System.Math.Sqrt(radius * radius - x * x);
            double y = Util.generateRandomDoubleBetween(-maxY, maxY);
            return new Point2D(center.getX() + x, center.getY() + y);
        }


        public bool isInside(Point2D point)
        {
            return center.vec(point).length() < radius;
        }


        public bool isInsideBorder(Point2D point)
        {
            return center.vec(point).length() <= radius;
        }


        public double rayCast(Ray2D ray)
        {
            double divisor = ray.getDirection().getX() * ray.getDirection().getX() + ray.getDirection().getY() * ray.getDirection().getY();
            if (Util.compareDoubles(divisor, 0.0d)) return double.PositiveInfinity;
            double squareRoot = System.Math.Sqrt(ray.getDirection().getY() * (ray.getDirection().getY() * (radius * radius + ray.getStart().getX() * (2 * center.getX() - ray.getStart().getX()) - center.getX() * center.getX()) + 2 * ray.getDirection().getX() * (ray.getStart().getX() * (ray.getStart().getY() - center.getY()) + center.getX() * (center.getY() - ray.getStart().getY()))) + ray.getDirection().getX() * ray.getDirection().getX() * (radius * radius + ray.getStart().getY() * (2 * center.getY() - ray.getStart().getY()) - center.getY() * center.getY()));
            if (Util.compareDoubles(squareRoot, double.NaN)) return double.PositiveInfinity;
            double factors = ray.getDirection().getX() * (center.getX() - ray.getStart().getX()) + ray.getDirection().getY() * (center.getY() - ray.getStart().getY());
            double result = (factors - System.Math.Abs(squareRoot)) / divisor;
            if (result >= 0) return ray.getDirection().length() * result;
            return double.PositiveInfinity;
        }


        public Rect2D getBounds()
        {
            return new Rect2D(center.getX() - radius, center.getY() - radius, center.getX() + radius, center.getY() + radius);
        }


        public IGeometric2D transform(TransformMatrix2D matrix)
        {
            Point2D centerNew = matrix.multiply(center);
            Point2D maxXNew = matrix.multiply(new Point2D(center.getX() + radius, center.getY()));
            Point2D maxYNew = matrix.multiply(new Point2D(center.getX(), center.getY() + radius));
            Vector2D vectorXNew = centerNew.vec(maxXNew);
            Vector2D vectorYNew = centerNew.vec(maxYNew);
            double radiusXNew = vectorXNew.length();
            double radiusYNew = vectorYNew.length();
            if (Util.compareDoubles(radiusXNew, radiusYNew))
            {
                //Transform let this circle stay a circle.
                return new Circle2D(centerNew, radiusXNew);
            }
            if (Util.compareDoubles(maxXNew.getY(), centerNew.getY()) && Util.compareDoubles(maxYNew.getX(), centerNew.getX()))
            {
                //Transform let this circle become a ellipse, but it is not rotated.
                return new Ellipse2D(centerNew, radiusXNew, radiusYNew);
            }
            //Transform let this circle become a rotated ellipse.
            return new Ellipse2D(centerNew, vectorXNew, vectorYNew);
        }
    }

}
