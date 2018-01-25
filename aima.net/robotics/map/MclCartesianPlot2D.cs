using System.IO;
using aima.net.collections.api;
using aima.net.exceptions;
using aima.net.robotics.api;
using aima.net.robotics.datatypes;
using aima.net.robotics.datatypes.api;
using aima.net.robotics.map.api;
using aima.net.util.math.geom;
using aima.net.util.math.geom.api;
using aima.net.util.math.geom.shapes;
using aima.net.util.math.geom.shapes.api;

namespace aima.net.robotics.map
{ 
    /// <summary>
    /// This class : the interface IMclMap using the classes Angle and AbstractRangeReading.<para />
    /// It uses a parser that generates two sets of IGeometric2D.<para />
    /// The first set describes obstacles that can be measured by the range sensor. 
    /// Thus only this group is considered for the rayCast function.<para />
    /// The second group specifies areas on the map. If a position is in one of these areas it is a valid position.<para />
    /// This functionality is implemented by isPoseValid which in addition tests whether 
    /// the heading of that pose is valid and the position is inside an obstacle which makes it an invalid position. 
    /// </summary>
    /// <typeparam name="P">a pose implementing IPose2D.</typeparam>
    /// <typeparam name="M">a movement (or sequence of movements) of the robot, implementing IMclMove</typeparam>
    /// <typeparam name="R">a range reading extending AbstractRangeReading.</typeparam>
    public class MclCartesianPlot2D<P, M, R> : IMclMap<P, Angle, M, AbstractRangeReading>
        where P : IPose2D<P, M>
        where M : IMclMove<M>
        where R : AbstractRangeReading
    {
        /// <summary>
        /// This is the identifier that is used to find a group of obstacles in the map file.
        /// </summary>
        public const string OBSTACLE_ID = "obstacles";
        /// <summary>
        /// This is the identifier that is used to find a group of areas in the map file.
        /// </summary>
        public const string AREA_ID = "validMovementArea";

        private IPoseFactory<P, M> poseFactory;
        private IRangeReadingFactory<R> rangeReadingFactory;

        private CartesianPlot2D obstacles;
        private CartesianPlot2D areas;

        private Exception obstaclesException;
        private Exception areasException;
         
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obstaclesParser">
        /// a map parser implementing IGroupParser. 
        /// This parser is used to load a map file for the obstacles.</param>
        /// <param name="areasParser">
        /// a map parser implementing IGroupParser. This parser is used to load a map file for the areas. 
        /// It should be a different object than obstaclesParser or implemented thread-safe.</param>
        /// <param name="poseFactory">a pose factory implementing IPoseFactory.</param>
        /// <param name="rangeReadingFactory">a range reading factory implementing IRangeReadingFactory.</param>
        public MclCartesianPlot2D(IGroupParser obstaclesParser, IGroupParser areasParser, IPoseFactory<P, M> poseFactory, IRangeReadingFactory<R> rangeReadingFactory)
        {
            this.poseFactory = poseFactory;
            this.rangeReadingFactory = rangeReadingFactory;
            obstacles = new CartesianPlot2D(obstaclesParser);
            areas = new CartesianPlot2D(areasParser);
        }
         
        /// <summary>
        /// Sets the sensor range.
        /// </summary>
        /// <param name="sensorRange">
        /// the maximum range that the sensor can reliably measure. 
        /// This parameter is used to speed up rayCast.</param>
        public void setSensorRange(double sensorRange)
        {
            obstacles.setRayRange(sensorRange);
            areas.setRayRange(sensorRange);
        }
       
        /// <summary>
        /// Calculate the maximum distance between all samples and compare it to maxDistance.
        /// If it is smaller or equals to {@code maxDistance} the mean is returned. null otherwise.
        /// </summary>
        /// <param name="samples">the set of samples to be checked against.</param>
        /// <param name="maxDistance">the maxDistance that the cloud should have to return a mean.</param>
        /// <returns>the mean of the samples or null.</returns>
        public P checkDistanceOfPoses(ISet<P> samples, double maxDistance)
        {
            double maxDistanceSamples = 0.0d;
            foreach (P first in samples)
            {
                foreach (P second in samples)
                {
                    double distance = first.DistanceTo(second);
                    maxDistanceSamples = distance > maxDistanceSamples ? distance : maxDistanceSamples;
                }
            }
            if (maxDistanceSamples <= maxDistance)
            {
                double averageX = 0.0d;
                double averageY = 0.0d;
                double averageHeading = 0.0d;
                foreach (P sample in samples)
                {
                    averageX += sample.GetX() / samples.Size();
                    averageY += sample.GetY() / samples.Size();
                    averageHeading += sample.GetHeading() / samples.Size();
                }
                return poseFactory.GetPose(new Point2D(averageX, averageY), averageHeading);
            }
            return default(P);
        }
        
        /// <summary>
        /// This function loads a map input stream into this Cartesian plot. 
        /// The two streams have to be two different instances to be thread safe.
        /// </summary>
        /// <param name="obstacleInput">the stream containing the obstacles.</param>
        /// <param name="areaInput">the stream containing the areas</param>
        public void loadMap(StreamReader obstacleInput, StreamReader areaInput)
        {
            obstaclesException = null;
            areasException = null;

            obstacles.loadMap(obstacleInput, OBSTACLE_ID);
            areas.loadMap(areaInput, AREA_ID);

            if (obstaclesException != null) throw obstaclesException;
            if (areasException != null) throw areasException;
        }

        /// <summary>
        /// Returns an iterator over the obstacle polygons.
        /// </summary>
        /// <returns>an iterator over the obstacle polygons.</returns>
        public IEnumerator<IGeometric2D> getObstacles()
        {
            return obstacles.getShapes();
        }

        /// <summary>
        /// Returns 
        /// </summary>
        /// <returns>an iterator over the boundaries of the obstacle polygons.</returns>
        public IEnumerator<Rect2D> getObstacleBoundaries()
        {
            return obstacles.getBoundaries();
        }

        /// <summary>
        /// Returns an iterator over the area polygons.
        /// </summary>
        /// <returns>an iterator over the area polygons.</returns>
        public IEnumerator<IGeometric2D> getAreas()
        {
            return areas.getShapes();
        }

        /// <summary>
        /// Returns an iterator over the boundaries of the area polygons.
        /// </summary>
        /// <returns>an iterator over the boundaries of the area polygons.</returns>
        public IEnumerator<Rect2D> getAreaBoundaries()
        {
            return areas.getBoundaries();
        }
         
        /// <summary>
        /// Checks whether a map was loaded.
        /// </summary>
        /// <returns>true if a map was loaded.</returns>
        public bool isLoaded()
        {
            return !areas.isEmpty();
        }
         
        public P randomPose()
        {
            Point2D point;
            do
            {
                point = areas.randomPoint();
            } while (obstacles.isPointInsideShape(point));
            return poseFactory.GetPose(point);
        }

        AbstractRangeReading IMclMap<P, Angle, M, AbstractRangeReading>.rayCast(P pose)
        {
            return rayCast(pose);
        }

        public R rayCast(P pose)
        {
            Ray2D ray = new Ray2D(new Point2D(pose.GetX(), pose.GetY()), Vector2D.calculateFromPolar(1, -pose.GetHeading()));
            return rangeReadingFactory.getRangeReading(obstacles.rayCast(ray));
        }
         
        public bool isPoseValid(P pose)
        {
            if (!poseFactory.IsHeadingValid(pose))
            {
                return false;
            }

            Point2D point = new Point2D(pose.GetX(), pose.GetY());
            return areas.isPointInsideBorderShape(point) && !obstacles.isPointInsideShape(point);
        }
    }

}
