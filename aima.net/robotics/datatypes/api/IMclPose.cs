using aima.net.api; 

namespace aima.net.robotics.datatypes.api
{ 
    /// <summary>
    /// A pose consists of a position in the environment and a heading in which the pose is facing.<para />
    /// In a two-dimensional environment this would be an position represented through a point with (x,y) and an angle.<para />
    /// In an n-dimensional environment the angle is represented through a vector.<para />
    /// </summary>
    /// <typeparam name="P">the class that is implementing this interface.</typeparam>
    /// <typeparam name="V">
    /// an n-1-dimensional vector implementing {@link IMclVector}, where n is the dimensionality of the environment. 
    /// This vector describes the angle between two rays in the environment.
    /// </typeparam>
    /// <typeparam name="M">a movement (or sequence of movements) of the robot, implementing IMclMove.</typeparam>
    public interface IMclPose<P, V, M> : ICloneable<P>
        where P : IMclPose<P, V, M>
        where V : IMclVector
        where M : IMclMove<M>

    {   
        /// <summary>
        /// Moves a pose according to a given move.<para />
        /// This function should return a new object to prevent unpredictable behavior through more than one usage of the same pose.
        /// </summary>
        /// <param name="move">the move to be added onto the pose.</param>
        /// <returns>a new pose that has been moved.</returns>
        P ApplyMovement(M move);
        
        /// <summary>
        /// Rotates a pose by a given IMclVector.<para />
        /// This function should return a new object to prevent unpredictable 
        /// behavior through more than one usage of the same pose.
        /// </summary>
        /// <param name="angle">angle the angle by which the pose should be rotated.</param>
        /// <returns>a new pose that has been rotated.</returns>
        P AddAngle(V angle);
        
        /// <summary>
        /// Calculates the length of the straight line between this pose and another pose.<para />
        /// x.distanceTo(y) has to return the same value as y.distanceTo(x).
        /// </summary>
        /// <param name="pose">another pose to which the distance is calculated.</param>
        /// <returns>the distance between the two poses.</returns>
        double DistanceTo(P pose);
    }
}
