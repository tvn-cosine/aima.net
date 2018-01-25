using aima.net.util.math.geom.shapes.api;

namespace aima.net.util.math.geom.shapes
{
    /**
    * This class : an ellipse in a two-dimensional Cartesian plot.<br/>
    * As an ellipse can be rotated around its center it is represented by a center {@link Point2D} and two {@link Vector2D} that are the horizontal and vertical radius.
    * 
    * @author Arno von Borries
    * @author Jan Phillip Kretzschmar
    * @author Andreas Walscheid
    *
    */
    public class Ellipse2D : IGeometric2D
    {
        private readonly Point2D center;
        private readonly Vector2D horizontal;
        private readonly double horizontalLength;
        private readonly Vector2D vertical;
        private readonly double verticalLength;
        private readonly double angle;
        private readonly TransformMatrix2D _transform;
        private readonly TransformMatrix2D transformInverse;

        /**
         * @param center the center point of the ellipse.
         * @param rx the radius of the ellipse in X direction.
         * @param ry the radius of the ellipse in Y direction.
         */
        public Ellipse2D(Point2D center, double rx, double ry)
        {
            this.center = center;
            this.horizontal = new Vector2D(rx, 0.0d);
            this.horizontalLength = rx;
            this.vertical = new Vector2D(0.0d, ry);
            this.verticalLength = ry;
            this.angle = 0.0d;
            this._transform = TransformMatrix2D.UNITY_MATRIX;
            this.transformInverse = TransformMatrix2D.UNITY_MATRIX;
        }

        /**
         * @param center the center point of the ellipse.
         * @param horizontal the horizontal radius of the ellipse as a vector.
         * @param vertical the vertical radius of the ellipse as a vector.
         */
        public Ellipse2D(Point2D center, Vector2D horizontal, Vector2D vertical)
        {
            this.center = center;
            if (Util.compareDoubles(horizontal.getX(), 0.0d) && Util.compareDoubles(vertical.getY(), 0.0d))
            {
                this.horizontal = vertical;
                this.vertical = horizontal;
            }
            else
            {
                this.horizontal = horizontal;
                this.vertical = vertical;
            }
            this.horizontalLength = this.horizontal.length();
            this.verticalLength = this.vertical.length();
            if (Util.compareDoubles(this.horizontal.getY(), 0.0d) && Util.compareDoubles(this.vertical.getX(), 0.0d))
            {
                this.angle = 0.0d;
                this._transform = TransformMatrix2D.UNITY_MATRIX;
                this.transformInverse = TransformMatrix2D.UNITY_MATRIX;
            }
            else
            {
                if (Util.compareDoubles(this.horizontal.getX(), 0.0d))
                {
                    this.angle = Vector2D.Y_VECTOR.angleTo(this.vertical);
                }
                else
                {
                    this.angle = Vector2D.X_VECTOR.angleTo(this.horizontal);
                }
                TransformMatrix2D result = TransformMatrix2D.translate(center.getX(), center.getY());
                result = result.multiply(TransformMatrix2D.rotate(-this.angle));
                this._transform = result.multiply(TransformMatrix2D.translate(-center.getX(), -center.getY()));
                this.transformInverse = this._transform.inverse();
            }
        }

        /**
         * @return the center point of the ellipse.
         */
        public Point2D getCenter()
        {
            return center;
        }

        /**
         * @return the length of the horizontal radius vector of the ellipse.
         */
        public double getHorizontalLength()
        {
            return horizontalLength;
        }

        /**
         * @return the length of the vertical radius vector of the ellipse.
         */
        public double getVerticalLength()
        {
            return verticalLength;
        }

        /**
         * @return the angle by which the ellipse is rotated.
         */
        public double getAngle()
        {
            return angle;
        }


        public Point2D randomPoint()
        {
            double x = Util.generateRandomDoubleBetween(-horizontalLength, horizontalLength);
            double delta = System.Math.Abs(verticalLength * System.Math.Sin(System.Math.Acos(x / horizontalLength)));
            double y = Util.generateRandomDoubleBetween(-delta, delta);
            return transformInverse.multiply(new Point2D(center.getX() + x, center.getY() + y));
        }


        public bool isInside(Point2D point)
        {
            Point2D transformedPoint = _transform.multiply(point);
            if (center.getX() - horizontalLength < transformedPoint.getX() && center.getX() + horizontalLength > transformedPoint.getX())
            {
                double delta = System.Math.Abs(verticalLength * System.Math.Sin(System.Math.Acos((transformedPoint.getX() - center.getX()) / horizontalLength)));
                return transformedPoint.getY() < center.getY() + delta && transformedPoint.getY() > center.getY() - delta;
            }
            else
            {
                return false;
            }
        }


