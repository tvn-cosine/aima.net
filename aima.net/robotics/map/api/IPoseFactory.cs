using aima.net.robotics.datatypes.api;
using aima.net.util.math.geom.shapes;

namespace aima.net.robotics.map.api
{ 
    /// <summary>
    /// This interface defines a factory for the class 
    /// implementing IPose2D in the context of the MclCartesianPlot2D.
    /// </summary>
    /// <typeparam name="P">a pose implementing IPose2D</typeparam>
    /// <typeparam name="M">a movement (or a sequence of movements) implementing IMclMove</typeparam>
    public interface IPoseFactory<P, M>
         where P : IPose2D<P, M>
         where M : IMclMove<M>
    { 
        /// <summary>
        /// Creates a new instance of <P> for the given parameters.
        /// </summary>
        /// <param name="point">the 2D coordinates of the new pose.</param>
        /// <returns>the new pose.</returns>
        P GetPose(Point2D point);
        
        /// <summary>
        /// Creates a new instance of <P> for the given parameters.<para />
        /// This function is used to create the result of MclCartesianPlot2D.getAverage().
        /// </summary>
        /// <param name="point">the 2D coordinates of the new pose.</param>
        /// <param name="heading">the heading of the pose. This heading may be invalid. 
        /// Based on the given environment this can be corrected or ignored.</param>
        /// <returns>the new pose.</returns>
        P GetPose(Point2D point, double heading);
         
        /// <summary>
        /// Checks whether the heading of a pose is valid.
        /// </summary>
        /// <param name="pose">the pose to be checked.</param>
        /// <returns>true if the heading is valid.</returns>
        bool IsHeadingValid(P pose);
    } 
}