        public bool isInsideBorder(Point2D point)
        {
            Point2D transformedPoint = _transform.multiply(point);
            if ((center.getX() - horizontalLength < transformedPoint.getX() || Util.compareDoubles(transformedPoint.getX(), center.getX() - horizontalLength)) && (center.getX() + horizontalLength > transformedPoint.getX() || Util.compareDoubles(transformedPoint.getX(), center.getX() + horizontalLength)))
            {
                double delta = System.Math.Abs(verticalLength * System.Math.Sin(System.Math.Acos((transformedPoint.getX() - center.getX()) / horizontalLength)));
                return (transformedPoint.getY() < center.getY() + delta || Util.compareDoubles(transformedPoint.getY(), center.getY() + delta)) && (transformedPoint.getY() >= center.getY() - delta || Util.compareDoubles(transformedPoint.getY(), center.getY() - delta));
            }
            return false;
        }


        public double rayCast(Ray2D ray)
        {
            Ray2D transformedRay = ray.transform(_transform);
            double squaredHorizontal = horizontalLength * horizontalLength;
            double squaredVertical = verticalLength * verticalLength;
            double squaredDirectionX = transformedRay.getDirection().getX() * transformedRay.getDirection().getX();
            double divisor = squaredHorizontal * transformedRay.getDirection().getY() * transformedRay.getDirection().getY() + squaredVertical * squaredDirectionX;
            if (Util.compareDoubles(divisor, 0.0d)) return double.PositiveInfinity;
            double squareRoot = System.Math.Sqrt(squaredHorizontal * squaredVertical * (squaredDirectionX * (squaredVertical - center.getY() * center.getY() + transformedRay.getStart().getY() * (2 * center.getY() - transformedRay.getStart().getY())) + transformedRay.getDirection().getY() * (transformedRay.getDirection().getY() * (squaredHorizontal - center.getX() * center.getX() + transformedRay.getStart().getX() * (2 * center.getX() - transformedRay.getStart().getX())) + 2 * transformedRay.getDirection().getX() * (transformedRay.getStart().getX() * (transformedRay.getStart().getY() - center.getY()) + center.getX() * (center.getY() - transformedRay.getStart().getY())))));
            if (Util.compareDoubles(squareRoot, double.NaN)) return double.PositiveInfinity;
            double tmpFactor1 = squaredVertical * transformedRay.getDirection().getX();
            double tmpFactor2 = squaredHorizontal * transformedRay.getDirection().getY();
            double factors = tmpFactor1 * center.getX() - tmpFactor1 * transformedRay.getStart().getX() + tmpFactor2 * center.getY() - tmpFactor2 * transformedRay.getStart().getY();
            double result = (factors - System.Math.Abs(squareRoot)) / divisor;
            if (result >= 0)
            {
                Point2D intersection = transformedRay.getStart().add(transformedRay.getDirection().multiply(result));
                return transformInverse.multiply(intersection).vec(ray.getStart()).length();
            }
            return double.PositiveInfinity;
        }


        public Rect2D getBounds()
        {
            double cosAngle = System.Math.Cos(angle);
            double sinAngle = System.Math.Sin(angle);
            double boundX = System.Math.Sqrt(horizontalLength * horizontalLength * cosAngle * cosAngle + verticalLength * verticalLength * sinAngle * sinAngle);
            double boundY = System.Math.Sqrt(horizontalLength * horizontalLength * sinAngle * sinAngle + verticalLength * verticalLength * cosAngle * cosAngle);
            return new Rect2D(center.getX() - boundX, center.getY() - boundY, center.getX() + boundX, center.getY() + boundY);
        }


        public IGeometric2D transform(TransformMatrix2D matrix)
        {
            Point2D centerNew = matrix.multiply(center);
            Vector2D horizontalNew = centerNew.vec(matrix.multiply(center.add(horizontal))),
                     verticalNew = centerNew.vec(matrix.multiply(center.add(vertical)));
            double radiusHorizontalNew = horizontalNew.length();
            double radiusVerticalNew = verticalNew.length();
            if (Util.compareDoubles(radiusHorizontalNew, radiusVerticalNew))
            {
                //Transform let this ellipse become a circle.
                return new Circle2D(centerNew, radiusHorizontalNew);
            }
            return new Ellipse2D(centerNew, horizontalNew, verticalNew);
        }
    } 
}
